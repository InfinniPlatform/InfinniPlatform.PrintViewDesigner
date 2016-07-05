using System;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Win32;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewDesigner
{
    public sealed partial class PrintViewDesignerControl : UserControl
    {
        // PrintView

        public static readonly DependencyProperty PrintViewProperty = DependencyProperty.Register("PrintView",
            typeof (object), typeof (PrintViewDesignerControl));

        // PrintViewChanged

        public static readonly RoutedEvent PrintViewChangedEvent = EventManager.RegisterRoutedEvent("PrintViewChanged",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintViewDesignerControl));

        private static readonly string[] ImportProperties
            =
        {
            "Caption",
            "Description",
            "ViewType",
            "Visibility",
            "Source",
            "Font",
            "TextCase",
            "Style",
            "Foreground",
            "Background",
            "PageSize",
            "PagePadding",
            "Styles",
            "Blocks"
        };

        public PrintViewDesignerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Печатное представление.
        /// </summary>
        public object PrintView
        {
            get { return GetValue(PrintViewProperty); }
            set { SetValue(PrintViewProperty, value); }
        }

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

        private void OnPrintViewChanged(object sender, RoutedEventArgs e)
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
            var printView = PrintView ?? new DynamicWrapper();

            var openFileDialog = new OpenFileDialog
            {
                Title = AppResources.Import,
                Filter = "JSON|*.json|Text|*.txt|All|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var sourcePrintView = Helpers.LoadObjectFromFile(openFileDialog.FileName);

                    if (sourcePrintView != null && Helpers.AcceptQuestionMessage(AppResources.RelpaceCurrentPrintView))
                    {
                        Helpers.CopyObject(sourcePrintView, printView, ImportProperties);

                        PrintView = printView;

                        RaisePrintViewChangedEvent(true);
                    }
                }
                catch (Exception error)
                {
                    Helpers.ShowWarningMessage(error.Message);
                }
            }
        }

        private void OnExport(object sender, RoutedEventArgs e)
        {
            var printView = PrintView ?? new DynamicWrapper();

            var saveFileDialog = new SaveFileDialog
            {
                Title = AppResources.Export,
                Filter = "JSON|*.json|Text|*.txt|All|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    Helpers.SaveObjectToFile(saveFileDialog.FileName, printView);
                }
                catch (Exception error)
                {
                    Helpers.ShowWarningMessage(error.Message);
                }
            }
        }

        private void OnPreview(object sender, RoutedEventArgs e)
        {
            Viewer.Preview();
        }
    }
}