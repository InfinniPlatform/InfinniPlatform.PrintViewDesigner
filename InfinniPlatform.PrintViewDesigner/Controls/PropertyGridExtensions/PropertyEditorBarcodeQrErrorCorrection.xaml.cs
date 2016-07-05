using System.Collections.Generic;

using InfinniPlatform.PrintViewDesigner.Controls.PropertyGrid;

using AppResources = InfinniPlatform.PrintViewDesigner.Properties.Resources;

namespace InfinniPlatform.PrintViewDesigner.Controls.PropertyGridExtensions
{
    public partial class PropertyEditorBarcodeQrErrorCorrection : PropertyEditorBase
    {
        public static readonly Dictionary<string, string> Items
            = new Dictionary<string, string>
              {
                  { "", "" },
                  { "Low", AppResources.PropertyEditorBarcodeQrErrorCorrectionLow },
                  { "Medium", AppResources.PropertyEditorBarcodeQrErrorCorrectionMedium },
                  { "Quartile", AppResources.PropertyEditorBarcodeQrErrorCorrectionQuartile },
                  { "High", AppResources.PropertyEditorBarcodeQrErrorCorrectionHigh }
              };

        public PropertyEditorBarcodeQrErrorCorrection()
        {
            InitializeComponent();
        }
    }
}