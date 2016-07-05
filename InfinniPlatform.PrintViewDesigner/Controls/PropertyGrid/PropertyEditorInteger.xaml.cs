using System;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    public sealed partial class PropertyEditorInteger : PropertyEditorBase
    {
        public PropertyEditorInteger()
        {
            InitializeComponent();
        }

        public int? MinValue
        {
            get { return ToInt32(Editor.MinValue); }
            set { Editor.MinValue = FromInt32(value); }
        }

        public int? MaxValue
        {
            get { return ToInt32(Editor.MaxValue); }
            set { Editor.MaxValue = FromInt32(value); }
        }

        private static int? ToInt32(decimal? value)
        {
            return (value != null) ? Convert.ToInt32(value) : default(int?);
        }

        private static decimal? FromInt32(int? value)
        {
            return (value != null) ? Convert.ToDecimal(value) : default(decimal?);
        }
    }
}