using Microsoft.Win32;

namespace StudentStatistics.Services
{
    public static class FileHandler
    {
        public static string? OpenExcelFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Vyberte Excel súbor"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public static string? OpenCsvFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Vyberte CSV súbor"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}
