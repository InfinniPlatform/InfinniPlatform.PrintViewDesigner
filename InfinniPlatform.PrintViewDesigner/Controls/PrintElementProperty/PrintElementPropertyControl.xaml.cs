using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintElementProperty
{
    /// <summary>
    ///     Редактор свойств элемента печатного представления.
    /// </summary>
    public sealed partial class PrintElementPropertyControl : UserControl
    {
        // ElementEditors

        private readonly Dictionary<string, Func<PropertyGridControl>> _elementEditors
            = new Dictionary<string, Func<PropertyGridControl>>(StringComparer.OrdinalIgnoreCase);

        public PrintElementPropertyControl()
        {
            InitializeComponent();

            var factory = new PrintElementPropertyEditorFactory(() => PrintView);

            // Blocks
            _elementEditors.Add("Section", factory.GetSectionEditor);
            _elementEditors.Add("Paragraph", factory.GetParagraphEditor);
            _elementEditors.Add("List", factory.GetListEditor);
            _elementEditors.Add("Table", factory.GetTableEditor);
            _elementEditors.Add("TableRow", factory.GetTableRowEditor);
            _elementEditors.Add("TableColumn", factory.GetTableColumnEditor);
            _elementEditors.Add("TableCell", factory.GetTableCellEditor);
            _elementEditors.Add("Line", factory.GetLineEditor);
            _elementEditors.Add("PageBreak", factory.GetPageBreakEditor);

            // Inlines
            _elementEditors.Add("Span", factory.GetSpanEditor);
            _elementEditors.Add("Bold", factory.GetBoldEditor);
            _elementEditors.Add("Italic", factory.GetItalicEditor);
            _elementEditors.Add("Underline", factory.GetUnderlineEditor);
            _elementEditors.Add("Hyperlink", factory.GetHyperlinkEditor);
            _elementEditors.Add("LineBreak", factory.GetLineBreakEditor);
            _elementEditors.Add("Run", factory.GetRunEditor);
            _elementEditors.Add("Image", factory.GetImageEditor);
            _elementEditors.Add("BarcodeEan13", factory.GetBarcodeEan13Editor);
            _elementEditors.Add("BarcodeQr", factory.GetBarcodeQrEditor);

            // Views
            _elementEditors.Add("PrintView", factory.GetPrintViewEditor);
            _elementEditors.Add("PrintViewStyle", factory.GetPrintViewStyleEditor);
        }

        /// <summary>
        ///     Печатное представление.
        /// </summary>
        public object PrintView
        {
            get { return GetValue(PrintViewProperty); }
            set { SetValue(PrintViewProperty, value); }
        }

        /// <summary>
        ///     Элемент печатного представления, который выделен в дереве.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode) GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        private PropertyGridControl GetElementEditor(string elementType)
        {
            if (!string.IsNullOrEmpty(elementType))
            {
                Func<PropertyGridControl> elementEditor;

                if (_elementEditors.TryGetValue(elementType, out elementEditor))
                {
                    return elementEditor();
                }
            }

            return null;
        }

        private static void OnSelectedElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintElementPropertyControl;

            if (control != null)
            {
                control.RemoveElementEditor();
                control.AddElementEditor(e.NewValue as PrintElementNode);
            }
        }

        private void AddElementEditor(PrintElementNode elementNode)
        {
            PropertyGridControl elementEditor = null;

            if (elementNode != null)
            {
                elementEditor = GetElementEditor(elementNode.ElementType);

                if (elementEditor != null)
                {
                    elementEditor.EditValue = elementNode.ElementMetadata;
                    elementEditor.EditValueChanged += OnElementEditorValueChanged;
                }
            }

            Content = elementEditor;
        }

        private void RemoveElementEditor()
        {
            var elementEditor = (Content as PropertyGridControl);

            Content = null;

            if (elementEditor != null)
            {
                elementEditor.EditValueChanged -= OnElementEditorValueChanged;
                elementEditor.EditValue = null;
            }
        }

        /// <summary>
        ///     Событие изменения метаданных выделенного элемента печатного представления.
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

        // PrintView

        public static readonly DependencyProperty PrintViewProperty = DependencyProperty.Register("PrintView",
            typeof (object), typeof (PrintElementPropertyControl));

        // SelectedElement

        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof (PrintElementNode),
                typeof (PrintElementPropertyControl), new FrameworkPropertyMetadata(OnSelectedElementChanged));

        // ElementMetadataChanged

        public static readonly RoutedEvent ElementMetadataChangedEvent =
            EventManager.RegisterRoutedEvent("ElementMetadataChanged", RoutingStrategy.Bubble,
                typeof (PropertyValueChangedEventHandler), typeof (PrintElementPropertyControl));
    }
}