using Nur.APIService.Models.ProductCategories;
using Nur.Bot.Models.Enums;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    public Dictionary<long, ProductCategoryDTO> category = new Dictionary<long, ProductCategoryDTO>();
    private async Task AdminHandleMainMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleMainMenuAsync is working..");

        var handle = message.Text switch
        {
            { } text when text == localizer["btnCategory"] => SendCategoryMenuAsync(message, cancellationToken),
            { } text when text == localizer["btnProduct"] => SendProductMenuAsync(message, cancellationToken),
            _ when message.Text == localizer["btnEditInfo"] => SendInfoAsync(message, cancellationToken),
            { } text when text == localizer["btnEditPhone"] => ShowFeedbackAsync(message, cancellationToken),
            { } text when text == localizer["btnOrdersList"] => SendContactAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task AdminHandleCategoryMenuAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await AdminSendMainMenuAsync(message, cancellationToken);
            return;
        }
        logger.LogInformation("AdminHandleCategoryMenuAsync is working..");

        if (message.Text.Equals(localizer["btnEditCategory"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateCategory;
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
            return;
        }
        if(message.Text.Equals(localizer["btnDeleteCategory"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.DeleteCategory;
            await AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken);
        }

        var handle = message.Text switch
        {
            { } text when text == localizer["btnAddCategory"] => SendAddCategoryQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnGetCategoryInfo"] => AdminSendCategoryKeyboardAsync(message.Chat.Id, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    private async Task AdminHandleCategoryEditAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleCategoryMenuAsync is working..");

        var handle = message.Text switch
        {
            { } text when text == localizer["btnEditCategoryName"] => SendEditCategoryNameQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnEditCategoryDesc"] => SendEditCategoryDescQueryAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendCategoryMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }

    public Dictionary<long, ProductCategoryCreationDTO> createCategory = new Dictionary<long, ProductCategoryCreationDTO>();
    private async Task AdminHandleCategoryNameAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendCategoryMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }

        logger.LogInformation("AdminHandleCategoryNameAsync is working..");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        var name = message.Text;
        var result = await categoryService.GetByNameAsync(message.Text, cancellationToken);
        if (result is null)
        {
            if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateCategory)
            {
                createCategory[message.Chat.Id] = new ProductCategoryCreationDTO { Name = name };
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCategoryRequestDescription"],
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForInputCategoryDesc;
            }
            else if (commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateCategory)
            {
                category[message.Chat.Id].Name = name;
                await categoryService.UpdateAsync(category[message.Chat.Id], cancellationToken);
                commonAdminStates[message.Chat.Id] = CommonAdminState.None;

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCategoryNameUpdated"],
                cancellationToken: cancellationToken);

                await SendCategoryMenuAsync(message, cancellationToken);
            }
        }
        else
            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtCategoryAlreadyExist"],
            cancellationToken: cancellationToken);
    }

    private async Task AdminHandleCategoryDescAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            await SendCategoryMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            return;
        }
        logger.LogInformation("AdminHandleCategoryDescAsync is working..");

        var desc = message.Text;
        if (commonAdminStates[message.Chat.Id] == CommonAdminState.CreateCategory)
        {
            createCategory[message.Chat.Id].Description = desc;
            await categoryService.AddAsync(createCategory[message.Chat.Id], cancellationToken);

            await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtAddedCategory"],
            cancellationToken: cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;

            await SendCategoryMenuAsync(message, cancellationToken);
        }
        else if(commonAdminStates[message.Chat.Id] == CommonAdminState.UpdateCategory)
        {
            category[message.Chat.Id].Description = desc;
            await categoryService.UpdateAsync(category[message.Chat.Id], cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCategoryDescUpdated"],
                cancellationToken: cancellationToken);

            await SendCategoryMenuAsync(message, cancellationToken);
        }
    }

    private async Task AdminHandleCategorySelectionAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text.Equals(localizer["btnBack"]))
        {
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            await SendCategoryMenuAsync(message, cancellationToken);
            return;
        }

        Dictionary<long, string> selectedCategoryName = new Dictionary<long, string>();

        selectedCategoryName[message.Chat.Id] = message.Text;

        category[message.Chat.Id] = await categoryService.GetByNameAsync(selectedCategoryName[message.Chat.Id], cancellationToken);

        if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.UpdateCategory))
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCategoryInfo", category[message.Chat.Id].Name, category[message.Chat.Id].Description],
                cancellationToken: cancellationToken);
            await SendEditCategoryPartsAsync(message, cancellationToken);
            return;
        }

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["txtCategoryInfo", category[message.Chat.Id].Name, category[message.Chat.Id].Description],
            cancellationToken: cancellationToken);

        if (category[message.Chat.Id].Products.Count() > 0)
        {
            StringBuilder productsBuilder = new StringBuilder("Mahsulotlar\n\n");

            foreach (var product in category[message.Chat.Id].Products)
            {
                productsBuilder.AppendLine(product.Name);
            }

            string products = productsBuilder.ToString();

            if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.DeleteCategory))
            {
                var replyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                   new[] { new KeyboardButton(localizer["btnCancel"]) },
                   new[] { new KeyboardButton(localizer["btnOk"]) },
                })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtDeleteCategoryWarning", products],
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForDeleteCategoryConfirm;
            }
            else
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: products,
                    cancellationToken: cancellationToken);
        }
        else if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.DeleteCategory))
        {
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                   new[] { new KeyboardButton(localizer["btnCancel"]) },
                   new[] { new KeyboardButton(localizer["btnOk"]) },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtEmptyCategory"],
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);
            adminStates[message.Chat.Id] = AdminState.WaitingForDeleteCategoryConfirm;
        }
        else
            await botClient.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: localizer["txtEmptyCategory"],
               cancellationToken: cancellationToken);
    }

    private async Task HandleCategoryDeleteConfirmAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleCategoryDeleteConfirmAsync is working..");

        if (message.Text.Equals(localizer["btnCancel"]))
        {
            await SendCategoryMenuAsync(message, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
        }
        else if (message.Text.Equals(localizer["btnOk"]))
        {
            await categoryService.DeleteAsync(category[message.Chat.Id].Id, cancellationToken);
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtRemovedCategory"],
                cancellationToken: cancellationToken);

            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
            await SendCategoryMenuAsync(message, cancellationToken);
        }
    }
}
