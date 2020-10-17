namespace Une.TalentPool.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToSubString(this string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
                return value.Substring(0, maxLength) + "...";
            else
                return value;
        }
    }
}
