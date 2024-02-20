using Nur.APIService.Models.CartItems;
using Nur.APIService.Models.Enums;
using Nur.APIService.Models.Payments;
using Nur.APIService.Models.Products;
using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleOrderTypeAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text == localizer["btnDelivery"] => SendDeliveryAsync(message, cancellationToken),
            { } text when text == localizer["btnTakeAway"] => SendTakeAwayAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendMainMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }
    private Dictionary<long, IEnumerable<ProductResultDTO>> products = new Dictionary<long, IEnumerable<ProductResultDTO>>();

    private async Task HandleCategorySelectionAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendOrderTypeAsync(message, cancellationToken);
            return;
        }

        var selectedCategoryName = message.Text;

        products[message.Chat.Id] = await productService.GetByCategoryNameAsync(selectedCategoryName, cancellationToken);
        if (products[message.Chat.Id].Count() == 0)
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtEmptyCategory"],
            cancellationToken: cancellationToken);

            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
        {
            await SendProductsKeyboardAsync(message.Chat.Id, products[message.Chat.Id], cancellationToken);
        }
    }

    private Dictionary<long, ProductResultDTO> selectedProduct = new Dictionary<long, ProductResultDTO>();

    private async Task HandleProductSelectionAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            return;
        }

        var selectedProductName = message.Text;

        var product = await productService.GetByProductNameAsync(selectedProductName, cancellationToken);
        if (product is null)
        {
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtNotFoundProduct"],
            cancellationToken: cancellationToken);
        }
        else
        {
            using (var httpClient = new HttpClient())
            {
                var url = product.Attachment.FilePath;
                var imageData = await httpClient.GetByteArrayAsync(url);
                using (var memoryStream = new MemoryStream(imageData))
                {
                    var inputFile = InputFileStream.FromStream(memoryStream, "image.jpg");
                    await botClient.SendPhotoAsync(
                          chatId: message.Chat.Id,
                          photo: inputFile,
                          caption: $" *{product.Name}*\n\n{product.Description}\n\n" +
                          $"{localizer["txtProductPrice", product.Price]}",
                          parseMode: ParseMode.Markdown,
                          cancellationToken: cancellationToken);
                }
            }

            selectedProduct[message.Chat.Id] = product;
            await SendProductInputQuantityAsync(message, cancellationToken);
        }
    }

    private async Task HandleQuantityInputAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductsKeyboardAsync(message.Chat.Id, products[message.Chat.Id], cancellationToken);
            return;
        }

        if (int.TryParse(message.Text, out int quantity) && quantity > 0)
        {
            var product = selectedProduct[message.Chat.Id];
            cart[message.Chat.Id] = await cartService.GetByUserIdAsync(user[message.Chat.Id].Id, cancellationToken);
            var cartItem = await cartItemService.GetByProductIdAsync(product.Id, cancellationToken);

            if (cartItem is null)
            {
                var cartItemCreate = new CartItemCreationDTO
                {
                    Quantity = quantity,
                    ProductId = product.Id,
                    CartId = cart[message.Chat.Id].Id,
                    Price = product.Price
                };

                cartItem = await cartItemService.AddAsync(cartItemCreate, cancellationToken);
            }
            else
            {
                var cartItemUpdate = new CartItemUpdateDTO
                {
                    Id = cartItem.Id,
                    Quantity = cartItem.Quantity + quantity,
                    ProductId = product.Id,
                    CartId = cart[message.Chat.Id].Id,
                    Price = cartItem.Price,
                };

                cartItem = await cartItemService.UpdateAsync(cartItemUpdate, cancellationToken);
            }

            if (cartItem is not null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtAddedProductInCart", quantity, product.Name],
                    cancellationToken: cancellationToken);

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtContinueOrder"],
                    cancellationToken: cancellationToken);

                await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtNotEnoughProduct"],
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtWrongQuantity"],
                cancellationToken: cancellationToken);

            await SendProductInputQuantityAsync(message, cancellationToken);
        }
    }

    private async Task HandleCartActionAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text.Contains("❌") => RequestProductFromDeleteCartAsync(message, cancellationToken),
            { } text when text == localizer["btnClearCart"] => RequestCleanCartAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex)
        {
            logger.LogError(ex,
            "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName);
        }
    }

    private async Task HandleDescriptionAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleDescriptionAsync is working...");
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductsKeyboardAsync(message.Chat.Id, products[message.Chat.Id], cancellationToken);
        }
        else
        {
            createOrder[message.Chat.Id].Description = message.Text;

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
            new[] { new KeyboardButton(localizer["btnPaymentTypeCash"]) },
            new[] { new KeyboardButton(localizer["btnPaymentTypePayme"]),
                    new KeyboardButton(localizer["btnPaymentTypeClick"]) },
            new[] { new KeyboardButton(localizer["btnMainMenu"]), new KeyboardButton(localizer["btnBack"]) }
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: localizer["txtSelectPaymentType"],
               replyMarkup: replyKeyboard,
               cancellationToken: cancellationToken);

            userStates[message.Chat.Id] = UserState.WaitingForPaymentTypeAction;
        }
    }

    private Dictionary<long, PaymentCreationDTO> payment = new Dictionary<long, PaymentCreationDTO>();
    private Dictionary<long, string> orderText = new Dictionary<long, string>();
    private async Task HandlePaymentMethodAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
        {
            payment[message.Chat.Id] = new PaymentCreationDTO();
            string paymentType = string.Empty;
            if (message.Text.Equals(localizer["btnPaymentTypeCash"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Cash;
                paymentType = localizer["btnPaymentTypeCash"];
            }
            else if (message.Text.Equals(localizer["btnPaymentTypePayme"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Payme;
                paymentType = localizer["btnPaymentTypePayme"];
            }
            else if (message.Text.Equals(localizer["btnPaymentTypeClick"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Click;
                paymentType = localizer["btnPaymentTypeClick"];
            }

            var cartItems = await cartItemService.GetByCartIdAsync(cart[message.Chat.Id].Id, cancellationToken);

            var cartItemsText = string.Join("\n\n", cartItems.Select(item =>
            $"{item.Product.Name}: {item.Quantity} x {item.Price} = {item.Sum}"));
            cartItemsText = $"{localizer["btnBasket"]}\n\n" + cartItemsText;
            string orderType = createOrder[message.Chat.Id].OrderType == OrderType.Delivery ?
                localizer["btnDelivery"] : localizer["btnTakeAway"];

            cartItemsText = $"{localizer["txtYourOrder"]}\n\n {localizer["txtOrderType"]} {orderType}\n\n " +
                $"{localizer["txtPhone"]} {user[message.Chat.Id].Phone}\n\n {localizer["txtPaymentType"]} " +
                $"{paymentType}\n\n {localizer["txtComments"]} {createOrder[message.Chat.Id].Description}\n\n" + cartItemsText;
            cartItemsText += $"\n\n {localizer["txtTotalPrice", cart[message.Chat.Id].TotalPrice]}";

            payment[message.Chat.Id].Amount = cart[message.Chat.Id].TotalPrice;
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
            new[] {new KeyboardButton(localizer["btnConfirmation"]) },
            new[] {new KeyboardButton(localizer["btnCancel"]) }
        })
            {
                ResizeKeyboard = true
            };

            userStates[message.Chat.Id] = UserState.WaitingForOrderSendToAdminAction;


            await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: cartItemsText,
                        replyMarkup: replyKeyboard,
                        cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: localizer["txtTakeAwayLocation"],
                        replyMarkup: replyKeyboard,
                        cancellationToken: cancellationToken);

            await botClient.SendLocationAsync(
                chatId: message.Chat.Id,
                latitude: 40.72803040927073,
                longitude: 72.31086991903557,
                cancellationToken: cancellationToken);

            orderText[message.Chat.Id] = cartItemsText;
        }
    }
}
