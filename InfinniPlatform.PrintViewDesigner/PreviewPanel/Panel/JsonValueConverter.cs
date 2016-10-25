using System;
using System.Globalization;
using System.Windows.Data;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Panel
{
    [ValueConversion(typeof(object), typeof(string))]
    internal class JsonValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueString = value as string;

            if (!string.IsNullOrEmpty(valueString))
            {
                try
                {
                    return JsonObjectSerializer.Formated.Deserialize(valueString);
                }
                catch
                {
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    return JsonObjectSerializer.Formated.ConvertToString(value);
                }
                catch
                {
                }
            }

            return null;
        }
    }
}