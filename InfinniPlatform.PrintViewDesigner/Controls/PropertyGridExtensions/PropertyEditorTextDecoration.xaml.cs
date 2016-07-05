using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorTextDecoration : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Normal", AppResources.TextDecorationNormal },
                  { "Overline", AppResources.TextDecorationOverline },
                  { "Strikethrough", AppResources.TextDecorationStrikethrough },
                  { "Underline", AppResources.TextDecorationUnderline }
              };

        public PropertyEditorTextDecoration()
        {
            InitializeComponent();
        }
    }
}