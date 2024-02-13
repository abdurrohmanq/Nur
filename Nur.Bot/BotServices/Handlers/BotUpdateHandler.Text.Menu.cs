using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleMainMenuAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            /*{ } text when text == localizer["btnOrder"] => SendMenuProfessionsAsync(botClient, message, cancellationToken),
            { } text when text == localizer["btnSetting"] => SendMenuSettingsAsync(botClient, message, cancellationToken),*/
            _ when message.Text == localizer["btnInfo"] => SendInfoAsync(message, cancellationToken),
            { } text when text == localizer["btnFeedback"] => ShowFeedbackAsync(message, cancellationToken),
            { } text when text == localizer["btnPhone"] => SendContactAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task HandleFeedBackAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleFeedBackAsync is working...");

        string userFeedback = message.Text;

        await botClient.SendTextMessageAsync(
            chatId: "@OnlineMarketFeedBack",
            text: $"Yangi Feedback:\n\n{userFeedback}",
            cancellationToken: cancellationToken);

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
           {
                new[] { new KeyboardButton(localizer["btnMainMenu"]) }
            })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtThankFeedback"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        await SendMainMenuAsync(message, cancellationToken);
    }
}
