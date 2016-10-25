using System;
using System.Windows;

using DevExpress.Xpf.Core;

using InfinniPlatform.PrintViewDesigner.Common;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Panel
{
    public partial class PrintViewPreviewDataDialog : DXWindow
    {
        public PrintViewPreviewDataDialog()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Редактируемые данные.
        /// </summary>
        public object EditValue
        {
            get
            {
                object value;
                Exception exception;

                return ConvertJsonToObject(JsonEditor.EditValue as string, out value, out exception) ? value : null;
            }
            set
            {
                string json;
                Exception exception;

                JsonEditor.EditValue = ConvertObjectToJson(value, out json, out exception) ? json : null;
            }
        }


        private static bool ConvertJsonToObject(string json, out object value, out Exception exception)
        {
            value = null;
            exception = null;

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    value = JsonObjectSerializer.Formated.Deserialize(json);
                }
                catch (Exception e)
                {
                    exception = e;
                    return false;
                }
            }

            return true;
        }

        private static bool ConvertObjectToJson(object value, out string json, out Exception exception)
        {
            json = null;
            exception = null;

            if (value != null)
            {
                try
                {
                    json = JsonObjectSerializer.Formated.ConvertToString(value);
                }
                catch (Exception e)
                {
                    exception = e;
                    return false;
                }
            }

            return true;
        }


        private void OnReformatButton(object sender, RoutedEventArgs e)
        {
            object value;
            Exception exception;

            var sourceJson = JsonEditor.EditValue as string;

            if (!ConvertJsonToObject(sourceJson, out value, out exception))
            {
                MessageBoxHelpers.ShowWarningMessage(Properties.Resources.SourceJsonHasWrongFormat, exception.Message);
                return;
            }

            string reformatedJson;

            if (!ConvertObjectToJson(value, out reformatedJson, out exception))
            {
                MessageBoxHelpers.ShowWarningMessage(Properties.Resources.SourceJsonHasWrongFormat, exception.Message);
                return;
            }

            JsonEditor.EditValue =  reformatedJson;
        }

        private void OnResetButton(object sender, RoutedEventArgs e)
        {
            JsonEditor.EditValue = null;
        }


        private void OnAcceptButton(object sender, RoutedEventArgs e)
        {
            object value;
            Exception exception;

            var sourceJson = JsonEditor.EditValue as string;

            if (!ConvertJsonToObject(sourceJson, out value, out exception))
            {
                MessageBoxHelpers.ShowWarningMessage(Properties.Resources.SourceJsonHasWrongFormat, exception.Message);
                return;
            }

            DialogResult = true;
        }

        private void OnCancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}