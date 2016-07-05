using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    internal sealed class FlowElementImageBuilder : IFlowElementBuilderBase<PrintElementImage>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementImage element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new InlineUIContainer {BaselineAlignment = BaselineAlignment.Baseline};

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            var imageSource = new BitmapImage();

            imageSource.BeginInit();

            var elementSourceStream = new MemoryStream();

            element.Source.Save(elementSourceStream, ImageFormat.Png);
            imageSource.StreamSource = elementSourceStream;
            imageSource.EndInit();

            var imageControl = new Image();

            imageControl.BeginInit();
            imageControl.Width = (element.Size != null
                                      ? element.Size.Width
                                      : null) ?? imageSource.Width;
            imageControl.Height = (element.Size != null
                                       ? element.Size.Height
                                       : null) ?? imageSource.Height;
            imageControl.Source = imageSource;
            imageControl.Stretch = Stretch.Fill;
            imageControl.EndInit();

            elementContent.Child = imageControl;

            return elementContent;
        }
    }
}