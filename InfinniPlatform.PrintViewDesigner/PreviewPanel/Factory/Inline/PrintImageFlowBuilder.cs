using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Inline
{
    internal sealed class PrintImageFlowBuilder : FlowBuilderBase<PrintImage>
    {
        protected override object Build(FlowBuilderContext context, PrintImage element, PrintDocumentMap documentMap)
        {
            var flowElement = new InlineUIContainer { BaselineAlignment = BaselineAlignment.Baseline };

            FlowBuilderHelper.ApplyElementStyles(flowElement, element);
            FlowBuilderHelper.ApplyInlineStyles(flowElement, element);

            var imageSource = CreateImageSource(element.Data);

            var imageControl = new Image();

            if (imageSource != null)
            {
                var imageWidth = imageSource.Width;
                var imageHeight = imageSource.Height;

                if (element.Size != null)
                {
                    imageWidth = FlowBuilderHelper.GetSizeInPixels(element.Size.Width, element.Size.SizeUnit);
                    imageHeight = FlowBuilderHelper.GetSizeInPixels(element.Size.Height, element.Size.SizeUnit);
                }

                var imageStretch = Stretch.Fill;

                if (element.Stretch != null)
                {
                    imageStretch = FlowBuilderHelper.GetEnumValue<Stretch>(element.Stretch);
                }

                imageControl.BeginInit();
                imageControl.Width = imageWidth;
                imageControl.Height = imageHeight;
                imageControl.Source = imageSource;
                imageControl.Stretch = imageStretch;
                imageControl.EndInit();
            }

            flowElement.Child = imageControl;

            return flowElement;
        }


        private static ImageSource CreateImageSource(byte[] imageData)
        {
            if (imageData != null)
            {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = new MemoryStream(imageData);
                imageSource.EndInit();

                return imageSource;
            }

            return null;
        }
    }
}