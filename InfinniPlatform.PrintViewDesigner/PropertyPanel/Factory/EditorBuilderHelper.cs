using System;
using System.Linq.Expressions;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Format;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Common;
using InfinniPlatform.PrintViewDesigner.PropertyPanel.Editors.Specific;

namespace InfinniPlatform.PrintViewDesigner.PropertyPanel.Factory
{
    internal static class EditorBuilderHelper
    {
        private static readonly EditorEnumCache EnumCache;


        static EditorBuilderHelper()
        {
            EnumCache = new EditorEnumCache();

            EnumCache.AddValue(PrintVisibility.Never, Properties.Resources.VisibilityNever)
                     .AddValue(PrintVisibility.Always, Properties.Resources.VisibilityAlways)
                     .AddValue(PrintVisibility.Source, Properties.Resources.VisibilitySource);

            EnumCache.AddValue(PrintFontStyle.Normal, Properties.Resources.FontStyleNormal)
                     .AddValue(PrintFontStyle.Italic, Properties.Resources.FontStyleItalic)
                     .AddValue(PrintFontStyle.Oblique, Properties.Resources.FontStyleOblique);

            EnumCache.AddValue(PrintFontStretch.Normal, Properties.Resources.FontStretchNormal)
                     .AddValue(PrintFontStretch.UltraCondensed, Properties.Resources.FontStretchUltraCondensed)
                     .AddValue(PrintFontStretch.ExtraCondensed, Properties.Resources.FontStretchExtraCondensed)
                     .AddValue(PrintFontStretch.Condensed, Properties.Resources.FontStretchCondensed)
                     .AddValue(PrintFontStretch.SemiCondensed, Properties.Resources.FontStretchSemiCondensed)
                     .AddValue(PrintFontStretch.SemiExpanded, Properties.Resources.FontStretchSemiExpanded)
                     .AddValue(PrintFontStretch.Expanded, Properties.Resources.FontStretchExpanded)
                     .AddValue(PrintFontStretch.ExtraExpanded, Properties.Resources.FontStretchExtraExpanded)
                     .AddValue(PrintFontStretch.UltraExpanded, Properties.Resources.FontStretchUltraExpanded);

            EnumCache.AddValue(PrintFontWeight.Normal, Properties.Resources.FontWeightNormal)
                     .AddValue(PrintFontWeight.UltraLight, Properties.Resources.FontWeightUltraLight)
                     .AddValue(PrintFontWeight.ExtraLight, Properties.Resources.FontWeightExtraLight)
                     .AddValue(PrintFontWeight.Light, Properties.Resources.FontWeightLight)
                     .AddValue(PrintFontWeight.Medium, Properties.Resources.FontWeightMedium)
                     .AddValue(PrintFontWeight.SemiBold, Properties.Resources.FontWeightSemiBold)
                     .AddValue(PrintFontWeight.Bold, Properties.Resources.FontWeightBold)
                     .AddValue(PrintFontWeight.ExtraBold, Properties.Resources.FontWeightExtraBold)
                     .AddValue(PrintFontWeight.UltraBold, Properties.Resources.FontWeightUltraBold);

            EnumCache.AddValue(PrintFontVariant.Normal, Properties.Resources.FontVariantNormal)
                     .AddValue(PrintFontVariant.Subscript, Properties.Resources.FontVariantSubscript)
                     .AddValue(PrintFontVariant.Superscript, Properties.Resources.FontVariantSuperscript);

            EnumCache.AddValue(PrintTextAlignment.Left, Properties.Resources.TextAlignmentLeft)
                     .AddValue(PrintTextAlignment.Center, Properties.Resources.TextAlignmentCenter)
                     .AddValue(PrintTextAlignment.Right, Properties.Resources.TextAlignmentRight)
                     .AddValue(PrintTextAlignment.Justify, Properties.Resources.TextAlignmentJustify);

            EnumCache.AddValue(PrintTextCase.Normal, Properties.Resources.TextCaseNormal)
                     .AddValue(PrintTextCase.SentenceCase, Properties.Resources.TextCaseSentenceCase)
                     .AddValue(PrintTextCase.Lowercase, Properties.Resources.TextCaseLowercase)
                     .AddValue(PrintTextCase.Uppercase, Properties.Resources.TextCaseUppercase)
                     .AddValue(PrintTextCase.ToggleCase, Properties.Resources.TextCaseToggleCase);

            EnumCache.AddValue(PrintListMarkerStyle.None, Properties.Resources.ListMarkerStyleNone)
                     .AddValue(PrintListMarkerStyle.Disc, Properties.Resources.ListMarkerStyleDisc)
                     .AddValue(PrintListMarkerStyle.Circle, Properties.Resources.ListMarkerStyleCircle)
                     .AddValue(PrintListMarkerStyle.Square, Properties.Resources.ListMarkerStyleSquare)
                     .AddValue(PrintListMarkerStyle.Box, Properties.Resources.ListMarkerStyleBox)
                     .AddValue(PrintListMarkerStyle.LowerRoman, Properties.Resources.ListMarkerStyleLowerRoman)
                     .AddValue(PrintListMarkerStyle.UpperRoman, Properties.Resources.ListMarkerStyleUpperRoman)
                     .AddValue(PrintListMarkerStyle.LowerLatin, Properties.Resources.ListMarkerStyleLowerLatin)
                     .AddValue(PrintListMarkerStyle.UpperLatin, Properties.Resources.ListMarkerStyleUpperLatin)
                     .AddValue(PrintListMarkerStyle.Decimal, Properties.Resources.ListMarkerStyleDecimal);

            EnumCache.AddValue(PrintTextDecoration.Normal, Properties.Resources.TextDecorationNormal)
                     .AddValue(PrintTextDecoration.OverLine, Properties.Resources.TextDecorationOverline)
                     .AddValue(PrintTextDecoration.Strikethrough, Properties.Resources.TextDecorationStrikethrough)
                     .AddValue(PrintTextDecoration.Underline, Properties.Resources.TextDecorationUnderline);

            EnumCache.AddValue(PrintImageStretch.None, Properties.Resources.ImageStretchNone)
                     .AddValue(PrintImageStretch.Fill, Properties.Resources.ImageStretchFill)
                     .AddValue(PrintImageStretch.Uniform, Properties.Resources.ImageStretchUniform);

            EnumCache.AddValue(PrintImageRotation.Rotate0, Properties.Resources.RotationRotate0)
                     .AddValue(PrintImageRotation.Rotate90, Properties.Resources.RotationRotate90)
                     .AddValue(PrintImageRotation.Rotate180, Properties.Resources.RotationRotate180)
                     .AddValue(PrintImageRotation.Rotate270, Properties.Resources.RotationRotate270);

            EnumCache.AddValue(PrintBarcodeQrErrorCorrection.Low, Properties.Resources.PropertyEditorBarcodeQrErrorCorrectionLow)
                     .AddValue(PrintBarcodeQrErrorCorrection.Medium, Properties.Resources.PropertyEditorBarcodeQrErrorCorrectionMedium)
                     .AddValue(PrintBarcodeQrErrorCorrection.Quartile, Properties.Resources.PropertyEditorBarcodeQrErrorCorrectionQuartile)
                     .AddValue(PrintBarcodeQrErrorCorrection.High, Properties.Resources.PropertyEditorBarcodeQrErrorCorrectionHigh);

            EnumCache.AddValue(PrintSizeUnit.Pt, Properties.Resources.SizeUnitPt)
                     .AddValue(PrintSizeUnit.Px, Properties.Resources.SizeUnitPx)
                     .AddValue(PrintSizeUnit.In, Properties.Resources.SizeUnitIn)
                     .AddValue(PrintSizeUnit.Cm, Properties.Resources.SizeUnitCm)
                     .AddValue(PrintSizeUnit.Mm, Properties.Resources.SizeUnitMm);
        }


