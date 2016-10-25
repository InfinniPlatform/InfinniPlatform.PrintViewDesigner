using System;
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

            foreach (var row in element.Rows)
            {
                var flowRow = new TableRow();

                ApplyRowStyles(flowRow, row);

                var colSpansCount = 0;

                foreach (var cell in row.Cells)
                {
                    var cellColumnSpan = Math.Max(cell.ColumnSpan ?? 1, 1);
                    var cellRowSpan = Math.Max(cell.RowSpan ?? 1, 1);

                    colSpansCount += cellColumnSpan;

                    if (colSpansCount > element.Columns.Count)
                    {
                        cellColumnSpan = element.Columns.Count - (colSpansCount - cellColumnSpan);
                    }

                    var flowCell = new TableCell { ColumnSpan = Math.Max(cellColumnSpan, 1), RowSpan = Math.Max(cellRowSpan, 1) };

                    ApplyCellStyles(flowCell, cell);

                    var flowCellContent = context.Build<BlockElement>(cell.Block, documentMap);

                    if (flowCellContent != null)
                    {
                        flowCell.Blocks.Add(flowCellContent);
                    }

                    documentMap.RemapElement(cell, flowCell);

                    flowRow.Cells.Add(flowCell);
                }

                documentMap.RemapElement(row, flowRow);

                flowElement.RowGroups[0].Rows.Add(flowRow);
            }

            return flowElement;
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