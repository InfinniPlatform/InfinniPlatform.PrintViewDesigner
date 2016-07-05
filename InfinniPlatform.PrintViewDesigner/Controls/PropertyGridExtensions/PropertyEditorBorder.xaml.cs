using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorBorder : PropertyEditorBase
    {
        public PropertyEditorBorder()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                var borderThickness = new PropertyEditorThickness();
                AddProperty("Thickness", AppResources.BorderThicknessProperty, borderThickness, Visibility.Visible);

                var borderColor = new PropertyEditorColor();
                AddProperty("Color", AppResources.BorderColorProperty, borderColor, Visibility.Visible);
            }
        }
    }
}