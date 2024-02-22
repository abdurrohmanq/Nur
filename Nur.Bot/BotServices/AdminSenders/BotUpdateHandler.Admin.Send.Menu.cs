using Nur.APIService.Models.ProductCategories;
using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task SendCategoryMenuAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendCategoryMenuAsync is working");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnAddCategory"]), new KeyboardButton(localizer["btnDeleteCategory"]) },
            new[] { new KeyboardButton(localizer["btnEditCategory"]),  new KeyboardButton(localizer["btnGetCategoryInfo"]) },
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

        adminStates[message.Chat.Id] = AdminState.WaitingForSelectCategoryMenu;
    }

    private async Task SendAddCategoryQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendAddCategoryQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCategoryRequestName"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.CreateCategory;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputCategoryName;
    }
    
    private async Task SendEditCategoryNameQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCategoryNameQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCategoryRequestName"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateCategory;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputCategoryName;
    }
    
    private async Task SendEditCategoryDescQueryAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCategoryDescQueryAsync is working");
        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnBack"]) },
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCategoryRequestDesc"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        commonAdminStates[message.Chat.Id] = CommonAdminState.UpdateCategory;
        adminStates[message.Chat.Id] = AdminState.WaitingForInputCategoryDesc;
    }

    private async Task SendEditCategoryPartsAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEditCategoryPartsAsync is working");

        var replyKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(localizer["btnEditCategoryName"]), new KeyboardButton(localizer["btnEditCategoryDesc"]) },
            new[] { new KeyboardButton(localizer["btnBack"])},
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: localizer["txtCategoryEditSelect"],
             replyMarkup: replyKeyboard,
             cancellationToken: cancellationToken);

        adminStates[message.Chat.Id] = AdminState.WaitingForSelectCategoryEdit;
    }

    private async Task AdminSendCategoryKeyboardAsync(long chatId, CancellationToken cancellationToken)
    {
        Dictionary<long, IEnumerable<ProductCategoryDTO>> categories = new Dictionary<long, IEnumerable<ProductCategoryDTO>>();

        categories[chatId] = await categoryService.GetAllAsync(cancellationToken);

        var additionalButtons = new List<KeyboardButton>
        {
        new KeyboardButton(localizer["btnBack"]),
        };

        var allButtons = new List<KeyboardButton[]>();
        var rowButtons = new List<KeyboardButton>();

        foreach (var category in categories[chatId])
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

        adminStates[chatId] = AdminState.WaitingForCategorySelection;
    }

}
