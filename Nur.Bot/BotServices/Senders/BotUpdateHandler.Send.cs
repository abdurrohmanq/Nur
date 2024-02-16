using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.Bot.Models.Enums;
using Nur.APIService.Constants;
using Telegram.Bot.Types.ReplyMarkups;

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

        userStates[message.Chat.Id] = UserState.None;
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

    private async Task SendOrderTypeAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendOrderTypeAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnDelivery"]),  new KeyboardButton(localizer["btnTakeAway"]) },
            new[] { new KeyboardButton(localizer["btnBack"]) }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtRequestToOrderType"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForSelectOrderType;
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

    private async Task SendMenuSettingsAsync(Message message, CancellationToken cancellationToken)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
       {
        new[] { new KeyboardButton(localizer["btnEditPersonalInfo"]), new KeyboardButton(localizer["btnEditLanguage"]) },
        new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtSettings"],
            replyMarkup: keyboard,
            cancellationToken: cancellationToken
        );

        userStates[message.Chat.Id] = UserState.WaitingForSelectSettings;
    }

    private async Task SendMenuEditPersonalInfoAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            [new(localizer["btnPhoneNumber"]), new(localizer["btnFullName"])],
            [new(localizer["btnBack"])]
        })
        { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtMenuPersonalInfo"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken
        );

        userStates[message.Chat.Id] = UserState.WaitingForSelectPersonalInfo;
    }

    private async Task SendRequestForPhoneNumberAsync(Message message, CancellationToken cancellationToken)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            [new(localizer["btnRequestContact"]) { RequestContact = true }],
            [new(localizer["btnCancel"])]
        })
        { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["RequestToContact"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken
        );

        userStates[message.Chat.Id] = UserState.WaitingForEnterPhoneNumber;
    }

    private async Task SendRequestForFullNameAsync(Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtCurrentFullName", user[message.Chat.Id].FullName],
            cancellationToken: cancellationToken);

        var replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            [new(localizer["btnCancel"])]
        })
        { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtNewFullName"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForEnterFullName;
    }

    private async Task SendSelectLanguageQueryAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            [new(localizer["ibtnUz"])],
            [new(localizer["ibtnRu"])],
            [new(localizer["ibtnEn"])],
            [new(localizer["btnBack"])]
        })
        { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["choose-language"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForSelectLanguage;
    }
}
