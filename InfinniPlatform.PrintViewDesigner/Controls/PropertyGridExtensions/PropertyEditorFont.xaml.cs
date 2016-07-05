using System.Collections.Generic;
using System.Windows;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorFont : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> FontStyleItems
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Normal", AppResources.FontStyleNormal },
                  { "Italic", AppResources.FontStyleItalic },
                  { "Oblique", AppResources.FontStyleOblique }
              };

        public static readonly Dictionary<string, string> FontStretchItems
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "UltraCondensed", AppResources.FontStretchUltraCondensed },
                  { "ExtraCondensed", AppResources.FontStretchExtraCondensed },
                  { "Condensed", AppResources.FontStretchCondensed },
                  { "SemiCondensed", AppResources.FontStretchSemiCondensed },
                  { "Normal", AppResources.FontStretchNormal },
                  { "SemiExpanded", AppResources.FontStretchSemiExpanded },
                  { "Expanded", AppResources.FontStretchExpanded },
                  { "ExtraExpanded", AppResources.FontStretchExtraExpanded },
                  { "UltraExpanded", AppResources.FontStretchUltraExpanded }
              };

        public static readonly Dictionary<string, string> FontWeightItems
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "UltraLight", AppResources.FontWeightUltraLight },
                  { "ExtraLight", AppResources.FontWeightExtraLight },
                  { "Light", AppResources.FontWeightLight },
                  { "Normal", AppResources.FontWeightNormal },
                  { "Medium", AppResources.FontWeightMedium },
                  { "SemiBold", AppResources.FontWeightSemiBold },
                  { "Bold", AppResources.FontWeightBold },
                  { "ExtraBold", AppResources.FontWeightExtraBold },
                  { "UltraBold", AppResources.FontWeightUltraBold }
              };

        public static readonly Dictionary<string, string> FontVariantItems
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Normal", AppResources.FontVariantNormal },
                  { "Subscript", AppResources.FontVariantSubscript },
                  { "Superscript", AppResources.FontVariantSuperscript }
              };

        public PropertyEditorFont()
        {
            InitializeComponent();
        }

        protected override void OnPropertiesChanged(PropertyCollection properties)
        {
            base.OnPropertiesChanged(properties);

            if (properties != null)
            {
                AddProperty("Family", AppResources.FontFamilyProperty, FontFamilyEditor, Visibility.Collapsed);
                AddProperty("Size", AppResources.FontSizeProperty, FontSizeEditor, Visibility.Collapsed);
                AddProperty("SizeUnit", AppResources.FontSizeUnitProperty, FontSizeUnitEditor, Visibility.Collapsed);

                AddPropertyEnum("Style", AppResources.FontStyleProperty, FontStyleItems);
                AddPropertyEnum("Stretch", AppResources.FontStretchProperty, FontStretchItems);
                AddPropertyEnum("Weight", AppResources.FontWeightProperty, FontWeightItems);
                AddPropertyEnum("Variant", AppResources.FontVariantProperty, FontVariantItems);
            }
        }

        private void AddPropertyEnum(string propertyName, string propertyCaption, object itemSource)
        {
            var propertyEditor = new PropertyEditorEnum
                                 {
                                     ValueMember = "Key",
                                     DisplayMember = "Value",
                                     ItemsSource = itemSource
                                 };

            AddProperty(propertyName, propertyCaption, propertyEditor, Visibility.Visible);
        }
    }
}