        public static void AddCommonEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintNamedItem
        {
            // Common
            var commonCategory = builder.AddCommonCategory(parent);
            builder.AddStringEditor(commonCategory, Properties.Resources.NameProperty, i => i.Name, "[a-zA-Z_]+[a-zA-Z_0-9]*");
        }

        public static void AddElementEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintElement
        {
            // Common
            builder.AddCommonEditors(parent);

            // Data
            var dataCategory = builder.AddDataCategory(parent);
            builder.AddVisibilityEditor(dataCategory, i => i.Visibility);
            builder.AddStringEditor(dataCategory, Properties.Resources.SourceProperty, i => i.Source, @"((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1}(\.((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1})*");
            builder.AddLongStringEditor(dataCategory, Properties.Resources.SourceExpression, i => i.Expression);

            // Text
            var textCategory = builder.AddTextCategory(parent);
            builder.AddEditor(textCategory, Properties.Resources.FontProperty, i => i.Font);
            builder.AddTextCaseEditor(textCategory, i => i.TextCase);

            // Appearance
            var appearanceCategory = builder.AddAppearanceCategory(parent);
            builder.AddStyleNameEditor(appearanceCategory, i => i.Style);
            builder.AddColorEditor(appearanceCategory, Properties.Resources.ForegroundProperty, i => i.Foreground);
            builder.AddColorEditor(appearanceCategory, Properties.Resources.BackgroundProperty, i => i.Background);
        }

