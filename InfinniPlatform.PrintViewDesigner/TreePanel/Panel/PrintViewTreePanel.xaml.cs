using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DevExpress.Xpf.Grid;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Panel
{
    /// <summary>
    /// Дерево элементов печатного представления.
    /// </summary>
    public partial class PrintViewTreePanel : UserControl
    {
        private static readonly PrintViewTreeFactory PrintViewTreeFactory = new PrintViewTreeFactory();


        public PrintViewTreePanel()
        {
            InitializeComponent();

            TreeList.View.NodeImageSelector = new PrintViewTreeImageSelector();
        }


        // DocumentTemplate

        public static readonly DependencyProperty DocumentTemplateProperty
            = DependencyProperty.Register(nameof(DocumentTemplate),
                                          typeof(PrintDocument),
                                          typeof(PrintViewTreePanel),
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
            var control = sender as PrintViewTreePanel;

            control?.BuildTree((PrintDocument)e.NewValue);
        }

        private void BuildTree(PrintDocument template)
        {
            var tree = new ObservableCollection<PrintElementNode>();

            // Построение дерева шаблона печатного представления
            PrintViewTreeFactory.CreateTree(tree, template);

            TreeList.BeginDataUpdate();

            try
            {
                TreeList.View.Nodes.Clear();

                TreeList.ItemsSource = tree;

                SelectElement(tree.FirstOrDefault());
            }
            finally
            {
                TreeList.EndDataUpdate();
            }

            ExpandElement(tree.FirstOrDefault(e => e.ElementType == PrintElementNodeType.PrintDocumentBlocksNode));
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


        // SelectedElement

        public static readonly DependencyProperty SelectedElementProperty
            = DependencyProperty.Register(nameof(SelectedElement),
                                          typeof(PrintElementNode),
                                          typeof(PrintViewTreePanel));

        /// <summary>
        /// Элемент печатного представления, который выделен в дереве.
        /// </summary>
        public PrintElementNode SelectedElement
        {
            get { return (PrintElementNode)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }


        // MouseOverElement

        public static readonly DependencyProperty MouseOverElementProperty
            = DependencyProperty.Register(nameof(MouseOverElement),
                                          typeof(PrintElementNode),
                                          typeof(PrintViewTreePanel));

        /// <summary>
        /// Элемент печатного представления, над которым находится указатель мыши.
        /// </summary>
        public PrintElementNode MouseOverElement
        {
            get { return (PrintElementNode)GetValue(MouseOverElementProperty); }
            set { SetValue(MouseOverElementProperty, value); }
        }


        // PrintViewChanged

        public static readonly RoutedEvent DocumentTemplateChangedEvent
            = EventManager.RegisterRoutedEvent(nameof(DocumentTemplateChanged),
                                               RoutingStrategy.Bubble,
                                               typeof(RoutedEventHandler),
                                               typeof(PrintViewTreePanel));

        /// <summary>
        /// Событие изменения печатного представления.
        /// </summary>
        public event RoutedEventHandler DocumentTemplateChanged
        {
            add { AddHandler(DocumentTemplateChangedEvent, value); }
            remove { RemoveHandler(DocumentTemplateChangedEvent, value); }
        }

        private void RaisePrintViewChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(DocumentTemplateChangedEvent));
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
            BuildTree(DocumentTemplate);
        }

        /// <summary>
        /// Обновляет выделенный элемент в дереве.
        /// </summary>
        public void RefreshSelectedElement()
        {
            var selectedElement = SelectedElement;

            selectedElement?.Refresh();
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


        // ContextMenu

        private void OnCanCutCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanCut?.Invoke() == true);
        }

        private void OnCutCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Cut?.Invoke() == true);
        }

        private void OnCanCopyCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanCopy?.Invoke() == true);
        }

        private void OnCopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Copy?.Invoke() == true);
        }

        private void OnCanPasteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElement(el => el.CanPaste?.Invoke() == true);
        }

        private void OnPasteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElement(el => el.Paste?.Invoke() == true);
        }

        private void OnCanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanDeleteChild?.Invoke(el, true) == true);
        }

        private void OnDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.DeleteChild?.Invoke(el, true) == true);
        }

        private void OnCanMoveUpCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanMoveChild?.Invoke(el, -1) == true);
        }

        private void OnMoveUpCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild?.Invoke(el, -1) == true);
        }

        private void OnCanMoveDownCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanModifySelectedElementParent(el => el.Parent.CanMoveChild?.Invoke(el, +1) == true);
        }

        private void OnMoveDownCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModifySelectedElementParent(el => el.Parent.MoveChild?.Invoke(el, +1) == true);
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

            if (selectedElement?.Parent != null)
            {
                return action(selectedElement);
            }

            return false;
        }

        private void ModifySelectedElementParent(Func<PrintElementNode, bool> action)
        {
            var selectedElement = SelectedElement;

            if (selectedElement?.Parent != null)
            {
                if (action(selectedElement))
                {
                    RaisePrintViewChangedEvent();
                }
            }
        }


        // Handlers

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
    }
}