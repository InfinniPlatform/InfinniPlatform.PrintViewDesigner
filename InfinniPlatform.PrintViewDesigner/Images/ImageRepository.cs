using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InfinniPlatform.PrintViewDesigner.Images
{
    /// <summary>
    /// Хранилище изображений.
    /// </summary>
    internal sealed class ImageRepository
    {
        private static readonly string[] Resources;
        private static readonly Dictionary<string, ImageSource> ImageCache;


        static ImageRepository()
        {
            Resources = GetResourceNames();
            ImageCache = new Dictionary<string, ImageSource>(StringComparer.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Получить изображение по имени.
        /// </summary>
        public static ImageSource GetImage(params string[] path)
        {
            ImageSource image = null;

            var imagePath = GetImagePath(path);

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (ImageCache.TryGetValue(imagePath, out image) == false)
                {
                    try
                    {
                        var imagResource =
                            Resources.FirstOrDefault(r => Regex.IsMatch(r, imagePath, RegexOptions.IgnoreCase));

                        if (imagResource != null)
                        {
                            image = new BitmapImage(new Uri(imagResource));
                        }
                    }
                    catch (Exception)
                    {
                        image = null;
                    }

                    ImageCache[imagePath] = image;
                }
            }

            return image;
        }


        private static string[] GetResourceNames()
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyName = assembly.GetName().Name;
            var assemblyResources = assemblyName + ".g.resources";

            using (var stream = assembly.GetManifestResourceStream(assemblyResources))
            {
                if (stream != null)
                {
                    using (var reader = new ResourceReader(stream))
                    {
                        return reader.Cast<DictionaryEntry>().Select(entry => $"pack://application:,,,/{assemblyName};component/{entry.Key}").ToArray();
                    }
                }
            }

            return new string[] { };
        }

        private static string GetImagePath(string[] path)
        {
            if (path != null && path.Length > 0)
            {
                var pathPattern = new StringBuilder("Images/");

                foreach (var i in path)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        pathPattern.Append(@".*?").Append(i);
                    }
                }

                pathPattern.Append(@"\.png$");

                return pathPattern.ToString();
            }

            return null;
        }
    }
}