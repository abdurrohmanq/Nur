using Nur.APIService.Models.Cafes;
using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    public Dictionary<long, CafeDTO> cafe = new Dictionary<long, CafeDTO>();
    private async Task SendEnterCalmAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEnterCalmAsync is working..");

        ArgumentNullException.ThrowIfNull(message);

        cafe[message.Chat.Id] = (await cafeService.GetAsync(cancellationToken)).FirstOrDefault();

        if (cafe is null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtEmptyCafe"],
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInstagramLink"],
                cancellationToken: cancellationToken);

            commonAdminStates[message.Chat.Id] = CommonAdminState.CreateCafe;
            adminStates[message.Chat.Id] = AdminState.WaitingForInstagramLink;
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
}
