using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (object), typeof (int))]
    internal sealed class IntegerValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToInt32(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToInt32(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }
    }
}