using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorThickness : PropertyEditorBase
    {
        public PropertyEditorThickness()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("Top", AppResources.ThicknessTopProperty, TopEditor, Visibility.Collapsed);
                AddProperty("Bottom", AppResources.ThicknessBottomProperty, BottomEditor, Visibility.Collapsed);
                AddProperty("Left", AppResources.ThicknessLeftProperty, LeftEditor, Visibility.Collapsed);
                AddProperty("Right", AppResources.ThicknessRightProperty, RightEditor, Visibility.Collapsed);
                AddProperty("All", AppResources.ThicknessAllProperty, AllEditor, Visibility.Collapsed);
                AddProperty("SizeUnit", AppResources.ThicknessSizeUnitProperty, SizeUnitEditor, Visibility.Collapsed);
            }
        }
    }
}