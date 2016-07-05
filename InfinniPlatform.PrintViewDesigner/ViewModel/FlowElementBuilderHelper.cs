using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    static class FlowElementBuilderHelper
    {
        public static void ApplyBaseStyles(dynamic elementContent, PrintElement element)
        {

            if (!string.IsNullOrWhiteSpace(element.Name))
            {
                elementContent.Name = element.Name;
            }

            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    elementContent.FontFamily = new FontFamily(element.Font.Family);
                }

                if (element.Font.Size != null)
                {
                    elementContent.FontSize = element.Font.Size.Value;
                }

                if (element.Font.Style != null)
                {
                    elementContent.FontStyle = GetFontStyle(element.Font.Style.Value);
                }

                if (element.Font.Stretch != null)
                {
                    elementContent.FontStretch = GetFontStretch(element.Font.Stretch.Value);
                }

                if (element.Font.Weight != null)
                {
                    elementContent.FontWeight = GetFontWeight(element.Font.Weight.Value);
                }

                if (element.Font.Variant != null)
                {
                    elementContent.Typography.Variants = GetFontVariant(element.Font.Variant.Value);
                }
            }

            var converter = new BrushConverter();

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                try
                {
                    elementContent.Foreground = (Brush)converter.ConvertFromString(element.Foreground);
                }
                catch
                {
                    // ignored
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                try
                {
                    elementContent.Background = (Brush)converter.ConvertFromString(element.Background);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void ApplyBlockStyles(Block elementContent, PrintElementBlock element)
        {
            if (element.Border != null)
            {
                elementContent.BorderThickness = new Thickness(
                    element.Border.Thickness.Left,
                    element.Border.Thickness.Top,
                    element.Border.Thickness.Right,
                    element.Border.Thickness.Bottom
                    );

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {
                    var converter = new BrushConverter();

                    try
                    {
                        elementContent.BorderBrush = (Brush)converter.ConvertFromString(element.Border.Color);
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            elementContent.Margin = new Thickness(
                element.Margin.Left,
                element.Margin.Top,
                element.Margin.Right,
                element.Margin.Bottom
                );

            elementContent.Padding = new Thickness(
                element.Padding.Left,
                element.Padding.Top,
                element.Padding.Right,
                element.Padding.Bottom
                );

            if (element.TextAlignment != null)
            {
                elementContent.TextAlignment = GetTextAlignment(element.TextAlignment.Value);
            }
        }


        public static void ApplyInlineStyles(Inline elementContent, PrintElementInline element)
        {
            if (element.TextDecoration != null)
            {
                elementContent.TextDecorations = GetTextDecoration(element.TextDecoration.Value);
            }

            if (elementContent.Typography.Variants == FontVariants.Subscript)
            {
                elementContent.BaselineAlignment = BaselineAlignment.Subscript;
            }

            if (elementContent.Typography.Variants == FontVariants.Superscript)
            {
                elementContent.BaselineAlignment = BaselineAlignment.Superscript;
            }
        }

        public static void ApplyRowStyles(TableRow elementContent, PrintElementTableRow element)
        {
            if (!string.IsNullOrWhiteSpace(element.Name))
            {
                elementContent.Name = element.Name;
            }

            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    elementContent.FontFamily = new FontFamily(element.Font.Family);
                }

                if (element.Font.Size != null)
                {
                    elementContent.FontSize = element.Font.Size.Value;
                }

                if (element.Font.Style != null)
                {
                    elementContent.FontStyle = GetFontStyle(element.Font.Style.Value);
                }

                if (element.Font.Stretch != null)
                {
                    elementContent.FontStretch = GetFontStretch(element.Font.Stretch.Value);
                }

                if (element.Font.Weight != null)
                {
                    elementContent.FontWeight = GetFontWeight(element.Font.Weight.Value);
                }

                if (element.Font.Variant != null)
                {
                    elementContent.Typography.Variants = GetFontVariant(element.Font.Variant.Value);
                }
            }

            var converter = new BrushConverter();

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                try
                {
                    elementContent.Foreground = (Brush)converter.ConvertFromString(element.Foreground);
                }
                catch
                {
                    // ignored
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                try
                {
                    elementContent.Background = (Brush)converter.ConvertFromString(element.Background);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void ApplyCellStyles(TableCell elementContent, PrintElementTableCell element)
        {
            //Base

            if (!string.IsNullOrWhiteSpace(element.Name))
            {
                elementContent.Name = element.Name;
            }

            if (element.Font != null)
            {
                if (!string.IsNullOrWhiteSpace(element.Font.Family))
                {
                    elementContent.FontFamily = new FontFamily(element.Font.Family);
                }

                if (element.Font.Size != null)
                {
                    elementContent.FontSize = element.Font.Size.Value;
                }

                if (element.Font.Style != null)
                {
                    elementContent.FontStyle = GetFontStyle(element.Font.Style.Value);
                }

                if (element.Font.Stretch != null)
                {
                    elementContent.FontStretch = GetFontStretch(element.Font.Stretch.Value);
                }

                if (element.Font.Weight != null)
                {
                    elementContent.FontWeight = GetFontWeight(element.Font.Weight.Value);
                }

                if (element.Font.Variant != null)
                {
                    elementContent.Typography.Variants = GetFontVariant(element.Font.Variant.Value);
                }
            }

            var converter = new BrushConverter();

            if (!string.IsNullOrWhiteSpace(element.Foreground))
            {
                try
                {
                    elementContent.Foreground = (Brush)converter.ConvertFromString(element.Foreground);
                }
                catch
                {
                    // ignored
                }
            }

            if (!string.IsNullOrWhiteSpace(element.Background))
            {
                try
                {
                    elementContent.Background = (Brush)converter.ConvertFromString(element.Background);
                }
                catch
                {
                    // ignored
                }
            }

            //Block

            if (element.Border != null)
            {
                elementContent.BorderThickness = new Thickness(
                    element.Border.Thickness.Left,
                    element.Border.Thickness.Top,
                    element.Border.Thickness.Right,
                    element.Border.Thickness.Bottom
                    );

                if (!string.IsNullOrWhiteSpace(element.Border.Color))
                {

                    try
                    {
                        elementContent.BorderBrush = (Brush)converter.ConvertFromString(element.Border.Color);
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            elementContent.Padding = new Thickness(
                element.Padding.Left,
                element.Padding.Top,
                element.Padding.Right,
                element.Padding.Bottom
                );

            if (element.TextAlignment != null)
            {
                elementContent.TextAlignment = GetTextAlignment(element.TextAlignment.Value);
            }

            if (element.ColumnSpan != null)
            {
                elementContent.ColumnSpan = element.ColumnSpan.Value;
            }

            if (element.RowSpan != null)
            {
                elementContent.RowSpan = element.RowSpan.Value;
            }
        }

        public static void ApplyDocumentStyles(System.Windows.Documents.FlowDocument document, PrintViewDocument element)
        {
            var pagePadding = new Thickness(
                element.PagePadding.Left,
                element.PagePadding.Top,
                element.PagePadding.Right,
                element.PagePadding.Bottom);


            if (element.PageSize != null)
            {
                if (element.PageSize.Width != null)
                {
                    document.PageWidth = element.PageSize.Width.Value - pagePadding.Left - pagePadding.Right + 10 /* RichTextBox Default Padding */;
                }

                if (element.PageSize.Height != null)
                {
                    document.PageHeight = element.PageSize.Height.Value;
                }
            }
        }

        public static TextAlignment GetTextAlignment(PrintElementTextAlignment align)
        {
            switch (align)
            {
                case PrintElementTextAlignment.Center:
                    return TextAlignment.Center;
                case PrintElementTextAlignment.Justify:
                    return TextAlignment.Justify;
                case PrintElementTextAlignment.Left:
                    return TextAlignment.Left;
                case PrintElementTextAlignment.Right:
                    return TextAlignment.Right;
            }

            return default(TextAlignment);
        }

        public static FontStyle GetFontStyle(PrintElementFontStyle style)
        {
            switch (style)
            {
                case PrintElementFontStyle.Normal:
                    return FontStyles.Normal;
                case PrintElementFontStyle.Italic:
                    return FontStyles.Italic;
                case PrintElementFontStyle.Oblique:
                    return FontStyles.Oblique;
            }

            return default(FontStyle);
        }

        public static FontStretch GetFontStretch(PrintElementFontStretch stretch)
        {
            switch (stretch)
            {
                case PrintElementFontStretch.UltraCondensed:
                    return FontStretches.UltraCondensed;
                case PrintElementFontStretch.ExtraCondensed:
                    return FontStretches.ExtraCondensed;
                case PrintElementFontStretch.SemiCondensed:
                    return FontStretches.SemiCondensed;
                case PrintElementFontStretch.Condensed:
                    return FontStretches.Condensed;
                case PrintElementFontStretch.UltraExpanded:
                    return FontStretches.UltraExpanded;
                case PrintElementFontStretch.ExtraExpanded:
                    return FontStretches.ExtraExpanded;
                case PrintElementFontStretch.SemiExpanded:
                    return FontStretches.SemiExpanded;
                case PrintElementFontStretch.Expanded:
                    return FontStretches.Expanded;
                case PrintElementFontStretch.Normal:
                    return FontStretches.Normal;
            }

            return default(FontStretch);
        }

        public static FontWeight GetFontWeight(PrintElementFontWeight weight)
        {
            switch (weight)
            {
                case PrintElementFontWeight.Normal:
                    return FontWeights.Normal;
                case PrintElementFontWeight.Medium:
                    return FontWeights.Medium;
                case PrintElementFontWeight.Bold:
                    return FontWeights.Bold;
                case PrintElementFontWeight.SemiBold:
                    return FontWeights.SemiBold;
                case PrintElementFontWeight.ExtraBold:
                    return FontWeights.ExtraBold;
                case PrintElementFontWeight.UltraBold:
                    return FontWeights.UltraBold;
                case PrintElementFontWeight.Light:
                    return FontWeights.Light;
                case PrintElementFontWeight.ExtraLight:
                    return FontWeights.ExtraLight;
                case PrintElementFontWeight.UltraLight:
                    return FontWeights.UltraLight;
            }

            return default(FontWeight);
        }

        public static FontVariants GetFontVariant(PrintElementFontVariant variant)
        {
            switch (variant)
            {
                case PrintElementFontVariant.Normal:
                    return FontVariants.Normal;
                case PrintElementFontVariant.Subscript:
                    return FontVariants.Subscript;
                case PrintElementFontVariant.Superscript:
                    return FontVariants.Superscript;
            }

            return default(FontVariants);
        }

        public static TextMarkerStyle GetMarkerStyle(PrintElementListMarkerStyle style)
        {
            switch (style)
            {
                case PrintElementListMarkerStyle.None:
                    return TextMarkerStyle.None;
                case PrintElementListMarkerStyle.Disc:
                    return TextMarkerStyle.Disc;
                case PrintElementListMarkerStyle.Circle:
                    return TextMarkerStyle.Circle;
                case PrintElementListMarkerStyle.Square:
                    return TextMarkerStyle.Square;
                case PrintElementListMarkerStyle.Box:
                    return TextMarkerStyle.Box;
                case PrintElementListMarkerStyle.LowerRoman:
                    return TextMarkerStyle.LowerRoman;
                case PrintElementListMarkerStyle.UpperRoman:
                    return TextMarkerStyle.UpperRoman;
                case PrintElementListMarkerStyle.LowerLatin:
                    return TextMarkerStyle.LowerLatin;
                case PrintElementListMarkerStyle.UpperLatin:
                    return TextMarkerStyle.UpperLatin;
                case PrintElementListMarkerStyle.Decimal:
                    return TextMarkerStyle.Decimal;
            }

            return default(TextMarkerStyle);
        }

        public static TextDecorationCollection GetTextDecoration(PrintElementTextDecoration decoration)
        {
            switch (decoration)
            {
                case PrintElementTextDecoration.OverLine:
                    return TextDecorations.OverLine;
                case PrintElementTextDecoration.Strikethrough:
                    return TextDecorations.Strikethrough;
                case PrintElementTextDecoration.Underline:
                    return TextDecorations.Underline;
            }

            return default(TextDecorationCollection);
        }


        public static void RemapElement(this PrintElementMetadataMap elementMetadataMap, dynamic element, object flowElement)
        {
            if (elementMetadataMap != null && flowElement != null)
            {
                var elementMetadata = elementMetadataMap.GetMetadata(element);
                elementMetadataMap.Map(flowElement, elementMetadata);
            }
        }
    }
}
