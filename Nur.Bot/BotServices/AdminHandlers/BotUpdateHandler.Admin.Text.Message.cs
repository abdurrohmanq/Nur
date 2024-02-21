using Nur.Bot.Models.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, AdminState> adminStates = new Dictionary<long, AdminState>();

    private async Task AdminHandleTextMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message.Text);
        var from = message.From;
        logger.LogInformation("From: {from.FirstName}", from?.FirstName);

        var adminState = adminStates.TryGetValue(message.Chat.Id, out var state) ? state : AdminState.None;

        var handler = adminState switch
        {
            AdminState.None => SendGreetingAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handler; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }
}
