using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using DevExpress.Xpf.Bars;

using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintElementToolbox
{
    /// <summary>
    /// Панель инструментов для редактирования элементов печатного представления.
    /// </summary>
    public sealed partial class PrintElementToolboxControl : UserControl
    {
        public PrintElementToolboxControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Элемент печатного представления, который выделен в дереве.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode) GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        private static void OnSelectedElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintElementToolboxControl;

            if (control != null)
            {
                var elementNode = e.NewValue as PrintElementNode;

                var hasElementNode = (elementNode != null);
                var hasElementParentNode = hasElementNode && (elementNode.Parent != null);

                // Регулировка доступности кнопок создания

                foreach (var itemLink in control.ToolboxCreateElementBar.ItemLinks.OfType<BarButtonItemLink>())
                {
                    var item = itemLink.Item;
                    var childElementNode = CreateElementNode(elementNode, item, true);

                    item.IsEnabled = (elementNode != null) && elementNode.CanInsertChild.TryInvoke(childElementNode);
                }

                // Регулировка доступности прочих операций

                control.CutElementButton.IsEnabled = hasElementNode && elementNode.CanCut.TryInvoke();
                control.CopyElementButton.IsEnabled = hasElementNode && elementNode.CanCopy.TryInvoke();
                control.PasteElementButton.IsEnabled = hasElementNode && elementNode.CanPaste.TryInvoke();

                control.DeleteElementButton.IsEnabled = hasElementParentNode && elementNode.Parent.CanDeleteChild.TryInvoke(elementNode, true);
                control.MoveUpElementButton.IsEnabled = hasElementParentNode && elementNode.Parent.CanMoveChild.TryInvoke(elementNode, -1);
                control.MoveDownElementButton.IsEnabled = hasElementParentNode && elementNode.Parent.CanMoveChild.TryInvoke(elementNode, +1);
            }
        }

        private void OnInsertChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => el.InsertChild.TryInvoke(CreateElementNode(el, e.Item, false)));
        }

        private void OnDeleteChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.DeleteChild.TryInvoke(el, true));
        }

        private void OnMoveUpChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild.TryInvoke(el, -1));
        }

        private void OnMoveDownChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild.TryInvoke(el, +1));
        }

        private void OnCutChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => !el.Cut.TryInvoke());
        }

        private void OnCopyChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => !el.Copy.TryInvoke());
        }

        private void OnPasteChildElement(object sender, ItemClickEventArgs e)
        {
            ModifySelectedElement(el => el.Paste.TryInvoke());
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

            if (selectedElement != null && selectedElement.Parent != null)
            {
                if (action(selectedElement))
                {
                    OnElementMetadataChanged();
                }
            }
        }

        /// <summary>
        ///     Событие разворачивания всех элементов в дереве печатного представления.
        /// </summary>
        public event RoutedEventHandler ExpandAll
        {
            add { AddHandler(ExpandAllEvent, value); }
            remove { RemoveHandler(ExpandAllEvent, value); }
        }

        private void OnExpandAll(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExpandAllEvent));
        }

        /// <summary>
        /// Событие сворачивания всех элементов в дереве печатного представления.
        /// </summary>
        public event RoutedEventHandler CollapseAll
        {
            add { AddHandler(CollapseAllEvent, value); }
            remove { RemoveHandler(CollapseAllEvent, value); }
        }

        private void OnCollapseAll(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CollapseAllEvent));
        }

        /// <summary>
        /// Событие предпросмотра печатного представления.
        /// </summary>
        public event RoutedEventHandler Preview
        {
            add { AddHandler(PreviewEvent, value); }
            remove { RemoveHandler(PreviewEvent, value); }
        }

        private void OnPreview(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PreviewEvent));
        }

        /// <summary>
        /// Событие импорта печатного представления.
        /// </summary>
        public event RoutedEventHandler Import
        {
            add { AddHandler(ImportEvent, value); }
            remove { RemoveHandler(ImportEvent, value); }
        }

        private void OnImport(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ImportEvent));
        }

        /// <summary>
        /// Событие экспорта печатного представления.
        /// </summary>
        public event RoutedEventHandler Export
        {
            add { AddHandler(ExportEvent, value); }
            remove { RemoveHandler(ExportEvent, value); }
        }

        private void OnExport(object sender, ItemClickEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ExportEvent));
        }

        /// <summary>
        /// Событие метаданных элемента печатного представления.
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

        private static PrintElementNode CreateElementNode(PrintElementNode elementParent, BarItem elementItem,
            bool useElementCache)
        {
            PrintElementNode elementNode;

            var elementType = elementItem.Tag as string ?? string.Empty;

            if (useElementCache)
            {
                if (!ElementNodeCache.TryGetValue(elementType, out elementNode))
                {
                    elementNode = new PrintElementNode(elementParent, elementType, new DynamicWrapper());
                    ElementNodeCache[elementType] = elementNode;
                }
            }
            else
            {
                elementNode = new PrintElementNode(elementParent, elementType, new DynamicWrapper());
            }

            return elementNode;
        }

        // SelectedElement

        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof (PrintElementNode),
                typeof (PrintElementToolboxControl), new FrameworkPropertyMetadata(OnSelectedElementChanged));

        // ExpandAll

        public static readonly RoutedEvent ExpandAllEvent = EventManager.RegisterRoutedEvent("ExpandAll",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintElementToolboxControl));

        // CollapseAll

        public static readonly RoutedEvent CollapseAllEvent = EventManager.RegisterRoutedEvent("CollapseAll",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintElementToolboxControl));

        // Preview

        public static readonly RoutedEvent PreviewEvent = EventManager.RegisterRoutedEvent("Preview",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintElementToolboxControl));

        // Import

        public static readonly RoutedEvent ImportEvent = EventManager.RegisterRoutedEvent("Import",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintElementToolboxControl));

        // Export

        public static readonly RoutedEvent ExportEvent = EventManager.RegisterRoutedEvent("Export",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintElementToolboxControl));

        // ElementMetadataChanged

        public static readonly RoutedEvent ElementMetadataChangedEvent =
            EventManager.RegisterRoutedEvent("ElementMetadataChanged", RoutingStrategy.Bubble,
                typeof (PropertyValueChangedEventHandler), typeof (PrintElementToolboxControl));

        // Helpers

        private static readonly Dictionary<string, PrintElementNode> ElementNodeCache =
            new Dictionary<string, PrintElementNode>();
    }
}