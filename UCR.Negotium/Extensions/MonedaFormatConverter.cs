﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UCR.Negotium.Extensions
{
    public class MonedaFormatConverter : MarkupExtension, IValueConverter
    {
        private static InverseBoolConverter converter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new InverseBoolConverter());
        }
    }
}
