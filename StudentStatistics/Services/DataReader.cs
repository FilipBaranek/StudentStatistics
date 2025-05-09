using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using StudentStatistics.Models;

namespace StudentStatistics.Services
{
    public static class DataReader
    {
        private static int WEEKS_COUNT = 13; 

        public static ObservableCollection<Student>? ReadData(string semesterFilePath, string admissionFilePath)
        {
            var students = new ObservableCollection<Student>();
            bool successfulyRead;

            if (!(successfulyRead = ReadAdmisionData(admissionFilePath, students)))
            {
                return null;
            }
            if (!(ReadSemesterData(semesterFilePath, students)))
            {
                return null;
            }

            return students;
        }

        private static bool ReadSemesterData(string filePath, ObservableCollection<Student> students)
        {
            try
            {
                StreamReader semesterReader = new StreamReader(filePath);
                string? line = semesterReader.ReadLine();
                string studyGroup = "";

                if (line != null)
                {
                    studyGroup = GetStudyGroup(line);
                }

                while ((line = semesterReader.ReadLine()) != null)
                {
                    string[] data = line.Split(';');

                    if (data[0].Equals(""))
                    {
                        break;
                    }

                    string name = "";
                    string surname = "";

                    SemesterResults semesterResults = GetSemesterResults(ref name, ref surname, studyGroup, data);

                    Student student = FindStudent(name, surname, students);
                    student.SemesterResults = semesterResults;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool ReadAdmisionData(string filePath, ObservableCollection<Student> students)
        {
            try
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

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static SemesterResults GetSemesterResults(ref string name, ref string surname, string studyGroup, string[] data)
        {
            double[] activities = new double[WEEKS_COUNT - 1];
            bool[] attendance = new bool[WEEKS_COUNT];
            InitializeAttendance(ref activities, ref attendance, data);
            double activitiesSum = double.Parse(data[16]);
            double firstTest = !data[17].Equals("") ? double.Parse(data[17]) : 0;
            double secondTest = !data[18].Equals("") ? double.Parse(data[18]) : 0;
            double testSum = double.Parse(data[19]);
            double semesterSum = double.Parse(data[20]);
            double missingPoints = !data[21].Equals("") ? double.Parse(data[21]) : 0;
            double writtenTestForm = !data[22].Equals("") ? double.Parse(data[22]) : 0;
            double? oralTestForm = !data[23].Equals("") ? double.Parse(data[23]) : null;
            double totalPoints = double.Parse(data[24]);
            char grade = data[25].Equals(":)") ? 'F' : char.Parse(data[25]);

            if (!data[1].Equals(""))
            {
                studyGroup = data[1];
            }

            string[] fullName = data[2].Split(" ");
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
                missingPoints, totalPoints, grade, activitiesSum, activities, attendance
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

        private static string GetStudyGroup(string line)
        {
            string[] lineData = line.Split(";");

            return lineData[2];
        }

        private static void InitializeAttendance(ref double[] activities, ref bool[] attendance, string[] data)
        {
            for (int i = 0; i < WEEKS_COUNT; ++i)
            {
                if (i == WEEKS_COUNT - 1 && !data[i + 3].Equals(""))
                {
                    attendance[i] = true;
                }
                else if (!data[i + 3].Equals(""))
                {
                    attendance[i] = true;
                    activities[i] = double.Parse(data[i + 3]);
                }
                else
                {
                    attendance[i] = false;
                }
            }
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
