using System;
using System.Collections.Generic;
using System.Windows.Documents;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    internal sealed class FlowElementBuilderContext
    {
        private readonly Dictionary<Type, IFlowElementBuilder> _builders
            = new Dictionary<Type, IFlowElementBuilder>();

        public FlowElementBuilderContext Register<TElement, TBuilder>()
            where TElement : PrintElement
            where TBuilder : IFlowElementBuilderBase<TElement>, new()
        {
            _builders[typeof(TElement)] = new TBuilder();

            return this;
        }

        public object Build(PrintElement element, PrintElementMetadataMap elementMetadataMap)
        {
            if (element != null)
            {
                IFlowElementBuilder builder;

                if (_builders.TryGetValue(element.GetType(), out builder))
                {
                    var flowElement = builder.Build(this, element, elementMetadataMap);

                    elementMetadataMap.RemapElement(element, flowElement);

                    return flowElement;
                }
            }

            return null;
        }

        public TResult Build<TResult>(PrintElement element, PrintElementMetadataMap elementMetadataMap) where TResult : TextElement
        {
            return (TResult)Build(element, elementMetadataMap);
        }
    }
}