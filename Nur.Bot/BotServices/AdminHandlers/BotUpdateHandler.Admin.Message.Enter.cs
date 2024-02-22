using Nur.APIService.Helpers;
using Nur.APIService.Models.Cafes;
using Nur.Bot.Models.Enums;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private async Task AdminHandlePasswordAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandlePasswordAsync is working..");

        var password = message.Text;

        if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.CreateCafe))
        {
            var createCafe = new CafeCreationDTO
            {
                InstagramLink = cafe[message.Chat.Id].InstagramLink,
                FacebookLink = cafe[message.Chat.Id].FacebookLink,
                Phone = cafe[message.Chat.Id].Phone,
                Password = password,
            };

            await cafeService.AddAsync(createCafe, cancellationToken);
            commonAdminStates[message.Chat.Id] = CommonAdminState.None;
        }

        if (cafe[message.Chat.Id].Password.Equals(password))
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "OK",
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInvalidPassword"],
                cancellationToken: cancellationToken);
        }
    }

    private async Task AdminHandleInstagramLinkAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleInstagramLinkAsync is working..");

        var link = message.Text;
        if (IsValidInstagramLink(link))
        {
            cafe[message.Chat.Id].InstagramLink = link;
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtFacebookLink"],
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForFacebookLink;
            }
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInvalidInstagramLink"],
                cancellationToken: cancellationToken);
    }
    
    private async Task AdminHandleFacebookLinkAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandleFacebookLinkAsync is working..");


        var link = message.Text;
        if (IsValidFacebookLink(link))
        {
            cafe[message.Chat.Id].FacebookLink = link;
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCafePhone"],
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForCafePhone;
            }
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInvalidFacebookLink"],
                cancellationToken: cancellationToken);
    }

    private async Task AdminHandlePhoneAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandlePhoneAsync is working..");

        var phone = message.Text;
        if(PhoneValidation.ValidatePhoneNumber(phone))
        {
            cafe[message.Chat.Id].Phone = phone;
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCafePassword"],
                cancellationToken: cancellationToken);

                adminStates[message.Chat.Id] = AdminState.WaitingForCafePassword;
            }
        }
    }

    private bool IsValidInstagramLink(string link)
    {
        Regex regex = new Regex(@"^(http(s)?:\/\/)?(www\.)?(instagram\.com|instagr\.am)\/[a-zA-Z0-9_]+\/?$");
        return regex.IsMatch(link);
    }

    private bool IsValidFacebookLink(string link)
    {
        Regex regex = new Regex(@"^(http(s)?:\/\/)?(www\.)?(facebook\.com|fb\.com)\/[a-zA-Z0-9_]+\/?$");
        return regex.IsMatch(link);
    }

}
