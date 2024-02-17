using Telegram.Bot;
using Telegram.Bot.Types;
using Nur.Bot.Models.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, UserState> userStates = new Dictionary<long, UserState>();
    private Dictionary<long, CommonUserState> commonUserStates = new Dictionary<long, CommonUserState>();

    private async Task HandleTextMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message.Text);
        var from = message.From;
        logger.LogInformation("From: {from.FirstName}", from?.FirstName);

        var userState = userStates.TryGetValue(message.Chat.Id, out var state) ? state : UserState.None;
        userState = message.Text.Equals("/start") ? UserState.None : userState;

        if (message.Text.Equals(localizer["btnMainMenu"])) await SendMainMenuAsync(message, cancellationToken);
        var handler = userState switch
        {
            UserState.None => SendGreetingAsync(message, cancellationToken),
            UserState.WaitingForFullName => HandleUserFullNameAsync(message, cancellationToken),
            UserState.WaitingForSelectMainMenu => HandleMainMenuAsync(message, cancellationToken),
            UserState.WaitingForHandlerFeedback => HandleFeedBackAsync(message, cancellationToken),
            UserState.WaitingForSelectSettings => HandleSelectedSettingsAsync(message, cancellationToken),
            UserState.WaitingForSelectPersonalInfo => HandleSelectedPersonalInfoAsync(message, cancellationToken),
            UserState.WaitingForEnterPhoneNumber => HandlePhoneNumberAsync(message, cancellationToken),
            UserState.WaitingForSelectLanguage => HandleSentLanguageAsync(message, cancellationToken),
            UserState.WaitingForSelectOrderType => HandleOrderTypeAsync(message, cancellationToken),
            UserState.WaitingForHandleTextLocation => HandleTextLocationAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        try { await handler; }
        catch (Exception ex) { logger.LogError(ex, "Error handling message from {user.FirstName}", user[message.Chat.Id].FirstName); }
    }
}
