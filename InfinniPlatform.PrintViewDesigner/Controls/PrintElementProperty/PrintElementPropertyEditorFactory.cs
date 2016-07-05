using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;
using InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions;
using InfinniPlatform.PrintViewDesigner.Properties;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintElementProperty
{
    /// <summary>
    /// Фабрика редакторов свойств для элементов печатного представления.
    /// </summary>
    internal sealed class PrintElementPropertyEditorFactory
    {
        private readonly Dictionary<string, PropertyGridControl> _elementEditors;
        private readonly Func<object> _printViewSource;

        public PrintElementPropertyEditorFactory(Func<object> printViewSource)
        {
            _printViewSource = printViewSource;
            _elementEditors = new Dictionary<string, PropertyGridControl>(StringComparer.OrdinalIgnoreCase);
        }

        private PropertyGridControl GetElementEditor(string elementType, Action<PropertyCollection> elementProperties)
        {
            PropertyGridControl elementEditor;

            if (!_elementEditors.TryGetValue(elementType, out elementEditor))
            {
                elementEditor = new PropertyGridControl();
                elementProperties(elementEditor.Properties);

                _elementEditors.Add(elementType, elementEditor);
            }

            return elementEditor;
        }

        // Blocks

        public PropertyGridControl GetSectionEditor()
        {
            return GetElementEditor("Section", properties => AddBlockProperties(properties, null));
        }

        public PropertyGridControl GetParagraphEditor()
        {
            return GetElementEditor("Paragraph", properties => AddParagraphProperties(properties, null));
        }

        public PropertyGridControl GetListEditor()
        {
            return GetElementEditor("List", properties => AddListProperties(properties, null));
        }

        public PropertyGridControl GetTableEditor()
        {
            return GetElementEditor("Table", properties => AddTableProperties(properties, null));
        }

        public PropertyGridControl GetTableRowEditor()
        {
            return GetElementEditor("TableRow", properties => AddTableRowProperties(properties, null));
        }

        public PropertyGridControl GetTableColumnEditor()
        {
            return GetElementEditor("TableColumn", properties => AddTableColumnProperties(properties, null));
        }

        public PropertyGridControl GetTableCellEditor()
        {
            return GetElementEditor("TableCell", properties => AddTableCellProperties(properties, null));
        }

        public PropertyGridControl GetLineEditor()
        {
            return GetElementEditor("Line", properties => AddBlockProperties(properties, null));
        }

        public PropertyGridControl GetPageBreakEditor()
        {
            return GetElementEditor("PageBreak", properties => AddCommonProperties(properties, null));
        }

        // Inlines

        public PropertyGridControl GetSpanEditor()
        {
            return GetElementEditor("Span", properties => AddInlineProperties(properties, null));
        }

        public PropertyGridControl GetBoldEditor()
        {
            return GetElementEditor("Bold", properties => AddInlineProperties(properties, null));
        }

        public PropertyGridControl GetItalicEditor()
        {
            return GetElementEditor("Italic", properties => AddInlineProperties(properties, null));
        }

        public PropertyGridControl GetUnderlineEditor()
        {
            return GetElementEditor("Underline", properties => AddInlineProperties(properties, null));
        }

        public PropertyGridControl GetHyperlinkEditor()
        {
            return GetElementEditor("Hyperlink", properties => AddHyperlinkProperties(properties, null));
        }

        public PropertyGridControl GetLineBreakEditor()
        {
            return GetElementEditor("LineBreak", properties => AddCommonProperties(properties, null));
        }

        public PropertyGridControl GetRunEditor()
        {
            return GetElementEditor("Run", properties => AddRunProperties(properties, null));
        }

        public PropertyGridControl GetImageEditor()
        {
            return GetElementEditor("Image", properties => AddImageProperties(properties, null));
        }

        public PropertyGridControl GetBarcodeEan13Editor()
        {
            return GetElementEditor("BarcodeEan13", properties => AddBarcodeEan13Properties(properties, null));
        }

        public PropertyGridControl GetBarcodeQrEditor()
        {
            return GetElementEditor("BarcodeQr", properties => AddBarcodeQrProperties(properties, null));
        }

        // Views

        public PropertyGridControl GetPrintViewEditor()
        {
            return GetElementEditor("PrintView", properties => AddPrintViewProperties(properties, null));
        }

        public PropertyGridControl GetPrintViewStyleEditor()
        {
            return GetElementEditor("PrintViewStyle", properties => AddPrintViewStyleProperties(properties, null));
        }

        // Свойства элементов печатного представления

        private void AddParagraphProperties(PropertyCollection properties, string propertyParent)
        {
            AddBlockProperties(properties, propertyParent);

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddProperty<PropertyEditorIndentSize>(textCategory.Properties, propertyParent, "", Resources.ParagraphIndent);
        }

        private void AddListProperties(PropertyCollection properties, string propertyParent)
        {
            AddBlockProperties(properties, propertyParent);

            // List
            var listCategory = AddCategory(properties, Resources.ListCategory);
            AddPropertyInteger(listCategory.Properties, propertyParent, "StartIndex", Resources.ListStartIndexProperty, 1);
            AddProperty<PropertyEditorListMarkerStyle>(listCategory.Properties, propertyParent, "MarkerStyle", Resources.ListMarkerStyleProperty);
            AddProperty<PropertyEditorMarkerOffsetSize>(listCategory.Properties, propertyParent, "", Resources.ListMarkerOffset);
        }

        private void AddTableProperties(PropertyCollection properties, string propertyParent)
        {
            AddBlockProperties(properties, propertyParent);

            // Table
            var tableCategory = AddCategory(properties, Resources.TableCategory);
            AddPropertyBoolean(tableCategory.Properties, propertyParent, "ShowHeader", Resources.ShowHeaderProperty);
        }

        private static void AddTableColumnProperties(PropertyCollection properties, string propertyParent)
        {
            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyString(commonCategory.Properties, propertyParent, "Name", Resources.NameProperty, @"[a-zA-Z_]+[a-zA-Z_0-9]*");

            // Layout
            var layoutCategory = AddCategory(properties, Resources.LayoutCategory);
            AddProperty<PropertyEditorColumnSize>(layoutCategory.Properties, propertyParent, "", Resources.ColumnSizeProperty);
        }

        private void AddTableRowProperties(PropertyCollection properties, string propertyParent)
        {
            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyString(commonCategory.Properties, propertyParent, "Name", Resources.NameProperty, @"[a-zA-Z_]+[a-zA-Z_0-9]*");

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyFont(textCategory.Properties, propertyParent, "Font", Resources.FontProperty);
            AddPropertyTextCase(textCategory.Properties, propertyParent, "TextCase", Resources.TextCaseProperty);

            // Appearance
            var appearanceCategory = AddCategory(properties, Resources.AppearanceCategory);
            AddPropertyStyle(appearanceCategory.Properties, propertyParent, "Style", Resources.StyleProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Foreground", Resources.ForegroundProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Background", Resources.BackgroundProperty);
        }

        private void AddTableCellProperties(PropertyCollection properties, string propertyParent)
        {
            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyString(commonCategory.Properties, propertyParent, "Name", Resources.NameProperty, @"[a-zA-Z_]+[a-zA-Z_0-9]*");

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyFont(textCategory.Properties, propertyParent, "Font", Resources.FontProperty);
            AddPropertyTextCase(textCategory.Properties, propertyParent, "TextCase", Resources.TextCaseProperty);
            AddPropertyTextAlignment(textCategory.Properties, propertyParent, "TextAlignment", Resources.TextAlignmentProperty);

            // Layout
            var layoutCategory = AddCategory(properties, Resources.LayoutCategory);
            AddPropertyThickness(layoutCategory.Properties, propertyParent, "Padding", Resources.PaddingProperty);
            AddPropertyInteger(layoutCategory.Properties, propertyParent, "ColumnSpan", Resources.TableCellColumnSpanProperty, 1);
            AddPropertyInteger(layoutCategory.Properties, propertyParent, "RowSpan", Resources.TableCellRowSpanProperty, 1);

            // Appearance
            var appearanceCategory = AddCategory(properties, Resources.AppearanceCategory);
            AddPropertyStyle(appearanceCategory.Properties, propertyParent, "Style", Resources.StyleProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Foreground", Resources.ForegroundProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Background", Resources.BackgroundProperty);
            AddPropertyBorder(appearanceCategory.Properties, propertyParent, "Border", Resources.BorderProperty);
        }

        private void AddHyperlinkProperties(PropertyCollection properties, string propertyParent)
        {
            AddInlineProperties(properties, propertyParent);

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddPropertyDisplayFormat(dataCategory.Properties, propertyParent, "SourceFormat", Resources.HyperlinkSourceFormatProperty);
            AddPropertyString(dataCategory.Properties, propertyParent, "Reference", Resources.HyperlinkReferenceProperty);
        }

        private void AddRunProperties(PropertyCollection properties, string propertyParent)
        {
            AddInlineProperties(properties, propertyParent);

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddPropertyDisplayFormat(dataCategory.Properties, propertyParent, "SourceFormat", Resources.RunSourceFormatProperty);
            AddProperty<PropertyEditorLongString>(dataCategory.Properties, propertyParent, "Text", Resources.RunTextProperty);
        }

        private static void AddImageProperties(PropertyCollection properties, string propertyParent)
        {
            AddCommonProperties(properties, propertyParent);

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddProperty<PropertyEditorImage>(dataCategory.Properties, propertyParent, "Data", Resources.ImageDataProperty);

            // Image
            var imageCategory = AddCategory(properties, Resources.ImageCategory);
            AddPropertySize(imageCategory.Properties, propertyParent, "Size", Resources.ImageSizeProperty);
            AddPropertyRotation(imageCategory.Properties, propertyParent, "Rotation", Resources.ImageRotationProperty);
            AddProperty<PropertyEditorImageStretch>(imageCategory.Properties, propertyParent, "Stretch", Resources.ImageStretchProperty);
        }

        private static void AddBarcodeEan13Properties(PropertyCollection properties, string propertyParent)
        {
            AddCommonProperties(properties, propertyParent);

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddPropertyDisplayFormat(dataCategory.Properties, propertyParent, "SourceFormat", Resources.RunSourceFormatProperty);
            AddPropertyString(dataCategory.Properties, propertyParent, "Text", Resources.RunTextProperty, @"[0-9]+");
            AddPropertyBoolean(dataCategory.Properties, propertyParent, "ShowText", Resources.ShowTextProperty);

            // Barcode
            var barcodeCategory = AddCategory(properties, Resources.BarcodeCategory);
            AddPropertyBoolean(barcodeCategory.Properties, propertyParent, "CalcCheckSum", Resources.BarcodeEan13CalcCheckSumProperty);
            AddPropertyDouble(barcodeCategory.Properties, propertyParent, "WideBarRatio", Resources.BarcodeEan13WideBarRatioProperty, 2);
            AddPropertyRotation(barcodeCategory.Properties, propertyParent, "Rotation", Resources.BarcodeEan13RotationProperty);
        }

        private static void AddBarcodeQrProperties(PropertyCollection properties, string propertyParent)
        {
            AddCommonProperties(properties, propertyParent);

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddPropertyDisplayFormat(dataCategory.Properties, propertyParent, "SourceFormat",
                Resources.RunSourceFormatProperty);
            AddPropertyLongString(dataCategory.Properties, propertyParent, "Text", Resources.RunTextProperty);
            AddPropertyBoolean(dataCategory.Properties, propertyParent, "ShowText", Resources.ShowTextProperty);

            // Barcode
            var barcodeCategory = AddCategory(properties, Resources.BarcodeCategory);
            AddProperty<PropertyEditorBarcodeQrErrorCorrection>(barcodeCategory.Properties, propertyParent, "ErrorCorrection", Resources.BarcodeQrErrorCorrectionProperty);
            AddPropertyRotation(barcodeCategory.Properties, propertyParent, "Rotation", Resources.BarcodeQrRotationProperty);
        }

        private void AddBlockProperties(PropertyCollection properties, string propertyParent)
        {
            AddElementProperties(properties, propertyParent);

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyTextAlignment(textCategory.Properties, propertyParent, "TextAlignment",
                Resources.TextAlignmentProperty);

            // Appearance
            var appearanceCategory = AddCategory(properties, Resources.AppearanceCategory);
            AddPropertyBorder(appearanceCategory.Properties, propertyParent, "Border", Resources.BorderProperty);

            // Layout
            var layoutCategory = AddCategory(properties, Resources.LayoutCategory);
            AddPropertyThickness(layoutCategory.Properties, propertyParent, "Margin", Resources.MarginProperty);
            AddPropertyThickness(layoutCategory.Properties, propertyParent, "Padding", Resources.PaddingProperty);
        }

        private void AddInlineProperties(PropertyCollection properties, string propertyParent)
        {
            AddElementProperties(properties, propertyParent);

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyTextDecoration(textCategory.Properties, propertyParent, "TextDecoration", Resources.TextDecorationProperty);
        }

        private void AddPrintViewProperties(PropertyCollection properties, string propertyParent)
        {
            AddElementProperties(properties, propertyParent);

            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyLongString(commonCategory.Properties, propertyParent, "Caption", Resources.CaptionProperty);
            AddPropertyLongString(commonCategory.Properties, propertyParent, "Description", Resources.DescriptionProperty);
            AddProperty<PropertyEditorViewType>(commonCategory.Properties, propertyParent, "ViewType", Resources.ViewTypeProperty);

            // Page
            var pageCategory = AddCategory(properties, Resources.PageCategory);
            AddPropertySize(pageCategory.Properties, propertyParent, "PageSize", Resources.PageSizeProperty);
            AddPropertyThickness(pageCategory.Properties, propertyParent, "PagePadding", Resources.PagePaddingProperty);
        }

        private static void AddPrintViewStyleProperties(PropertyCollection properties, string propertyParent)
        {
            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyString(commonCategory.Properties, propertyParent, "Name", Resources.NameProperty, @"[a-zA-Z_]+[a-zA-Z_0-9]*");

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyFont(textCategory.Properties, propertyParent, "Font", Resources.FontProperty);
            AddPropertyTextCase(textCategory.Properties, propertyParent, "TextCase", Resources.TextCaseProperty);
            AddPropertyTextDecoration(textCategory.Properties, propertyParent, "TextDecoration", Resources.TextDecorationProperty);
            AddPropertyTextAlignment(textCategory.Properties, propertyParent, "TextAlignment", Resources.TextAlignmentProperty);

            // Layout
            var layoutCategory = AddCategory(properties, Resources.LayoutCategory);
            AddPropertyThickness(layoutCategory.Properties, propertyParent, "Margin", Resources.MarginProperty);
            AddPropertyThickness(layoutCategory.Properties, propertyParent, "Padding", Resources.PaddingProperty);

            // Appearance
            var appearanceCategory = AddCategory(properties, Resources.AppearanceCategory);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Foreground", Resources.ForegroundProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Background", Resources.BackgroundProperty);
            AddPropertyBorder(appearanceCategory.Properties, propertyParent, "Border", Resources.BorderProperty);
        }

        private void AddElementProperties(PropertyCollection properties, string propertyParent)
        {
            AddCommonProperties(properties, propertyParent);

            // Text
            var textCategory = AddCategory(properties, Resources.TextCategory);
            AddPropertyFont(textCategory.Properties, propertyParent, "Font", Resources.FontProperty);
            AddPropertyTextCase(textCategory.Properties, propertyParent, "TextCase", Resources.TextCaseProperty);

            // Appearance
            var appearanceCategory = AddCategory(properties, Resources.AppearanceCategory);
            AddPropertyStyle(appearanceCategory.Properties, propertyParent, "Style", Resources.StyleProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Foreground", Resources.ForegroundProperty);
            AddPropertyColor(appearanceCategory.Properties, propertyParent, "Background", Resources.BackgroundProperty);
        }

        private static void AddCommonProperties(PropertyCollection properties, string propertyParent)
        {
            // Common
            var commonCategory = AddCategory(properties, Resources.CommonCategory);
            AddPropertyString(commonCategory.Properties, propertyParent, "Name", Resources.NameProperty, "[a-zA-Z_]+[a-zA-Z_0-9]*");

            // Data
            var dataCategory = AddCategory(properties, Resources.DataCategory);
            AddProperty<PropertyEditorVisibility>(dataCategory.Properties, propertyParent, "Visibility", Resources.VisibilityProperty);
            AddPropertyString(dataCategory.Properties, propertyParent, "Source", Resources.SourceProperty, @"((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1}(\.((\$)|([0-9]+)|([a-zA-Z_]+[a-zA-Z_0-9]*)){1})*");
            AddPropertyLongString(dataCategory.Properties, propertyParent, "Expression", Resources.SourceExpression);
        }

        // Редакторы сложных свойств элементов печатного представления

        private static void AddPropertyDisplayFormat(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorDisplayFormat>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyBorder(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorBorder>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertySize(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorSize>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyThickness(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorThickness>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyFont(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorFont>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyRotation(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorRotation>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyTextCase(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorTextCase>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyTextDecoration(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorTextDecoration>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyTextAlignment(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorTextAlignment>(properties, propertyParent, propertyName, propertyCaption);
        }

        private void AddPropertyStyle(PropertyCollection properties, string propertyParent, string propertyName,
            string propertyCaption)
        {
            var propertyEditor = AddProperty<PropertyEditorStyle>(properties, propertyParent, propertyName, propertyCaption);
            propertyEditor.PrintViewSource = _printViewSource;
        }

        // Вспомогательные методы для простых свойств свойств элементов печатного представления

        private static PropertyEditorBase AddCategory(PropertyCollection properties, string categoryCaption)
        {
            var propertyDefinition = properties.FirstOrDefault(p => p.Caption == categoryCaption);

            return (propertyDefinition == null)
                ? AddProperty<PropertyEditorObject>(properties, null, null, categoryCaption)
                : propertyDefinition.Editor;
        }

        private static void AddPropertyString(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption, string propertyRegex = ".*")
        {
            var propertyEditor = AddProperty<PropertyEditorString>(properties, propertyParent, propertyName, propertyCaption);
            propertyEditor.Regex = propertyRegex;
        }

        private static void AddPropertyLongString(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorLongString>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyDouble(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption, double? minValue = null, double? maxValue = null)
        {
            var propertyEditor = AddProperty<PropertyEditorDouble>(properties, propertyParent, propertyName, propertyCaption);
            propertyEditor.MinValue = minValue;
            propertyEditor.MaxValue = maxValue;
        }

        private static void AddPropertyInteger(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption, int? minValue = null, int? maxValue = null)
        {
            var propertyEditor = AddProperty<PropertyEditorInteger>(properties, propertyParent, propertyName, propertyCaption);
            propertyEditor.MinValue = minValue;
            propertyEditor.MaxValue = maxValue;
        }

        private static void AddPropertyBoolean(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorBoolean>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static void AddPropertyColor(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption)
        {
            AddProperty<PropertyEditorColor>(properties, propertyParent, propertyName, propertyCaption);
        }

        private static T AddProperty<T>(PropertyCollection properties, string propertyParent, string propertyName, string propertyCaption) where T : PropertyEditorBase, new()
        {
            var propertyPath = Helpers.CombineProperties(propertyParent, propertyName);

            var propertyEditor = new T {Property = propertyPath};

            properties.Add(new PropertyDefinition
            {
                Name = propertyPath,
                Caption = propertyCaption ?? propertyName,
                Editor = propertyEditor
            });

            return propertyEditor;
        }
    }
}