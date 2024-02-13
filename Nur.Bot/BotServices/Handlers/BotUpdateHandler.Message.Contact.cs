using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleContactAsync(Message message, CancellationToken cancellationToken)
    {
        var contact = message.Contact;
        var userPhoneNumber = contact.PhoneNumber;

    }
}
