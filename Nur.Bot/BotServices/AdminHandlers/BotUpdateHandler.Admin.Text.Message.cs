using Nur.Bot.Models.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, AdminState> adminStates = new Dictionary<long, AdminState>();
    private Dictionary<long, CommonAdminState> commonAdminStates = new Dictionary<long, CommonAdminState>();

    private async Task AdminHandleTextMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message.Text);
        var from = message.From;
        logger.LogInformation("From: {from.FirstName}", from?.FirstName);

        var adminState = adminStates.TryGetValue(message.Chat.Id, out var state) ? state : AdminState.None;

        var handler = adminState switch
        {
            AdminState.None => SendEnterCalmAsync(message, cancellationToken),
            AdminState.WaitingForCafePassword => AdminHandlePasswordAsync(message, cancellationToken),
            AdminState.WaitingForInstagramLink => AdminHandleInstagramLinkAsync(message, cancellationToken),
            AdminState.WaitingForFacebookLink => AdminHandleFacebookLinkAsync(message, cancellationToken),
            AdminState.WaitingForCafePhone => AdminHandlePhoneAsync(message, cancellationToken),
            AdminState.WaitingForSelectMainMenu => AdminHandleMainMenuAsync(message, cancellationToken),
            AdminState.WaitingForSelectCategoryMenu => AdminHandleCategoryMenuAsync(message, cancellationToken),
            AdminState.WaitingForInputCategoryName => AdminHandleCategoryNameAsync(message, cancellationToken),
            AdminState.WaitingForInputCategoryDesc => AdminHandleCategoryDescAsync(message, cancellationToken),
            AdminState.WaitingForCategorySelection => AdminHandleCategorySelectionAsync(message, cancellationToken),
            AdminState.WaitingForSelectCategoryEdit => AdminHandleCategoryEditAsync(message, cancellationToken),
            AdminState.WaitingForDeleteCategoryConfirm => HandleCategoryDeleteConfirmAsync(message, cancellationToken),
            AdminState.WaitingForSelectProductMenu => AdminHandleProductMenuAsync(message, cancellationToken),
            AdminState.WaitingForCategorySelectionForProduct => AdminHandleCategorySelectionForProductAsync(message, cancellationToken),
            AdminState.WaitingForInputProductName => AdminHandleProductNameAsync(message, cancellationToken),
            AdminState.WaitingForInputProductPrice => AdminHandleProductPriceAsync(message, cancellationToken),
            AdminState.WaitingForInputProductQuantity => AdminHandleProductQuantityAsync(message, cancellationToken),
            AdminState.WaitingForInputProductDesc => AdminHandleProductDescAsync(message, cancellationToken),
            AdminState.WaitingForProductSelection => AdminHandleProductSelectionAsync(message, cancellationToken),
            AdminState.WaitingForSelectProductEdit => AdminHandleProductEditAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handler; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }
}
