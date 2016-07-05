using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (object), typeof (double))]
    internal sealed class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToDouble(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToDouble(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }
    }
}