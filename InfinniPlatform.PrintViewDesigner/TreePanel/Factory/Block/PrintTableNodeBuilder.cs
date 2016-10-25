using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintTableNodeBuilder : NodeBuilderBase<PrintTable>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintTable elementTemplate)
        {
            var tableNode = parentNode.CreateChildNode(PrintElementNodeType.PrintTableNode, elementTemplate);

            tableNode.CanCut = NodeBuilderHelper.CanCut(tableNode);
            tableNode.Cut = NodeBuilderHelper.Cut(tableNode);

            tableNode.CanCopy = NodeBuilderHelper.CanCopy(tableNode);
            tableNode.Copy = NodeBuilderHelper.Copy(tableNode);

            BuildTableColumns(factory, tableNode, elementTemplate);
            BuildTableRows(factory, tableNode, elementTemplate);
        }


        // COLUMNS


        private static void BuildTableColumns(NodeFactory factory, PrintElementNode tableNode, PrintTable table)
        {
            var columnsNode = tableNode.CreateChildNode(PrintElementNodeType.PrintTableColumnsNode, table);

            columnsNode.ElementChildrenTypes = new [] { PrintElementNodeType.PrintTableColumnNode };

            columnsNode.CanPaste = NodeBuilderHelper.CanPaste(columnsNode);
            columnsNode.Paste = NodeBuilderHelper.Paste(columnsNode);

            columnsNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintTable, PrintTableColumn>(columnsNode);
            columnsNode.InsertChild = InsertTableColumn(factory, columnsNode);

            columnsNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintTable, PrintTableColumn>(columnsNode);
            columnsNode.DeleteChild = DeleteTableColumn(columnsNode);

            columnsNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintTable, PrintTableColumn>(columnsNode);
            columnsNode.MoveChild = MoveTableColumn(columnsNode);

            factory.CreateNodes(columnsNode, table.Columns);
        }

        private static Func<PrintElementNode, bool> InsertTableColumn(NodeFactory factory, PrintElementNode columnsNode)
        {
            // Добавление столбца добавляет ячейку во все строки таблицы

            var insertChildFunc = NodeBuilderHelper.InsertChildToCollection<PrintTable, PrintTableColumn>(factory, columnsNode, i => i.Columns);

            return columnElementNode =>
                   {
                       if (insertChildFunc(columnElementNode))
                       {
                           var rows = GetTableRows(columnsNode.Parent);

                           if (rows != null)
                           {
                               foreach (var row in rows)
                               {
                                   var cell = new PrintElementNode(row, PrintElementNodeType.PrintTableCellNode, new PrintTableCell());
                                   row.InsertChild?.Invoke(cell);
                               }
                           }

                           return true;
                       }

                       return false;
                   };
        }

        private static Func<PrintElementNode, bool, bool> DeleteTableColumn(PrintElementNode columnsNode)
        {
            // Удаление столбца удаляет соответствующие ячейки во всех строчках таблицы

            var deleteChildFunc = NodeBuilderHelper.DeleteChildFromCollection<PrintTable, PrintTableColumn>(columnsNode, i => i.Columns);

            return (columnElementNode, accept) =>
                   {
                       var columnIndex = columnElementNode.Index;

                       if (deleteChildFunc(columnElementNode, accept))
                       {
                           var rows = GetTableRows(columnsNode.Parent);

                           if (rows != null)
                           {
                               foreach (var row in rows)
                               {
                                   foreach (var cell in row.Nodes)
                                   {
                                       if (cell.Index == columnIndex)
                                       {
                                           row.DeleteChild?.Invoke(cell, false);
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

        private static Func<PrintElementNode, int, bool> MoveTableColumn(PrintElementNode columnsNode)
        {
            // Перемещение столбца перемещает соответствующие ячейки во всех строчках таблицы

            var moveChildFunc = NodeBuilderHelper.MoveChildInCollection<PrintTable, PrintTableColumn>(columnsNode, i => i.Columns);

            return (columnElementNode, delta) =>
                   {
                       var columnIndex = columnElementNode.Index;

                       if (moveChildFunc(columnElementNode, delta))
                       {
                           var rows = GetTableRows(columnsNode.Parent);

                           if (rows != null)
                           {
                               foreach (var row in rows)
                               {
                                   foreach (var cell in row.Nodes)
                                   {
                                       if (cell.Index == columnIndex)
                                       {
                                           row.MoveChild?.Invoke(cell, delta);
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

        private static List<PrintElementNode> GetTableRows(PrintElementNode tableNode)
        {
            // Узел с определением строк таблицы
            var rowsNode = tableNode.Nodes.FirstOrDefault(n => n.ElementType == PrintElementNodeType.PrintTableRowsNode);

            return rowsNode?.Nodes;
        }


        // ROWS


        private static void BuildTableRows(NodeFactory factory, PrintElementNode tableNode, PrintTable table)
        {
            var rowsNode = tableNode.CreateChildNode(PrintElementNodeType.PrintTableRowsNode, table);

            rowsNode.ElementChildrenTypes = new [] { PrintElementNodeType.PrintTableRowNode };

            rowsNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintTable, PrintTableRow>(rowsNode);
            rowsNode.InsertChild = InsertTableRow(factory, rowsNode);

            rowsNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintTable, PrintTableRow>(rowsNode);
            rowsNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintTable, PrintTableRow>(rowsNode, p => p.Rows);

            rowsNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintTable, PrintTableRow>(rowsNode);
            rowsNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintTable, PrintTableRow>(rowsNode, p => p.Rows);

            rowsNode.CanPaste = NodeBuilderHelper.CanPaste(rowsNode);
            rowsNode.Paste = NodeBuilderHelper.Paste(rowsNode);

            factory.CreateNodes(rowsNode, table.Rows);
        }

        private static Func<PrintElementNode, bool> InsertTableRow(NodeFactory factory, PrintElementNode rowsNode)
        {
            // Добавление строки добавляет в нее недостающие ячейки или удаляет из нее лишние

            var insertRowNode = NodeBuilderHelper.InsertChildToCollection<PrintTable, PrintTableRow>(factory, rowsNode, p => p.Rows);

            return rowNode =>
                   {
                       if (insertRowNode(rowNode))
                       {
                           var insertedRow = rowsNode.Nodes.LastOrDefault();

                           if (insertedRow != null)
                           {
                               var columnCount = GetTableColumnCount(rowsNode.Parent);
                               var insertedRowCellCount = insertedRow.Nodes.Count;

                               // Добавление недостающих ячеек
                               if (insertedRowCellCount < columnCount)
                               {
                                   for (var i = insertedRowCellCount; i < columnCount; ++i)
                                   {
                                       var cellNode = new PrintElementNode(insertedRow, PrintElementNodeType.PrintTableCellNode, new PrintTableCell());

                                       insertedRow.InsertChild?.Invoke(cellNode);
                                   }
                               }
                               // Удаление лишних ячеек
                               else if (insertedRowCellCount > columnCount)
                               {
                                   for (var i = insertedRowCellCount - 1; i >= columnCount; --i)
                                   {
                                       var cellNode = insertedRow.Nodes[i];

                                       insertedRow.DeleteChild?.Invoke(cellNode, false);
                                   }
                               }
                           }

                           return true;
                       }

                       return false;
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