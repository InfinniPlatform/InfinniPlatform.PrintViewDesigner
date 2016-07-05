using System;
using System.Globalization;
using System.Windows.Data;
using InfinniPlatform.PrintViewDesigner.Properties;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
    [ValueConversion(typeof (object), typeof (string))]
    internal sealed class DisplayFormatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;

            dynamic editValue = value;

            if (editValue != null)
            {
                if (editValue.BooleanFormat != null)
                {
                    result = string.Format(Resources.BooleanFormat,
                        CastToString(editValue.BooleanFormat.FalseText, bool.FalseString),
                        CastToString(editValue.BooleanFormat.TrueText, bool.TrueString));
                }
                else if (editValue.DateTimeFormat != null)
                {
                    result = string.Format(Resources.DateTimeFormat, CastToString(editValue.DateTimeFormat.Format, "G"));
                }
                else if (editValue.NumberFormat != null)
                {
                    result = string.Format(Resources.NumberFormat, CastToString(editValue.NumberFormat.Format, "n"));
                }
                else if (editValue.ObjectFormat != null)
                {
                    result = string.Format(Resources.ObjectFormat, CastToString(editValue.ObjectFormat.Format, "{}"));
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private string CastToString(object value, string defaultResult)
        {
            string result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToString(value);
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = defaultResult;
            }

            return result;
        }
    }
}