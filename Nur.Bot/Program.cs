using Nur.APIService;
using Nur.Bot.BotServices;
using Nur.Bot.BotServices.Commons;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var token = builder.Configuration.GetValue("Token", string.Empty);

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(token));
builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();

builder.Services.AddApiServices();

builder.Services.AddLocalization();

//builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

var app = builder.Build();

// Localization
var supportedCultures = new[] { "uz-Uz", "en-Us", "ru-Ru" };
var localizationOptions = new RequestLocalizationOptions()
  .SetDefaultCulture(defaultCulture: supportedCultures[0])
  .AddSupportedCultures(cultures: supportedCultures)
  .AddSupportedUICultures(uiCultures: supportedCultures);
app.UseRequestLocalization(options: localizationOptions);

// Configure the HTTP request pipeline.

app.Run();
