using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using InfinniPlatform.Core.PrintView;
using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.PrintViewDesigner.ViewModel;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;
using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewViewer
{
    /// <summary>
    /// Элемент для просмотра печатного представления.
    /// </summary>
    public sealed partial class PrintViewViewerControl : UserControl
    {
        private static readonly IFlowDocumentPrintViewFactory PrintViewFactory;
        private static readonly IPrintViewBuilder PrintViewBuilder;
        private static readonly SolidColorBrush HighlightElementBrush;


        static PrintViewViewerControl()
        {
            PrintViewFactory = new FlowDocumentPrintViewDesignerFactory();
            PrintViewBuilder = new FlowDocumentPrintViewBuilder(PrintViewFactory, new FlowDocumentPrintViewConverter(PrintViewSettings.Default));
            HighlightElementBrush = new SolidColorBrush(SystemColors.HighlightColor) { Opacity = 0.4 };
        }


        public PrintViewViewerControl()
        {
            InitializeComponent();
        }


        private PrintElementMetadataMap _elementMetadataMap;


        // PrintView

        public static readonly DependencyProperty PrintViewProperty = DependencyProperty.Register("PrintView", typeof(object), typeof(PrintViewViewerControl), new FrameworkPropertyMetadata(OnPrintViewChanged));

        /// <summary>
        /// Печатное представление.
        /// </summary>
        public object PrintView
        {
            get { return GetValue(PrintViewProperty); }
            set { SetValue(PrintViewProperty, value); }
        }

        private static void OnPrintViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewViewerControl;

            if (control != null)
            {
                control.RenderPrintView(e.NewValue);
            }
        }

        private void RenderPrintView(object printView)
        {
            PrintElementMetadataMap elementMetadataMap = null;
            FrameworkFlowDocument printViewDocument = null;

            if (printView != null)
            {
                try
                {
                    elementMetadataMap = new PrintElementMetadataMap();

                    var printViewDocumentModel = PrintViewFactory.Create(printView, null, elementMetadataMap);

                    printViewDocument = FlowDocumentBuilder.Build(printViewDocumentModel, elementMetadataMap);
                }
                catch (Exception error)
                {
                    elementMetadataMap = null;
                    printViewDocument = CreateDocumentByError(error);
                }
            }

            _elementMetadataMap = elementMetadataMap;

            Editor.Document = printViewDocument;
        }

        private static FrameworkFlowDocument CreateDocumentByError(Exception error)
        {
            var errorMessage = new Paragraph(new Run(error.Message));

            return new FrameworkFlowDocument(errorMessage) { Foreground = Brushes.Red };
        }


        // HighlightElement

        public static readonly DependencyProperty HighlightElementProperty = DependencyProperty.Register("HighlightElement", typeof(PrintElementNode), typeof(PrintViewViewerControl), new FrameworkPropertyMetadata(OnHighlightElementChanged));

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
            var control = sender as PrintViewViewerControl;

            if (control != null)
            {
                var elementMetadataMap = control._elementMetadataMap;

                if (elementMetadataMap != null)
                {
                    var oldValue = e.OldValue as PrintElementNode;
                    var oldElement = elementMetadataMap.GetElement((oldValue != null) ? oldValue.ElementMetadata : null);
                    DeleteHighlightElement(oldElement);

                    var newValue = e.NewValue as PrintElementNode;
                    var newElement = elementMetadataMap.GetElement((newValue != null) ? newValue.ElementMetadata : null);
                    AddHighlightElement(newElement);
                }
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
                else if (element is FrameworkFlowDocument)
                {
                    var flowDocument = (FrameworkFlowDocument)element;
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
                else if (element is FrameworkFlowDocument)
                {
                    var flowDocument = (FrameworkFlowDocument)element;
                    flowDocument.Background = flowDocument.Tag as Brush;
                }
            }
        }


        // InspectElementMetadata

        public static readonly RoutedEvent InspectElementMetadataEvent = EventManager.RegisterRoutedEvent("InspectElementMetadata", RoutingStrategy.Bubble, typeof(PropertyValueChangedEventHandler), typeof(PrintViewViewerControl));

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
            _mousePosition = e.GetPosition(Editor);
        }

        private void OnInspectElementMetadata(object sender, RoutedEventArgs e)
        {
            var position = Editor.GetPositionFromPoint(_mousePosition, true);

            if (position != null && position.Parent != null)
            {
                var elementMetadataMap = _elementMetadataMap;

                if (elementMetadataMap != null)
                {
                    var elementMetadata = elementMetadataMap.GetMetadata(position.Parent);

                    if (elementMetadata != null)
                    {
                        RaiseEvent(new PropertyValueChangedEventArgs(InspectElementMetadataEvent, null, null, elementMetadata));
                    }
                }
            }
        }


        /// <summary>
        /// Обновляет печатное представление.
        /// </summary>
        public void RefreshPrintView()
        {
            Editor.BeginInit();

            try
            {
                var vOffset = Editor.VerticalOffset;
                var hOffset = Editor.HorizontalOffset;

                RenderPrintView(PrintView);

                Editor.ScrollToVerticalOffset(vOffset);
                Editor.ScrollToHorizontalOffset(hOffset);
            }
            finally
            {
                Editor.EndInit();
            }
        }

        /// <summary>
        /// Открывает печатное представление на предпросмотр.
        /// </summary>
        public void Preview()
        {
            var printView = PrintView;

            if (printView != null)
            {
                try
                {
                    var fileData = PrintViewBuilder.BuildFile(printView, null);
                    var fileName = string.Format("PrintView.{0}.pdf", Guid.NewGuid());
                    File.WriteAllBytes(fileName, fileData);

                    var previewProcess = Process.Start(fileName);

                    if (previewProcess != null)
                    {
                        previewProcess.EnableRaisingEvents = true;

                        previewProcess.Exited += (s, e) =>
                                                 {
                                                     try
                                                     {
                                                         File.Delete(fileName);
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