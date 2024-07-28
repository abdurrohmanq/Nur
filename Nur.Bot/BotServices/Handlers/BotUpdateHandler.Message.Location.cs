using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.APIService.Models.Addresses;
using System.Device.Location;
using Nur.Bot.BotServices.Location;
using System.Globalization;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, AddressCreationDTO> createAddress = new Dictionary<long, AddressCreationDTO>();
    private static readonly GeoCoordinate RestaurantLocation = new GeoCoordinate(40.72803040927073, 72.31086991903557);
    private readonly LocationService _locationService = new ("AIzaSyBEZdqr_QqybVZram2K0M8J-J83Wr4ftno");

    private async Task HandleLocationAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleLocationAsync is working..");

        var userLocation = new GeoCoordinate(message.Location.Latitude, message.Location.Longitude);

        var locationInfo = await _locationService.GetLocationInfoAsync(userLocation.Latitude, userLocation.Longitude);

        if (locationInfo == null)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Unable to retrieve location information.",
                cancellationToken: cancellationToken);
            return;
        }

        var addressComponents = locationInfo.Results.First().AddressComponents;
        var city = addressComponents.FirstOrDefault(c => c.Types.Contains("locality"))?.LongName;
        var state = addressComponents.FirstOrDefault(c => c.Types.Contains("administrative_area_level_1"))?.LongName;
        var street = addressComponents.FirstOrDefault(c => c.Types.Contains("route"))?.LongName;
        var doorCode = addressComponents.FirstOrDefault(c => c.Types.Contains("street_number"))?.LongName;

        if (city != "Andijan" || !state.Contains("Andijan region"))
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtMustWithinAndijan"],
                cancellationToken: cancellationToken);
            return;
        }

        createAddress[message.Chat.Id] = new AddressCreationDTO()
        {
            City = city,
            State = state,
            Street = street ?? localizer["txtUnknownStreet"],
            DoorCode = doorCode ?? localizer["txtUnknownDoorCode"],
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

            createOrder[message.Chat.Id].AddressId = result.Id;

            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtNotReceiveLocation"],
                cancellationToken: cancellationToken);
        }

        string distanceText = await _locationService.GetDrivingDistanceAsync(
            originLatitude: RestaurantLocation.Latitude,
            originLongitude: RestaurantLocation.Longitude,
            destinationLatitude: userLocation.Latitude,
            destinationLongitude: userLocation.Longitude);

        var distanceNumericPart = distanceText.Replace(" km", "").Trim();

        if (!double.TryParse(distanceNumericPart, NumberStyles.Float, CultureInfo.InvariantCulture, out double distanceInKm))
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtUnableParseDestination"],
                cancellationToken: cancellationToken);
            return;
        }

        decimal deliveryFee = 0;

        if (distanceInKm > 6)
        {
            deliveryFee = (decimal)distanceInKm * 1500;
            createOrder[message.Chat.Id].DeliveryFee = decimal.Truncate(deliveryFee);
        }

        string response = localizer["txtDistanceLocation", distanceText];
        if (deliveryFee > 0)
        {
            response += localizer["txtDeliveryInfo", deliveryFee];
        }

        await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: response,
               cancellationToken: cancellationToken);
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