        public static void AddInlineEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintInline
        {
            // Element
            builder.AddElementEditors(parent);

            // Text
            var textCategory = builder.AddTextCategory(parent);
            builder.AddTextDecorationEditor(textCategory, i => i.TextDecoration);
        }

        public static void AddBlockEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintBlock
        {
            // Element
            builder.AddElementEditors(parent);

            // Text
            var textCategory = builder.AddTextCategory(parent);
            builder.AddTextAlignmentEditor(textCategory, i => i.TextAlignment);

            // Appearance
            var appearanceCategory = builder.AddAppearanceCategory(parent);
            builder.AddEditor(appearanceCategory, Properties.Resources.BorderProperty, i => i.Border);

            // Layout
            var layoutCategory = builder.AddLayoutCategory(parent);
            builder.AddEditor(layoutCategory, Properties.Resources.MarginProperty, i => i.Margin);
            builder.AddEditor(layoutCategory, Properties.Resources.PaddingProperty, i => i.Padding);
        }


        // COMMON CATEGORIES

        public static PropertyEditorBase AddCommonCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Properties.Resources.CommonCategory);
        }

        public static PropertyEditorBase AddDataCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Properties.Resources.DataCategory);
        }

        public static PropertyEditorBase AddTextCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Properties.Resources.TextCategory);
        }

        public static PropertyEditorBase AddAppearanceCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Properties.Resources.AppearanceCategory);
        }

        public static PropertyEditorBase AddLayoutCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Properties.Resources.LayoutCategory);
        }


        // COMMON EDITORS

        public static void AddIntegerEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, int?>> property, int? minValue = null, int? maxValue = null)
        {
            var editor = new IntegerEditor { Caption = caption, MinValue = minValue, MaxValue = maxValue };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddDoubleEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, double?>> property, double? minValue = null, double? maxValue = null)
        {
            var editor = new DoubleEditor { Caption = caption, MinValue = minValue, MaxValue = maxValue };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddBooleanEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, bool?>> property)
        {
            var editor = new BooleanEditor { Caption = caption };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddStringEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, string>> property, string regex = ".*")
        {
            var editor = new StringEditor { Caption = caption, Regex = regex };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddLongStringEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, string>> property)
        {
            var editor = new TextEditor { Caption = caption };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddColorEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, string>> property)
        {
            var editor = new ColorEditor { Caption = caption };

            builder.AddEditor(parent, editor, property);
        }


        // ENUM EDITORS

        private static void AddVisibilityEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintVisibility?>> property)
        {
            builder.AddEnumEditor(parent, Properties.Resources.VisibilityProperty, property);
        }

        public static void AddTextCaseEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextCase?>> property)
        {
            builder.AddEnumEditor(parent, Properties.Resources.TextCaseProperty, property);
        }

        public static void AddTextDecorationEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextDecoration?>> property)
        {
            builder.AddEnumEditor(parent, Properties.Resources.TextDecorationProperty, property);
        }

        public static void AddTextAlignmentEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextAlignment?>> property)
        {
            var editor = new TextAlignmentEditor { Caption = Properties.Resources.TextAlignmentProperty, Items = EnumCache.GetValues<PrintTextAlignment>() };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddImageRotation<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintImageRotation?>> property)
        {
            var editor = new ImageRotationEditor { Caption = Properties.Resources.ImageRotationProperty, Items = EnumCache.GetValues<PrintImageRotation>() };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddEnumEditor<T, TEnum>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, TEnum?>> property) where TEnum : struct
        {
            var editor = new EnumEditor { Caption = caption, ItemsSource = EnumCache.GetValues<TEnum>() };

            builder.AddEditor(parent, editor, property);
        }


        // SPECIFIC EDITORS

        public static void AddStyleNameEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, string>> property)
        {
            var editor = new StyleNameEditor { Caption = Properties.Resources.StyleProperty, DocumentTemplate = builder.Template };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddValueFormatEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, ValueFormat>> property)
        {
            var editor = new ValueFormatEditor { Caption = Properties.Resources.RunSourceFormatProperty };

            builder.AddEditor(parent, editor, property);
        }
    }
}