using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    internal static class NodeBuilderHelper
    {
        /// <summary>
        /// Список типов для <see cref="PrintBlock"/>.
        /// </summary>
        public static readonly PrintElementNodeType[] BlockTypes =
        {
            PrintElementNodeType.PrintLineNode,
            PrintElementNodeType.PrintListNode,
            PrintElementNodeType.PrintPageBreakNode,
            PrintElementNodeType.PrintParagraphNode,
            PrintElementNodeType.PrintSectionNode,
            PrintElementNodeType.PrintTableNode
        };

        /// <summary>
        /// Список типов для <see cref="PrintInline"/>.
        /// </summary>
        public static readonly PrintElementNodeType[] InlineTypes =
        {
            PrintElementNodeType.PrintBarcodeEan13Node,
            PrintElementNodeType.PrintBarcodeQrNode,
            PrintElementNodeType.PrintBoldNode,
            PrintElementNodeType.PrintHyperlinkNode,
            PrintElementNodeType.PrintImageNode,
            PrintElementNodeType.PrintItalicNode,
            PrintElementNodeType.PrintLineBreakNode,
            PrintElementNodeType.PrintRunNode,
            PrintElementNodeType.PrintSpanNode,
            PrintElementNodeType.PrintUnderlineNode
        };


        /// <summary>
        /// Возвращает функцию проверки возможности помещения узла в буфер.
        /// </summary>
        public static Func<bool> CanCut(PrintElementNode node)
        {
            return () => (node.Cut != null);
        }

        /// <summary>
        /// Возвращает функцию помещения узла в буфер.
        /// </summary>
        public static Func<bool> Cut(PrintElementNode node)
        {
            return () =>
                   {
                       if (node.CanCut?.Invoke() == true)
                       {
                           AppClipboard.Default.SetData(node, false);
                           return true;
                       }

                       return false;
                   };
        }


        /// <summary>
        /// Возвращает функцию проверки возможности копирования узла в буфер.
        /// </summary>
        public static Func<bool> CanCopy(PrintElementNode node)
        {
            return () => (node.Copy != null);
        }

        /// <summary>
        /// Возвращает функцию копирования узла в буфер.
        /// </summary>
        public static Func<bool> Copy(PrintElementNode node)
        {
            return () =>
                   {
                       if (node.CanCopy?.Invoke() == true)
                       {
                           AppClipboard.Default.SetData(node, true);
                           return true;
                       }

                       return false;
                   };
        }


        /// <summary>
        /// Возвращает функцию проверки возможности вставки узла из буфера.
        /// </summary>
        public static Func<bool> CanPaste(PrintElementNode node)
        {
            return () => (node.Paste != null) && AppClipboard.Default.ContainsData<PrintElementNode>();
        }

        /// <summary>
        /// Возвращает функцию вставки узла из буфера.
        /// </summary>
        public static Func<bool> Paste(PrintElementNode node)
        {
            return () =>
                   {
                       if (node.CanPaste?.Invoke() == true)
                       {
                           var entry = AppClipboard.Default.GetData<PrintElementNode>();

                           if (entry != null)
                           {
                               var copy = entry.Copy;
                               var childElementNode = (PrintElementNode)entry.Data;

                               // Элемент не может быть вставлен сам в себя
                               if (childElementNode == node)
                               {
                                   MessageBoxHelpers.ShowWarningMessage(Resources.CannotBePastedIntoItself);
                                   return false;
                               }

                               // Родитель не может быть вставлен в своего потомка
                               if (IsParentNode(childElementNode, node))
                               {
                                   MessageBoxHelpers.ShowWarningMessage(Resources.CannotInsertParentIntoChild, node, childElementNode);
                                   return false;
                               }

                               // Невозможно поместить заданный элемент в родительский
                               if (node.CanInsertChild?.Invoke(childElementNode) == false)
                               {
                                   MessageBoxHelpers.ShowWarningMessage(Resources.CannotInsertChildElement, childElementNode, node);
                                   return false;
                               }

                               // Удаление элемента из текущего родительского
                               if (!copy)
                               {
                                   childElementNode.Parent?.DeleteChild(childElementNode, false);
                               }

                               // Вставка элемента в указанный родительский
                               var newElementTemplate = CloneElementTemplate(childElementNode.ElementMetadata);
                               var newChildElementNode = new PrintElementNode(node, childElementNode.ElementType, newElementTemplate);
                               node.InsertChild?.Invoke(newChildElementNode);

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


        /// <summary>
        /// Возвращает функцию проверки возможности осуществления вставки дочернего узла из буфера.
        /// </summary>
        public static Func<PrintElementNode, bool> CanInsertChild<TParent, TChild>(PrintElementNode parentNode)
        {
            return childNode => (parentNode.InsertChild != null)
                                && (parentNode.ElementMetadata is TParent)
                                && (childNode.ElementMetadata is TChild);
        }

        /// <summary>
        /// Возвращает функцию вставки дочернего узла из буфера.
        /// </summary>
        public static Func<PrintElementNode, bool> InsertChildToCollection<TParent, TChild>(NodeFactory factory, PrintElementNode parentNode, Func<TParent, List<TChild>> children)
        {
            return childNode =>
                   {
                       if (parentNode.CanInsertChild?.Invoke(childNode) == true)
                       {
                           var parentElement = (TParent)parentNode.ElementMetadata;
                           var childElement = (TChild)childNode.ElementMetadata;

                           if (parentElement != null && childElement != null)
                           {
                               // Добавление дочернего элемента в родительский
                               children(parentElement)?.Add(childElement);

                               // Добавление дочернего элемента в визуальное дерево
                               factory.CreateNode(parentNode, childElement);
                           }

                           return true;
                       }

                       return false;
                   };
        }

        /// <summary>
        /// Возвращает функцию вставки дочернего узла из буфера.
        /// </summary>
        public static Func<PrintElementNode, bool> InsertChildToContainer<TParent, TChild>(NodeFactory factory, PrintElementNode parentNode, Action<TParent, TChild> children)
        {
            return childNode =>
                   {
                       if (parentNode.CanInsertChild?.Invoke(childNode) == true)
                       {
                           var parentElement = (TParent)parentNode.ElementMetadata;
                           var childElement = (TChild)childNode.ElementMetadata;

                           var oldChildNode = parentNode.Nodes.FirstOrDefault();

                           if (oldChildNode == null || MessageBoxHelpers.AcceptQuestionMessage(Resources.ReplaceElementQuestion, parentNode, childNode))
                           {
                               // Добавление нового дочернего элемента в родительский
                               children(parentElement, childElement);

                               if (oldChildNode != null)
                               {
                                   // Удаление старого дочернего элемента из визуального дерева
                                   parentNode.RemoveChildNode(oldChildNode);
                               }

                               // Добавление нового дочернего элемента в визуальное дерево
                               factory.CreateNode(parentNode, childElement);

                               return true;
                           }
                       }

                       return false;
                   };
        }


        /// <summary>
        /// Возвращает функцию проверки возможности осуществления удаления дочернего узла.
        /// </summary>
        public static Func<PrintElementNode, bool, bool> CanDeleteChild<TParent, TChild>(PrintElementNode parentNode)
        {
            return (childNode, accept) => (parentNode.DeleteChild != null)
                                          && (parentNode.ElementMetadata is TParent)
                                          && (childNode.ElementMetadata is TChild);
        }

        /// <summary>
        /// Возвращает функцию удаления дочернего узла.
        /// </summary>
        public static Func<PrintElementNode, bool, bool> DeleteChildFromCollection<TParent, TChild>(PrintElementNode parentNode, Func<TParent, List<TChild>> children)
        {
            return (childNode, accept) =>
                   {
                       if (parentNode.CanDeleteChild?.Invoke(childNode, accept) == true)
                       {
                           if (!accept || MessageBoxHelpers.AcceptQuestionMessage(Resources.DeleteElementQuestion, childNode))
                           {
                               var parentElement = (TParent)parentNode.ElementMetadata;
                               var childElement = (TChild)childNode.ElementMetadata;

                               if (parentElement != null && childElement != null)
                               {
                                   // Удаление дочернего элемента из родительского
                                   children(parentElement)?.Remove(childElement);

                                   // Удаление дочернего элемента из визуального дерева
                                   parentNode.RemoveChildNode(childNode);

                                   return true;
                               }
                           }
                       }

                       return false;
                   };
        }

        /// <summary>
        /// Возвращает функцию удаления дочернего узла.
        /// </summary>
        public static Func<PrintElementNode, bool, bool> DeleteChildFromContainer<TParent, TChild>(PrintElementNode parentNode, Action<TParent, TChild> children)
        {
            return (childNode, accept) =>
                   {
                       if (parentNode.CanDeleteChild?.Invoke(childNode, accept) == true)
                       {
                           if (!accept || MessageBoxHelpers.AcceptQuestionMessage(Resources.DeleteElementQuestion, childNode))
                           {
                               var parentElement = (TParent)parentNode.ElementMetadata;
                               var childElement = (TChild)childNode.ElementMetadata;

                               // Удаление дочернего элемента из родительского
                               children(parentElement, childElement);

                               // Удаление дочернего элемента из визуального дерева
                               parentNode.RemoveChildNode(childNode);

                               return true;
                           }
                       }

                       return false;
                   };
        }


        /// <summary>
        /// Возвращает функцию проверки возможности осуществления изменения порядкового номера дочернего узла.
        /// </summary>
        public static Func<PrintElementNode, int, bool> CanMoveChildInCollection<TParent, TChild>(PrintElementNode parentNode)
        {
            return (childNode, delta) => (parentNode.MoveChild != null)
                                         && (parentNode.ElementMetadata is TParent)
                                         && (childNode.ElementMetadata is TChild);
        }

        /// <summary>
        /// Возвращает функцию изменения порядкового номера дочернего узла.
        /// </summary>
        public static Func<PrintElementNode, int, bool> MoveChildInCollection<TParent, TChild>(PrintElementNode parentNode, Func<TParent, List<TChild>> children)
        {
            return (childNode, delta) =>
                   {
                       if (parentNode.CanMoveChild?.Invoke(childNode, delta) == true)
                       {
                           var parentElement = (TParent)parentNode.ElementMetadata;
                           var childElement = (TChild)childNode.ElementMetadata;

                           if (parentElement != null && childElement != null)
                           {
                               // Перемещение дочернего элемента в родительском
                               children(parentElement)?.MoveItem(childElement, delta);

                               // Перемещение дочернего элемента в визуальном дереве
                               parentNode.MoveChildNode(childNode, delta);

                               return true;
                           }
                       }

                       return false;
                   };
        }


        private static object CloneElementTemplate(object elementTemplate)
        {
            // При копировании элемента происходит глубокое клонирование
            // с помощью бинарной сериализации и десериализации

            if (elementTemplate != null)
            {
                var serializer = new BinaryFormatter();

                using (var memory = new MemoryStream())
                {
                    serializer.Serialize(memory, elementTemplate);

                    memory.Position = 0;

                    return serializer.Deserialize(memory);
                }
            }

            return null;
        }
    }
}