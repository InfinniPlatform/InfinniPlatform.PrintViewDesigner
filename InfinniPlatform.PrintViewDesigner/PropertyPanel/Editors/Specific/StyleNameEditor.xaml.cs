using System;
using System.Windows;

using DevExpress.Xpf.Editors;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific
{
    public partial class StyleNameEditor : PropertyEditorBase
    {
        public StyleNameEditor()
        {
            InitializeComponent();
        }


        public Func<PrintDocument> DocumentTemplate { get; set; }


        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, true))
            {
                RefreshItems();
            }
        }

        private void OnPopupOpening(object sender, OpenPopupEventArgs e)
        {
            RefreshItems();
        }

        private void RefreshItems()
        {
            Editor.ItemsSource = DocumentTemplate?.Invoke()?.Styles;
        }
    }
}