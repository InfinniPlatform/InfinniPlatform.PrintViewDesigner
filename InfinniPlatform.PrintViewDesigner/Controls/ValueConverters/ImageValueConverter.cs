using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (object), typeof (BitmapImage))]
    internal sealed class ImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result = null;
            byte[] bytes = null;

            if (value is byte[])
            {
                bytes = (byte[]) value;
            }
            else if (value is string)
            {
                try
                {
                    bytes = System.Convert.FromBase64String((string) value);
                }
                catch
                {
                }
            }

            if (bytes != null)
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(bytes);
                    image.EndInit();

                    result = image;
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
            byte[] bytes = null;

            if (value is BitmapImage)
            {
                var image = (BitmapImage) value;
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            if (bytes != null)
            {
                try
                {
                    result = System.Convert.ToBase64String(bytes);
                }
                catch
                {
                }
            }

            return result;
        }
    }
}