using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;
using System.Globalization;
using Nur.APIService.Models.Orders;
using Nur.APIService.Models.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(callbackQuery);
        try
        {
            var userState = userStates.TryGetValue(callbackQuery.Message.Chat.Id, out var state) ? state : UserState.None;

            await (userState switch
            {
                UserState.WaitingForSelectLanguage => HandleSelectedLanguageAsync(botClient, callbackQuery, cancellationToken),
                UserState.WaitingForAdminConfirmation => HandleAdminResponseAsync(botClient, callbackQuery, cancellationToken),
                _ => HandleUnknownCallbackQueryAsync(botClient, callbackQuery, cancellationToken)
            });
        }
        catch (Exception ex) { logger.LogError(ex, "Error handling callback query: {callbackQuery.Data}", callbackQuery.Data); }
    }

    private async Task HandleSelectedLanguageAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(callbackQuery);
        ArgumentNullException.ThrowIfNull(callbackQuery.Data);
        ArgumentNullException.ThrowIfNull(callbackQuery.Message);

        user[callbackQuery.Message.Chat.Id].LanguageCode = callbackQuery.Data switch
        {
            "ibtnEn" => "en",
            "ibtnRu" => "ru",
            _ => "uz"
        };

        await userService.UpdateAsync(user[callbackQuery.Message.Chat.Id], cancellationToken);

        var culture = new CultureInfo(user[callbackQuery.Message.Chat.Id].LanguageCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        await botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: localizer["txtSelectedLanguage"],
            messageId: callbackQuery.Message.MessageId,
            cancellationToken: cancellationToken);

        await RequestForContactAsync(callbackQuery.Message, cancellationToken);
    }

    private async Task HandleAdminResponseAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var dataParts = callbackQuery.Data.Split('_');
        var action = dataParts[0];
        var userId = long.Parse(dataParts[1]);

        var culture = user[userId].LanguageCode switch
        {
            "uz" => new CultureInfo("uz-Uz"),
            "en" => new CultureInfo("en-US"),
            "ru" => new CultureInfo("ru-RU"),
            _ => CultureInfo.CurrentCulture
        };

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        switch (action)
        {
            case "btnConfirmation":
                await botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: order[userId].OrderType == OrderType.Delivery? localizer["txtConfirmation"] : localizer["txtConfirmation2"],
                    cancellationToken: cancellationToken);

                var updateOrderConfirm = new OrderUpdateDTO()
                {
                    Id = order[userId].Id,
                    Status = Status.Preparing,
                    TotalPrice = order[userId].TotalPrice,
                    OrderType = order[userId].OrderType,
                    Description = order[userId].Description,
                    UserId = order[userId].User.Id,
                    AddressId = order[userId].Address.Id == 0 ? null : order[userId].Address.Id,
                    SupplierId = order[userId].Supplier.Id == 0 ? null : order[userId].Supplier.Id,
                    PaymentId = order[userId].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrderConfirm, cancellationToken);
                break;
            case "btnPending":
                await botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: localizer["txtPendingOrder"],
                    cancellationToken: cancellationToken);
                break;
            case "btnDelivered":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Buyurtma muvaffaqiyatli yetkazib berildi.",
                    replyToMessageId: orderMessage[userId].MessageId,
                    cancellationToken: cancellationToken);

                await botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: order[userId].OrderType == OrderType.Delivery ? localizer["txtOrderDelivered"] : localizer["txtOrderTakeawayed"],
                    cancellationToken: cancellationToken);

                var updateOrder5 = new OrderUpdateDTO()
                {
                    Id = order[userId].Id,
                    Status = Status.Delivered,
                    TotalPrice = order[userId].TotalPrice,
                    OrderType = order[userId].OrderType,
                    Description = order[userId].Description,
                    UserId = order[userId].User.Id,
                    AddressId = order[userId].Address.Id == 0 ? null : order[userId].Address.Id,
                    SupplierId = order[userId].Supplier.Id == 0 ? null : order[userId].Supplier.Id,
                    PaymentId = order[userId].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder5, cancellationToken);
                break;

            case "btnCancel":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: localizer["txtOrderCancel"],
                    replyToMessageId: orderMessage[userId].MessageId,
                    cancellationToken: cancellationToken);
                
                await botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: localizer["txtOrderCancel"],
                    cancellationToken: cancellationToken);

                await orderService.DeleteAsync(order[userId].Id, cancellationToken);

                break;
        }

        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
    }

    private Task HandleUnknownCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received unknown callback query: {callbackQuery.Data}", callbackQuery?.Data);
        return Task.CompletedTask;
    }
}
