using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorVisibility : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Never", AppResources.VisibilityNever },
                  { "Always", AppResources.VisibilityAlways },
                  { "Source", AppResources.VisibilitySource }
              };

        public PropertyEditorVisibility()
        {
            InitializeComponent();
        }
    }
}