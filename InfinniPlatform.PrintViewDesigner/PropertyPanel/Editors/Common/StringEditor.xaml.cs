namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Common
{
    public partial class StringEditor : PropertyEditorBase
    {
        public StringEditor()
        {
            InitializeComponent();
        }

        public string Regex
        {
            get { return Editor.Mask; }
            set { Editor.Mask = value; }
        }
    }
}