using Microsoft.Win32;

namespace StudentStatistics.Services
{
    public static class FileHandler
    {
        public static string? OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Select a CSV File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}
