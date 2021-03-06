﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.ValueConverters
{
    [ValueConversion(typeof (object), typeof (bool))]
    internal class BooleanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToBoolean(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? result = null;

            if (value != null)
            {
                try
                {
                    result = System.Convert.ToBoolean(value, culture);
                }
                catch
                {
                }
            }

            return result;
        }
    }
}