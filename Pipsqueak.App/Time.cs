namespace Pipsqueak.App;

public static class Time
{
    public static DateTime GetEventTimeAsDateTime(long unixTime)
    {
        return DateTimeOffset
            .FromUnixTimeMilliseconds(unixTime)
            .LocalDateTime;
    }
}