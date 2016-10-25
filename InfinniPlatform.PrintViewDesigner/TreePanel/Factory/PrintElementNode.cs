using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    /// <summary>
    /// Узел дерева для отображения шаблона документа печатного представления <see cref="PrintDocument"/>.
    /// </summary>
    /// <remarks>
    /// Документ печатного представления <see cref="PrintDocument"/> представляется в виде дерева. Каждый узел этого дерева
    /// описывается с помощью экземпляра класса <see cref="PrintElementNode"/>, который связан с определенной частью документа.
    /// Узел предоставляет информацию об элементе и набор функций для его перемещения внутри документа.
    /// </remarks>
    public sealed class PrintElementNode : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentNode">Родительский узел.</param>
        /// <param name="elementType">Тип элемента печатного представления.</param>
        /// <param name="elementTemplate">Шаблон элемента печатного представления.</param>
        public PrintElementNode(PrintElementNode parentNode, PrintElementNodeType elementType, object elementTemplate)
            : this(parentNode.Tree, parentNode, elementType, elementTemplate)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tree">Дерево узлов.</param>
        /// <param name="parentNode">Родительский узел.</param>
        /// <param name="elementType">Тип элемента печатного представления.</param>
        /// <param name="elementTemplate">Шаблон элемента печатного представления.</param>
        public PrintElementNode(ICollection<PrintElementNode> tree, PrintElementNode parentNode, PrintElementNodeType elementType, object elementTemplate)
        {
            Tree = tree;

            Key = this;
            Parent = parentNode;
            Nodes = new List<PrintElementNode>();

            ElementType = elementType;
            ElementMetadata = elementTemplate;

            Refresh();
        }


        /// <summary>
        /// Дерево узлов.
        /// </summary>
        public readonly ICollection<PrintElementNode> Tree;


        /// <summary>
        /// Идентификатор узла.
        /// </summary>
        public PrintElementNode Key { get; }

        /// <summary>
        /// Родительский узел.
        /// </summary>
        public PrintElementNode Parent { get; }

        /// <summary>
        /// Список дочерних узлов.
        /// </summary>
        public List<PrintElementNode> Nodes { get; }

        /// <summary>
        /// Тип элемента печатного представления.
        /// </summary>
        public PrintElementNodeType ElementType { get; }

        /// <summary>
        /// Шаблон элемента печатного представления.
        /// </summary>
        public object ElementMetadata { get; }


        private int _index;

        /// <summary>
        /// Порядковый номер элемента печатного представления.
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (!Equals(_index, value))
                {
                    _index = value;

                    OnPropertyChanged();
                }
            }
        }


        private PrintVisibility? _visibility;

        /// <summary>
        /// Видимость элемента печатного представления.
        /// </summary>
        public PrintVisibility? Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                if (!Equals(_visibility, value))
                {
                    _visibility = value;

                    OnPropertyChanged();
                }
            }
        }


        private string _displayName;

        /// <summary>
        /// Отображаемое имя элемента печатного представления.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (!Equals(_displayName, value))
                {
                    _displayName = value;

                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Список допустимых типов дочерних элементов.
        /// </summary>
        public PrintElementNodeType[] ElementChildrenTypes { get; set; }


        /// <summary>
        /// Проверяет возможность помещения узла в буфер.
        /// </summary>
        public Func<bool> CanCut { get; set; }

        /// <summary>
        /// Помещает узел в буфер.
        /// </summary>
        public Func<bool> Cut { get; set; }


        /// <summary>
        /// Проверяет возможность копирования узла в буфер.
        /// </summary>
        public Func<bool> CanCopy { get; set; }

        /// <summary>
        /// Копирует узле в буфер.
        /// </summary>
        public Func<bool> Copy { get; set; }


        /// <summary>
        /// Проверяет возможность вставки узла из буфера.
        /// </summary>
        public Func<bool> CanPaste { get; set; }

        /// <summary>
        /// Вставляет узел из буфера.
        /// </summary>
        public Func<bool> Paste { get; set; }


        /// <summary>
        /// Проверяет возможность осуществления вставки дочернего узла из буфера.
        /// </summary>
        public Func<PrintElementNode, bool> CanInsertChild { get; set; }

        /// <summary>
        /// Вставляет дочерний узел из буфера.
        /// </summary>
        public Func<PrintElementNode, bool> InsertChild { get; set; }


        /// <summary>
        /// Проверяет возможность осуществления удаления дочернего узла.
        /// </summary>
        public Func<PrintElementNode, bool, bool> CanDeleteChild { get; set; }

        /// <summary>
        /// Удаляет дочерний узел.
        /// </summary>
        public Func<PrintElementNode, bool, bool> DeleteChild { get; set; }


        /// <summary>
        /// Проверяет возможность осуществления изменения порядкового номера дочернего узла.
        /// </summary>
        public Func<PrintElementNode, int, bool> CanMoveChild { get; set; }

        /// <summary>
        /// Изменяет порядковый номер дочернего узла.
        /// </summary>
        public Func<PrintElementNode, int, bool> MoveChild { get; set; }


        /// <summary>
        /// Событие для оповещения об изменении свойств узла.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие для оповещения об изменении свойств узла <see cref="PropertyChanged"/>.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Обновляет свойства узла.
        /// </summary>
        public void Refresh()
        {
            RefreshVisibility();
            RefreshDisplayName();
        }

        /// <summary>
        /// Обновляет свойство <see cref="Visibility"/>.
        /// </summary>
        private void RefreshVisibility()
        {
            var printElement = ElementMetadata as PrintElement;

            Visibility = printElement?.Visibility;
        }

        /// <summary>
        /// Обновляет свойство <see cref="DisplayName"/>.
        /// </summary>
        private void RefreshDisplayName()
        {
            if (IsLogicGroup())
            {
                DisplayName = Resources.ResourceManager.GetString(ElementType.ToString(), Resources.Culture);
            }
            else
            {
                var namedItem = ElementMetadata as PrintNamedItem;

                DisplayName = (namedItem != null) ? namedItem.GetDisplayName() : Resources.ResourceManager.GetString(ElementType.ToString(), Resources.Culture);
            }
        }


        /// <summary>
        /// Создает дочерний узел.
        /// </summary>
        public PrintElementNode CreateChildNode(PrintElementNodeType elementType, object elementTemplate)
        {
            var childNode = new PrintElementNode(this, elementType, elementTemplate);

            AddChildNode(childNode);

            return childNode;
        }


        /// <summary>
        /// Добавляет дочерний узел.
        /// </summary>
        public void AddChildNode(PrintElementNode childNode)
        {
            // Добавление дочернего узла
            Nodes.Add(childNode);

            // Добавление узла в дерево
            Tree.Add(childNode);

            // Установка порядкового номера
            childNode.Index = Nodes.Count;
        }


        /// <summary>
        /// Удаляет дочерний узел.
        /// </summary>
        public void RemoveChildNode(PrintElementNode childNode)
        {
            // Удаление дочернего узла
            Nodes.Remove(childNode);

            // Удаление узла из дерева
            DeleteChildNodeFromTree(childNode);

            // Пересчет порядковых номеров
            RecalcChildNodeIndexes();
        }


        /// <summary>
        /// Перемещает дочерний узел.
        /// </summary>
        public void MoveChildNode(PrintElementNode childNode, int delta)
        {
            // Перемещение дочернего узла
            Nodes.MoveItem(childNode, delta);

            // Пересчет порядковых номеров
            RecalcChildNodeIndexes();
        }


        private void DeleteChildNodeFromTree(PrintElementNode childNode)
        {
            Tree.Remove(childNode);

            foreach (var node in childNode.Nodes)
            {
                DeleteChildNodeFromTree(node);
            }
        }

        private void RecalcChildNodeIndexes()
        {
            var index = 0;

            foreach (var node in Nodes)
            {
                node.Index = index++;
            }
        }


        /// <summary>
        /// Проверяет, является ли элемент логической группой.
        /// </summary>
        public bool IsLogicGroup()
        {
            return !(ElementMetadata is PrintNamedItem)
                   || (ElementMetadata == Parent?.ElementMetadata);
        }


        /// <summary>
        /// Возвращает строковое представление узла.
        /// </summary>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}