namespace BMS_API.Constants;

public static class Constants
{
    public const int TokenExpiryTimeHrs = 1;
    public static DateTime TokenExpiryTime => DateTime.Now.AddHours(TokenExpiryTimeHrs); 
}
