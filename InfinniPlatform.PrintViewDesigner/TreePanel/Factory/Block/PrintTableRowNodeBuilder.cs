using System;
using System.Linq;

using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintTableRowNodeBuilder : NodeBuilderBase<PrintTableRow>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintTableRow elementTemplate)
        {
            var rowNode = parentNode.CreateChildNode(PrintElementNodeType.PrintTableRowNode, elementTemplate);

            rowNode.CanCut = NodeBuilderHelper.CanCut(rowNode);
            rowNode.Cut = NodeBuilderHelper.Cut(rowNode);

            rowNode.CanCopy = NodeBuilderHelper.CanCopy(rowNode);
            rowNode.Copy = NodeBuilderHelper.Copy(rowNode);

            rowNode.CanPaste = NodeBuilderHelper.CanPaste(rowNode);
            rowNode.Paste = NodeBuilderHelper.Paste(rowNode);

            rowNode.CanInsertChild = CanInsertTableRowCell(rowNode);
            rowNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintTableRow, PrintTableCell>(factory, rowNode, p => p.Cells);

            rowNode.CanDeleteChild = CanDeleteTableRowCell(rowNode);
            rowNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintTableRow, PrintTableCell>(rowNode, p => p.Cells);

            rowNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintTableRow, PrintTableCell>(rowNode);
            rowNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintTableRow, PrintTableCell>(rowNode, p => p.Cells);

            factory.CreateNodes(rowNode, elementTemplate.Cells);
        }


        private static Func<PrintElementNode, bool> CanInsertTableRowCell(PrintElementNode rowNode)
        {
            // Добавление ячейки в строку возможно, если количество ячеек в строке меньше количества столбцов

            var canInsertRowCell = NodeBuilderHelper.CanInsertChild<PrintTableRow, PrintTableCell>(rowNode);

            return rowCellNode =>
            {
                var columnCount = GetTableColumnCount(rowNode.Parent.Parent);
                var rowCellCount = rowNode.Nodes.Count;

                return (rowCellCount < columnCount) && canInsertRowCell(rowCellNode);
            };
        }

        private static Func<PrintElementNode, bool, bool> CanDeleteTableRowCell(PrintElementNode rowNode)
        {
            // Удаление ячейки из строки возможно, если количество ячеек в строке больше количества столбцов

            var canDeleteRowCell = NodeBuilderHelper.CanDeleteChild<PrintTableRow, PrintTableCell>(rowNode);

            return (rowCellNode, accept) =>
            {
                var columnCount = GetTableColumnCount(rowNode.Parent.Parent);
                var rowCellCount = rowNode.Nodes.Count;

                return (rowCellCount > columnCount) && canDeleteRowCell(rowCellNode, accept);
            };
        }

        private static int GetTableColumnCount(PrintElementNode tableNode)
        {
            // Узел с определением столбцов таблицы
            var columnsNode = tableNode?.Nodes.FirstOrDefault(n => n.ElementType == PrintElementNodeType.PrintTableColumnsNode);

            if (columnsNode != null)
            {
                return columnsNode.Nodes.Count;
            }

            return 0;
        }
    }
}