using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, UserState> userStates = new Dictionary<long, UserState>();

    private async Task HandleTextMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message.Text);
        var from = message.From;
        logger.LogInformation("From: {from.FirstName}", from?.FirstName);

        var userState = userStates.TryGetValue(message.Chat.Id, out var state) ? state : UserState.None;
        userState = message.Text.Equals("/start") ? UserState.None : userState;

        var handler = userState switch
        {
            UserState.None => SendGreetingAsync(message, cancellationToken),
            UserState.WaitingForFullName => HandleUserFullNameAsync(message, cancellationToken),
            UserState.WaitingForSelectMainMenu => HandleMainMenuAsync(message, cancellationToken),
            UserState.WaitingForHandlerFeedback => HandleFeedBackAsync(message, cancellationToken),
        };

        try { await handler; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }
}
