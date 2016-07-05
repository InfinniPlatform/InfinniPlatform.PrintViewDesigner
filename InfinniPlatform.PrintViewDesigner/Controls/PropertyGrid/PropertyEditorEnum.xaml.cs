namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid
{
    public sealed partial class PropertyEditorEnum : PropertyEditorBase
    {
        public PropertyEditorEnum()
        {
            InitializeComponent();
        }

        public string ValueMember
        {
            get { return Editor.ValueMember; }
            set { Editor.ValueMember = value; }
        }

        public string DisplayMember
        {
            get { return Editor.DisplayMember; }
            set { Editor.DisplayMember = value; }
        }

        public object ItemsSource
        {
            get { return Editor.ItemsSource; }
            set { Editor.ItemsSource = value; }
        }
    }
}