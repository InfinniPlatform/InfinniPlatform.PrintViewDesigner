using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorViewType : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "ObjectView", AppResources.ViewTypeObjectView },
                  { "ListView", AppResources.ViewTypeListView }
              };

        public PropertyEditorViewType()
        {
            InitializeComponent();
        }
    }
}