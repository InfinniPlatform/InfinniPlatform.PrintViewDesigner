using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory
{
    /// <summary>
    /// Типы узлов <see cref="PrintElementNode" />.
    /// </summary>
    public enum PrintElementNodeType
    {
        /// <summary>
        /// Узел для <see cref="PrintLine" />.
        /// </summary>
        PrintLineNode,

        /// <summary>
        /// Узел для <see cref="PrintList" />.
        /// </summary>
        PrintListNode,

        /// <summary>
        /// Узел для <see cref="PrintList.ItemTemplate" />.
        /// </summary>
        PrintListItemTemplateNode,

        /// <summary>
        /// Узел для <see cref="PrintList.Items" />.
        /// </summary>
        PrintListItemsNode,

        /// <summary>
        /// Узел для <see cref="PrintPageBreak" />.
        /// </summary>
        PrintPageBreakNode,

        /// <summary>
        /// Узел для <see cref="PrintParagraph" />.
        /// </summary>
        PrintParagraphNode,

        /// <summary>
        /// Узел для <see cref="PrintSection" />.
        /// </summary>
        PrintSectionNode,

        /// <summary>
        /// Узел для <see cref="PrintTable" />.
        /// </summary>
        PrintTableNode,

        /// <summary>
        /// Узел для <see cref="PrintTable.Columns" />.
        /// </summary>
        PrintTableColumnsNode,

        /// <summary>
        /// Узел для <see cref="PrintTableColumn" />.
        /// </summary>
        PrintTableColumnNode,

        /// <summary>
        /// Узел для <see cref="PrintTableColumn.Header" />.
        /// </summary>
        PrintTableColumnHeaderNode,

        /// <summary>
        /// Узел для <see cref="PrintTableColumn.CellTemplate" />.
        /// </summary>
        PrintTableColumnCellTemplateNode,

        /// <summary>
        /// Узел для <see cref="PrintTable.Rows" />.
        /// </summary>
        PrintTableRowsNode,

        /// <summary>
        /// Узел для <see cref="PrintTableRow" />.
        /// </summary>
        PrintTableRowNode,

        /// <summary>
        /// Узел для <see cref="PrintTableRow.Cells" />.
        /// </summary>
        PrintTableRowCellsNode,

        /// <summary>
        /// Узел для <see cref="PrintTableCell" />.
        /// </summary>
        PrintTableCellNode,

        /// <summary>
        /// Узел для <see cref="PrintBarcodeEan13" />.
        /// </summary>
        PrintBarcodeEan13Node,

        /// <summary>
        /// Узел для <see cref="PrintBarcodeQr" />.
        /// </summary>
        PrintBarcodeQrNode,

        /// <summary>
        /// Узел для <see cref="PrintBold" />.
        /// </summary>
        PrintBoldNode,

        /// <summary>
        /// Узел для <see cref="PrintHyperlink" />.
        /// </summary>
        PrintHyperlinkNode,

        /// <summary>
        /// Узел для <see cref="PrintImage" />.
        /// </summary>
        PrintImageNode,

        /// <summary>
        /// Узел для <see cref="PrintItalic" />.
        /// </summary>
        PrintItalicNode,

        /// <summary>
        /// Узел для <see cref="PrintLineBreak" />.
        /// </summary>
        PrintLineBreakNode,

        /// <summary>
        /// Узел для <see cref="PrintRun" />.
        /// </summary>
        PrintRunNode,

        /// <summary>
        /// Узел для <see cref="PrintSpan" />.
        /// </summary>
        PrintSpanNode,

        /// <summary>
        /// Узел для <see cref="PrintUnderline" />.
        /// </summary>
        PrintUnderlineNode,

        /// <summary>
        /// Узел для <see cref="PrintDocument" />.
        /// </summary>
        PrintDocumentNode,

        /// <summary>
        /// Узел для <see cref="PrintDocument.Styles" />.
        /// </summary>
        PrintDocumentStylesNode,

        /// <summary>
        /// Узел для <see cref="PrintDocument.Blocks" />.
        /// </summary>
        PrintDocumentBlocksNode,

        /// <summary>
        /// Узел для <see cref="PrintStyle" />.
        /// </summary>
        PrintStyleNode
    }
}