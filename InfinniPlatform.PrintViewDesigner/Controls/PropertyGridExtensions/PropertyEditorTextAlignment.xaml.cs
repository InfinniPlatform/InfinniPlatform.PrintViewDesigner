using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorTextAlignment : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "Left", AppResources.TextAlignmentLeft },
                  { "Center", AppResources.TextAlignmentCenter },
                  { "Right", AppResources.TextAlignmentRight },
                  { "Justify", AppResources.TextAlignmentJustify }
              };

        public PropertyEditorTextAlignment()
        {
            InitializeComponent();
        }
    }
}