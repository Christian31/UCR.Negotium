using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UCR.Negotium.Extensions
{
    public class InverseBoolConverter : MarkupExtension, IValueConverter
    {
        private static InverseBoolConverter converter;

        public InverseBoolConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !((bool)value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !((bool)value);
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new InverseBoolConverter());
        }
    }
}
