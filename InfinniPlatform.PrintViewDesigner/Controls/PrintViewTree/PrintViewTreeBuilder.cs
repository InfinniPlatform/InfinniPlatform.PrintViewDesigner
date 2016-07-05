using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders;
using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Blocks;
using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Inlines;
using InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders.Views;
using InfinniPlatform.PrintViewDesigner.Properties;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTree
{
    internal sealed class PrintViewTreeBuilder
    {
        private static readonly PrintElementNodeBuilder ElementBuilder;

        static PrintViewTreeBuilder()
        {
            ElementBuilder = new PrintElementNodeBuilder();

            // Blocks
            ElementBuilder.Register("Section", Resources.Section, new PrintElementSectionNodeFactory());
            ElementBuilder.Register("Paragraph", Resources.Paragraph, new PrintElementParagraphNodeFactory());
            ElementBuilder.Register("List", Resources.List, new PrintElementListNodeFactory());
            ElementBuilder.Register("ListItemTemplate", Resources.ListItemTemplate, new PrintElementListItemTemplateNodeFactory());
            ElementBuilder.Register("ListItems", Resources.ListItems, new PrintElementListItemsNodeFactory());
            ElementBuilder.Register("Table", Resources.Table, new PrintElementTableNodeFactory());
            ElementBuilder.Register("TableColumns", Resources.TableColumns, new PrintElementTableColumnsNodeFactory());
            ElementBuilder.Register("TableColumn", Resources.TableColumn, new PrintElementTableColumnNodeFactory());
            ElementBuilder.Register("TableColumnHeader", Resources.TableColumnHeader, new PrintElementTableColumnHeaderNodeFactory());
            ElementBuilder.Register("TableColumnCellTemplate", Resources.TableColumnCellTemplate, new PrintElementTableColumnCellTemplateNodeFactory());
            ElementBuilder.Register("TableRows", Resources.TableRows, new PrintElementTableRowsNodeFactory());
            ElementBuilder.Register("TableRow", Resources.TableRow, new PrintElementTableRowNodeFactory());
            ElementBuilder.Register("TableCell", Resources.TableCell, new PrintElementTableCellNodeFactory());
            ElementBuilder.Register("Line", Resources.Line, new PrintElementLineNodeFactory());
            ElementBuilder.Register("PageBreak", Resources.PageBreak, new PrintElementPageBreakNodeFactory());

            // Inlines
            ElementBuilder.Register("Span", Resources.Span, new PrintElementSpanNodeFactory());
            ElementBuilder.Register("Bold", Resources.Bold, new PrintElementBoldNodeFactory());
            ElementBuilder.Register("Italic", Resources.Italic, new PrintElementItalicNodeFactory());
            ElementBuilder.Register("Underline", Resources.Underline, new PrintElementUnderlineNodeFactory());
            ElementBuilder.Register("Hyperlink", Resources.Hyperlink, new PrintElementHyperlinkNodeFactory());
            ElementBuilder.Register("LineBreak", Resources.LineBreak, new PrintElementLineBreakNodeFactory());
            ElementBuilder.Register("Run", Resources.Run, new PrintElementRunNodeFactory());
            ElementBuilder.Register("Image", Resources.Image, new PrintElementImageNodeFactory());
            ElementBuilder.Register("BarcodeEan13", Resources.BarcodeEan13, new PrintElementBarcodeEan13NodeFactory());
            ElementBuilder.Register("BarcodeQr", Resources.BarcodeQr, new PrintElementBarcodeQrNodeFactory());

            // Views
            ElementBuilder.Register("PrintView", Resources.PrintView, new PrintViewNodeFactory());
            ElementBuilder.Register("PrintViewStyles", Resources.PrintViewStyles, new PrintViewStylesNodeFactory());
            ElementBuilder.Register("PrintViewStyle", Resources.PrintViewStyle, new PrintViewStyleNodeFactory());
            ElementBuilder.Register("PrintViewBlocks", Resources.PrintViewBlocks, new PrintViewBlocksNodeFactory());
        }

        public void Build(ICollection<PrintElementNode> elements, object printView)
        {
            ElementBuilder.BuildElement(elements, null, printView, "PrintView");
        }
    }
}