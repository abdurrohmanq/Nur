using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task SendEnterCalmAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("SendEnterCalmAsync is working..");

        ArgumentNullException.ThrowIfNull(message);

        if (user[message.Chat.Id].Password == null)
        {
            
        }
    }
}
