namespace Nur.Bot.Models.Enums;

public enum UserState
{
    None,
    WaitingForFullName,
    WaitingForSelectLanguage,
    WaitingForSelectMainMenu,
    WaitingForHandlerFeedback,
    WaitingForSelectSettings,
    WaitingForSelectPersonalInfo,
    WaitingForEnterPhoneNumber,
    WaitingForEnterFullName
}
