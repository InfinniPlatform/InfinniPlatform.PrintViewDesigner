using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Block;

using BlockElement = System.Windows.Documents.Block;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    internal sealed class PrintTableFlowBuilder : FlowBuilderBase<PrintTable>
    {
        protected override object Build(FlowBuilderContext context, PrintTable element, PrintDocumentMap documentMap)
        {
            var flowElement = new Table { CellSpacing = 0, RowGroups = { new TableRowGroup() } };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyBlockStyles(flowElement, element);

            foreach (var column in element.Columns)
            {
                var columnSize = Math.Max(FlowBuilderHelper.GetSizeInPixels(column.Size, column.SizeUnit), 1);
                var flowColumn = new TableColumn { Width = new GridLength(columnSize) };
                documentMap.RemapElement(column, flowColumn);
                flowElement.Columns.Add(flowColumn);
            }

            var ignoredCells = new List<CellCoordinate>();

            for (var i = 0; i < element.Rows.Count; i++)
            {
                var flowRow = new TableRow();

                ApplyRowStyles(flowRow, element.Rows[i]);

                var colSpansCount = 0;

                for (var j = 0; j < element.Rows[i].Cells.Count; j++)
                {
                    var cellIndex = new CellCoordinate(i, j);

                    if (ignoredCells.Contains(cellIndex))
                    {
                        continue;
                    }

                    var cellColumnSpan = Math.Max(element.Rows[i].Cells[j].ColumnSpan ?? 1, 1);
                    var cellRowSpan = Math.Max(element.Rows[i].Cells[j].RowSpan ?? 1, 1);

                    if (cellRowSpan > 1)
                    {
                        AddIgnoredCells(ignoredCells, cellIndex, cellRowSpan);
                    }

                    colSpansCount += cellColumnSpan;

                    if (colSpansCount > element.Columns.Count)
                    {
                        cellColumnSpan = element.Columns.Count - (colSpansCount - cellColumnSpan);
                    }

                    var flowCell = new TableCell { ColumnSpan = Math.Max(cellColumnSpan, 1), RowSpan = Math.Max(cellRowSpan, 1) };

                    ApplyCellStyles(flowCell, element.Rows[i].Cells[j]);

                    var flowCellContent = context.Build<BlockElement>(element.Rows[i].Cells[j].Block, documentMap);

                    if (flowCellContent != null)
                    {
                        flowCell.Blocks.Add(flowCellContent);
                    }

                    documentMap.RemapElement(element.Rows[i].Cells[j], flowCell);

                    flowRow.Cells.Add(flowCell);
                }

                documentMap.RemapElement(element.Rows[i], flowRow);

                flowElement.RowGroups[0].Rows.Add(flowRow);
            }

            return flowElement;
        }

        private static void AddIgnoredCells(ICollection<CellCoordinate> ignoredCells, CellCoordinate currentCellCoordinate, int? rowSpan)
        {
            if (rowSpan == null)
            {
                return;
            }

            for (var i = 1; i < rowSpan.Value; i++)
            {
                ignoredCells.Add(new CellCoordinate(currentCellCoordinate.Row + i, currentCellCoordinate.Column));
            }
        }


        private static void ApplyRowStyles(TableRow flowElement, PrintTableRow element)
        {
            FlowBuilderHelper.SetFont(flowElement, element.Font);
            FlowBuilderHelper.SetForeground(flowElement, element.Foreground);
            FlowBuilderHelper.SetBackground(flowElement, element.Background);
        }

        private static void ApplyCellStyles(TableCell flowElement, PrintTableCell element)
        {
            FlowBuilderHelper.SetFont(flowElement, element.Font);
            FlowBuilderHelper.SetForeground(flowElement, element.Foreground);
            FlowBuilderHelper.SetBackground(flowElement, element.Background);

            FlowBuilderHelper.SetBorder(flowElement, element.Border);
            FlowBuilderHelper.SetPadding(flowElement, element.Padding);
            FlowBuilderHelper.SetTextAlignment(flowElement, element.TextAlignment);
        }
    }
}