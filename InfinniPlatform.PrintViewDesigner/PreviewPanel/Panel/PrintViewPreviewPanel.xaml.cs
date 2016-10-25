using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Pdf;
using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Panel
{
    /// <summary>
    /// Элемент для предпросмотра шаблона печатного представления.
    /// </summary>
    public partial class PrintViewPreviewPanel : UserControl
    {
        private static readonly SolidColorBrush HighlightElementBrush
            = new SolidColorBrush(SystemColors.HighlightColor) { Opacity = 0.4 };


        public PrintViewPreviewPanel()
        {
            InitializeComponent();

            _printDocumentBuilder = CreatePrintDocumentBuilder();
            _printViewWriter = CreatePrintViewWriter();
            _printViewBuilder = CreatePrintViewBuilder(_printDocumentBuilder, _printViewWriter);
            _printViewPreviewBuilder = new PrintViewPreviewBuilder();
        }


        private readonly IPrintDocumentBuilder _printDocumentBuilder;
        private readonly IPrintViewWriter _printViewWriter;
        private readonly IPrintViewBuilder _printViewBuilder;
        private readonly PrintViewPreviewBuilder _printViewPreviewBuilder;


        private PrintDocumentMap _documentMap;


        private static IPrintDocumentBuilder CreatePrintDocumentBuilder()
        {
            return new PrintDocumentBuilder(true);
        }

        private static IPrintViewWriter CreatePrintViewWriter()
        {
            var result = new PrintViewWriter();

            var settings = PrintViewSettings.Default;

            var htmlWriter = new HtmlPrintDocumentWriter();
            var pdfWriter = new PdfPrintDocumentWriter(settings, htmlWriter);

            result.RegisterWriter(PrintViewFileFormat.Html, htmlWriter);
            result.RegisterWriter(PrintViewFileFormat.Pdf, pdfWriter);

            return result;
        }

        private static IPrintViewBuilder CreatePrintViewBuilder(IPrintDocumentBuilder printDocumentBuilder, IPrintViewWriter printViewWriter)
        {
            var printViewSerializer = new PrintViewSerializer();

            return new PrintViewBuilder(printViewSerializer, printDocumentBuilder, printViewWriter);
        }


        // DocumentTemplate

        public static readonly DependencyProperty DocumentTemplateProperty
            = DependencyProperty.Register(nameof(DocumentTemplate),
                                          typeof(PrintDocument),
                                          typeof(PrintViewPreviewPanel),
                                          new FrameworkPropertyMetadata(OnPrintViewChanged));

        /// <summary>
        /// Шаблон документа печатного представления.
        /// </summary>
        public PrintDocument DocumentTemplate
        {
            get { return (PrintDocument)GetValue(DocumentTemplateProperty); }
            set { SetValue(DocumentTemplateProperty, value); }
        }

        private static void OnPrintViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewPreviewPanel;

            var template = e.NewValue as PrintDocument;

            control?.RenderTemplatePreview(template);
        }

        private void RenderTemplatePreview(PrintDocument template)
        {
            PrintDocumentMap documentMap = null;
            FlowDocument flowDocument = null;

            if (template != null)
            {
                SetPageSize(template);

                try
                {
                    documentMap = new PrintDocumentMap();

                    var document = _printDocumentBuilder.Build(template, dataSource: null, documentMap: documentMap);

                    flowDocument = _printViewPreviewBuilder.CreatePreview(document, documentMap);
                }
                catch (Exception error)
                {
                    documentMap = null;

                    var errorMessage = new Paragraph(new Run(error.Message));

                    flowDocument = new FlowDocument(errorMessage) { Foreground = Brushes.Red };
                }
            }

            _documentMap = documentMap;

            PageContainerEditor.Document = flowDocument;
        }

        private void SetPageSize(PrintDocument template)
        {
            var pagePaddig = template.PagePadding ?? PrintViewDefaults.Document.PagePadding;

            if (pagePaddig != null)
            {
                var paddingLeft = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Left, pagePaddig.SizeUnit);
                var paddingTop = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Top, pagePaddig.SizeUnit);
                var paddingRigt = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Right, pagePaddig.SizeUnit);
                var paddingBottom = FlowBuilderHelper.GetSizeInPixels(pagePaddig.Bottom, pagePaddig.SizeUnit);

                PageContainer.Padding = new Thickness(paddingLeft, paddingTop, paddingRigt, paddingBottom);
            }
            else
            {
                PageContainer.Padding = new Thickness(0);
            }

            var pageSize = template.PageSize ?? PrintViewDefaults.Document.PageSize;

            if (pageSize != null)
            {
                var pageWidth = FlowBuilderHelper.GetSizeInPixels(pageSize.Width, pageSize.SizeUnit);

                PageContainer.Width = pageWidth;
            }
            else
            {
                PageContainer.Width = 0;
            }
        }


        // HighlightElement

        public static readonly DependencyProperty HighlightElementProperty = DependencyProperty.Register("HighlightElement", typeof(PrintElementNode), typeof(PrintViewPreviewPanel), new FrameworkPropertyMetadata(OnHighlightElementChanged));

        /// <summary>
        /// Элемент печатного представления, который нужно подсветить.
        /// </summary>
        public PrintElementNode HighlightElement
        {
            get { return (PrintElementNode)GetValue(HighlightElementProperty); }
            set { SetValue(HighlightElementProperty, value); }
        }

        private static void OnHighlightElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewPreviewPanel;

            var documentMap = control?._documentMap;

            if (documentMap != null)
            {
                var oldValue = e.OldValue as PrintElementNode;
                var oldElement = documentMap.GetElement(oldValue?.ElementMetadata);
                DeleteHighlightElement(oldElement);

                var newValue = e.NewValue as PrintElementNode;
                var newElement = documentMap.GetElement(newValue?.ElementMetadata);
                AddHighlightElement(newElement);
            }
        }

        private static void AddHighlightElement(object element)
        {
            if (element != null)
            {
                if (element is TextElement)
                {
                    var textElement = (TextElement)element;
                    textElement.Tag = textElement.Background;
                    textElement.Background = HighlightElementBrush;
                }
                else if (element is TableColumn)
                {
                    var tableColumn = (TableColumn)element;
                    tableColumn.Tag = tableColumn.Background;
                    tableColumn.Background = HighlightElementBrush;
                }
                else if (element is FlowDocument)
                {
                    var flowDocument = (FlowDocument)element;
                    flowDocument.Tag = flowDocument.Background;
                    flowDocument.Background = HighlightElementBrush;
                }

                if (element is FrameworkContentElement)
                {
                    ((FrameworkContentElement)element).BringIntoView();
                }
            }
        }

        private static void DeleteHighlightElement(object element)
        {
            if (element != null)
            {
                if (element is TextElement)
                {
                    var textElement = (TextElement)element;
                    textElement.Background = textElement.Tag as Brush;
                }
                else if (element is TableColumn)
                {
                    var tableColumn = (TableColumn)element;
                    tableColumn.Background = tableColumn.Tag as Brush;
                }
                else if (element is FlowDocument)
                {
                    var flowDocument = (FlowDocument)element;
                    flowDocument.Background = flowDocument.Tag as Brush;
                }
            }
        }


        // InspectElementMetadata

        public static readonly RoutedEvent InspectElementMetadataEvent = EventManager.RegisterRoutedEvent("InspectElementMetadata", RoutingStrategy.Bubble, typeof(PropertyValueChangedEventHandler), typeof(PrintViewPreviewPanel));

        /// <summary>
        /// Событие перехода к метаданным элемента.
        /// </summary>
        public event PropertyValueChangedEventHandler InspectElementMetadata
        {
            add { AddHandler(InspectElementMetadataEvent, value); }
            remove { RemoveHandler(InspectElementMetadataEvent, value); }
        }

        private Point _mousePosition;

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mousePosition = e.GetPosition(PageContainerEditor);
        }

        private void OnInspectElementMetadata(object sender, RoutedEventArgs e)
        {
            var position = PageContainerEditor.GetPositionFromPoint(_mousePosition, true);

            if (position?.Parent != null)
            {
                var documentMap = _documentMap;

                var elementMetadata = documentMap?.GetTemplate(position.Parent);

                if (elementMetadata != null)
                {
                    RaiseEvent(new PropertyValueChangedEventArgs(InspectElementMetadataEvent, null, null, elementMetadata));
                }
            }
        }


        /// <summary>
        /// Обновляет печатное представление.
        /// </summary>
        public void RefreshPrintView()
        {
            PageContainerEditor.BeginInit();

            try
            {
                var vOffset = PageContainerEditor.VerticalOffset;
                var hOffset = PageContainerEditor.HorizontalOffset;

                RenderTemplatePreview(DocumentTemplate);

                PageContainerEditor.ScrollToVerticalOffset(vOffset);
                PageContainerEditor.ScrollToHorizontalOffset(hOffset);
            }
            finally
            {
                PageContainerEditor.EndInit();
            }
        }


        private object _previewData;


        /// <summary>
        /// Открывает диалог установки данных предпросмотра.
        /// </summary>
        public void SetPreviewData()
        {
            var dialog = new PrintViewPreviewDataDialog { EditValue = _previewData };

            if (dialog.ShowDialog() == true)
            {
                _previewData = dialog.EditValue;
            }
        }


        /// <summary>
        /// Открывает печатное представление на предпросмотр.
        /// </summary>
        public async Task Preview(PrintViewFileFormat fileFormat)
        {
            var template = DocumentTemplate;

            if (template != null)
            {
                try
                {
                    var documentPath = $"PrintView.{Guid.NewGuid():N}.{fileFormat}";

                    using (var documentStream = File.Create(documentPath))
                    {
                        var task = _printViewBuilder.Build(documentStream, template, dataSource: _previewData, fileFormat: fileFormat);

                        await task;
                    }

                    var previewProcess = Process.Start(documentPath);

                    if (previewProcess != null)
                    {
                        previewProcess.EnableRaisingEvents = true;

                        previewProcess.Exited += (s, e) =>
                                                 {
                                                     try
                                                     {
                                                         File.Delete(documentPath);
                                                     }
                                                     catch
                                                     {
                                                     }
                                                 };
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, AppResources.PrintViewDesignerName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}