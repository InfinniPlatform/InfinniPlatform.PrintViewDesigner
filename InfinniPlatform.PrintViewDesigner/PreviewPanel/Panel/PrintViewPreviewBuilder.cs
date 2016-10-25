using System.Windows.Documents;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory;
using InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block;
using InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Panel
{
    internal class PrintViewPreviewBuilder
    {
        private static readonly FlowBuilderContext Context;


        static PrintViewPreviewBuilder()
        {
            Context = new FlowBuilderContext();

            Context.Register(new PrintDocumentFlowBuilder());

            // Block
            Context.Register(new PrintLineFlowBuilder());
            Context.Register(new PrintListFlowBuilder());
            Context.Register(new PrintPageBreakFlowBuilder());
            Context.Register(new PrintParagraphFlowBuilder());
            Context.Register(new PrintSectionFlowBuilder());
            Context.Register(new PrintTableFlowBuilder());

            // Inline
            Context.Register(new PrintBoldFlowBuilder());
            Context.Register(new PrintHyperlinkFlowBuilder());
            Context.Register(new PrintImageFlowBuilder());
            Context.Register(new PrintItalicFlowBuilder());
            Context.Register(new PrintLineBreakFlowBuilder());
            Context.Register(new PrintRunFlowBuilder());
            Context.Register(new PrintSpanFlowBuilder());
            Context.Register(new PrintUnderlineFlowBuilder());
        }


        public FlowDocument CreatePreview(PrintDocument document, PrintDocumentMap documentMap)
        {
            return Context.Build<FlowDocument>(document, documentMap);
        }
    }
}