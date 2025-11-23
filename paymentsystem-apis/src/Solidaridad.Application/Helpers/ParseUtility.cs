using System.Globalization;

namespace Solidaridad.Application.Helpers;

public static class ParseUtility
    {
        public static string ParseStringValue(object value)
        {
            if (value != null)
            {
                if (value is string)
                {
                    var strValue = value as string;
                    if (!string.IsNullOrWhiteSpace(strValue))
                    {
                        return strValue.Trim();
                    }
                }
            }
            return null;
        }

        public static string ParseAnyValueToString(object value)
        {
            if (value != null)
            {
                var strValue = Convert.ToString(value);
                if (!string.IsNullOrWhiteSpace(strValue))
                {
                    return strValue.Trim();
                }
            }
            return null;
        }

        public static bool ParseBoolean(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return (string.Compare(value, "yes", true) == 0);
            }
            return false;
        }

        public static decimal ParseDecimalValue(object value)
        {
            var result = 0m;
            if (value != null)
            {
                decimal.TryParse(value.ToString(), out result);
            }
            return result;
        }
        public static decimal? ParseNullableDecimalValue(object value)
        {
            if (value != null)
            {
                var result = 0m;
                decimal.TryParse(value.ToString(), out result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static int ParseIntValue(object value)
        {
            var result = 0;
            if (value != null)
            {
                int.TryParse(value.ToString(), out result);
            }
            return result;
        }
        public static int? ParseNullableIntValue(object value)
        {

            if (value != null)
            {
                var result = 0;
                int.TryParse(value.ToString(), out result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ParseDateTimeValue(object value, bool parseUKFormat = false)
        {
            var result = (DateTime?)null;
            try
            {
                if (value != null && value.ToString() != "")
                {

                    result = DateTime.ParseExact(value.ToString(), new string[]
                    { "M/d/yyyy",
                    "dd-MM-yyyy",
                    "d/M/yyyy",
                    "dd/MM/yyyy",
                    "dd/MMM/yyyy",
                    "dd/MMMM/yyyy",
                    "dd MMM yyyy",
                    "dd MMMM yyyy",
                    "yyyy-MM-dd",
                    "dd/MM/yyyy hh:mm:ss tt",
                    "d/MM/yyyy hh:mm:ss tt",
                    "d/M/yyyy hh:mm:ss tt",
                    "MM/dd/yyyy hh:mm:ss tt",
                    "MM/d/yyyy hh:mm:ss tt",
                    "M/d/yyyy hh:mm:ss tt",
                    "dd-MMM-yyyy",
                    "dd-MMM-yy",
                    "d-MMM-yy"},
                        CultureInfo.InvariantCulture, DateTimeStyles.None);

                    //if (result == null)
                    //{
                    //    result = result.Date;
                    //}

                    // Parse both US & UK formats.
                    //string[] formats = { "d/M/yyyy", "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy hh:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "dd/M/yyyy hh:mm:ss tt", "d/MM/yyyy hh:mm:ss tt", "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss tt", "MM/d/yyyy hh:mm:ss tt" };
                    //DateTime.TryParseExact(value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

                    //if (parseUKFormat)
                    //{
                    //    string[] formats = { "d/M/yyyy", "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy hh:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "dd/M/yyyy hh:mm:ss tt", "d/MM/yyyy hh:mm:ss tt", "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss tt", "MM/d/yyyy hh:mm:ss tt" };
                    //    DateTime.TryParseExact(value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
                    //}
                    //else
                    //{
                    //    DateTime.TryParse(value.ToString(), out result);
                    //}
                }
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }
    }

