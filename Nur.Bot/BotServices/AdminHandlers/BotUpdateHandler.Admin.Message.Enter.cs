using Nur.APIService.Helpers;
using Nur.APIService.Models.Cafes;
using Nur.Bot.Models.Enums;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nur.Bot.BotServices;

public partial class BotUpdateHandler
{
    private Dictionary<long, CafeCreationDTO> createCafe = new Dictionary<long, CafeCreationDTO>();
    private async Task AdminHandlePasswordAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInformation("AdminHandlePasswordAsync is working..");

        var password = message.Text;

        if (commonAdminStates[message.Chat.Id].Equals(CommonAdminState.CreateCafe))
        {
            createCafe[message.Chat.Id] = new CafeCreationDTO
            {
                Password = password
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtInstagramLink"],
                cancellationToken: cancellationToken);

            adminStates[message.Chat.Id] = AdminState.WaitingForInstagramLink;
        }
        else if (cafe[message.Chat.Id].Password.Equals(password))
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "OK",
                cancellationToken: cancellationToken);

            await AdminSendMainMenuAsync(message, cancellationToken);
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

        if (message.Text.Equals(localizer["btnBack"]) && commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
        {
            await SendEditCafeInfoPartsAsync(message, cancellationToken);
        }
        var link = message.Text;
        if (IsValidInstagramLink(link))
        {
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
            {
                cafe[message.Chat.Id].InstagramLink = link;
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);
                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtUpdatedInstagramLink"],
                cancellationToken: cancellationToken);

                await AdminSendMainMenuAsync(message, cancellationToken);
            }
            else
            {
                createCafe[message.Chat.Id].InstagramLink = link;
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
        if (message.Text.Equals(localizer["btnBack"]) && commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
        {
            await SendEditCafeInfoPartsAsync(message, cancellationToken);
        }

        var link = message.Text;
        if (IsValidFacebookLink(link))
        {
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
            {
                cafe[message.Chat.Id].FacebookLink = link;
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtUpdatedFacebookLink"],
                cancellationToken: cancellationToken);

                await AdminSendMainMenuAsync(message, cancellationToken);
            }
            else
            {
                createCafe[message.Chat.Id].FacebookLink = link;
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

        if (message.Text.Equals(localizer["btnBack"]) && commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
        {
            await AdminSendMainMenuAsync(message, cancellationToken);
        }
        var phone = message.Text;
        if(PhoneValidation.ValidatePhoneNumber(phone))
        {
            if (commonAdminStates[message.Chat.Id] != CommonAdminState.CreateCafe)
            {
                cafe[message.Chat.Id].Phone = phone;
                await cafeService.UpdateAsync(cafe[message.Chat.Id], cancellationToken);

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtUpdatedPhone"],
                cancellationToken: cancellationToken);

                await AdminSendMainMenuAsync(message, cancellationToken);
            }
            else
            {
                createCafe[message.Chat.Id].Phone = phone;

                await cafeService.AddAsync(createCafe[message.Chat.Id], cancellationToken);
                commonAdminStates[message.Chat.Id] = CommonAdminState.None;

                await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: localizer["txtCreatedCafe"],
                cancellationToken: cancellationToken);

                await AdminSendMainMenuAsync(message, cancellationToken);
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
        Regex regex = new Regex(@"^(http(s)?:\/\/)?(www\.)?(facebook\.com|fb\.com)\/[a-zA-Z0-9_\.]+\/?$");
        return regex.IsMatch(link);
    }
}
