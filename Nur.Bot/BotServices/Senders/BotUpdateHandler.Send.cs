using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Nur.APIService.Constants;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    public async Task SendGreetingAsync(Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (user[message.Chat.Id].CreatedAt.AddSeconds(5) > DateTime.UtcNow.AddHours(TimeConstant.UTC))
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][] {
                [InlineKeyboardButton.WithCallbackData("🇺🇿 o'zbekcha 🇺🇿", "ibtnUz")],
                [InlineKeyboardButton.WithCallbackData("🇬🇧 english 🇬🇧", "ibtnEn")],
                [InlineKeyboardButton.WithCallbackData("🇷🇺 русский 🇷🇺", "ibtnRu")] });

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["greeting", user[message.Chat.Id].FirstName],
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: localizer["choose-language"],
               replyMarkup: keyboard,
               cancellationToken: cancellationToken);

            userStates[message.Chat.Id] = UserState.WaitingForSelectLanguage;
        }
        else
            await SendMainMenuAsync(message, cancellationToken);
    }

    private async Task RequestForContactAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("RequestForContactAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton(localizer["btnRequestContact"]) {RequestContact = true }
        }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["RequestToContact"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task SendMainMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendMainMenuAsync is working..");

        await botClient.SendTextMessageAsync(
           chatId: message.Chat.Id,
           text: localizer["OpenToMenu"],
           cancellationToken: cancellationToken);

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnOrder"]) },
            new[] { new KeyboardButton(localizer["btnFeedback"]),  new KeyboardButton(localizer["btnPhone"]) },
            new[] { new KeyboardButton(localizer["btnInfo"]), new KeyboardButton(localizer["btnSetting"])}
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["select-menu"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForSelectMainMenu;
    }

    private async Task SendContactAsync(Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtContactInfo"],
            cancellationToken: cancellationToken
        );

        await botClient.SendLocationAsync(
            chatId: message.Chat.Id,
            latitude: 41.31255776545841,
            longitude: 69.24048566441775,
            cancellationToken: cancellationToken);
    }

    private async Task ShowFeedbackAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("ShowFeedbackAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[] { new KeyboardButton(localizer["txtVeryGood"]) },
        new[] { new KeyboardButton(localizer["txtGood"]), new KeyboardButton(localizer["txtBad"]) },
        new[] { new KeyboardButton(localizer["txtDislike"]),new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };


        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtFeedback"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForHandlerFeedback;
    }

    private async Task SendInfoAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendInfoAsync is working..");

        await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInfo"],
                cancellationToken: cancellationToken);
    }
}
