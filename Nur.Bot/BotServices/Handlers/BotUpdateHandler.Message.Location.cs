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
        createAddress[message.Chat.Id] = new AddressCreationDTO()
        {
            City = "Andijan",
            State = "Andijan State",
            Street = "Ahmad Street",
            DoorCode = "212",
            Latitude = message.Location.Latitude,
            Longitude = message.Location.Longitude,
        };

        var result = await addressService.AddAsync(createAddress[message.Chat.Id], cancellationToken);

        if (result != null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtReceiveLocation"],
                cancellationToken: cancellationToken);

            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);    
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtNotReceiveLocation"],
                cancellationToken: cancellationToken);
        }
    }

    private async Task HandleTextLocationAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleTextLocationAsync is working..");

        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendOrderTypeAsync(message, cancellationToken);
            return;
        }

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtTextLocation"],
            cancellationToken: cancellationToken);

        await SendDeliveryAsync(message, cancellationToken);
    }
}
