using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Nur.Bot.BotServices.Commons;

public class BotBackgroundService(ITelegramBotClient botClient,
    ILogger<BackgroundService> logger,
    IUpdateHandler updateHandler) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = await botClient.GetMeAsync(stoppingToken);

        logger.LogInformation("Bot {BotName} started successfully", bot.Username);

        botClient.StartReceiving(
            updateHandler: updateHandler.HandleUpdateAsync,
            pollingErrorHandler: updateHandler.HandlePollingErrorAsync,
            receiverOptions: null,
            cancellationToken: stoppingToken);
    }
}
