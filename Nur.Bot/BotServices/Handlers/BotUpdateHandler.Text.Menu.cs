using Telegram.Bot;
using Telegram.Bot.Types;
using System.Globalization;
using Nur.APIService.Helpers;
using Telegram.Bot.Types.ReplyMarkups;
using Nur.Bot.Models.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleMainMenuAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text == localizer["btnOrder"] => SendOrderTypeAsync(message, cancellationToken),
            { } text when text == localizer["btnSetting"] => SendMenuSettingsAsync(message, cancellationToken),
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

        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendMainMenuAsync(message, cancellationToken);
        }
        else
        {
            string userFeedback = message.Text;

            await botClient.SendTextMessageAsync(
                chatId: "@NurFeedBack",
                text: $"Yangi Fikr-mulohaza:\n\n{userFeedback}",
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtThankFeedback"],
                cancellationToken: cancellationToken);

            await SendMainMenuAsync(message, cancellationToken);
        }
    }

    private async Task HandleSelectedSettingsAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text == localizer["btnEditLanguage"] => SendSelectLanguageQueryAsync(botClient, message, cancellationToken),
            { } text when text == localizer["btnEditPersonalInfo"] => SendMenuEditPersonalInfoAsync(botClient, message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendMainMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task HandleSelectedPersonalInfoAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text == localizer["btnPhoneNumber"] => SendRequestForPhoneNumberAsync(message, cancellationToken),
            { } text when text == localizer["btnFullName"] => SendRequestForFullNameAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendMenuSettingsAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task HandlePhoneNumberAsync(Message message, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(message.Text);

            if (message.Text.Equals(localizer["btnCancel"]))
            {
                await SendMenuSettingsAsync(message, cancellationToken); return;
            }
            else
            {
                if (PhoneValidation.ValidatePhoneNumber(message.Text))
                {
                    user[message.Chat.Id].Phone = message.Text;

                    await userService.UpdateAsync(user[message.Chat.Id], cancellationToken);

                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["GetContact"],
                    cancellationToken: cancellationToken);
                    if (commonUserStates[message.Chat.Id] == CommonUserState.CreatingUser)
                    {
                        await botClient.SendTextMessageAsync(
                              chatId: message.Chat.Id,
                              text: localizer["RequestForFullName"],
                              cancellationToken: cancellationToken);

                        userStates[message.Chat.Id] = UserState.WaitingForFullName;
                        commonUserStates[message.Chat.Id] = CommonUserState.None;
                    }
                    else
                        await SendMainMenuAsync(message, cancellationToken);
                }
                else
                {
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[] { new KeyboardButton(localizer["btnMainMenu"]) }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtPhoneError"],
                    cancellationToken: cancellationToken);
                }
            }
        }

        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task HandleSentLanguageAsync(Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(message.Text);
        if (!message.Text.Equals(localizer["btnBack"]))
        {
            user[message.Chat.Id].LanguageCode = message.Text switch
            {
                { } text when text == localizer["ibtnEn"] => "en",
                { } text when text == localizer["ibtnRu"] => "ru",
                _ => "uz"
            };

            await userService.UpdateAsync(user[message.Chat.Id], cancellationToken);

            var culture = new CultureInfo(user[message.Chat.Id].LanguageCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        await SendMenuSettingsAsync(message, cancellationToken);
    }
}
