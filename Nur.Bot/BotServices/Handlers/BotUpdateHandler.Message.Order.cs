using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleOrderTypeAsync(Message message, CancellationToken cancellationToken)
    {
        var handle = message.Text switch
        {
            { } text when text == localizer["btnDelivery"] => SendDeliveryAsync(message, cancellationToken),
            { } text when text == localizer["btnTakeAway"] => SendMenuSettingsAsync(message, cancellationToken),
            { } text when text == localizer["btnBack"] => SendMainMenuAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handle; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }


}
