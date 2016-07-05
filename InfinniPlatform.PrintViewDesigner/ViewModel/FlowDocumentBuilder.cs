using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Model.Views;
using InfinniPlatform.PrintViewDesigner.ViewModel.Blocks;
using InfinniPlatform.PrintViewDesigner.ViewModel.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    static class FlowDocumentBuilder
    {
        static FlowDocumentBuilder()
        {
            Context = new FlowElementBuilderContext();

            //Blocks
            Context.Register<PrintElementLine, FlowElementLineBuilder>();
            Context.Register<PrintElementList, FlowElementListBuilder>();
            Context.Register<PrintElementPageBreak, FlowElementPageBreakBuilder>();
            Context.Register<PrintElementParagraph, FlowElementParagraphBuilder>();
            Context.Register<PrintElementSection, FlowElementSectionBuilder>();
            Context.Register<PrintElementTable, FlowElementTableBuilder>();

            //Inlines
            Context.Register<PrintElementBold, FlowElementBoldBuilder>();
            Context.Register<PrintElementHyperlink, FlowElementHyperlinkBuilder>();
            Context.Register<PrintElementImage, FlowElementImageBuilder>();
            Context.Register<PrintElementItalic, FlowElementItalicBuilder>();
            Context.Register<PrintElementLineBreak, FlowElementLineBreakBuilder>();
            Context.Register<PrintElementRun, FlowElementRunBuilder>();
            Context.Register<PrintElementSpan, FlowElementSpanBuilder>();
            Context.Register<PrintElementUnderline, FlowElementUnderlineBuilder>();

            Context.Register<PrintViewDocument, FlowElementFlowDocumentBuilder>();
        }


        static readonly FlowElementBuilderContext Context;


        public static System.Windows.Documents.FlowDocument Build(PrintViewDocument innerDocument, PrintElementMetadataMap elementMetadataMap)
        {
            return (System.Windows.Documents.FlowDocument)Context.Build(innerDocument, elementMetadataMap);
        }
    }
}
