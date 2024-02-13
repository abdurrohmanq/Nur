using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, string> userPhoneNumber = new Dictionary<long, string>();

    private async Task HandleContactAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var contact = message.Contact;
        userPhoneNumber[message.Chat.Id] = contact.PhoneNumber;
        logger.LogInformation("User's phone number: {userPhoneNumber}", userPhoneNumber);

        await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["GetContact"],
            cancellationToken: cancellationToken);

        await client.SendTextMessageAsync(
           chatId: message.Chat.Id,
           text: localizer["RequestForFullName"],
           cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForFullName;
    }

    private async Task HandleUserFullNameAsync(Message message, CancellationToken cancellationToken)
    {
        var fullName = message.Text;
        logger.LogInformation("User's fullName: {fullName}", fullName);

        user[message.Chat.Id].FullName = fullName;
        user[message.Chat.Id].Phone = userPhoneNumber[message.Chat.Id];

        await userService.UpdateAsync(user[message.Chat.Id], cancellationToken);

        await SendMainMenuAsync(message, cancellationToken);
    }
}
