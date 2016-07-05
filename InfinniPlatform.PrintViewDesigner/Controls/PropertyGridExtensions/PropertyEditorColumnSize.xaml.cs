using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorColumnSize : PropertyEditorBase
    {
        public PropertyEditorColumnSize()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("Size", AppResources.ColumnSizeProperty, ColumnSizeEditor, Visibility.Collapsed);
                AddProperty("SizeUnit", AppResources.ColumnSizeUnitProperty, ColumnSizeUnitEditor, Visibility.Collapsed);
            }
        }
    }
}