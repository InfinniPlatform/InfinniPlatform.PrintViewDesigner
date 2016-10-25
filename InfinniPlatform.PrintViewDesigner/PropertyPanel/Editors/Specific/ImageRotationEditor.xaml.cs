using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class ImageRotationEditor : PropertyEditorBase
    {
        public ImageRotationEditor()
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