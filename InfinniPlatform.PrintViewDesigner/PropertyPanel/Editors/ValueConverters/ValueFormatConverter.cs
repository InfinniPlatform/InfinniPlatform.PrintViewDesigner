using System;
using System.Globalization;
using System.Windows.Data;

using InfinniPlatform.PrintView.Model.Format;
using InfinniPlatform.PrintViewDesigner.Properties;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof(object), typeof(string))]
    internal class ValueFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;

            var editValue = value as ValueFormat;

            if (editValue != null)
            {
                if (editValue is BooleanFormat)
                {
                    result = string.Format(Resources.BooleanFormat,
                        CastToString(((BooleanFormat)editValue).FalseText, bool.FalseString),
                        CastToString(((BooleanFormat)editValue).TrueText, bool.TrueString));
                }
                else if (editValue is DateTimeFormat)
                {
                    result = string.Format(Resources.DateTimeFormat,
                        CastToString(((DateTimeFormat)editValue).Format, "G"));
                }
                else if (editValue is NumberFormat)
                {
                    result = string.Format(Resources.NumberFormat,
                        CastToString(((NumberFormat)editValue).Format, "n"));
                }
                else if (editValue is ObjectFormat)
                {
                    result = string.Format(Resources.ObjectFormat,
                        CastToString(((ObjectFormat)editValue).Format, "{}"));
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static string CastToString(object value, string defaultResult)
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