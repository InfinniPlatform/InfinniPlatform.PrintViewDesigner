using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using InfinniPlatform.PrintViewDesigner.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintViewDesigner.Controls
{
    internal static class Helpers
    {
        private static readonly JsonObjectSerializer Serializer = new JsonObjectSerializer(withFormatting: true);

        public static string CombineProperties(params object[] properties)
        {
            return string.Join(".",
                properties.Select(p => (p != null) ? p.ToString() : null).Where(p => !string.IsNullOrEmpty(p)));
        }

        public static object LoadObjectFromFile(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return Serializer.Deserialize(stream, typeof (DynamicWrapper));
            }
        }

        public static void SaveObjectToFile(string file, object value)
        {
            using (var stream = File.OpenWrite(file))
            {
                Serializer.Serialize(stream, value);
            }
        }

        public static void CopyObject(dynamic source, dynamic destination, IList<string> properties)
        {
            if (source != null && destination != null && properties != null)
            {
                foreach (var propertyName in properties)
                {
                    destination[propertyName] = source[propertyName];
                }
            }
        }

        public static void ShowWarningMessage(string message, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Resources.PrintViewDesignerName, MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        public static bool AcceptQuestionMessage(string message, params object[] args)
        {
            return
                MessageBox.Show(string.Format(message, args), Resources.PrintViewDesignerName, MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes;
        }
    }
}