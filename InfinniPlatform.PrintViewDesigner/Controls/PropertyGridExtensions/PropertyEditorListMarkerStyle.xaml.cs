using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorListMarkerStyle : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "None", AppResources.ListMarkerStyleNone },
                  { "Disc", AppResources.ListMarkerStyleDisc },
                  { "Circle", AppResources.ListMarkerStyleCircle },
                  { "Square", AppResources.ListMarkerStyleSquare },
                  { "Box", AppResources.ListMarkerStyleBox },
                  { "LowerRoman", AppResources.ListMarkerStyleLowerRoman },
                  { "UpperRoman", AppResources.ListMarkerStyleUpperRoman },
                  { "LowerLatin", AppResources.ListMarkerStyleLowerLatin },
                  { "UpperLatin", AppResources.ListMarkerStyleUpperLatin },
                  { "Decimal", AppResources.ListMarkerStyleDecimal }
              };

        public PropertyEditorListMarkerStyle()
        {
            InitializeComponent();
        }
    }
}