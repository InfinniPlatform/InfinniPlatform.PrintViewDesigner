using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DevExpress.Xpf.Grid;

using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTree
{
    /// <summary>
    /// Дерево элементов печатного представления.
    /// </summary>
    public sealed partial class PrintViewTreeControl : UserControl
    {
        public PrintViewTreeControl()
        {
            InitializeComponent();

            TreeList.View.NodeImageSelector = PrintViewTreeImageSelector.Instance;
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
        /// Элемент печатного представления, над которым находится указатель мыши.
        /// </summary>
        public PrintElementNode MouseOverElement
        {
            get { return (PrintElementNode) GetValue(MouseOverElementProperty); }
            set { SetValue(MouseOverElementProperty, value); }
        }

        /// <summary>
        /// Элемент печатного представления, который выделен в дереве.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode) GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        private static void OnPrintViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PrintViewTreeControl;

            if (control != null)
            {
                control.BuildTree(e.NewValue);
            }
        }

        private void BuildTree(object printView)
        {
            var elements = new ObservableCollection<PrintElementNode>();

            TreeList.BeginDataUpdate();

            try
            {
                TreeList.View.Nodes.Clear();

                TreeBuilder.Build(elements, printView);
                TreeList.ItemsSource = elements;

                SelectElement(elements.FirstOrDefault());
            }
            finally
            {
                TreeList.EndDataUpdate();
            }

            ExpandElement(elements.FirstOrDefault(e => e.ElementType == "PrintViewBlocks"));
        }

        private void SelectElement(PrintElementNode element)
        {
            TreeList.SelectedItem = element;
        }

        private void ExpandElement(PrintElementNode element)
        {
            if (element != null)
            {
                var elementNode = TreeList.View.GetNodeByKeyValue(element);

                if (elementNode != null)
                {
                    var parentNodes = new Stack<TreeListNode>();

                    while (elementNode != null && !elementNode.IsExpanded)
                    {
                        parentNodes.Push(elementNode);
                        elementNode = elementNode.ParentNode;
                    }

                    foreach (var node in parentNodes)
                    {
                        node.IsExpanded = true;
                    }
                }
            }
        }

        /// <summary>
        /// Событие изменения печатного представления.
        /// </summary>
        public event RoutedEventHandler PrintViewChanged
        {
            add { AddHandler(PrintViewChangedEvent, value); }
            remove { RemoveHandler(PrintViewChangedEvent, value); }
        }

        private void RaisePrintViewChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(PrintViewChangedEvent));
        }

        private void OnTreeListMouseMove(object sender, MouseEventArgs e)
        {
            MouseOverElement = GetNodeByMouseEventArgs(e);
        }

        private void OnTreeListMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectElement(GetNodeByMouseEventArgs(e));
        }

        private PrintElementNode GetNodeByMouseEventArgs(MouseEventArgs e)
        {
            var rowHandle = TreeList.View.GetRowHandleByMouseEventArgs(e);
            return TreeList.GetRow(rowHandle) as PrintElementNode;
        }

        private void OnTreeListSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedElement = e.NewItem as PrintElementNode;
        }

        // ContextMenu

        private void OnCanCutCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanCut.TryInvoke());
        }

        private void OnCutCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Cut.TryInvoke());
        }

        private void OnCanCopyCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanCopy.TryInvoke());
        }

        private void OnCopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Copy.TryInvoke());
        }

        private void OnCanPasteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanPaste.TryInvoke());
        }

        private void OnPasteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Paste.TryInvoke());
        }

        private void OnCanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanDeleteChild.TryInvoke(el, true));
        }

        private void OnDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.DeleteChild.TryInvoke(el, true));
        }

        private void OnCanMoveUpCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanMoveChild.TryInvoke(el, -1));
        }

        private void OnMoveUpCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild.TryInvoke(el, -1));
        }

        private void OnCanMoveDownCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanMoveChild.TryInvoke(el, +1));
        }

        private void OnMoveDownCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild.TryInvoke(el, +1));
        }

        private bool CanModifySelectedElement(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null)
            {
                return action(selectedElement);
            }

            return false;
        }

        private void ModifySelectedElement(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null)
            {
                if (action(selectedElement))
                {
                    RaisePrintViewChangedEvent();
                }
            }
        }

        private bool CanModifySelectedElementParent(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null && selectedElement.Parent != null)
            {
                return action(selectedElement);
            }

            return false;
        }

        private void ModifySelectedElementParent(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null && selectedElement.Parent != null)
            {
                if (action(selectedElement))
                {
                    RaisePrintViewChangedEvent();
                }
            }
        }

        // Methods

        /// <summary>
        /// Выделяет элемент в дереве, который соответствует указанным метаданным.
        /// </summary>
        public void SelectElementByMetadata(object elementMetadata)
        {
            if (elementMetadata != null)
            {
                var elements = TreeList.ItemsSource as IEnumerable<PrintElementNode>;

                if (elements != null)
                {
                    SelectElement(elements.FirstOrDefault(e => e.ElementMetadata == elementMetadata));
                }
            }
        }

        /// <summary>
        /// Обновляет выделенный элемент в дереве.
        /// </summary>
        public void RefreshElementTree()
        {
            BuildTree(PrintView);
        }

        /// <summary>
        /// Обновляет выделенный элемент в дереве.
        /// </summary>
        public void RefreshSelectedElement()
        {
            var selectedElement = SelectedElement;

            if (selectedElement != null)
            {
                selectedElement.Refresh();
            }
        }

        /// <summary>
        /// Разворачивает все элементы в дереве.
        /// </summary>
        public void ExpandAllElements()
        {
            TreeList.View.ExpandAllNodes();
        }

        /// <summary>
        /// Сворачивает все элементы в дереве.
        /// </summary>
        public void CollapseAllElements()
        {
            TreeList.View.CollapseAllNodes();
        }

        private static readonly PrintViewTreeBuilder TreeBuilder = new PrintViewTreeBuilder();
        // PrintView

        public static readonly DependencyProperty PrintViewProperty = DependencyProperty.Register("PrintView",
            typeof (object), typeof (PrintViewTreeControl), new FrameworkPropertyMetadata(OnPrintViewChanged));

        // PrintViewChanged

        public static readonly RoutedEvent PrintViewChangedEvent = EventManager.RegisterRoutedEvent("PrintViewChanged",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (PrintViewTreeControl));

        // MouseOverElement

        public static readonly DependencyProperty MouseOverElementProperty =
            DependencyProperty.Register("MouseOverElement", typeof (PrintElementNode), typeof (PrintViewTreeControl));

        // SelectedElement

        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register("SelectedElement", typeof (PrintElementNode), typeof (PrintViewTreeControl));
    }
}