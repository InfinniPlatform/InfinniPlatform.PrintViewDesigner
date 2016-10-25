using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof(object), typeof(string))]
    internal class BooleanFormatValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool? value = null;
            string falseText = null;
            string trueText = null;

            if (values != null)
            {
                if (values.Length > 0)
                {
                    try
                    {
                        value = System.Convert.ToBoolean(values[0], culture);
                    }
                    catch
                    {
                    }

                    if (values.Length > 1)
                    {
                        try
                        {
                            falseText = System.Convert.ToString(values[1], culture);
                        }
                        catch
                        {
                        }

                        if (values.Length > 2)
                        {
                            try
                            {
                                trueText = System.Convert.ToString(values[2], culture);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            if (value != null)
            {
                if (string.IsNullOrEmpty(falseText))
                {
                    falseText = bool.FalseString;
                }

                if (string.IsNullOrEmpty(trueText))
                {
                    trueText = bool.TrueString;
                }

                return value.Value ? trueText : falseText;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}