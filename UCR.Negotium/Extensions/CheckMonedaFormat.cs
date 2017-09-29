using System.Text.RegularExpressions;

namespace UCR.Negotium.Extensions
{
    public static class CheckMonedaFormat
    {
        private const string RGXNUMBERS = "[^0-9+-]";
        private const string STRFORMAT = "{0:N}";

        public static string CheckStringFormat(this string stringVal, double defaultValue=0)
        {
            bool requireFormat = false;
            if (!stringVal.Length.Equals(0))
            {
                double costoUnitario = 0;
                if (!double.TryParse(stringVal, out costoUnitario))
                {
                    stringVal = Regex.Replace(stringVal, RGXNUMBERS, "");
                    if (stringVal.Equals(string.Empty))
                        stringVal = defaultValue.ToString();

                    requireFormat = true;
                }
            }
            else
            {
                stringVal = defaultValue.ToString();
                requireFormat = true;
            }

            return requireFormat ? string.Format(STRFORMAT, stringVal) : stringVal;
        }
    }
}
