namespace Nur.Bot.Models.Enums;

public enum UserState
{
    None,
    WaitingForFullName,
    WaitingForSelectLanguage,
    WaitingForSelectMainMenu,
    WaitingForSelectSettings,
    WaitingForHandlerFeedback,
    WaitingForSelectOrderType,
    WaitingForEnterPhoneNumber,
    WaitingForSelectPersonalInfo,
    WaitingForCategorySelection,
    WaitingForProductSelection,
    WaitingForHandleTextLocation,
    WaitingForQuantityInput,
    WaitingForCartAction,
    WaitingForCommentAction,
    WaitingForPaymentTypeAction,
    WaitingForOrderSendToAdminAction,
    WaitingForAdminConfirmation,
    AdminState,
}
