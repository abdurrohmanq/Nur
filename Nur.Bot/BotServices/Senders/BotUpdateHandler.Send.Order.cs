using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.Bot.Models.Enums;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.Orders;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, OrderCreationDTO> createOrder = new Dictionary<long, OrderCreationDTO>();

    private async Task SendDeliveryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendDeliveryAsync is working..");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
         {
             new KeyboardButton(localizer["txtSendLocation"]) { RequestLocation = true }
         },
        new[] { new KeyboardButton(localizer["btnBack"]) }
        })
        {
            ResizeKeyboard = true
        };

        createOrder[message.Chat.Id] = new OrderCreationDTO();
        createOrder[message.Chat.Id].OrderType = OrderType.Delivery;

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtRequestForLocation"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForHandleTextLocation;
    }
}
