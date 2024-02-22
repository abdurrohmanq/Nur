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
}
