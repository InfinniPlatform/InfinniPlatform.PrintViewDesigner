using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Common
{
    public partial class EnumEditor : PropertyEditorBase
    {
        public EnumEditor()
        {
            InitializeComponent();
        }

        public IDictionary<object, string> ItemsSource
        {
            get { return (IDictionary<object, string>)Editor.ItemsSource; }
            set { Editor.ItemsSource = value; }
        }
    }
}