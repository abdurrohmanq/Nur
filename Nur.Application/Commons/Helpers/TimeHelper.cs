using Nur.Application.Commons.Constants;

namespace Nur.Application.Commons.Helpers;

public class TimeHelper
{
    public static DateTime GetDateTime()
        => DateTime.UtcNow.AddHours(TimeConstant.UTC);
}
