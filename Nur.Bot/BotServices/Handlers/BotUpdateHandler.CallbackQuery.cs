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
    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery, CancellationToken cancellationToken)
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
        switch (callbackQuery.Data)
        {
            case "btnPending":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: localizer["txtPending"],
                    cancellationToken: cancellationToken);

                var updateOrder = new OrderUpdateDTO()
                {
                    Id = order[callbackQuery.From.Id].Id,
                    Status = Status.Pending,
                    TotalPrice = order[callbackQuery.From.Id].TotalPrice,
                    OrderType = order[callbackQuery.From.Id].OrderType,
                    Description = order[callbackQuery.From.Id].Description,
                    UserId = order[callbackQuery.From.Id].User.Id,
                    AddressId = order[callbackQuery.From.Id].Address.Id,
                    SupplierId = order[callbackQuery.From.Id].Supplier.Id,
                    PaymentId = order[callbackQuery.From.Id].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder, cancellationToken);
                break;
            case "btnPreparing":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: localizer["txtPreparing"],
                    cancellationToken: cancellationToken);

                var updateOrder2 = new OrderUpdateDTO()
                {
                    Id = order[callbackQuery.From.Id].Id,
                    Status = Status.Preparing,
                    TotalPrice = order[callbackQuery.From.Id].TotalPrice,
                    OrderType = order[callbackQuery.From.Id].OrderType,
                    Description = order[callbackQuery.From.Id].Description,
                    UserId = order[callbackQuery.From.Id].User.Id,
                    AddressId = order[callbackQuery.From.Id].Address.Id,
                    SupplierId = order[callbackQuery.From.Id].Supplier.Id,
                    PaymentId = order[callbackQuery.From.Id].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder2, cancellationToken);
                break;
            case "btnPrepared":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: localizer["txtPrepared"],
                    cancellationToken: cancellationToken);

                var updateOrder3 = new OrderUpdateDTO()
                {
                    Id = order[callbackQuery.From.Id].Id,
                    Status = Status.Prepared,
                    TotalPrice = order[callbackQuery.From.Id].TotalPrice,
                    OrderType = order[callbackQuery.From.Id].OrderType,
                    Description = order[callbackQuery.From.Id].Description,
                    UserId = order[callbackQuery.From.Id].User.Id,
                    AddressId = order[callbackQuery.From.Id].Address.Id,
                    SupplierId = order[callbackQuery.From.Id].Supplier.Id,
                    PaymentId = order[callbackQuery.From.Id].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder3, cancellationToken);
                break;
            case "btnOnRoad":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: localizer["txtOnRoad"],
                    cancellationToken: cancellationToken);

                var updateOrder4 = new OrderUpdateDTO()
                {
                    Id = order[callbackQuery.From.Id].Id,
                    Status = Status.OnRoad,
                    TotalPrice = order[callbackQuery.From.Id].TotalPrice,
                    OrderType = order[callbackQuery.From.Id].OrderType,
                    Description = order[callbackQuery.From.Id].Description,
                    UserId = order[callbackQuery.From.Id].User.Id,
                    AddressId = order[callbackQuery.From.Id].Address.Id,
                    SupplierId = order[callbackQuery.From.Id].Supplier.Id,
                    PaymentId = order[callbackQuery.From.Id].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder4, cancellationToken);
                break;
            case "btnDelivered":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Buyurtma muvaffaqiyatli yetkazib berildi.",
                    cancellationToken: cancellationToken);

                var updateOrder5 = new OrderUpdateDTO()
                {
                    Id = order[callbackQuery.From.Id].Id,
                    Status = Status.Delivered,
                    TotalPrice = order[callbackQuery.From.Id].TotalPrice,
                    OrderType = order[callbackQuery.From.Id].OrderType,
                    Description = order[callbackQuery.From.Id].Description,
                    UserId = order[callbackQuery.From.Id].User.Id,
                    AddressId = order[callbackQuery.From.Id].Address.Id,
                    SupplierId = order[callbackQuery.From.Id].Supplier.Id,
                    PaymentId = order[callbackQuery.From.Id].Payment.Id,
                };

                await orderService.UpdateAsync(updateOrder5, cancellationToken);
                break;
            case "btnCancel":
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: localizer["txtOrderCancel"],
                    cancellationToken: cancellationToken);
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
