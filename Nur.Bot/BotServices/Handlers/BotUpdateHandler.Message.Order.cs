using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.APIService.Models.Products;

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
}
