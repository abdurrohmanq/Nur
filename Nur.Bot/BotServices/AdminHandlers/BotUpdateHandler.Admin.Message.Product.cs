using Nur.APIService.Models.Enums;
using Nur.APIService.Models.ProductCategories;
using Nur.APIService.Models.Products;
using Nur.Bot.Models.Enums;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task AdminHandleProductMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleProductMenuAsync is working..");

        if (message.Text.Equals(localizer["btnAddProduct"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.CreateProduct;
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelectionForProduct;
        }
        else if (message.Text.Equals(localizer["btnEditCategory"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateProduct;
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            adminStates[message.Chat.Id] = AdminState.WaitingForCategorySelectionForProduct;
        }
        else if (message.Text.Equals(localizer["btnDeleteCategory"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.DeleteProduct;
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else if (message.Text.Equals(localizer["btnGetProductInfo"]))
        {
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }
        else
            await HandleUnknownMessageAsync(botClient, message, cancellationToken);
    }

    Dictionary<long, ProductResultDTO> product = new Dictionary<long, ProductResultDTO>();
    private async Task AdminHandleCategorySelectionForProductAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            await SendProductMenuAsync(message, cancellationToken);
            return;
        }

        Dictionary<long, string> selectedCategoryName = new Dictionary<long, string>();

        selectedCategoryName[message.Chat.Id] = message.Text;

        category[message.Chat.Id] = await categoryService.GetByNameAsync(selectedCategoryName[message.Chat.Id], cancellationToken);
        if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.CreateProduct))
        {
            await SendAddProductQueryAsync(message, cancellationToken);
        }
        else if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.UpdateCategory))
            await AdminSendProductsKeyboardAsync(message.Chat.Id, category[message.Chat.Id].Products, cancellationToken);
    }

    private async Task AdminHandleProductSelectionAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
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
            if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateProduct)
                await SendEditProductPartsAsync(message, cancellationToken);
        }
    }

    private async Task AdminHandleProductEditAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleProductEditAsync is working..");

        var handle = message.Text switch
        {
            { } text when text == localizer["btnEditProductName"] => SendEditCategoryNameQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditProductPrice"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditProductQuantity"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditProductDesc"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditProductCategory"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditProductPhoto"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendCategoryMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    public Dictionary<long, ProductCreationDTO> createProduct = new Dictionary<long, ProductCreationDTO>();
    private async Task AdminHandleProductNameAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }

        logger.LogInformation("AdminHandleProductNameAsync is working..");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
         new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        var name = message.Text;
        var result = await productService.GetByProductNameAsync(name, cancellationToken);
        if (result is null)
        {
            if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateProduct)
            {
                createProduct[message.Chat.Id] = new ProductCreationDTO { Name = name };
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtProductRequestPrice"],
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForInputProductPrice;
            }
            else if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateProduct)
            {
                var updateProduct = new ProductUpdateDTO
                {
                    Id = product[message.Chat.Id].Id,
                    Name = name,
                    Price = product[message.Chat.Id].Price,
                    Quantity = product[message.Chat.Id].Quantity,
                    Description = product[message.Chat.Id].Description,
                    Unit = product[message.Chat.Id].Unit,
                    CategoryId = product[message.Chat.Id].Category.Id,
                };
                await productService.UpdateAsync(updateProduct, cancellationToken);
                commonAdminStates[message.Chat.Id] = CommonAdminState.None;

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtProductNameUpdated"],
                cancellationToken: cancellationToken);

                await SendCategoryMenuAsync(message, cancellationToken);
            }
        }
        else
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtProductAlreadyExist"],
            cancellationToken: cancellationToken);
    }

    private async Task AdminHandleProductPriceAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }

        logger.LogInformation("AdminHandleProductPriceAsync is working..");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
         new[] { new KeyboardButton(localizer["btnAsterisk"]) },
         new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        try
        {
            var price = decimal.Parse(message.Text);

                if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateProduct)
                {
                    createProduct[message.Chat.Id].Price = price;
                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtProductRequestQuantity", createProduct[message.Chat.Id].Name],
                    replyMarkup: replyKeyboard,
                    cancellationToken: cancellationToken);

                    adminStates[message.Chat.Id] = AdminState.WaitingForInputProductQuantity;
                }
                else if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateProduct)
                {
                    var updateProduct = new ProductUpdateDTO
                    {
                        Id = product[message.Chat.Id].Id,
                        Name = product[message.Chat.Id].Name,
                        Price = price,
                        Quantity = product[message.Chat.Id].Quantity,
                        Description = product[message.Chat.Id].Description,
                        Unit = product[message.Chat.Id].Unit,
                        CategoryId = product[message.Chat.Id].Category.Id,
                    };
                    await productService.UpdateAsync(updateProduct, cancellationToken);
                    commonAdminStates[message.Chat.Id] = CommonAdminState.None;

                    await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtProductPriceUpdated"],
                    cancellationToken: cancellationToken);

                    await SendCategoryMenuAsync(message, cancellationToken);
                }
        }
        catch
        {
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtProductPriceError"],
                    cancellationToken: cancellationToken);
        }
    }

    private async Task AdminHandleProductQuantityAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }

        logger.LogInformation("AdminHandleProductQuantityAsync is working..");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
         new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        try
        {
            double? quantity;
            if (message.Text.Contains(localizer["btnAsterisk"]))
                quantity = null;
            else
                quantity = double.Parse(message.Text);

            if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateProduct)
            {
                createProduct[message.Chat.Id].Quantity = quantity;
                createProduct[message.Chat.Id].Unit = Unit.dona;
                createProduct[message.Chat.Id].CategoryId = category[message.Chat.Id].Id;
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtProductRequestDesc"],
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForInputProductDesc;
            }
            else if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateProduct)
            {
                var updateProduct = new ProductUpdateDTO
                {
                    Id = product[message.Chat.Id].Id,
                    Name = product[message.Chat.Id].Name,
                    Price = product[message.Chat.Id].Price,
                    Quantity = quantity,
                    Description = product[message.Chat.Id].Description,
                    Unit = product[message.Chat.Id].Unit,
                    CategoryId = product[message.Chat.Id].Category.Id,
                };
                await productService.UpdateAsync(updateProduct, cancellationToken);
                commonAdminStates[message.Chat.Id] = CommonAdminState.None;

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtProductQuantityUpdated"],
                cancellationToken: cancellationToken);

                await SendCategoryMenuAsync(message, cancellationToken);
            }
        }
        catch
        {
            await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: localizer["txtProductQuantityError"],
                    cancellationToken: cancellationToken);
        }
    }

    private async Task AdminHandleProductDescAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }

        logger.LogInformation("AdminHandleProductDescAsync is working..");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
         new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };
        var desc = message.Text;

        if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateProduct)
        {
            createProduct[message.Chat.Id].Description = desc;
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtProductRequestPhoto"],
            replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForInputProductPhoto;
        }
        else if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateProduct)
        {
            var updateProduct = new ProductUpdateDTO
            {
                Id = product[message.Chat.Id].Id,
                Name = product[message.Chat.Id].Name,
                Price = product[message.Chat.Id].Price,
                Quantity = product[message.Chat.Id].Quantity,
                Description = desc,
                Unit = product[message.Chat.Id].Unit,
                CategoryId = product[message.Chat.Id].Category.Id,
            };
            await productService.UpdateAsync(updateProduct, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtProductDescUpdated"],
            cancellationToken: cancellationToken);

            await SendCategoryMenuAsync(message, cancellationToken);
        }
    }

    private async Task HandleProductPhotoAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendProductMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }
        logger.LogInformation("HandleProductPhotoAsync is working..");

        var photo = message.Photo;
        if (adminStates[message.Chat.Id] == AdminState.WaitingForInputProductPhoto)
        {
            if (photo != null && photo.Length > 0)
            {
                var largestPhoto = photo.OrderByDescending(p => p.FileSize).First();
                using (var stream = new MemoryStream())
                {
                    await botClient.GetInfoAndDownloadFileAsync(largestPhoto.FileId, stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var formFile = new FormFile(stream, 0, stream.Length, "photo", largestPhoto.FileId + ".jpg");
                    if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.CreateProduct))
                    {
                        createProduct[message.Chat.Id].Attachment = formFile;
                        var result = await productService.AddAsync(createProduct[message.Chat.Id], cancellationToken);
                        if (result != null)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: localizer["txtAddedProduct"],
                                cancellationToken: cancellationToken);

                            await SendProductMenuAsync(message, cancellationToken);
                        }
                        else
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: localizer["txtAddedProductError"],
                                cancellationToken: cancellationToken);
                    }
                    else if(commonAdminStates[message.Chat.Id].Equals(CommonAdminState.UpdateProduct))
                    {
                        var updateProduct = new ProductUpdateDTO
                        {
                            Id = product[message.Chat.Id].Id,
                            Name = product[message.Chat.Id].Name,
                            Price = product[message.Chat.Id].Price,
                            Quantity = product[message.Chat.Id].Quantity,
                            Description = product[message.Chat.Id].Description,
                            Unit = product[message.Chat.Id].Unit,
                            CategoryId = product[message.Chat.Id].Category.Id,
                        };
                        var result = await productService.UpdateAsync(updateProduct, cancellationToken);
                        if (result != null)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: localizer["txtEditedProduct"],
                                cancellationToken: cancellationToken);

                            await SendProductMenuAsync(message, cancellationToken);
                        }
                        else
                            await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: localizer["txtEditedProductError"],
                                cancellationToken: cancellationToken);
                    }
                }
            }
            else
                await botClient.SendTextMessageAsync(
                           chatId: message.Chat.Id,
                           text: localizer["txtPhotoError"],
                           cancellationToken: cancellationToken);
        }
        else
            await AdminSendMainMenuAsync(message, cancellationToken);
    }

}
