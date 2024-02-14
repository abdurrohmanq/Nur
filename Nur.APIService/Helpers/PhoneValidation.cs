using System.Text.RegularExpressions;

namespace Nur.APIService.Helpers;

public static class PhoneValidation
{
    public static bool ValidatePhoneNumber(string phoneNumber)
    {
        string pattern = @"^\+(?:[0-9] ?){6,14}[0-9]$";

        return Regex.IsMatch(phoneNumber, pattern);
    }
}
