using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorRotation : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "Rotate0", AppResources.RotationRotate0 },
                  { "Rotate90", AppResources.RotationRotate90 },
                  { "Rotate180", AppResources.RotationRotate180 },
                  { "Rotate270", AppResources.RotationRotate270 }
              };

        public PropertyEditorRotation()
        {
            InitializeComponent();
        }
    }
}