using Nur.APIService.Models.Cafes;
using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    public Dictionary<long, CafeDTO> cafe = new Dictionary<long, CafeDTO>();
    private async Task SendEnterCalmAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEnterCalmAsync is working..");

        ArgumentNullException.ThrowIfNull(message);

        if (cafe[message.Chat.Id] is null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtEmptyCafeInfo"],
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtSetCafePassword"],
                cancellationToken: cancellationToken);

            commonAdminStates[message.Chat.Id] = CommonAdminState.CreateCafe;
            adminStates[message.Chat.Id] = AdminState.WaitingForCafePassword;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCafePassword"],
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForCafePassword;
        }
    }

    private async Task AdminSendMainMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminSendMainMenuAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnCategory"]), new KeyboardButton(localizer["btnProduct"]) },
            new[] { new KeyboardButton(localizer["btnEditInfo"]),  new KeyboardButton(localizer["btnEditPhone"]) },
            new[] { new KeyboardButton(localizer["btnOrdersList"])}
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["AdminOpenMenu"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForSelectMainMenu;
    }

    private async Task SendEditCafeInfoPartsAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCafeInfoPartsAsync is working");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnEditCafeInstagramLink"]), new KeyboardButton(localizer["btnEditCafeFacebookLink"]) },
            new[] { new KeyboardButton(localizer["btnBack"])},
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCafeEditInfoSelect"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForSelectCafeInfoEdit;
    }

    private async Task AdminHandleCafeInfoEditAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleCafeInfoEditAsync is working..");

        var handle = message.Text switch
        {
            { } text when text == localizer["btnEditCafeFacebookLink"] => SendEditCafeInfoFacebookLinkQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditCafeInstagramLink"] => SendEditCafeInfoInstaLinkQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => AdminSendMainMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task SendEditCafeInfoInstaLinkQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCafeInfoInstaLinkQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCurrentInstaLink", cafe[message.Chat.Id].InstagramLink],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCafeInstaLinkRequest"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForInstagramLink;
    }
    
    private async Task SendEditCafeInfoFacebookLinkQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCafeInfoFacebookLinkQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCurrentFacebookLink", cafe[message.Chat.Id].FacebookLink],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCafeFacebookLinkRequest"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForFacebookLink;
    }
    
    private async Task SendEditCafePhoneQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCafePhoneQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCurrentPhone", cafe[message.Chat.Id].Phone],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCafePhone"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForCafePhone;
    }
}
