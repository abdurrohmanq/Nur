using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.APIService.Models.Addresses;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, AddressCreationDTO> createAddress = new Dictionary<long, AddressCreationDTO>();

    private async Task HandleLocationAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleLocationAsync is working..");

        createAddress[message.Chat.Id].Latitude = message.Location.Latitude;
        createAddress[message.Chat.Id].Longitude += message.Location.Longitude;



        if (createAddress[message.Chat.Id] != null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtReceiveLocation"],
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtNotReceiveLocation"],
                cancellationToken: cancellationToken);
        }
    }
}
