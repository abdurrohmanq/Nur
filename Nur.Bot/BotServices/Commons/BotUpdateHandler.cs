using Microsoft.Extensions.Localization;
using Nur.APIService.Interfaces;
using Nur.APIService.Models.Users;
using Nur.Bot.Models.Enums;
using Nur.Bot.Resources;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler(ILogger<BotUpdateHandler> logger,
    ITelegramBotClient botClient,
    IServiceScopeFactory scopeFactory,
    ICartService cartService,
    IUserService userService,
    ICafeService cafeService,
    IOrderService orderService,
    IAddressService addressService,
    IProductService productService,
    IPaymentService paymentService,
    ICartItemService cartItemService,
    IOrderItemService orderItemService,
    IProductCategoryService categoryService) : IUpdateHandler
{
    private IStringLocalizer localizer = default!;
    private Dictionary<long, UserDTO> user = new Dictionary<long, UserDTO>();

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message.Type == MessageType.Text)
        {
            if (update.Message.Text.Equals("/admin")) { userStates[update.Message.Chat.Id] = UserState.AdminState; }
            else if (update.Message.Text.Equals("/start"))
            {
                userStates[update.Message.Chat.Id] = UserState.None;
                adminStates[update.Message.Chat.Id] = AdminState.None;
            }
        }

        var userState = userStates.TryGetValue(update.Message.Chat.Id, out var state) ? state : UserState.None;

        if (userState == UserState.AdminState)
        {
            user[update.Message.Chat.Id] = await GetUserAsync(update, cancellationToken);

            using var scope = scopeFactory.CreateScope();
            localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<BotLocalizer>>();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("uz-Uz");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uz-Uz");
            var handler = update.Type switch
            {
                UpdateType.Message => AdminHandleMessageAsync(botClient, update.Message, cancellationToken),
                _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }
        }
        else
        {
            using var scope = scopeFactory.CreateScope();
            localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<BotLocalizer>>();
            if (update.Type == UpdateType.Message)
            {
                user[update.Message.Chat.Id] = await GetUserAsync(update, cancellationToken);
                cart[update.Message.Chat.Id] = await cartService.GetByUserIdAsync(user[update.Message.Chat.Id].Id, cancellationToken);

                var culture = user[update.Message.Chat.Id].LanguageCode switch
                {
                    "uz" => new CultureInfo("uz-Uz"),
                    "en" => new CultureInfo("en-US"),
                    "ru" => new CultureInfo("ru-RU"),
                    _ => CultureInfo.CurrentCulture
                };

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                UpdateType.EditedMessage => HandleEditMessageAsync(botClient, update.EditedMessage, cancellationToken),
                UpdateType.CallbackQuery => HandleCallbackQuery(botClient, update.CallbackQuery, cancellationToken),
                _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandlePollingErrorAsync(botClient, ex, cancellationToken);
            }
        }
    }

    private async Task<UserDTO> GetUserAsync(Update update, CancellationToken cancellationToken)
    {
        var updateContent = BotUpdateHandler.GetUpdateType(update);
        var from = updateContent.From;

        return await userService.GetAsync((long)from.Id, cancellationToken)
            ?? await userService.AddAsync(new UserCreationDTO
            {
                TelegramId = from.Id,
                LastName = from.LastName,
                Username = from.Username,
                FirstName = from.FirstName,
                ChatId = update.Message!.Chat.Id,
                LanguageCode = from.LanguageCode,
            }, cancellationToken);
    }

    private static dynamic GetUpdateType(Update update)
        => (update.Type switch
        {
            UpdateType.Message => update.Message,
            UpdateType.ChatMember => update.ChatMember,
            UpdateType.ChannelPost => update.ChannelPost,
            UpdateType.InlineQuery => update.InlineQuery,
            UpdateType.MyChatMember => update.MyChatMember,
            UpdateType.CallbackQuery => update.CallbackQuery,
            UpdateType.EditedMessage => update.EditedMessage,
            UpdateType.ShippingQuery => update.ShippingQuery,
            UpdateType.PreCheckoutQuery => update.PreCheckoutQuery,
            UpdateType.EditedChannelPost => update.EditedChannelPost,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult,
            _ => update.Message,
        })!;

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogInformation("Error occur with Telegram bot: {ex.Message}", exception);
        return Task.CompletedTask;
    }

    private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}
