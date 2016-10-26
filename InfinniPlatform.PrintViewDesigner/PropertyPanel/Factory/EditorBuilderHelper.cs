using System;
using System.Linq.Expressions;
using System.Windows.Controls;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Format;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintViewDesigner.Properties;
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

            EnumCache.AddValue(PrintVisibility.Never, Resources.VisibilityNever)
                     .AddValue(PrintVisibility.Always, Resources.VisibilityAlways)
                     .AddValue(PrintVisibility.Source, Resources.VisibilitySource);

            EnumCache.AddValue(PrintFontStyle.Normal, Resources.FontStyleNormal)
                     .AddValue(PrintFontStyle.Italic, Resources.FontStyleItalic)
                     .AddValue(PrintFontStyle.Oblique, Resources.FontStyleOblique);

            EnumCache.AddValue(PrintFontStretch.Normal, Resources.FontStretchNormal)
                     .AddValue(PrintFontStretch.UltraCondensed, Resources.FontStretchUltraCondensed)
                     .AddValue(PrintFontStretch.ExtraCondensed, Resources.FontStretchExtraCondensed)
                     .AddValue(PrintFontStretch.Condensed, Resources.FontStretchCondensed)
                     .AddValue(PrintFontStretch.SemiCondensed, Resources.FontStretchSemiCondensed)
                     .AddValue(PrintFontStretch.SemiExpanded, Resources.FontStretchSemiExpanded)
                     .AddValue(PrintFontStretch.Expanded, Resources.FontStretchExpanded)
                     .AddValue(PrintFontStretch.ExtraExpanded, Resources.FontStretchExtraExpanded)
                     .AddValue(PrintFontStretch.UltraExpanded, Resources.FontStretchUltraExpanded);

            EnumCache.AddValue(PrintFontWeight.Normal, Resources.FontWeightNormal)
                     .AddValue(PrintFontWeight.UltraLight, Resources.FontWeightUltraLight)
                     .AddValue(PrintFontWeight.ExtraLight, Resources.FontWeightExtraLight)
                     .AddValue(PrintFontWeight.Light, Resources.FontWeightLight)
                     .AddValue(PrintFontWeight.Medium, Resources.FontWeightMedium)
                     .AddValue(PrintFontWeight.SemiBold, Resources.FontWeightSemiBold)
                     .AddValue(PrintFontWeight.Bold, Resources.FontWeightBold)
                     .AddValue(PrintFontWeight.ExtraBold, Resources.FontWeightExtraBold)
                     .AddValue(PrintFontWeight.UltraBold, Resources.FontWeightUltraBold);

            EnumCache.AddValue(PrintFontVariant.Normal, Resources.FontVariantNormal)
                     .AddValue(PrintFontVariant.Subscript, Resources.FontVariantSubscript)
                     .AddValue(PrintFontVariant.Superscript, Resources.FontVariantSuperscript);

            EnumCache.AddValue(PrintTextAlignment.Left, Resources.TextAlignmentLeft)
                     .AddValue(PrintTextAlignment.Center, Resources.TextAlignmentCenter)
                     .AddValue(PrintTextAlignment.Right, Resources.TextAlignmentRight)
                     .AddValue(PrintTextAlignment.Justify, Resources.TextAlignmentJustify);

            EnumCache.AddValue(PrintTextCase.Normal, Resources.TextCaseNormal)
                     .AddValue(PrintTextCase.SentenceCase, Resources.TextCaseSentenceCase)
                     .AddValue(PrintTextCase.Lowercase, Resources.TextCaseLowercase)
                     .AddValue(PrintTextCase.Uppercase, Resources.TextCaseUppercase)
                     .AddValue(PrintTextCase.ToggleCase, Resources.TextCaseToggleCase);

            EnumCache.AddValue(PrintListMarkerStyle.None, Resources.ListMarkerStyleNone)
                     .AddValue(PrintListMarkerStyle.Disc, Resources.ListMarkerStyleDisc)
                     .AddValue(PrintListMarkerStyle.Circle, Resources.ListMarkerStyleCircle)
                     .AddValue(PrintListMarkerStyle.Square, Resources.ListMarkerStyleSquare)
                     .AddValue(PrintListMarkerStyle.Box, Resources.ListMarkerStyleBox)
                     .AddValue(PrintListMarkerStyle.LowerRoman, Resources.ListMarkerStyleLowerRoman)
                     .AddValue(PrintListMarkerStyle.UpperRoman, Resources.ListMarkerStyleUpperRoman)
                     .AddValue(PrintListMarkerStyle.LowerLatin, Resources.ListMarkerStyleLowerLatin)
                     .AddValue(PrintListMarkerStyle.UpperLatin, Resources.ListMarkerStyleUpperLatin)
                     .AddValue(PrintListMarkerStyle.Decimal, Resources.ListMarkerStyleDecimal);

            EnumCache.AddValue(PrintTextDecoration.Normal, Resources.TextDecorationNormal)
                     .AddValue(PrintTextDecoration.OverLine, Resources.TextDecorationOverline)
                     .AddValue(PrintTextDecoration.Strikethrough, Resources.TextDecorationStrikethrough)
                     .AddValue(PrintTextDecoration.Underline, Resources.TextDecorationUnderline);

            EnumCache.AddValue(PrintImageStretch.None, Resources.ImageStretchNone)
                     .AddValue(PrintImageStretch.Fill, Resources.ImageStretchFill)
                     .AddValue(PrintImageStretch.Uniform, Resources.ImageStretchUniform);

            EnumCache.AddValue(PrintImageRotation.Rotate0, Resources.RotationRotate0)
                     .AddValue(PrintImageRotation.Rotate90, Resources.RotationRotate90)
                     .AddValue(PrintImageRotation.Rotate180, Resources.RotationRotate180)
                     .AddValue(PrintImageRotation.Rotate270, Resources.RotationRotate270);

            EnumCache.AddValue(PrintBarcodeQrErrorCorrection.Low, Resources.PropertyEditorBarcodeQrErrorCorrectionLow)
                     .AddValue(PrintBarcodeQrErrorCorrection.Medium, Resources.PropertyEditorBarcodeQrErrorCorrectionMedium)
                     .AddValue(PrintBarcodeQrErrorCorrection.Quartile, Resources.PropertyEditorBarcodeQrErrorCorrectionQuartile)
                     .AddValue(PrintBarcodeQrErrorCorrection.High, Resources.PropertyEditorBarcodeQrErrorCorrectionHigh);

            EnumCache.AddValue(PrintSizeUnit.Pt, Resources.SizeUnitPt)
                     .AddValue(PrintSizeUnit.Px, Resources.SizeUnitPx)
                     .AddValue(PrintSizeUnit.In, Resources.SizeUnitIn)
                     .AddValue(PrintSizeUnit.Cm, Resources.SizeUnitCm)
                     .AddValue(PrintSizeUnit.Mm, Resources.SizeUnitMm);
        }


        public static void AddCommonEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintNamedItem
        {
            // Common
            var commonCategory = builder.AddCommonCategory(parent);
            builder.AddStringEditor(commonCategory, Resources.NameProperty, i => i.Name, "[a-zA-Z_]+[a-zA-Z_0-9]*");
        }

        public static void AddElementEditors<T>(this EditorBuilder<T> builder, PropertyEditorBase parent) where T : PrintElement
        {
            // Common
            builder.AddCommonEditors(parent);

            // Data
            var dataCategory = builder.AddDataCategory(parent);
            builder.AddVisibilityEditor(dataCategory, i => i.Visibility);
            builder.AddStringEditor(dataCategory, Resources.SourceProperty, i => i.Source, @"((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1}(\.((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1})*");
            builder.AddTextEditor(dataCategory, Resources.SourceExpression, i => i.Expression);

            // Text
            var textCategory = builder.AddTextCategory(parent);
            builder.AddEditor(textCategory, Resources.FontProperty, i => i.Font);
            builder.AddTextCaseEditor(textCategory, i => i.TextCase);

            // Appearance
            var appearanceCategory = builder.AddAppearanceCategory(parent);
            builder.AddStyleNameEditor(appearanceCategory, i => i.Style);
            builder.AddColorEditor(appearanceCategory, Resources.ForegroundProperty, i => i.Foreground);
            builder.AddColorEditor(appearanceCategory, Resources.BackgroundProperty, i => i.Background);
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
            builder.AddEditor(appearanceCategory, Resources.BorderProperty, i => i.Border);

            // Layout
            var layoutCategory = builder.AddLayoutCategory(parent);
            builder.AddEditor(layoutCategory, Resources.MarginProperty, i => i.Margin);
            builder.AddEditor(layoutCategory, Resources.PaddingProperty, i => i.Padding);
        }


        // COMMON CATEGORIES

        public static PropertyEditorBase AddCommonCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Resources.CommonCategory);
        }

        public static PropertyEditorBase AddDataCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Resources.DataCategory);
        }

        public static PropertyEditorBase AddTextCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Resources.TextCategory);
        }

        public static PropertyEditorBase AddAppearanceCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Resources.AppearanceCategory);
        }

        public static PropertyEditorBase AddLayoutCategory<T>(this EditorBuilder<T> builder, PropertyEditorBase parent)
        {
            return builder.AddCategory(parent, Resources.LayoutCategory);
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

        public static void AddTextEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, string caption, Expression<Func<T, string>> property)
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
            builder.AddEnumEditor(parent, Resources.VisibilityProperty, property);
        }

        public static void AddTextCaseEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextCase?>> property)
        {
            builder.AddEnumEditor(parent, Resources.TextCaseProperty, property);
        }

        public static void AddTextDecorationEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextDecoration?>> property)
        {
            builder.AddEnumEditor(parent, Resources.TextDecorationProperty, property);
        }

        public static void AddTextAlignmentEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintTextAlignment?>> property)
        {
            var editor = new TextAlignmentEditor { Caption = Resources.TextAlignmentProperty, Items = EnumCache.GetValues<PrintTextAlignment>() };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddImageRotation<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, PrintImageRotation?>> property)
        {
            var editor = new ImageRotationEditor { Caption = Resources.ImageRotationProperty, Items = EnumCache.GetValues<PrintImageRotation>() };

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
            var editor = new StyleNameEditor { Caption = Resources.StyleProperty, DocumentTemplate = builder.Template };

            builder.AddEditor(parent, editor, property);
        }

        public static void AddValueFormatEditor<T>(this EditorBuilder<T> builder, PropertyEditorBase parent, Expression<Func<T, ValueFormat>> property)
        {
            var editor = new ValueFormatEditor { Caption = Resources.RunSourceFormatProperty };

            builder.AddEditor(parent, editor, property);
        }


        // MENU

        public static void AddClearButton<T>(this EditorBuilder<T> builder, PropertyEditorBase editor)
        {
            var clearMenuItem = new MenuItem { Header = Resources.Clear };
            clearMenuItem.Click += (s, e) => editor.EditValue = null;

            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(clearMenuItem);

            editor.ContextMenu = contextMenu;
        }
    }
}