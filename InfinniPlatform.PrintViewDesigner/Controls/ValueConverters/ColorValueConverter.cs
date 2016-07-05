using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (object), typeof (Color))]
    internal sealed class ColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color? result = null;

            if (value != null)
            {
                try
                {
                    result = ColorConverter.ConvertFromString(value.ToString()) as Color?;
                }
                catch
                {
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;

            var color = value as Color?;

            if (color != null)
            {
                result = color.Value.ToString(culture);
            }

            return result;
        }
    }
}