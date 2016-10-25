using System.Collections.Generic;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors
{
    /// <summary>
    /// Редактор свойств объекта в виде дерева.
    /// </summary>
    public partial class PropertyEditor : PropertyEditorBase
    {
        public PropertyEditor()
        {
            InitializeComponent();
        }

        public IEnumerable<PropertyEditorBase> Editors
        {
            get { return (IEnumerable<PropertyEditorBase>)TreeList.ItemsSource; }
            set { TreeList.ItemsSource = value; }
        }
    }
}