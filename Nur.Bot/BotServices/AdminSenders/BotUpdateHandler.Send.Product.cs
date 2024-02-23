using Nur.Bot.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.APIService.Models.Products;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task SendProductMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendProductMenuAsync is working");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnAddProduct"]), new KeyboardButton(localizer["btnDeleteProduct"]) },
            new[] { new KeyboardButton(localizer["btnEditProduct"]),  new KeyboardButton(localizer["btnGetProductInfo"]) },
            new[] { new KeyboardButton(localizer["btnBack"])},
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCategoryMenu"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.None;
        adminStates[message.Chat.Id] = AdminState.WaitingForSelectProductMenu;
    }

    private async Task SendAddProductQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendAddProductQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestName"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductName;
    }

    private async Task SendEditProductPartsAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductPartsAsync is working");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnEditProductName"]), new KeyboardButton(localizer["btnEditProductPrice"]) },
            new[] { new KeyboardButton(localizer["btnEditProductQuantity"]), new KeyboardButton(localizer["btnEditProductDesc"]) },
            new[] { new KeyboardButton(localizer["btnEditProductCategory"]), new KeyboardButton(localizer["btnEditProductPhoto"]) },
            new[] { new KeyboardButton(localizer["btnBack"])},
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductEditSelect"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.None;
        adminStates[message.Chat.Id] = AdminState.WaitingForSelectProductEdit;
    }

    private async Task AdminSendProductsKeyboardAsync(long chatId, IEnumerable<ProductResultDTO> products, CancellationToken cancellationToken)
    {
        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton(localizer["btnBack"]),
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

        adminStates[chatId] = AdminState.WaitingForProductSelection;
    }

    private async Task AdminSendAllProductsKeyboardAsync(long chatId, CancellationToken cancellationToken)
    {
        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton(localizer["btnBack"]),
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        var products = await productService.GetAllAsync(cancellationToken);
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

        adminStates[chatId] = AdminState.WaitingForGetProductSelection;
    }

    private async Task SendEditProductNameQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductNameQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestName"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductName;
    }
    
    private async Task SendEditProductQuantityQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductQuantityQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnAsterisk"]) },
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestQuantity", selectedProduct[message.Chat.Id].Name],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductQuantity;
    }
    
    private async Task SendEditProductDescQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductDescQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestDesc"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductDesc;
    }
    
    private async Task SendEditProductPriceQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductPriceQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestPrice"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductPrice;
    }
    
    private async Task SendEditProductCategoryQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductCategoryQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCurrentCategory", category[message.Chat.Id].Name],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.EditCategoryForProduct;
        await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelectionForProduct;
    }

    private async Task SendEditProductPhotoQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditProductPhotoQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtProductRequestPhoto"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputProductPhoto;
    }
}
