using Telegram.Bot.Types;
using Telegram.Bot;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleEditMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;

        logger.LogInformation("Received Edit message from {from.FirstName}: {message.Text}", from?.FirstName, message.Text);
    }
}
