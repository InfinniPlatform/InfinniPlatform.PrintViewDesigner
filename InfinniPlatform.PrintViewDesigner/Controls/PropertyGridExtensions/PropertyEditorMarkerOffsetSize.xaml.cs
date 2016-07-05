using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorMarkerOffsetSize : PropertyEditorBase
    {
        public PropertyEditorMarkerOffsetSize()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("MarkerOffsetSize", AppResources.ListMarkerOffsetSizeProperty, MarkerOffsetSizeEditor, Visibility.Collapsed);
                AddProperty("MarkerOffsetSizeUnit", AppResources.ListMarkerOffsetSizeUnitProperty, MarkerOffsetSizeUnitEditor, Visibility.Collapsed);
            }
        }
    }
}