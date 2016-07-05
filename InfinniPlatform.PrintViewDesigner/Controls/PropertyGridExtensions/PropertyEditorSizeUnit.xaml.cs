using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorSizeUnit : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Pt", AppResources.SizeUnitPt },
                  { "Px", AppResources.SizeUnitPx },
                  { "In", AppResources.SizeUnitIn },
                  { "Cm", AppResources.SizeUnitCm },
                  { "Mm", AppResources.SizeUnitMm }
              };

        public PropertyEditorSizeUnit()
        {
            InitializeComponent();
        }
    }
}