using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;
using System.Globalization;

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
                _ => HandleUnknownCallbackQueryAsync(botClient, callbackQuery, cancellationToken)
            });
        }
        catch (Exception ex) { logger.LogError(ex, "Error handling callback query: {callbackQuery.Data}", callbackQuery.Data); }
    }

    private async Task HandleSelectedLanguageAsync(ITelegramBotClient botClient, CallbackQuery? callbackQuery, CancellationToken cancellationToken)
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

    }

    private Task HandleUnknownCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery? callbackQuery, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received unknown callback query: {callbackQuery.Data}", callbackQuery?.Data);
        return Task.CompletedTask;
    }
}
