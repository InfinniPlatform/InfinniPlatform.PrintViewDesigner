﻿using System;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Common
{
    public partial class DoubleEditor : PropertyEditorBase
    {
        public DoubleEditor()
        {
            InitializeComponent();
        }

        public string Format
        {
            get { return Editor.Mask; }
            set { Editor.Mask = Editor.DisplayFormatString = value; }
        }

        public double? MinValue
        {
            get { return ToDouble(Editor.MinValue); }
            set { Editor.MinValue = FromDouble(value); }
        }

        public double? MaxValue
        {
            get { return ToDouble(Editor.MaxValue); }
            set { Editor.MaxValue = FromDouble(value); }
        }

        private static double? ToDouble(decimal? value)
        {
            return (value != null) ? Convert.ToDouble(value) : default(double?);
        }

        private static decimal? FromDouble(double? value)
        {
            return (value != null) ? Convert.ToDecimal(value) : default(decimal?);
        }
    }
}