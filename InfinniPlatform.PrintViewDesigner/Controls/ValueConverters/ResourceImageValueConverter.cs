using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using InfinniPlatform.PrintViewDesigner.Images;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (string), typeof (ImageSource))]
    internal sealed class ResourceImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageRepository.GetImage(parameter as string, value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}