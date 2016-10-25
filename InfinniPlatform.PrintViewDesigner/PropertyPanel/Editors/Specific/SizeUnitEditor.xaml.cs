using System.Collections.Generic;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class SizeUnitEditor : PropertyEditorBase
    {
        public SizeUnitEditor()
        {
            InitializeComponent();

            Editor.ItemsSource = new Dictionary<object, string>
                                 {
                                     { PrintSizeUnit.Pt, Properties.Resources.SizeUnitPt },
                                     { PrintSizeUnit.Px, Properties.Resources.SizeUnitPx },
                                     { PrintSizeUnit.In, Properties.Resources.SizeUnitIn },
                                     { PrintSizeUnit.Cm, Properties.Resources.SizeUnitCm },
                                     { PrintSizeUnit.Mm, Properties.Resources.SizeUnitMm }
                                 };
        }
    }
}