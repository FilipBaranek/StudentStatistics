using System.Windows;

namespace StudentStatistics.Services
{
    public static class ResourceHandler
    {
        public static void LoadResource(string resourceName)
        {
            string resourceFilePath = $"Resources/{resourceName}.xaml";
            var resource = new ResourceDictionary
            {
                Source = new System.Uri(resourceFilePath, System.UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Add(resource);
        }
    }
}
