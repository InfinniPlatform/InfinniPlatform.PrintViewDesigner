﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof (object), typeof (string))]
    internal class DateTimeFormatValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? value = null;
            string format = null;

            if (values != null)
            {
                if (values.Length > 0)
                {
                    try
                    {
                        value = System.Convert.ToDateTime(values[0], culture);
                    }
                    catch
                    {
                    }

                    if (values.Length > 1)
                    {
                        try
                        {
                            format = System.Convert.ToString(values[1], culture);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (value != null)
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = "G";
                }

                try
                {
                    return value.Value.ToString(format);
                }
                catch
                {
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}