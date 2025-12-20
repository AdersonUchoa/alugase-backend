namespace Domain.Extensions
{
    public static class DatetimeExtensions
    {
        public static string ToStringDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
