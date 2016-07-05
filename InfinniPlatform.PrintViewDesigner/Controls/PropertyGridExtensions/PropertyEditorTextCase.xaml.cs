using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorTextCase : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Normal", AppResources.TextCaseNormal },
                  { "SentenceCase", AppResources.TextCaseSentenceCase },
                  { "Lowercase", AppResources.TextCaseLowercase },
                  { "Uppercase", AppResources.TextCaseUppercase },
                  { "ToggleCase", AppResources.TextCaseToggleCase }
              };

        public PropertyEditorTextCase()
        {
            InitializeComponent();
        }
    }
}