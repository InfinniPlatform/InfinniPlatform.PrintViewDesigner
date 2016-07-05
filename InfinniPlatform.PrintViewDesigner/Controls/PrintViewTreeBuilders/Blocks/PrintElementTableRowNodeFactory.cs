using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks
{
    internal sealed class PrintElementTableRowNodeFactory : IPrintElementNodeFactory
    {
        private static readonly string[] TableCells = {"TableCell"};

        public void Create(PrintElementNodeBuilder builder, ICollection<PrintElementNode> elements, PrintElementNode elementNode)
        {
            elementNode.ElementChildrenTypes = TableCells;
            elementNode.CanInsertChild = CanInsertTableRowCell(elementNode);
            elementNode.InsertChild = BuildHelper.InsertChildToCollection(builder, elements, elementNode, "Cells", false);

            elementNode.CanDeleteChild = CanDeleteTableRowCell(elementNode);
            elementNode.DeleteChild = BuildHelper.DeleteChildFromCollection(elements, elementNode, "Cells", false);

            elementNode.CanMoveChild = BuildHelper.CanMoveChild(elementNode);
            elementNode.MoveChild = BuildHelper.MoveChildInCollection(elementNode, "Cells", false);

            elementNode.CanCut = BuildHelper.CanCut(elementNode);
            elementNode.Cut = BuildHelper.Cut(elementNode);

            elementNode.CanCopy = BuildHelper.CanCopy(elementNode);
            elementNode.Copy = BuildHelper.Copy(elementNode);

            elementNode.CanPaste = BuildHelper.CanPaste(elementNode);
            elementNode.Paste = BuildHelper.Paste(elementNode);

            builder.BuildElements(elements, elementNode, elementNode.ElementMetadata.Cells, "TableCell");
        }

        private static Func<PrintElementNode, bool> CanInsertTableRowCell(PrintElementNode parentElementNode)
        {
            // Добавление ячейки в строку возможно, если количество ячеек в строке меньше количества столбцов

            var getColumnCountFunc = GetTableColumnCount(parentElementNode);
            var getTableRowCellCountFunc = GetTableRowCellCount(parentElementNode);
            var canInsertChildFunc = BuildHelper.CanInsertChild(parentElementNode);

            return cellElementNode =>
            {
                var columnCount = getColumnCountFunc();
                var rowCellCount = getTableRowCellCountFunc();
                return (rowCellCount < columnCount) && canInsertChildFunc(cellElementNode);
            };
        }

        private static Func<PrintElementNode, bool, bool> CanDeleteTableRowCell(PrintElementNode parentElementNode)
        {
            // Удаление ячейки из строки возможно, если количество ячеек в строке больше количества столбцов

            var getColumnCountFunc = GetTableColumnCount(parentElementNode);
            var getTableRowCellCountFunc = GetTableRowCellCount(parentElementNode);
            var canDeleteChildFunc = BuildHelper.CanDeleteChild(parentElementNode);

            return (cellElementNode, accept) =>
            {
                var columnCount = getColumnCountFunc();
                var rowCellCount = getTableRowCellCountFunc();
                return (rowCellCount > columnCount) && canDeleteChildFunc(cellElementNode, accept);
            };
        }

        private static Func<int> GetTableColumnCount(PrintElementNode parentElementNode)
        {
            return () =>
            {
                // Узел с определение строк таблицы
                var rowsNode = parentElementNode.Parent;

                if (rowsNode != null)
                {
                    // Узел с определением таблицы
                    var tableNode = rowsNode.Parent;

                    if (tableNode != null)
                    {
                        // Узел с определением столбцов таблицы
                        var columnsNode = tableNode.Nodes.FirstOrDefault(n => string.Equals(n.ElementType, "TableColumns", StringComparison.OrdinalIgnoreCase));

                        if (columnsNode != null)
                        {
                            return columnsNode.Nodes.Count;
                        }
                    }
                }

                return 0;
            };
        }

        private static Func<int> GetTableRowCellCount(PrintElementNode parentElementNode)
        {
            return () => parentElementNode.Nodes.Count;
        }
    }
}