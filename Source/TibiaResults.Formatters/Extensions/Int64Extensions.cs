namespace TibiaResults.Formatters
{
    internal static class Int64Extensions
    {
        public static string ToNumberString(this long value) => value.ToString("N0");

        public static string ToSignedNumberString(this long value) => value.ToString("+###,###,###,###,##0;-###,###,###,###,###");
    }
}
