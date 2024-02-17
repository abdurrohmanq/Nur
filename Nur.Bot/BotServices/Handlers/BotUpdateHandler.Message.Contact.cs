using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleContactAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        var contact = message.Contact;
        user[message.Chat.Id].Phone = contact.PhoneNumber;
        logger.LogInformation("User's phone number: {userPhoneNumber}", contact.PhoneNumber);

        await client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["GetContact"],
            cancellationToken: cancellationToken);

        await userService.UpdateAsync(user[message.Chat.Id], cancellationToken);
        
        if (commonUserStates[message.Chat.Id] == CommonUserState.CreatingUser)
        {
            await client.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: localizer["RequestForFullName"],
               cancellationToken: cancellationToken);

            userStates[message.Chat.Id] = UserState.WaitingForFullName;
            commonUserStates[message.Chat.Id] = CommonUserState.None;
        }
        else
            await SendMainMenuAsync(message, cancellationToken);
    }

    private async Task HandleUserFullNameAsync(Message message, CancellationToken cancellationToken)
    {
        var fullName = message.Text;
        logger.LogInformation("User's fullName: {fullName}", fullName);

        if (message.Text.Equals(localizer["btnCancel"]))
            await SendMenuSettingsAsync(message, cancellationToken);
        else
        {
            user[message.Chat.Id].FullName = fullName;

            await userService.UpdateAsync(user[message.Chat.Id], cancellationToken);

            await SendMainMenuAsync(message, cancellationToken);
        }
    }
}
