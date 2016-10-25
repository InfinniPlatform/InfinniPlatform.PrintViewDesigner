using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DevExpress.Xpf.Bars;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.PrintViewDesigner.Images;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;

namespace InfinniPlatform.PrintViewDesigner.MenuPanel
{
    /// <summary>
    /// Панель инструментов для редактирования элементов печатного представления.
    /// </summary>
    public sealed partial class PrintViewMenuPanel : UserControl
    {
        public PrintViewMenuPanel()
        {
            InitializeComponent();

            // Block
            AddCreateButton(Properties.Resources.PrintSectionNode, PrintElementNodeType.PrintSectionNode, () => new PrintSection());
            AddCreateButton(Properties.Resources.PrintParagraphNode, PrintElementNodeType.PrintParagraphNode, CreateDefaultParagraph);
            AddCreateButton(Properties.Resources.PrintListNode, PrintElementNodeType.PrintListNode, () => new PrintList());
            AddCreateButton(Properties.Resources.PrintTableNode, PrintElementNodeType.PrintTableNode, () => new PrintTable());
            AddCreateButton(Properties.Resources.PrintLineNode, PrintElementNodeType.PrintLineNode, () => new PrintLine());
            AddCreateButton(Properties.Resources.PrintPageBreakNode, PrintElementNodeType.PrintPageBreakNode, () => new PrintPageBreak(), menuSeparator: true);

            // Inline
            AddCreateButton(Properties.Resources.PrintSpanNode, PrintElementNodeType.PrintSpanNode, CreateDefaultSpan);
            AddCreateButton(Properties.Resources.PrintBoldNode, PrintElementNodeType.PrintBoldNode, CreateDefaultBold);
            AddCreateButton(Properties.Resources.PrintItalicNode, PrintElementNodeType.PrintItalicNode, CreateDefaultItalic);
            AddCreateButton(Properties.Resources.PrintUnderlineNode, PrintElementNodeType.PrintUnderlineNode, CreateDefaultUnderline);
            AddCreateButton(Properties.Resources.PrintHyperlinkNode, PrintElementNodeType.PrintHyperlinkNode, CreateDefaultHyperlink);
            AddCreateButton(Properties.Resources.PrintRunNode, PrintElementNodeType.PrintRunNode, () => new PrintRun());
            AddCreateButton(Properties.Resources.PrintImageNode, PrintElementNodeType.PrintImageNode, () => new PrintImage());
            AddCreateButton(Properties.Resources.PrintBarcodeEan13Node, PrintElementNodeType.PrintBarcodeEan13Node, () => new PrintBarcodeEan13());
            AddCreateButton(Properties.Resources.PrintBarcodeQrNode, PrintElementNodeType.PrintBarcodeQrNode, () => new PrintBarcodeQr());
            AddCreateButton(Properties.Resources.PrintLineBreakNode, PrintElementNodeType.PrintLineBreakNode, () => new PrintLineBreak(), menuSeparator: true);

            // Table
            AddCreateButton(Properties.Resources.PrintTableColumnNode, PrintElementNodeType.PrintTableColumnNode, () => new PrintTableColumn());
            AddCreateButton(Properties.Resources.PrintTableRowNode, PrintElementNodeType.PrintTableRowNode, () => new PrintTableRow());
            AddCreateButton(Properties.Resources.PrintTableCellNode, PrintElementNodeType.PrintTableCellNode, CreateDefaultTableCell);

            // Style
            AddCreateButton(Properties.Resources.PrintViewStyle, PrintElementNodeType.PrintStyleNode, CreateDefaultStyle, menuSeparator: true);

            // File
            AddModifyButton("ImportButton", Properties.Resources.Import, "Import", OnImportClick);
            AddModifyButton("ExportButton", Properties.Resources.Export, "Export", OnExportClick, menuSeparator: true);

            // Preview
            AddModifyButton("PreviewDataButton", Properties.Resources.PreviewData, "PreviewData", OnPreviewDataClick);
            AddModifyButton("PreviewPdfButton", Properties.Resources.PreviewPdf, "PreviewPdf", OnPreviewPdfClick);
            AddModifyButton("PreviewHtmlButton", Properties.Resources.PreviewHtml, "PreviewHtml", OnPreviewHtmlClick, menuSeparator: true);

            // Layout
            _cutElementButton = AddModifyButton(nameof(_cutElementButton), Properties.Resources.Cut, "Cut", OnCutChildElement, buttonEnabled: false);
            _copyElementButton = AddModifyButton(nameof(_copyElementButton), Properties.Resources.Copy, "Copy", OnCopyChildElement, buttonEnabled: false);
            _pasteElementButton = AddModifyButton(nameof(_pasteElementButton), Properties.Resources.Paste, "Paste", OnPasteChildElement, buttonEnabled: false);
            _deleteElementButton = AddModifyButton(nameof(_deleteElementButton), Properties.Resources.Delete, "Delete", OnDeleteChildElement, buttonEnabled: false);
            _moveUpElementButton = AddModifyButton(nameof(_moveUpElementButton), Properties.Resources.MoveUp, "MoveUp", OnMoveUpChildElement, buttonEnabled: false);
            _moveDownElementButton = AddModifyButton(nameof(_moveDownElementButton), Properties.Resources.MoveDown, "MoveDown", OnMoveDownChildElement, buttonEnabled: false, menuSeparator: true);

            // Helper
            AddModifyButton("ExpandAllButton", Properties.Resources.ExpandAll, "ExpandAll", OnExpandAllClick);
            AddModifyButton("CollapseAllButton", Properties.Resources.CollapseAll, "CollapseAll", OnCollapseAllClick, menuSeparator: true);
        }


