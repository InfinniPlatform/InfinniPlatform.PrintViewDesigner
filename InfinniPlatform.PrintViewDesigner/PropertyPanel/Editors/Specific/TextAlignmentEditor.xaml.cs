using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class TextAlignmentEditor : PropertyEditorBase
    {
        public TextAlignmentEditor()
        {
            InitializeComponent();
        }


        public IDictionary<object, string> Items
        {
            get { return (IDictionary<object, string>)Editor.ItemsSource; }
            set { Editor.ItemsSource = value; }
        }
    }
}