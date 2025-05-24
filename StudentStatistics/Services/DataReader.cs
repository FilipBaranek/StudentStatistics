using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using StudentStatistics.Models;

namespace StudentStatistics.Services
{
    public static class DataReader
    {
        public static ObservableCollection<Student>? ReadAdmissionFile(string admissionFilePath)
        {
            var students = new ObservableCollection<Student>();

            try
            {
                ReadAdmissionFile(admissionFilePath, students);
            }
            catch (Exception)
            {
                return null;
            }

            return students;
        }

        public static void ReadSemesterFile(string semesterFilePath, string teacher, ObservableCollection<Student> students)
        {
            using (var excelFile = new XLWorkbook(semesterFilePath))
            {
                foreach (var sheet in excelFile.Worksheets)
                {
                    string studyGroup = sheet.Row(1).Cell(3).GetValue<string>();

                    foreach (var row in sheet.RowsUsed())
                    {
                        if (row.Cell(3).IsEmpty())
                        {
                            break;
                        }

                        if (row.RowNumber() > 1)
                        {
                            string name = "";
                            string surname = "";

                            SemesterResults semesterResults = GetSemesterResults(ref name, ref surname, teacher, studyGroup, row);

                            Student student = FindStudent(name, surname, students);
                            student.SemesterResults = semesterResults;
                        }
                    }
                }
            };
        }

        private static void ReadAdmissionFile(string filePath, ObservableCollection<Student> students)
        {
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
        }

        private static SemesterResults GetSemesterResults(ref string name, ref string surname, string teacher, string studyGroup, IXLRow row)
        {
            double[] activities = new double[12];
            bool[] attendance = new bool[13];
            ReadAttendance(ref activities, ref attendance, row);
            double activitiesSum = row.Cell(17).GetValue<double?>() != null ? row.Cell(17).GetDouble() : 0;
            double firstTest = row.Cell(18).GetValue<double?>() != null ? row.Cell(18).GetDouble() : 0;
            double secondTest = row.Cell(19).GetValue<double?>() != null ? row.Cell(19).GetDouble() : 0;
            double testSum = row.Cell(20).GetValue<double?>() != null ? row.Cell(20).GetDouble() : 0;
            double semesterSum = row.Cell(21).GetValue<double?>() != null ? row.Cell(21).GetDouble() : 0;
            double missingPoints = row.Cell(22).GetValue<double?>() != null ? row.Cell(22).GetDouble() : 0;
            double? writtenTestForm = row.Cell(23).GetValue<double?>();
            double? oralTestForm = row.Cell(24).GetValue<double?>();
            double totalPoints = row.Cell(25).GetValue<double?>() != null ? row.Cell(25).GetDouble() : 0;
            char grade = row.Cell(26).GetString().Equals(":)") ? 'F' : char.Parse(row.Cell(26).GetString());

            if (!row.Cell(2).IsEmpty())
            {
                studyGroup = row.Cell(2).GetString();
            }

            string[] fullName = row.Cell(3).GetString().Split(" ");
            for (int i = 0; i < fullName.Length; ++i)
            {
                if (i == 0)
                {
                    surname = fullName[i];
                }
                else if (i == 1)
                {
                    name = fullName[i];
                }
                else
                {
                    name += $" {fullName[i]}";
                }
            }

            SemesterResults semesterResults = new SemesterResults
            (
                studyGroup, firstTest, secondTest, writtenTestForm, oralTestForm, testSum, semesterSum,
                missingPoints, totalPoints, grade, teacher, activitiesSum, activities, attendance
            );

            return semesterResults;
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

        private static void ReadAttendance(ref double[] activities, ref bool[] attendance, IXLRow row)
        {
            for (int i = 0; i < 13; ++i)
            {
                var backgroundColor = row.Cell(i + 4).Style.Fill.BackgroundColor;
                bool isAbsentColor = backgroundColor.ColorType == XLColorType.Color ? IsAbsentColor(backgroundColor, row.Cell(i + 3)) : IsAbsentTheme(backgroundColor);

                if (row.Cell(i + 3).GetValue<string>().Equals("n") || isAbsentColor)
                {
                    attendance[i] = false;
                }
                else
                {
                    attendance[i] = true;

                    if (i < 12)
                    {
                        activities[i] = row.Cell(i + 4).GetValue<double?>() != null ? row.Cell(i + 4).GetDouble() : 0;
                    }
                }
            }
        }

        private static bool IsAbsentColor(XLColor color, IXLCell cell)
        {
            return cell.IsEmpty() || color == XLColor.White || color == XLColor.Red;
        }

        private static bool IsAbsentTheme(XLColor color)
        {
            return color.ThemeColor == XLThemeColor.Background1 || color.ThemeColor == XLThemeColor.Accent4;
        }

        private static Student FindStudent(string name, string surname, ObservableCollection<Student> students)
        {
            return students.Where(x => x.Name.Equals(name) && x.Surname.Equals(surname)).ElementAt(0);
        }

        private static double FormatNumber(string number)
        {
            string formatedNumber = number.Replace('.', ',');

            return double.Parse(formatedNumber);
        }

    }
}
