using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders
{
    internal static class BuildHelper
    {
        public static readonly string[] BlockTypes =
        {
            "Section", "Paragraph", "List", "Table", "Line", "PageBreak"
        };

        public static readonly string[] InlineTypes =
        {
            "Span", "Bold", "Italic", "Underline", "Hyperlink", "LineBreak", "Run", "Image", "BarcodeEan13", "BarcodeQr"
        };

        // Insert

        public static Func<PrintElementNode, bool> CanInsertChild(PrintElementNode parentElementNode)
        {
            return childElementNode => (childElementNode != null)
                                       && (childElementNode.ElementType != null)
                                       && (parentElementNode.InsertChild != null)
                                       && (parentElementNode.ElementChildrenTypes != null)
                                       && (parentElementNode.ElementChildrenTypes.Contains(childElementNode.ElementType));
        }

        public static Func<PrintElementNode, bool> InsertChildToCollection(
            PrintElementNodeBuilder builder, 
            ICollection<PrintElementNode> elements, 
            PrintElementNode parentElementNode, 
            string childElementCollection, 
            bool wrapElementMetadata = true)
        {
            return childElementNode =>
            {
                if (parentElementNode.CanInsertChild.TryInvoke(childElementNode))
                {
                    // Создание метаданных нового дочернего элемента
                    object newChildElementMetadata = childElementNode.ElementMetadata ?? new DynamicWrapper();

                    // Добавление дочернего элемента в коллекцию метаданных родительского элемента
                    var childElements = GetChildElementCollection(parentElementNode, childElementCollection, true);
                    childElements.AddItem(wrapElementMetadata
                        ? WrapElementMetadata(childElementNode.ElementType, newChildElementMetadata)
                        : newChildElementMetadata);

                    // Добавление дочернего элемента в визуальное дерево элементов
                    builder.BuildElement(elements, parentElementNode, newChildElementMetadata,
                        childElementNode.ElementType);

                    return true;
                }

                return false;
            };
        }

        public static Func<PrintElementNode, bool> InsertChildToContainer(
            PrintElementNodeBuilder builder,
            ICollection<PrintElementNode> elements,
            PrintElementNode parentElementNode,
            string childElementContainer,
            bool wrapElementMetadata = true)
        {
            return childElementNode =>
            {
                if (parentElementNode.CanInsertChild.TryInvoke(childElementNode))
                {
                    var oldChildElement = parentElementNode.Nodes.FirstOrDefault();

                    if (oldChildElement == null || Helpers.AcceptQuestionMessage(Resources.ReplaceElementQuestion, parentElementNode, childElementNode))
                    {
                        // Создание метаданных нового дочернего элемента
                        object newChildElementMetadata = childElementNode.ElementMetadata ?? new DynamicWrapper();

                        // Добавление дочернего элемента в контейнер метаданных родительского элемента
                        parentElementNode.ElementMetadata[childElementContainer] = wrapElementMetadata
                            ? WrapElementMetadata(childElementNode.ElementType, newChildElementMetadata)
                            : newChildElementMetadata;

                        if (oldChildElement != null)
                        {
                            // Удаление старого дочернего элемента из визуального дерева элементов
                            parentElementNode.Nodes.RemoveItem(oldChildElement);
                            DeleteChildNodes(elements, oldChildElement);
                        }

                        // Добавление дочернего элемента в визуальное дерево элементов
                        builder.BuildElement(elements, parentElementNode, newChildElementMetadata, childElementNode.ElementType);

                        return true;
                    }
                }

                return false;
            };
        }

        private static DynamicWrapper WrapElementMetadata(string elementType, object elementMetadata)
        {
            var wrapper = new DynamicWrapper();
            wrapper[elementType] = elementMetadata;
            return wrapper;
        }

        // Delete

        public static Func<PrintElementNode, bool, bool> CanDeleteChild(PrintElementNode parentElementNode)
        {
            return (childElementNode, accept) => (childElementNode != null) && (parentElementNode.DeleteChild != null);
        }

        public static Func<PrintElementNode, bool, bool> DeleteChildFromCollection(
            ICollection<PrintElementNode> elements,
            PrintElementNode parentElementNode,
            string childElementCollection,
            bool wrapElementMetadata = true)
        {
            return (childElementNode, accept) =>
            {
                if (parentElementNode.CanDeleteChild.TryInvoke(childElementNode, accept))
                {
                    if (!accept || Helpers.AcceptQuestionMessage(Resources.DeleteElementQuestion, childElementNode))
                    {
                        // Удаление дочернего элемента из коллекции метаданных родительского элемента
                        var childElements = GetChildElementCollection(parentElementNode, childElementCollection, false);
                        ProcessChildElement(childElements, childElementNode, childElements.RemoveItem, wrapElementMetadata);

                        // Удаление дочернего элемента из визуального дерева элементов
                        parentElementNode.Nodes.RemoveItem(childElementNode);
                        DeleteChildNodes(elements, childElementNode);
                        RecalcChildNodes(parentElementNode);

                        return true;
                    }
                }

                return false;
            };
        }

        public static Func<PrintElementNode, bool, bool> DeleteChildFromContainer(
            ICollection<PrintElementNode> elements, 
            PrintElementNode parentElementNode, 
            string childElementContainer)
        {
            return (childElementNode, accept) =>
            {
                if (parentElementNode.CanDeleteChild.TryInvoke(childElementNode, accept))
                {
                    if (!accept || Helpers.AcceptQuestionMessage(Resources.DeleteElementQuestion, childElementNode))
                    {
                        // Удаление дочернего элемента из контейнера метаданных родительского элемента
                        parentElementNode.ElementMetadata[childElementContainer] = null;

                        // Удаление дочернего элемента из визуального дерева элементов
                        parentElementNode.Nodes.RemoveItem(childElementNode);
                        DeleteChildNodes(elements, childElementNode);

                        return true;
                    }
                }

                return false;
            };
        }

        private static void DeleteChildNodes(ICollection<PrintElementNode> elements, PrintElementNode parentElementNode)
        {
            elements.Remove(parentElementNode);

            foreach (var childElementNode in parentElementNode.Nodes)
            {
                DeleteChildNodes(elements, childElementNode);
            }
        }

        private static void RecalcChildNodes(PrintElementNode parentElementNode)
        {
            var index = 0;

            foreach (var childElementNode in parentElementNode.Nodes)
            {
                childElementNode.Index = index++;
            }
        }

        // Move

        public static Func<PrintElementNode, int, bool> CanMoveChild(PrintElementNode parentElementNode)
        {
            return (childElementNode, delta) => (childElementNode != null) && (parentElementNode.MoveChild != null);
        }

        public static Func<PrintElementNode, int, bool> MoveChildInCollection(
            PrintElementNode parentElementNode,
            string childElementCollection,
            bool wrapElementMetadata = true)
        {
            return (childElementNode, delta) =>
            {
                if (parentElementNode.CanMoveChild.TryInvoke(childElementNode, delta))
                {
                    // Перемещение дочернего элемента в коллекции метаданных родительского элемента
                    var childElements = GetChildElementCollection(parentElementNode, childElementCollection, false);
                    ProcessChildElement(childElements, childElementNode, element => childElements.MoveItem(element, delta), wrapElementMetadata);

                    // Перемещение дочернего элемента в визуальном дереве элементов
                    parentElementNode.Nodes.MoveItem(childElementNode, delta);
                    RecalcChildNodes(parentElementNode);

                    return true;
                }

                return false;
            };
        }

        // Cut

        public static Func<bool> CanCut(PrintElementNode parentElementNode)
        {
            return () => (parentElementNode.Cut != null);
        }

        public static Func<bool> Cut(PrintElementNode parentElementNode)
        {
            return () =>
            {
                if (parentElementNode.CanCut.TryInvoke())
                {
                    AppClipboard.Default.SetData(parentElementNode, false);
                    return true;
                }

                return false;
            };
        }

        // Copy

        public static Func<bool> CanCopy(PrintElementNode parentElementNode)
        {
            return () => (parentElementNode.Copy != null);
        }

        public static Func<bool> Copy(PrintElementNode parentElementNode)
        {
            return () =>
            {
                if (parentElementNode.CanCopy.TryInvoke())
                {
                    AppClipboard.Default.SetData(parentElementNode, true);
                    return true;
                }

                return false;
            };
        }

        // Paste

        public static Func<bool> CanPaste(PrintElementNode parentElementNode)
        {
            return () => (parentElementNode.Paste != null) && AppClipboard.Default.ContainsData<PrintElementNode>();
        }

        public static Func<bool> Paste(PrintElementNode parentElementNode)
        {
            return () =>
            {
                if (parentElementNode.CanPaste.TryInvoke())
                {
                    var entry = AppClipboard.Default.GetData<PrintElementNode>();

                    if (entry != null)
                    {
                        var copy = entry.Copy;
                        var childElementNode = (PrintElementNode) entry.Data;

                        // Элемент не может быть вставлен сам в себя
                        if (childElementNode == parentElementNode)
                        {
                            Helpers.ShowWarningMessage(Resources.CannotBePastedIntoItself);
                            return false;
                        }

                        // Родитель не может быть вставлен в своего потомка
                        if (IsParentNode(childElementNode, parentElementNode))
                        {
                            Helpers.ShowWarningMessage(Resources.CannotInsertParentIntoChild, parentElementNode,
                                childElementNode);
                            return false;
                        }

                        // Невозможно поместить заданный элемент в родительский
                        if (!parentElementNode.CanInsertChild.TryInvoke(childElementNode))
                        {
                            Helpers.ShowWarningMessage(Resources.CannotInsertChildElement, childElementNode,
                                parentElementNode);
                            return false;
                        }

                        // Удаление элемента из текущего родительского
                        if (!copy && childElementNode.Parent != null)
                        {
                            childElementNode.Parent.DeleteChild(childElementNode, false);
                        }

                        // Вставка элемента в указанный родительский
                        var newChildElementNode = new PrintElementNode(parentElementNode, childElementNode.ElementType, childElementNode.ElementMetadata.Clone());
                        parentElementNode.InsertChild.TryInvoke(newChildElementNode);

                        return true;
                    }
                }

                return false;
            };
        }

        private static bool IsParentNode(PrintElementNode parentNode, PrintElementNode childNode)
        {
            var isParent = false;

            while (childNode != null)
            {
                if (childNode.Parent == parentNode)
                {
                    isParent = true;
                    break;
                }

                childNode = childNode.Parent;
            }

            return isParent;
        }

        // Helpers

        private static IList GetChildElementCollection(PrintElementNode parentElementNode, string childElementCollection, bool createIfNull)
        {
            var childElements = parentElementNode.ElementMetadata[childElementCollection];

            if (childElements == null && createIfNull)
            {
                childElements = new List<object>();
                parentElementNode.ElementMetadata[childElementCollection] = childElements;
            }

            return childElements;
        }

        private static void ProcessChildElement(IList childElements, PrintElementNode childElementNode, Action<object> action, bool wrapElementMetadata)
        {
            if (childElements != null)
            {
                var childElementType = childElementNode.ElementType;
                var childElementMetadata = childElementNode.ElementMetadata;

                foreach (dynamic element in childElements)
                {
                    if ((wrapElementMetadata && (element[childElementType] == childElementMetadata)) ||
                        (!wrapElementMetadata && (element == childElementMetadata)))
                    {
                        action(element);
                        break;
                    }
                }
            }
        }

        public static TR TryInvoke<TR>(this Func<TR> func)
        {
            return (func != null) ? func() : default(TR);
        }

        public static TR TryInvoke<T1, TR>(this Func<T1, TR> func, T1 arg1)
        {
            return (func != null) ? func(arg1) : default(TR);
        }

        public static TR TryInvoke<T1, T2, TR>(this Func<T1, T2, TR> func, T1 arg1, T2 arg2)
        {
            return (func != null) ? func(arg1, arg2) : default(TR);
        }
    }
}