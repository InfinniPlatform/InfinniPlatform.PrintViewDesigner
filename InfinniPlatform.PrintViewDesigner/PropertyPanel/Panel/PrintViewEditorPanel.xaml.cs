using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Panel
{
    /// <summary>
    /// Редактор свойств элемента печатного представления.
    /// </summary>
    public partial class PrintViewEditorPanel : UserControl
    {
        public PrintViewEditorPanel()
        {
            InitializeComponent();

            _editorFactory = new PrintViewEditorFactory(() => DocumentTemplate);
        }


        private readonly PrintViewEditorFactory _editorFactory;


        // DocumentTemplate

        public static readonly DependencyProperty DocumentTemplateProperty
            = DependencyProperty.Register(nameof(DocumentTemplate),
                                          typeof(PrintDocument),
                                          typeof(PrintViewEditorPanel));

        /// <summary>
        /// Шаблон документа печатного представления.
        /// </summary>
        public PrintDocument DocumentTemplate
        {
            get { return (PrintDocument)GetValue(DocumentTemplateProperty); }
            set { SetValue(DocumentTemplateProperty, value); }
        }


        // SelectedElement

        public static readonly DependencyProperty SelectedElementProperty
            = DependencyProperty.Register(nameof(SelectedElement),
                                          typeof(PrintElementNode),
                                          typeof(PrintViewEditorPanel),
                                          new FrameworkPropertyMetadata(OnSelectedElementChanged));

        /// <summary>
        /// Выделенный элемент.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        private static void OnSelectedElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewEditorPanel;

            if (control != null)
            {
                control.RemoveElementEditor();
                control.AddElementEditor(e.NewValue as PrintElementNode);
            }
        }

        private void AddElementEditor(PrintElementNode elementNode)
        {
            PropertyEditorBase elementEditor = null;

            if (elementNode?.ElementMetadata != null && !elementNode.IsLogicGroup())
            {
                elementEditor = _editorFactory.CreateEditor(elementNode.ElementMetadata.GetType());
            }

            if (elementEditor != null)
            {
                elementEditor.DataContext = elementNode.ElementMetadata;
                //element.EditValue = elementNode.ElementMetadata;
                elementEditor.EditValueChanged += OnElementEditorValueChanged;
            }

            Content = elementEditor;
        }

        private void RemoveElementEditor()
        {
            var elementEditor = (Content as PropertyEditorBase);

            Content = null;

            if (elementEditor != null)
            {
                elementEditor.EditValueChanged -= OnElementEditorValueChanged;
                //elementEditor.EditValue = null;
                elementEditor.DataContext = null;
            }
        }


        // ElementMetadataChanged

        public static readonly RoutedEvent ElementMetadataChangedEvent
            = EventManager.RegisterRoutedEvent(nameof(ElementMetadataChanged),
                                               RoutingStrategy.Bubble,
                                               typeof(PropertyValueChangedEventHandler),
                                               typeof(PrintViewEditorPanel));

        /// <summary>
        /// Событие изменения выделенного элемента.
        /// </summary>
        public event PropertyValueChangedEventHandler ElementMetadataChanged
        {
            add { AddHandler(ElementMetadataChangedEvent, value); }
            remove { RemoveHandler(ElementMetadataChangedEvent, value); }
        }

        private void OnElementEditorValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Property))
            {
                RaiseEvent(e.Create(ElementMetadataChangedEvent));
            }
        }
    }
}