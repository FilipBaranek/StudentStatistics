using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Microsoft.Win32;
using StudentStatistics.Models;

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

        public static void ExportToCsv(ObservableCollection<Student> students)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "students.csv",
                Title = "Uložiť ako CSV"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string data = SerializeToCsv(students);

                File.WriteAllText(filePath, data, Encoding.UTF8);
            }
        }

        public static string SerializeToCsv(ObservableCollection<Student> students)
        {
            var sb = new StringBuilder();

            foreach (var student in students)
            {
                sb.AppendLine($"{student.PersonalNumber};{student.Name};{student.Surname};{student.SemesterResults?.Teacher}");
            }

            return sb.ToString();
        }

    }
}
