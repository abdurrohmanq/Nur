using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.Bot.Models.Enums;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.Orders;
using Telegram.Bot.Types.ReplyMarkups;
using Nur.APIService.Models.Products;
using Nur.APIService.Models.Carts;
using Nur.APIService.Models.OrderItems;

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

    private async Task SendProductInputQuantityAsync(Message message, CancellationToken cancellationToken)
    {
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
        new[]
        {
            new KeyboardButton("1"),
            new KeyboardButton("2"),
            new KeyboardButton("3"),
        },
        new[]
        {
            new KeyboardButton("4"),
            new KeyboardButton("5"),
            new KeyboardButton("6"),
        },
        new[]
        {
            new KeyboardButton("7"),
            new KeyboardButton("8"),
            new KeyboardButton("9"),
        },
        new[]
        {
        new KeyboardButton(localizer["btnMainMenu"]),
        new KeyboardButton(localizer["btnBack"]),
        new KeyboardButton(localizer["btnBasket"])
        }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtInputProductQuantity"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForQuantityInput;
    }

    public Dictionary<long, CartDTO> cart = new Dictionary<long, CartDTO>();
    private async Task SendCartAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendCartAsync is working..");

        var cartItems = await cartItemService.GetByCartIdAsync(cart[message.Chat.Id].Id, cancellationToken);
        if (cartItems.Count() > 0)
        {
            var cartItemsText = string.Join("\n\n", cartItems.Select(item => 
            $"{item.Product.Name}: {item.Quantity} x {item.Price} = {item.Sum}"));
            cartItemsText = $"{localizer["btnBasket"]}\n\n" + cartItemsText;

            var additionalButtons = new List<KeyboardButton>
            {
            new KeyboardButton(localizer["btnClearCart"]),
            new KeyboardButton(localizer["btnPlaceOrder"]),
            new KeyboardButton(localizer["btnBack"])
            };

            var allButtons = new List<KeyboardButton[]>();
            var rowButtons = new List<KeyboardButton>();

            foreach (var item in cartItems)
            {
                var button = new KeyboardButton($"❌ {item.Product.Name}");
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

            cartItemsText += $"\n\n {localizer["txtTotalPrice", cart[message.Chat.Id].TotalPrice]}";

            await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: localizer["txtCleanCartInfo"],
               cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: cartItemsText,
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

            userStates[message.Chat.Id] = UserState.WaitingForCartAction;
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCartEmptyInfo"],
                cancellationToken: cancellationToken);
        }
    }

    private async Task RequestProductFromDeleteCartAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleProductFromDeleteCartAsync is working..");
        var productName = message.Text;

        var wasDeleted = await cartItemService.DeleteByProductNameAsync(productName, cancellationToken);
        if (wasDeleted)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtRemovedCartItem", productName],
                cancellationToken: cancellationToken);

            if (cart[message.Chat.Id].CartItems.Count() == 0)
                await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            else
                await SendCartAsync(message, cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtWrongInputProduct"],
                cancellationToken: cancellationToken);
    }

    private async Task RequestCleanCartAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("RequestCleanCartAsync is working...");

        if (cart[message.Chat.Id].CartItems.Count() == 0)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCartEmpty"],
                cancellationToken: cancellationToken);
            return;
        }
        var isTrue = await cartItemService.DeleteAllAsync(cart[message.Chat.Id].Id, cancellationToken);
        if (isTrue)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCartReleased"],
                cancellationToken: cancellationToken);

            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtErrorDeleteCart"],
                cancellationToken: cancellationToken);
    }

    private async Task SendOrderActionAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendOrderActionAsync is working...");

        if (cart[message.Chat.Id].CartItems.Count() > 0)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] { new KeyboardButton(localizer["txtNoComment"])},
            new[] { new KeyboardButton(localizer["btnMainMenu"]), new KeyboardButton(localizer["btnBack"]) }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtWriteCommentForOrder"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

            userStates[message.Chat.Id] = UserState.WaitingForCommentAction;
        }
        else
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtCartEmptyInfo"],
            cancellationToken: cancellationToken);

            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
    }

    public async Task SendOrderToAdminAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendOrderToAdminAsync is working...");

        if (message.Text.Equals(localizer["btnConfirmation"]))
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][] {
                [InlineKeyboardButton.WithCallbackData("Kutish", "btnPending")],
                [InlineKeyboardButton.WithCallbackData("Tayyorlanyapti", "btnPreparing")],
                [InlineKeyboardButton.WithCallbackData("Tayyor yetkazib beriladi", "btnPrepared")],
                [InlineKeyboardButton.WithCallbackData("Yo'lda", "btnOnRoad")],
                [InlineKeyboardButton.WithCallbackData("Yetkazib berildi", "btnDelivered")],
                [InlineKeyboardButton.WithCallbackData("Bekor qilish", "btnCancel")] });

            var createdPayment = await paymentService.AddAsync(payment[message.Chat.Id], cancellationToken);

            createOrder[message.Chat.Id].TotalPrice = payment[message.Chat.Id].Amount;
            createOrder[message.Chat.Id].UserId = user[message.Chat.Id].Id;
            createOrder[message.Chat.Id].PaymentId = createdPayment.Id;
            createOrder[message.Chat.Id].Status = Status.Pending;

            var order = await orderService.AddAsync(createOrder[message.Chat.Id], cancellationToken);
            var cartItems = cart[message.Chat.Id].CartItems;

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItemCreationDTO
                {
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Sum = cartItem.Sum,
                    OrderId = order.Id, 
                    ProductId = cartItem.Product.Id,
                };

                var result =await orderItemService.AddAsync(orderItem, cancellationToken);
            }

            await cartItemService.DeleteAllAsync(cart[message.Chat.Id].Id, cancellationToken);

            string remove = localizer["txtYourOrder"];
            int startIndex = orderText[message.Chat.Id].IndexOf(remove);

            if (startIndex != -1)
            {
                string result = orderText[message.Chat.Id].Substring(0, startIndex) +
                    orderText[message.Chat.Id].Substring(startIndex + remove.Length);
                orderText[message.Chat.Id] = result;
            }
            var orderMessage = await botClient.SendTextMessageAsync(
                chatId: "@NurOrders",
                text: orderText[message.Chat.Id],
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);

            if(order.Address.Id != 0)
            {
                await botClient.SendTextMessageAsync(
                chatId: "@NurOrders",
                text: "lokatsiya 👇",
                replyToMessageId: orderMessage.MessageId,
                cancellationToken: cancellationToken);

                await botClient.SendLocationAsync(
                chatId: "@NurOrders",
                latitude: order.Address.Latitude,
                longitude: order.Address.Longitude,
                cancellationToken: cancellationToken);
            }

        }
        else if (message.Text.Equals(localizer["btnCancel"]))
        {
            await HandleDescriptionAsync(message, cancellationToken);
        }
    }
}
