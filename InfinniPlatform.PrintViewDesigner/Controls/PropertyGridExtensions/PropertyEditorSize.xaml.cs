using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorSize : PropertyEditorBase
    {
        public PropertyEditorSize()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("Width", AppResources.SizeWidthProperty, WidthEditor, Visibility.Collapsed);
                AddProperty("Height", AppResources.SizeHeightProperty, HeightEditor, Visibility.Collapsed);
                AddProperty("SizeUnit", AppResources.SizeSizeUnitProperty, SizeUnitEditor, Visibility.Collapsed);
            }
        }
    }
}