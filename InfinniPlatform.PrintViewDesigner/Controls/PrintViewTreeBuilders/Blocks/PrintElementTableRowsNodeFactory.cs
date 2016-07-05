using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementTableRowsNodeFactory : IPrintElementNodeFactory
    {
        private static readonly string[] TableRows = {"TableRow"};

        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.ElementChildrenTypes = TableRows;
            elementNode.CanInsertChild = BuildHelper.CanInsertChild(elementNode);
            elementNode.InsertChild = InsertTableRow(builder, elements, elementNode);

            elementNode.CanDeleteChild = BuildHelper.CanDeleteChild(elementNode);
            elementNode.DeleteChild = DeleteTableRow(elements, elementNode);

            elementNode.CanMoveChild = BuildHelper.CanMoveChild(elementNode);
            elementNode.MoveChild = MoveTableRow(elementNode);

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElements(elements, elementNode, elementNode.ElementMetadata.Rows, "TableRow");
        }

        private static Func<PrintElementNode, bool> InsertTableRow(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode parentElementNode)
        {
            // Добавление строки добавляет в нее недостающие ячейки или удаляет из нее лишние

            var getColumnCountFunc = GetTableColumnCount(parentElementNode);
            var getLastTableRowFunc = GetLastTableRow(parentElementNode);
            var insertChildFunc = BuildHelper.InsertChildToCollection(builder, elements, parentElementNode, "Rows",
                false);

            return rowElementNode =>
            {
                if (insertChildFunc(rowElementNode))
                {
                    var insertedRow = getLastTableRowFunc();

                    if (insertedRow != null)
                    {
                        var columnCount = getColumnCountFunc();
                        var insertedRowCellCount = insertedRow.Nodes.Count;

                        // Добавление недостающих ячеек
                        if (insertedRowCellCount < columnCount)
                        {
                            for (var i = insertedRowCellCount; i < columnCount; ++i)
                            {
                                var cellNode = new PrintElementNode(insertedRow, "TableCell", new DynamicWrapper());

                                insertedRow.InsertChild.TryInvoke(cellNode);
                            }
                        }
                        // Удаление лишних ячеек
                        else if (insertedRowCellCount > columnCount)
                        {
                            for (var i = insertedRowCellCount - 1; i >= columnCount; --i)
                            {
                                var cellNode = insertedRow.Nodes[i];

                                insertedRow.DeleteChild.TryInvoke(cellNode, false);
                            }
                        }
                    }

                    return true;
                }

                return false;
            };
        }

        private static Func<PrintElementNode, bool, bool> DeleteTableRow(ICollection<PrintElementNode> elements, PrintElementNode parentElementNode)
        {
            return BuildHelper.DeleteChildFromCollection(elements, parentElementNode, "Rows", false);
        }

        private static Func<PrintElementNode, int, bool> MoveTableRow(PrintElementNode parentElementNode)
        {
            return BuildHelper.MoveChildInCollection(parentElementNode, "Rows", false);
        }

        private static Func<int> GetTableColumnCount(PrintElementNode parentElementNode)
        {
            return () =>
            {
                // Узел с определением таблицы
                var tableNode = parentElementNode.Parent;

                if (tableNode != null)
                {
                    // Узел с определением столбцов таблицы
                    var columnsNode = tableNode.Nodes.FirstOrDefault(n => string.Equals(n.ElementType, "TableColumns", StringComparison.OrdinalIgnoreCase));

                    if (columnsNode != null)
                    {
                        return columnsNode.Nodes.Count;
                    }
                }

                return 0;
            };
        }

        private static Func<PrintElementNode> GetLastTableRow(PrintElementNode parentElementNode)
        {
            return () => parentElementNode.Nodes.LastOrDefault();
        }
    }
}