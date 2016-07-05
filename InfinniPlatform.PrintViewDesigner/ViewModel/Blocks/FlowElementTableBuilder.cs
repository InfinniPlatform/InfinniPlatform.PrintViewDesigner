using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    internal sealed class FlowElementTableBuilder : IFlowElementBuilderBase<PrintElementTable>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementTable element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new Table
            {
                CellSpacing = 0,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 0, 0),
                RowGroups = { new TableRowGroup() }
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            foreach (var column in element.Columns)
            {
                var columnContent = new TableColumn { Width = new GridLength(column.Size ?? 1) };

                elementMetadataMap.RemapElement(column, columnContent);

                elementContent.Columns.Add(columnContent);
            }

            foreach (var row in element.Rows)
            {
                var rowContent = new TableRow();

                FlowElementBuilderHelper.ApplyRowStyles(rowContent, row);

                var colSpansCount = 0;

                foreach (var cell in row.Cells)
                {
                    var cellContent = new TableCell
                    {
                        ColumnSpan = 1,
                        RowSpan = 1,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1)
                    };

                    //Check ColSpan and RowSpan

                    colSpansCount += cell.ColumnSpan ?? 1;

                    if (colSpansCount > element.Columns.Count)
                    {
                        cell.ColumnSpan = element.Columns.Count - (colSpansCount - cell.ColumnSpan);
                    }

                    FlowElementBuilderHelper.ApplyCellStyles(cellContent, cell);

                    var blockContent = context.Build<Block>(cell.Block, elementMetadataMap);

                    if (blockContent != null)
                    {
                        cellContent.Blocks.Add(blockContent);
                    }

                    elementMetadataMap.RemapElement(cell, cellContent);

                    rowContent.Cells.Add(cellContent);
                }

                elementMetadataMap.RemapElement(row, rowContent);

                elementContent.RowGroups[0].Rows.Add(rowContent);
            }

            return elementContent;
        }
    }
}
