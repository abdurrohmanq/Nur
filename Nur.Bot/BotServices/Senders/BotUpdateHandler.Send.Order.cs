﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.Bot.Models.Enums;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.Orders;
using Telegram.Bot.Types.ReplyMarkups;
using Nur.APIService.Models.Products;

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

    private async Task SendTakeAwayAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendTakeAwayAsync is working..");

        createOrder[message.Chat.Id] = new OrderCreationDTO();
        createOrder[message.Chat.Id].OrderType = OrderType.TakeAway;

        await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
    }

    private async Task SendCategoryKeyboardAsync(long chatId, CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);

        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton(localizer["btnBack"]),
        new KeyboardButton(localizer["btnPlaceOrder"]),
        new KeyboardButton(localizer["btnBasket"])
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var category in categories)
        {
            var button = new KeyboardButton(category.Name);
            rowButtons.Add(button);

            if (rowButtons.Count == 2)
            {
                allButtons.Add(rowButtons.ToArray());
                rowButtons.Clear();
            }
        }

        if (rowButtons.Any())
        {
            allButtons.Add(rowButtons.ToArray());
        }

        allButtons.Add(additionalButtons.ToArray());

        var replyKeyboard = new ReplyKeyboardMarkup(allButtons) { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: localizer["txtSelectCategory"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForCategorySelection;
    }

    private async Task SendProductsKeyboardAsync(long chatId, IEnumerable<ProductResultDTO> products, CancellationToken cancellationToken)
    {
        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton(localizer["btnMainMenu"]),
        new KeyboardButton(localizer["btnBack"]),
        new KeyboardButton(localizer["btnBasket"])
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var product in products)
        {
            var button = new KeyboardButton(product.Name);
            rowButtons.Add(button);

            if (rowButtons.Count == 2)
            {
                allButtons.Add(rowButtons.ToArray());
                rowButtons.Clear();
            }
        }

        if (rowButtons.Any())
        {
            allButtons.Add(rowButtons.ToArray());
        }

        allButtons.Add(additionalButtons.ToArray());

        var replyKeyboard = new ReplyKeyboardMarkup(allButtons) { ResizeKeyboard = true };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: localizer["txtSelectProduct"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[chatId] = UserState.WaitingForProductSelection;
    }
}
