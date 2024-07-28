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

        Dictionary<long, string> selectedCategoryName = new Dictionary<long, string>();

        selectedCategoryName[message.Chat.Id] = message.Text;

        products[message.Chat.Id] = await productService.GetByCategoryNameAsync(selectedCategoryName[message.Chat.Id], cancellationToken);
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

        Dictionary<long, string> selectedProductName = new Dictionary<long, string>();
        selectedProductName[message.Chat.Id] = message.Text;

        Dictionary<long, ProductResultDTO> product = new Dictionary<long, ProductResultDTO>();

        product[message.Chat.Id] = await productService.GetByProductNameAsync(selectedProductName[message.Chat.Id], cancellationToken);
        if (product[message.Chat.Id] is null)
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
                var url = product[message.Chat.Id].Attachment.FilePath;
                var imageData = await httpClient.GetByteArrayAsync(url);
                using (var memoryStream = new MemoryStream(imageData))
                {
                    var inputFile = InputFileStream.FromStream(memoryStream, "image.jpg");
                    await botClient.SendPhotoAsync(
                          chatId: message.Chat.Id,
                          photo: inputFile,
                          caption: $" *{product[message.Chat.Id].Name}*\n\n{product[message.Chat.Id].Description}\n\n" +
                          $"{localizer["txtProductPrice", product[message.Chat.Id].Price]}",
                          parseMode: ParseMode.Markdown,
                          cancellationToken: cancellationToken);
                }
            }

            selectedProduct[message.Chat.Id] = product[message.Chat.Id];
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
            cart[message.Chat.Id] = await cartService.GetByUserIdAsync(user[message.Chat.Id].Id, cancellationToken);
            Dictionary<long, CartItemResultDTO> cartItem = new Dictionary<long, CartItemResultDTO>();
            cartItem[message.Chat.Id] = await cartItemService.GetByProductIdAsync(selectedProduct[message.Chat.Id].Id, cancellationToken);

            if (cartItem[message.Chat.Id] is null)
            {
                var cartItemCreate = new CartItemCreationDTO
                {
                    Quantity = quantity,
                    ProductId = selectedProduct[message.Chat.Id].Id,
                    CartId = cart[message.Chat.Id].Id,
                    Price = selectedProduct[message.Chat.Id].Price
                };

                cartItem[message.Chat.Id] = await cartItemService.AddAsync(cartItemCreate, cancellationToken);
            }
            else
            {
                var cartItemUpdate = new CartItemUpdateDTO
                {
                    Id = cartItem[message.Chat.Id].Id,
                    Quantity = cartItem[message.Chat.Id].Quantity + quantity,
                    ProductId = selectedProduct[message.Chat.Id].Id,
                    CartId = cart[message.Chat.Id].Id,
                    Price = cartItem[message.Chat.Id].Price,
                };

                cartItem[message.Chat.Id] = await cartItemService.UpdateAsync(cartItemUpdate, cancellationToken);
            }

            if (cartItem[message.Chat.Id] is not null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtAddedProductInCart", quantity, selectedProduct[message.Chat.Id].Name],
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
            Dictionary<long, string> paymentType = new Dictionary<long, string>();

            if (message.Text.Equals(localizer["btnPaymentTypeCash"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Cash;
                paymentType[message.Chat.Id] = localizer["btnPaymentTypeCash"];
            }
            else if (message.Text.Equals(localizer["btnPaymentTypePayme"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Payme;
                paymentType[message.Chat.Id] = localizer["btnPaymentTypePayme"];
            }
            else if (message.Text.Equals(localizer["btnPaymentTypeClick"]))
            {
                payment[message.Chat.Id].Type = PaymentType.Click;
                paymentType[message.Chat.Id] = localizer["btnPaymentTypeClick"];
            }

            Dictionary<long, IEnumerable<CartItemResultDTO>> cartItems = new Dictionary<long, IEnumerable<CartItemResultDTO>>();
            cartItems[message.Chat.Id] = await cartItemService.GetByCartIdAsync(cart[message.Chat.Id].Id, cancellationToken);

            orderText[message.Chat.Id] = string.Join("\n\n", cartItems[message.Chat.Id].Select(item =>
            $"{item.Product.Name}: {item.Quantity} x {item.Price} = {item.Sum}"));
            orderText[message.Chat.Id] = $"{localizer["btnBasket"]}\n\n" + orderText[message.Chat.Id];
            string orderType = createOrder[message.Chat.Id].OrderType == OrderType.Delivery ?
                localizer["btnDelivery"] : localizer["btnTakeAway"];

            orderText[message.Chat.Id] = $"{localizer["txtYourOrder"]}\n\n {localizer["txtOrderType"]} {orderType}\n\n " +
                $"{localizer["txtPhone"]} {user[message.Chat.Id].Phone}\n\n {localizer["txtFullName", user[message.Chat.Id].FullName]}\n\n " +
                $" {localizer["txtPaymentType"]} " +
                $"{paymentType[message.Chat.Id]}\n\n {localizer["txtComments"]} {createOrder[message.Chat.Id].Description}\n\n" + orderText[message.Chat.Id];
            orderText[message.Chat.Id] += $"\n\n {localizer["txtDeliveryInfo", createOrder[message.Chat.Id].DeliveryFee]}";
            cart[message.Chat.Id].TotalPrice += (decimal)createOrder[message.Chat.Id].DeliveryFee;
            orderText[message.Chat.Id] += $"\n\n {localizer["txtTotalPrice", cart[message.Chat.Id].TotalPrice]}";

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
                        text: orderText[message.Chat.Id],
                        replyMarkup: replyKeyboard,
                        cancellationToken: cancellationToken);

            if (createOrder[message.Chat.Id].OrderType == OrderType.TakeAway)
            {

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
            }
        }
    }
}
