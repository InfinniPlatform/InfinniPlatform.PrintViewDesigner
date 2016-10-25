using System;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Common;

using Microsoft.Win32;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.MainView
{
    public sealed partial class PrintViewDesignerControl : UserControl
    {
        private const string TemplateFileFilter = "JSON|*.json|Text|*.txt|All|*.*";


        public PrintViewDesignerControl()
        {
            InitializeComponent();

            _printViewSerializer = new PrintViewSerializer();
        }

        private readonly IPrintViewSerializer _printViewSerializer;


        // DocumentTemplate

        public static readonly DependencyProperty DocumentTemplateProperty
            = DependencyProperty.Register(nameof(DocumentTemplate),
                                          typeof(PrintDocument),
                                          typeof(PrintViewDesignerControl));

        /// <summary>
        /// Шаблон документа печатного представления.
        /// </summary>
        public PrintDocument DocumentTemplate
        {
            get { return (PrintDocument)GetValue(DocumentTemplateProperty); }
            set { SetValue(DocumentTemplateProperty, value); }
        }


        // PrintViewChanged

        public static readonly RoutedEvent PrintViewChangedEvent
            = EventManager.RegisterRoutedEvent(nameof(PrintViewChanged),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewDesignerControl));

        /// <summary>
        /// Событие изменения печатного представления.
        /// </summary>
        public event RoutedEventHandler PrintViewChanged
        {
            add { AddHandler(PrintViewChangedEvent, value); }
            remove { RemoveHandler(PrintViewChangedEvent, value); }
        }

        private void RaisePrintViewChangedEvent(bool refreshTree)
        {
            if (refreshTree)
            {
                Tree.RefreshElementTree();
            }

            Tree.RefreshSelectedElement();
            Viewer.RefreshPrintView();

            RaiseEvent(new RoutedEventArgs(PrintViewChangedEvent));
        }


        // Handlers

        private void OnInspectElementMetadata(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                Tree.SelectElementByMetadata(e.NewValue);
            }
        }

        private void OnDocumentTemplateChanged(object sender, RoutedEventArgs e)
        {
            RaisePrintViewChangedEvent(false);
        }

        private void OnElementMetadataChanged(object sender, PropertyValueChangedEventArgs e)
        {
            RaisePrintViewChangedEvent(false);
        }

        private void OnExpandAllElements(object sender, RoutedEventArgs e)
        {
            Tree.ExpandAllElements();
        }

        private void OnCollapseAllElements(object sender, RoutedEventArgs e)
        {
            Tree.CollapseAllElements();
        }

        private void OnImport(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = AppResources.Import,
                Filter = "JSON|*.json|Text|*.txt|All|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    PrintDocument newTemplate;

                    using (var templateStream = openFileDialog.OpenFile())
                    {
                        newTemplate = _printViewSerializer.Deserialize(templateStream);
                    }
                    
                    if (newTemplate != null && MessageBoxHelpers.AcceptQuestionMessage(AppResources.RelpaceCurrentPrintView))
                    {
                        DocumentTemplate = newTemplate;

                        RaisePrintViewChangedEvent(true);
                    }
                }
                catch (Exception error)
                {
                    MessageBoxHelpers.ShowWarningMessage(error.Message);
                }
            }
        }

        private void OnExport(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = AppResources.Export,
                Filter = TemplateFileFilter
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var templateStream = saveFileDialog.OpenFile())
                    {
                        _printViewSerializer.Serialize(templateStream, DocumentTemplate);
                    }
                }
                catch (Exception error)
                {
                    MessageBoxHelpers.ShowWarningMessage(error.Message);
                }
            }
        }

        private void OnPreviewData(object sender, RoutedEventArgs e)
        {
            Viewer.SetPreviewData();
        }

        private async void OnPreviewPdf(object sender, RoutedEventArgs e)
        {
            await Viewer.Preview(PrintViewFileFormat.Pdf);
        }

        private async void OnPreviewHtml(object sender, RoutedEventArgs e)
        {
            await Viewer.Preview(PrintViewFileFormat.Html);
        }
    }
}