namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    public sealed partial class PropertyEditorString : PropertyEditorBase
    {
        public PropertyEditorString()
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