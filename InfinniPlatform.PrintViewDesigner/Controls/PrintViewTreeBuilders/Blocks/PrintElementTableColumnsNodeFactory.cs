using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementTableColumnsNodeFactory : IPrintElementNodeFactory
    {
        private static readonly string[] TableColumns = {"TableColumn"};

        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.ElementChildrenTypes = TableColumns;
            elementNode.CanInsertChild = BuildHelper.CanInsertChild(elementNode);
            elementNode.InsertChild = InsertTableColumn(builder, elements, elementNode);

            elementNode.CanDeleteChild = BuildHelper.CanDeleteChild(elementNode);
            elementNode.DeleteChild = DeleteTableColumn(elements, elementNode);

            elementNode.CanMoveChild = BuildHelper.CanMoveChild(elementNode);
            elementNode.MoveChild = MoveTableColumn(elementNode);

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElements(elements, elementNode, elementNode.ElementMetadata.Columns, "TableColumn");
        }

        private static Func<PrintElementNode, bool> InsertTableColumn(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode parentElementNode)
        {
            // Добавление столбца добавляет ячейку во все строки таблицы

            var getTableRowsFunc = GetTableRows(parentElementNode);
            var insertChildFunc = BuildHelper.InsertChildToCollection(builder, elements, parentElementNode, "Columns", false);

            return columnElementNode =>
            {
                if (insertChildFunc(columnElementNode))
                {
                    var rows = getTableRowsFunc();

                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            row.InsertChild.TryInvoke(new PrintElementNode(row, "TableCell", new DynamicWrapper()));
                        }
                    }

                    return true;
                }

                return false;
            };
        }

        private static Func<PrintElementNode, bool, bool> DeleteTableColumn(ICollection<PrintElementNode> elements, PrintElementNode parentElementNode)
        {
            // Удаление столбца удаляет соответствующие ячейки во всех строчках таблицы

            var getTableRowsFunc = GetTableRows(parentElementNode);
            var deleteChildFunc = BuildHelper.DeleteChildFromCollection(elements, parentElementNode, "Columns", false);

            return (columnElementNode, accept) =>
            {
                var columnIndex = columnElementNode.Index;

                if (deleteChildFunc(columnElementNode, accept))
                {
                    var rows = getTableRowsFunc();

                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            foreach (var cell in row.Nodes)
                            {
                                if (cell.Index == columnIndex)
                                {
                                    row.DeleteChild.TryInvoke(cell, false);
                                    break;
                                }
                            }
                        }
                    }

                    return true;
                }

                return false;
            };
        }

        private static Func<PrintElementNode, int, bool> MoveTableColumn(PrintElementNode parentElementNode)
        {
            // Перемещение столбца перемещает соответствующие ячейки во всех строчках таблицы

            var getTableRowsFunc = GetTableRows(parentElementNode);
            var moveChildFunc = BuildHelper.MoveChildInCollection(parentElementNode, "Columns", false);

            return (columnElementNode, delta) =>
            {
                var columnIndex = columnElementNode.Index;

                if (moveChildFunc(columnElementNode, delta))
                {
                    var rows = getTableRowsFunc();

                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            foreach (var cell in row.Nodes)
                            {
                                if (cell.Index == columnIndex)
                                {
                                    row.MoveChild.TryInvoke(cell, delta);
                                    break;
                                }
                            }
                        }
                    }

                    return true;
                }

                return false;
            };
        }

        private static Func<IEnumerable<PrintElementNode>> GetTableRows(PrintElementNode parentElementNode)
        {
            return () =>
            {
                // Узел с определением таблицы
                var tableNode = parentElementNode.Parent;

                if (tableNode != null)
                {
                    // Узел с определением строк таблицы
                    var rowsNode = tableNode.Nodes.FirstOrDefault(n => string.Equals(n.ElementType, "TableRows", StringComparison.OrdinalIgnoreCase));

                    if (rowsNode != null)
                    {
                        return rowsNode.Nodes;
                    }
                }

                return null;
            };
        }
    }
}