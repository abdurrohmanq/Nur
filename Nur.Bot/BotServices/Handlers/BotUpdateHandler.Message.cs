﻿using Nur.Bot.Models.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task HandleMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;


        logger.LogInformation("Received message from {from.FirstName}", from.FirstName);

        var commonUserState = commonUserStates.TryGetValue(message.Chat.Id, out var commonState) ? commonState : CommonUserState.None;
        cafe[message.Chat.Id] = (await cafeService.GetAsync(cancellationToken)).FirstOrDefault();

        var handlerUserMessage = message.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(client, message, cancellationToken),
            MessageType.Contact => HandleContactAsync(client, message, cancellationToken),
            MessageType.Location => HandleLocationAsync(message, cancellationToken),
            _ => HandleUnknownMessageAsync(client, message, cancellationToken)
        };

        await handlerUserMessage;
    }

    private Task HandleUnknownMessageAsync(ITelegramBotClient client, Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Received message type: {message.Type}");
        return Task.CompletedTask;
    }
}
