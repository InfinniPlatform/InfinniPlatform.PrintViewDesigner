using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorIndentSize : PropertyEditorBase
    {
        public PropertyEditorIndentSize()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("IndentSize", AppResources.ParagraphIndentSizeProperty, IndentSizeEditor, Visibility.Collapsed);
                AddProperty("IndentSizeUnit", AppResources.ParagraphIndentSizeUnitProperty, IndentSizeUnitEditor, Visibility.Collapsed);
            }
        }
    }
}