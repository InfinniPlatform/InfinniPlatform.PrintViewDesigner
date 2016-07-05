using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorImageStretch : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "None", AppResources.ImageStretchNone },
                  { "Fill", AppResources.ImageStretchFill },
                  { "Uniform", AppResources.ImageStretchUniform }
              };

        public PropertyEditorImageStretch()
        {
            InitializeComponent();
        }
    }
}