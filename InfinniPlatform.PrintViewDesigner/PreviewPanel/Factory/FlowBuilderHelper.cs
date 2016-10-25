using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Inline;

using BlockElement = System.Windows.Documents.Block;
using InlineElement = System.Windows.Documents.Inline;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory
{
    internal static class FlowBuilderHelper
    {
        public const double FontSizeMin = 0.1;

        private static readonly FlowEnumCache EnumCache;


        static FlowBuilderHelper()
        {
            EnumCache = new FlowEnumCache();

            EnumCache.AddValue(PrintFontStyle.Normal, FontStyles.Normal)
                     .AddValue(PrintFontStyle.Italic, FontStyles.Italic)
                     .AddValue(PrintFontStyle.Oblique, FontStyles.Oblique);

            EnumCache.AddValue(PrintFontStretch.Normal, FontStretches.Normal)
                     .AddValue(PrintFontStretch.UltraCondensed, FontStretches.UltraCondensed)
                     .AddValue(PrintFontStretch.ExtraCondensed, FontStretches.ExtraCondensed)
                     .AddValue(PrintFontStretch.Condensed, FontStretches.Condensed)
                     .AddValue(PrintFontStretch.SemiCondensed, FontStretches.SemiCondensed)
                     .AddValue(PrintFontStretch.SemiExpanded, FontStretches.SemiExpanded)
                     .AddValue(PrintFontStretch.Expanded, FontStretches.Expanded)
                     .AddValue(PrintFontStretch.ExtraExpanded, FontStretches.ExtraExpanded)
                     .AddValue(PrintFontStretch.UltraExpanded, FontStretches.UltraExpanded);

            EnumCache.AddValue(PrintFontWeight.Normal, FontWeights.Normal)
                     .AddValue(PrintFontWeight.UltraLight, FontWeights.UltraLight)
                     .AddValue(PrintFontWeight.ExtraLight, FontWeights.ExtraLight)
                     .AddValue(PrintFontWeight.Light, FontWeights.Light)
                     .AddValue(PrintFontWeight.Medium, FontWeights.Medium)
                     .AddValue(PrintFontWeight.SemiBold, FontWeights.SemiBold)
                     .AddValue(PrintFontWeight.Bold, FontWeights.Bold)
                     .AddValue(PrintFontWeight.ExtraBold, FontWeights.ExtraBold)
                     .AddValue(PrintFontWeight.UltraBold, FontWeights.UltraBold);

            EnumCache.AddValue(PrintFontVariant.Normal, FontVariants.Normal)
                     .AddValue(PrintFontVariant.Subscript, FontVariants.Subscript)
                     .AddValue(PrintFontVariant.Superscript, FontVariants.Superscript);

            EnumCache.AddValue(PrintTextAlignment.Left, TextAlignment.Left)
                     .AddValue(PrintTextAlignment.Center, TextAlignment.Center)
                     .AddValue(PrintTextAlignment.Right, TextAlignment.Right)
                     .AddValue(PrintTextAlignment.Justify, TextAlignment.Justify);

            EnumCache.AddValue(PrintListMarkerStyle.None, TextMarkerStyle.None)
                     .AddValue(PrintListMarkerStyle.Disc, TextMarkerStyle.Disc)
                     .AddValue(PrintListMarkerStyle.Circle, TextMarkerStyle.Circle)
                     .AddValue(PrintListMarkerStyle.Square, TextMarkerStyle.Square)
                     .AddValue(PrintListMarkerStyle.Box, TextMarkerStyle.Box)
                     .AddValue(PrintListMarkerStyle.LowerRoman, TextMarkerStyle.LowerRoman)
                     .AddValue(PrintListMarkerStyle.UpperRoman, TextMarkerStyle.UpperRoman)
                     .AddValue(PrintListMarkerStyle.LowerLatin, TextMarkerStyle.LowerLatin)
                     .AddValue(PrintListMarkerStyle.UpperLatin, TextMarkerStyle.UpperLatin)
                     .AddValue(PrintListMarkerStyle.Decimal, TextMarkerStyle.Decimal);

            EnumCache.AddValue(PrintTextDecoration.Normal, null)
                     .AddValue(PrintTextDecoration.OverLine, TextDecorations.OverLine)
                     .AddValue(PrintTextDecoration.Strikethrough, TextDecorations.Strikethrough)
                     .AddValue(PrintTextDecoration.Underline, TextDecorations.Underline);

            EnumCache.AddValue(PrintImageStretch.None, Stretch.Fill)
                     .AddValue(PrintImageStretch.Fill, Stretch.Fill)
                     .AddValue(PrintImageStretch.Uniform, Stretch.Uniform);
        }


        public static void ApplyElementStyles(TextElement flowElement, PrintElement element)
        {
            SetFont(flowElement, element.Font);
            SetForeground(flowElement, element.Foreground);
            SetBackground(flowElement, element.Background);
        }

        public static void ApplyBlockStyles(BlockElement flowElement, PrintBlock element)
        {
            SetBorder(flowElement, element.Border);
            SetMargin(flowElement, element.Margin);
            SetPadding(flowElement, element.Padding);
            SetTextAlignment(flowElement, element.TextAlignment);
        }

        public static void ApplyInlineStyles(InlineElement flowElement, PrintInline element)
        {
            SetTextDecoration(flowElement, element.TextDecoration);
        }


        public static void SetFont(dynamic flowElement, PrintFont font)
        {
            if (font == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(font.Family))
            {
                flowElement.FontFamily = new FontFamily(font.Family);
            }

            if (font.Size != null)
            {
                flowElement.FontSize = GetSizeInPixels(font.Size, font.SizeUnit);
            }

            if (font.Style != null)
            {
                flowElement.FontStyle = GetEnumValue<FontStyle>(font.Style);
            }

            if (font.Stretch != null)
            {
                flowElement.FontStretch = GetEnumValue<FontStretch>(font.Stretch);
            }

            if (font.Weight != null)
            {
                flowElement.FontWeight = GetEnumValue<FontWeight>(font.Weight);
            }

            if (font.Variant != null)
            {
                var fontVariant = GetEnumValue<FontVariants>(font.Variant);

                flowElement.Typography.Variants = fontVariant;

                var inlineElement = flowElement as InlineElement;

                if (inlineElement != null)
                {
                    if (fontVariant == FontVariants.Subscript)
                    {
                        inlineElement.BaselineAlignment = BaselineAlignment.Subscript;
                    }

                    if (fontVariant == FontVariants.Superscript)
                    {
                        inlineElement.BaselineAlignment = BaselineAlignment.Superscript;
                    }
                }
            }
        }

        public static void SetForeground(dynamic flowElement, string foreground)
        {
            if (!string.IsNullOrWhiteSpace(foreground))
            {
                flowElement.Foreground = TryCreateBrush(foreground);
            }
        }

        public static void SetBackground(dynamic flowElement, string background)
        {
            if (!string.IsNullOrWhiteSpace(background))
            {
                flowElement.Background = TryCreateBrush(background);
            }
        }

        public static void SetBorder(dynamic flowElement, PrintBorder border)
        {
            if (border == null)
            {
                return;
            }

            if (border.Thickness != null)
            {
                flowElement.BorderThickness = TryCreateThickness(border.Thickness);
            }

            if (!string.IsNullOrWhiteSpace(border.Color))
            {
                flowElement.BorderBrush = TryCreateBrush(border.Color);
            }
        }

        public static void SetMargin(BlockElement flowElement, PrintThickness margin)
        {
            if (margin != null)
            {
                flowElement.Margin = TryCreateThickness(margin);
            }
        }

        public static void SetPadding(dynamic flowElement, PrintThickness padding)
        {
            if (padding != null)
            {
                flowElement.Padding = TryCreateThickness(padding);
            }
        }

        public static void SetTextAlignment(dynamic flowElement, PrintTextAlignment? textAlignment)
        {
            if (textAlignment != null)
            {
                flowElement.TextAlignment = GetEnumValue<TextAlignment>(textAlignment);
            }
        }

        private static void SetTextDecoration(InlineElement flowElement, PrintTextDecoration? textDecoration)
        {
            if (textDecoration != null)
            {
                flowElement.TextDecorations = GetEnumValue<TextDecorationCollection>(textDecoration);
            }
        }


        private static Brush TryCreateBrush(string color)
        {
            if (!string.IsNullOrWhiteSpace(color))
            {
                var converter = new BrushConverter();

                try
                {
                    return (Brush)converter.ConvertFromString(color);
                }
                catch
                {
                }
            }

            return null;
        }

        private static Thickness TryCreateThickness(PrintThickness thickness)
        {
            if (thickness != null)
            {
                var left = GetSizeInPixels(thickness.Left, thickness.SizeUnit);
                var top = GetSizeInPixels(thickness.Top, thickness.SizeUnit);
                var right = GetSizeInPixels(thickness.Right, thickness.SizeUnit);
                var bottom = GetSizeInPixels(thickness.Bottom, thickness.SizeUnit);

                return new Thickness(left, top, right, bottom);
            }

            return default(Thickness);
        }


        public static TValue GetEnumValue<TValue>(object key)
        {
            return EnumCache.GetValue<TValue>(key);
        }

        public static double GetSizeInPixels(double? fromSize, PrintSizeUnit? fromSizeUnit)
        {
            return (fromSize != null) ? PrintSizeUnitConverter.ToSpecifiedSize(fromSize.Value, fromSizeUnit ?? PrintSizeUnit.Pt, PrintSizeUnit.Px) : 0;
        }


        public static void RemapElement(this PrintDocumentMap documentMap, object element, object flowElement)
        {
            if ((documentMap != null) && (flowElement != null))
            {
                var elementMetadata = documentMap.GetTemplate(element);

                documentMap.Map(flowElement, elementMetadata);
            }
        }
    }
}