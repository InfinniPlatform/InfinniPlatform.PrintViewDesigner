using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    abstract class IFlowElementBuilderBase<TElement> : IFlowElementBuilder where TElement: PrintElement
    {
        public object Build(FlowElementBuilderContext context, PrintElement element, PrintElementMetadataMap elementMetadataMap)
        {
            return Build(context, (TElement)element, elementMetadataMap);
        }

        public abstract object Build(FlowElementBuilderContext context, TElement element, PrintElementMetadataMap elementMetadataMap);
    }
}