        private readonly BarButtonItem _cutElementButton;
        private readonly BarButtonItem _copyElementButton;
        private readonly BarButtonItem _pasteElementButton;
        private readonly BarButtonItem _deleteElementButton;
        private readonly BarButtonItem _moveUpElementButton;
        private readonly BarButtonItem _moveDownElementButton;


        // DocumentTemplate

        public static readonly DependencyProperty DocumentTemplateProperty
            = DependencyProperty.Register(nameof(DocumentTemplate),
                                          typeof(PrintDocument),
                                          typeof(PrintViewMenuPanel));

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
                                          typeof(PrintViewMenuPanel),
                                          new FrameworkPropertyMetadata(OnSelectedElementChanged));

        /// <summary>
        /// Элемент печатного представления, который выделен в дереве.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        private static void OnSelectedElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewMenuPanel;

            if (control != null)
            {
                var parentNode = e.NewValue as PrintElementNode;

                var hasElementNode = (parentNode != null);
                var hasElementParentNode = hasElementNode && (parentNode.Parent != null);

                // Регулировка доступности кнопок создания

                foreach (var itemLink in control.ToolboxCreateElementBar.ItemLinks.OfType<BarButtonItemLink>())
                {
                    var item = itemLink.Item;


                    if (parentNode != null)
                    {
                        var childNode = CreateElementNode(parentNode, item);

                        // TODO: Оптимизировать способ определения возможности вставки.
                        item.IsEnabled = (parentNode.CanInsertChild?.Invoke(childNode) == true);
                    }
                    else
                    {
                        item.IsEnabled = false;
                    }
                }

                // Регулировка доступности прочих операций

                control._cutElementButton.IsEnabled = hasElementNode && (parentNode.CanCut?.Invoke() == true);
                control._copyElementButton.IsEnabled = hasElementNode && (parentNode.CanCopy?.Invoke() == true);
                control._pasteElementButton.IsEnabled = hasElementNode && (parentNode.CanPaste?.Invoke() == true);

                control._deleteElementButton.IsEnabled = hasElementParentNode && (parentNode.Parent.CanDeleteChild?.Invoke(parentNode, true) == true);
                control._moveUpElementButton.IsEnabled = hasElementParentNode && (parentNode.Parent.CanMoveChild?.Invoke(parentNode, -1) == true);
                control._moveDownElementButton.IsEnabled = hasElementParentNode && (parentNode.Parent.CanMoveChild?.Invoke(parentNode, +1) == true);
            }
        }


        // OnExpandAll

        public static readonly RoutedEvent OnExpandAllEvent
            = EventManager.RegisterRoutedEvent(nameof(OnExpandAll),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие разворачивания всех элементов в дереве печатного представления.
        /// </summary>
        public event RoutedEventHandler OnExpandAll
        {
            add { AddHandler(OnExpandAllEvent, value); }
            remove { RemoveHandler(OnExpandAllEvent, value); }
        }

        private void OnExpandAllClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnExpandAllEvent));
        }


        // OnCollapseAll

        public static readonly RoutedEvent OnCollapseAllEvent
            = EventManager.RegisterRoutedEvent(nameof(OnCollapseAll),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие сворачивания всех элементов в дереве печатного представления.
        /// </summary>
        public event RoutedEventHandler OnCollapseAll
        {
            add { AddHandler(OnCollapseAllEvent, value); }
            remove { RemoveHandler(OnCollapseAllEvent, value); }
        }

        private void OnCollapseAllClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnCollapseAllEvent));
        }


        // OnImport

        public static readonly RoutedEvent OnImportEvent
            = EventManager.RegisterRoutedEvent(nameof(OnImport),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие импорта печатного представления.
        /// </summary>
        public event RoutedEventHandler OnImport
        {
            add { AddHandler(OnImportEvent, value); }
            remove { RemoveHandler(OnImportEvent, value); }
        }

        private void OnImportClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnImportEvent));
        }


        // OnExport

        public static readonly RoutedEvent OnExportEvent
            = EventManager.RegisterRoutedEvent(nameof(OnExport),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие экспорта печатного представления.
        /// </summary>
        public event RoutedEventHandler OnExport
        {
            add { AddHandler(OnExportEvent, value); }
            remove { RemoveHandler(OnExportEvent, value); }
        }

        private void OnExportClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnExportEvent));
        }


        // OnPreviewData

        public static readonly RoutedEvent OnPreviewDataEvent
            = EventManager.RegisterRoutedEvent(nameof(OnPreviewData),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие установки данных печатного представления.
        /// </summary>
        public event RoutedEventHandler OnPreviewData
        {
            add { AddHandler(OnPreviewDataEvent, value); }
            remove { RemoveHandler(OnPreviewDataEvent, value); }
        }

        private void OnPreviewDataClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnPreviewDataEvent));
        }


        // OnPreviewPdf

        public static readonly RoutedEvent OnPreviewPdfEvent
            = EventManager.RegisterRoutedEvent(nameof(OnPreviewPdf),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие предпросмотра печатного представления в виде PDF.
        /// </summary>
        public event RoutedEventHandler OnPreviewPdf
        {
            add { AddHandler(OnPreviewPdfEvent, value); }
            remove { RemoveHandler(OnPreviewPdfEvent, value); }
        }

        private void OnPreviewPdfClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnPreviewPdfEvent));
        }


        // OnPreviewHtml

        public static readonly RoutedEvent OnPreviewHtmlEvent
            = EventManager.RegisterRoutedEvent(nameof(OnPreviewHtml),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие предпросмотра печатного представления в виде HTML.
        /// </summary>
        public event RoutedEventHandler OnPreviewHtml
        {
            add { AddHandler(OnPreviewHtmlEvent, value); }
            remove { RemoveHandler(OnPreviewHtmlEvent, value); }
        }

        private void OnPreviewHtmlClick(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnPreviewHtmlEvent));
        }


        // ElementMetadataChanged

        public static readonly RoutedEvent ElementMetadataChangedEvent
            = EventManager.RegisterRoutedEvent(nameof(ElementMetadataChanged),
                                               RoutingStrategy.Bubble,
                                               typeof(PropertyValueChangedEventHandler),
                                               typeof(PrintViewMenuPanel));

        /// <summary>
        /// Событие изменения элемента печатного представления.
        /// </summary>
        public event PropertyValueChangedEventHandler ElementMetadataChanged
        {
            add { AddHandler(ElementMetadataChangedEvent, value); }
            remove { RemoveHandler(ElementMetadataChangedEvent, value); }
        }

        private void OnElementMetadataChanged()
        {
            RaiseEvent(new PropertyValueChangedEventArgs(ElementMetadataChangedEvent, null, null, null));
        }


        // Helpers

        private void AddCreateButton(string buttonCaption,
                                     PrintElementNodeType elementType,
                                     Func<object> elementTemplate,
                                     bool menuSeparator = false)
        {
            var buttonName = elementType.ToString();

            Func<PrintElementNode, PrintElementNode> nodeFactory
                = p => new PrintElementNode(p, elementType, elementTemplate());

            var createButton = new BarButtonItem
                               {
                                   Name = buttonName,
                                   Content = buttonCaption,
                                   Glyph = ImageRepository.GetImage(elementType.ToString()),
                                   IsEnabled = false,
                                   Tag = nodeFactory
                               };

            var createButtonLink = new BarButtonItemLink
                                   {
                                       BarItemName = buttonName
                                   };

            createButton.ItemClick += OnInsertChildElement;

            ToolboxBarManager.Items.Add(createButton);
            ToolboxCreateElementBar.ItemLinks.Add(createButtonLink);

            if (menuSeparator)
            {
                ToolboxCreateElementBar.ItemLinks.Add(new BarItemLinkSeparator());
            }
        }

        private BarButtonItem AddModifyButton(string buttonName,
                                              string buttonCaption,
                                              string buttonImage,
                                              ItemClickEventHandler buttonHandler,
                                              bool buttonEnabled = true,
                                              bool menuSeparator = false)
        {
            var modifyButton = new BarButtonItem
                               {
                                   Name = buttonName,
                                   Content = buttonCaption,
                                   Glyph = ImageRepository.GetImage(buttonImage),
                                   IsEnabled = buttonEnabled
                               };

            var modifyButtonLink = new BarButtonItemLink
                                   {
                                       BarItemName = buttonName
                                   };

            modifyButton.ItemClick += buttonHandler;

            ToolboxBarManager.Items.Add(modifyButton);
            ToolboxModifyElementBar.ItemLinks.Add(modifyButtonLink);

            if (menuSeparator)
            {
                ToolboxModifyElementBar.ItemLinks.Add(new BarItemLinkSeparator());
            }

            return modifyButton;
        }

        private static PrintElementNode CreateElementNode(PrintElementNode parentNode, BarItem elementItem)
        {
            var nodeFactory = elementItem.Tag as Func<PrintElementNode, PrintElementNode>;

            return nodeFactory?.Invoke(parentNode);
        }


        // Handlers

        private void OnInsertChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => el.InsertChild?.Invoke(CreateElementNode(el, e.Item)) == true);
        }

        private void OnDeleteChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.DeleteChild?.Invoke(el, true) == true);
        }

        private void OnMoveUpChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild?.Invoke(el, -1) == true);
        }

        private void OnMoveDownChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild?.Invoke(el, +1) == true);
        }

        private void OnCutChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => !el.Cut?.Invoke() == true);
        }

        private void OnCopyChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => !el.Copy?.Invoke() == true);
        }

        private void OnPasteChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => el.Paste?.Invoke() == true);
        }

        private void ModifySelectedElement(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null)
            {
                if (action(selectedElement))
                {
                    OnElementMetadataChanged();
                }
            }
        }

        private void ModifySelectedElementParent(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement?.Parent != null)
            {
                if (action(selectedElement))
                {
                    OnElementMetadataChanged();
                }
            }
        }


        // Element Factories

        private static PrintParagraph CreateDefaultParagraph()
        {
            var paragraph = new PrintParagraph();

            paragraph.Inlines.Add(new PrintRun());

            return paragraph;
        }

        private static PrintSpan CreateDefaultSpan()
        {
            var span = new PrintSpan();

            span.Inlines.Add(new PrintRun());

            return span;
        }

        private static PrintSpan CreateDefaultBold()
        {
            var bold = new PrintBold();

            bold.Inlines.Add(new PrintRun());

            return bold;
        }

        private static PrintSpan CreateDefaultItalic()
        {
            var italic = new PrintItalic();

            italic.Inlines.Add(new PrintRun());

            return italic;
        }

        private static PrintSpan CreateDefaultUnderline()
        {
            var underline = new PrintUnderline();

            underline.Inlines.Add(new PrintRun());

            return underline;
        }

        private static PrintSpan CreateDefaultHyperlink()
        {
            var hyperlink = new PrintHyperlink();

            hyperlink.Inlines.Add(new PrintRun());

            return hyperlink;
        }

        private static PrintTableCell CreateDefaultTableCell()
        {
            var paragraph = new PrintParagraph();

            paragraph.Inlines.Add(new PrintRun());

            var tableCell = new PrintTableCell();

            tableCell.Block = paragraph;

            return tableCell;
        }

        private PrintStyle CreateDefaultStyle()
        {
            var style = new PrintStyle();

            style.Name = $"Style{DocumentTemplate?.Styles?.Count + 1}";

            return style;
        }
    }
}