﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof (object), typeof (string))]
    internal class StringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToString(value, culture);
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

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToString(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }
    }
}