using Telegram.Bot.Types;
using Telegram.Bot;
using Nur.Bot.Models.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    public async Task SendGreetingAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][] {
                [InlineKeyboardButton.WithCallbackData("🇺🇿 o'zbekcha 🇺🇿", "ibtnUz")],
                [InlineKeyboardButton.WithCallbackData("🇬🇧 english 🇬🇧", "ibtnEn")],
                [InlineKeyboardButton.WithCallbackData("🇷🇺 русский 🇷🇺", "ibtnRu")] });

        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: localizer["greeting"],
            cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
           chatId: message.Chat.Id,
           text: localizer["choose-language"],
           replyMarkup : keyboard,
           cancellationToken: cancellationToken);

        userStates[message.Chat.Id] = UserState.WaitingForSelectLanguage;
    }
}
