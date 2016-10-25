using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using InfinniPlatform.PrintViewDesigner.Images;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof (string), typeof (ImageSource))]
    internal class ResourceImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter?.ToString();
            var valueString = value?.ToString();

            return ImageRepository.GetImage(parameterString, valueString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}