namespace CrossCutting.Data.Sql.Extensions
{
    public static class IntExtensions
    {
        public static int IfNotGreaterThan(this int? queryValue, int? overrideValue)
        {
            var result = 0;

            if (queryValue.HasValue && queryValue.Value > 0)
            {
                result = queryValue.Value;
            }

            if (overrideValue.HasValue && overrideValue.Value > 0 && ((overrideValue.Value < result && overrideValue.Value >= 0) || result == 0))
            {
                result = overrideValue.Value;
            }

            return result;
        }
    }
}
