using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTreeBuilders
{
    internal sealed class PrintElementNodeBuilder
    {
        private readonly Dictionary<string, PrintElementNodeFactoryInfo> _factories
            = new Dictionary<string, PrintElementNodeFactoryInfo>(StringComparer.OrdinalIgnoreCase);

        public void Register(string elementType, string elementTypeName, IPrintElementNodeFactory elementFactory)
        {
            if (string.IsNullOrEmpty(elementType))
            {
                throw new ArgumentNullException(nameof(elementType));
            }

            if (string.IsNullOrEmpty(elementTypeName))
            {
                throw new ArgumentNullException(nameof(elementTypeName));
            }

            if (elementFactory == null)
            {
                throw new ArgumentNullException(nameof(elementFactory));
            }

            _factories.Add(elementType, new PrintElementNodeFactoryInfo(elementTypeName, elementFactory));
        }

        public void BuildElement(ICollection<PrintElementNode> elements, PrintElementNode elementParent,
            dynamic elementMetadata)
        {
            if (elementMetadata is IDynamicMetaObjectProvider)
            {
                foreach (var property in elementMetadata)
                {
                    BuildElement(elements, elementParent, property.Value, property.Key);
                }
            }
        }

        public void BuildElement(ICollection<PrintElementNode> elements, PrintElementNode elementParent,
            dynamic elementMetadata, string elementType)
        {
            if (!ReferenceEquals(elementMetadata, null))
            {
                PrintElementNodeFactoryInfo info;

                if (_factories.TryGetValue(elementType, out info))
                {
                    var element = new PrintElementNode(elementParent, elementType, elementMetadata);
                    element.BuildVisibility = BuildVisibility(element);
                    element.BuildDisplayName = BuildDisplayName(element, info.ElementTypeName);
                    element.Refresh();

                    elements.Add(element);

                    if (elementParent != null)
                    {
                        element.Index = elementParent.Nodes.Count;
                        elementParent.Nodes.Add(element);
                    }

                    info.ElementFactory.Create(this, elements, element);
                }
            }
        }

        public void BuildElements(ICollection<PrintElementNode> elements, PrintElementNode elementParent, IEnumerable elementMetadata)
        {
            if (!ReferenceEquals(elementMetadata, null))
            {
                foreach (var itemMetadata in elementMetadata)
                {
                    BuildElement(elements, elementParent, itemMetadata);
                }
            }
        }

        public void BuildElements(ICollection<PrintElementNode> elements, PrintElementNode elementParent, IEnumerable elementMetadata, string elementType)
        {
            if (!ReferenceEquals(elementMetadata, null))
            {
                foreach (var itemMetadata in elementMetadata)
                {
                    BuildElement(elements, elementParent, itemMetadata, elementType);
                }
            }
        }

        private static Func<string> BuildVisibility(PrintElementNode elementNode)
        {
            return () => elementNode.ElementMetadata.Visibility as string;
        }

        private static Func<string> BuildDisplayName(PrintElementNode elementNode, string elementTypeName)
        {
            return () =>
            {
                var result = elementTypeName;

                var metadata = elementNode.ElementMetadata;
                var parentMetadata = (elementNode.Parent != null) ? elementNode.Parent.ElementMetadata : null;

                if (metadata != parentMetadata)
                {
                    var name = metadata.Name as string;

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        result = name;
                    }

                    var source = metadata.Source as string;

                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        result = string.Format("{0}: {1}", result, source);
                    }
                }

                return result;
            };
        }

        private class PrintElementNodeFactoryInfo
        {
            public readonly IPrintElementNodeFactory ElementFactory;
            public readonly string ElementTypeName;

            public PrintElementNodeFactoryInfo(string elementTypeName, IPrintElementNodeFactory elementFactory)
            {
                ElementTypeName = elementTypeName;
                ElementFactory = elementFactory;
            }
        }
    }
}