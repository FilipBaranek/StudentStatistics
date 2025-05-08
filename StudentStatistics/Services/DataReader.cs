using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using StudentStatistics.Models;

namespace StudentStatistics.Services
{
    public static class DataReader
    {
        public static ObservableCollection<Student> ReadData(string filePath)
        {
            ObservableCollection<Student> students = new ObservableCollection<Student>();

            using StreamReader admissionReader = new StreamReader(filePath);
            string? line = admissionReader.ReadLine();

            while ((line = admissionReader.ReadLine()) != null)
            {
                string[] data = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                string name = data[6];
                string surname = data[7];
                string gender = data[8];
                int? personalNumber = !data[0].Equals("") ? int.Parse(data[0]) : null;
                string studyProgram = data[1];
                string nationality = data[5];

                students.Add(new Student(name, surname, gender, personalNumber, studyProgram, nationality, GetAdmissionProcess(data)));
            }

            return students;
        }

        private static AdmissionProcess GetAdmissionProcess(string[] data)
        {
            string result = data[4];
            string? firstAlternativeProgram = !data[2].Equals("") ? data[2] : null;
            string? secondAlternativeProgram = !data[3].Equals("") ? data[3] : null;
            string? highSchoolCompetetions = !data[9].Equals("") ? data[9] : null;
            int graduationYear = int.Parse(data[10]);
            int? highSchoolCode = !data[11].Equals("") ? int.Parse(data[11]) : null;
            string? highSchoolName = !data[12].Equals("") ? data[12] : null;
            string highSchoolType = data[13];
            string? highSchoolLocation = !data[14].Equals("") ? data[14] : null;
            double thirdYearAverage = FormatNumber(data[15]);
            double? externGraduationPart = !data[16].Equals("0") ? FormatNumber(data[16]) : null;
            double? externPartPercentile = !data[17].Equals("0") ? FormatNumber(data[17]) : null;
            double? scioPercentile = !data[18].Equals("0") ? FormatNumber(data[18]) : null;
            double? mathPoints = !data[19].Equals("0") ? FormatNumber(data[19]) : null;
            string registration = data[21];

            return new AdmissionProcess(result, firstAlternativeProgram, secondAlternativeProgram, highSchoolCompetetions, graduationYear, highSchoolCode,
                                        highSchoolName, highSchoolType, highSchoolLocation, thirdYearAverage, externGraduationPart, externPartPercentile,
                                        scioPercentile, mathPoints, registration);
        }
        
        private static double FormatNumber(string number)
        {
            string formatedNumber = number.Replace('.', ',');

            return double.Parse(formatedNumber);
        }

    }
}
