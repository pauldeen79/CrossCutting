namespace CrossCutting.DataTableDumper.Extensions
{
    public static class StringExtensions
    {
        public static string EscapePipes(this string instance, string escapeValue = "_")
        {
            if (instance == null)
            {
                return null;
            }

            return instance.Replace("|", escapeValue);
        }

        public static string UnescapePipes(this string instance, string escapeValue = "_")
        {
            if (instance == null)
            {
                return null;
            }

            return instance.Replace(escapeValue, "|");
        }
    }
}
