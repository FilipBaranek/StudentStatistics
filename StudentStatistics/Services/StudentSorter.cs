using System.Collections.ObjectModel;
using StudentStatistics.Models;

namespace StudentStatistics.Services
{
    public static class StudentSorter
    {
        public static Student? FindSelectedStudent(ObservableCollection<Student> students)
        {
            try
            {
                var student = students.Where(x => (x.IsSelected));

                return student.ElementAt(0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Student> SearchStudent(string searchText, ObservableCollection<Student> students)
        {
            var sortedStudents = students.Where(x => ($"{x.Name} {x.Surname}").Contains(searchText));

            return new ObservableCollection<Student>(sortedStudents);
        }

        public static ObservableCollection<Student> SuccessfullStudents(ObservableCollection<Student> students)
        {
            var successfullStudents = students.Where(x => (x.SemesterResults != null && !x.SemesterResults.Grade.Equals('F')));

            return new ObservableCollection<Student>(successfullStudents);
        }

        public static ObservableCollection<Student> UnsuccessfullStudents(ObservableCollection<Student> students)
        {
            var unsuccessfullStudents = students.Where(x => (x.SemesterResults != null) && x.SemesterResults.Grade.Equals('F'));

            return new ObservableCollection<Student>(unsuccessfullStudents);
        }

        public static ObservableCollection<Student> GrammarSchoolStudents(ObservableCollection<Student> students)
        {
            var gramarSchoolStudents = students.Where(x => (x.AdmissionProcess.HighSchoolType.Equals("gymnázium")));

            return new ObservableCollection<Student>(gramarSchoolStudents);
        }

        public static ObservableCollection<Student> VocationalSchoolStudents(ObservableCollection<Student> students)
        {
            var vocationalSchoolStudents = students.Where(x => (!x.AdmissionProcess.HighSchoolType.Equals("gymnázium")));

            return new ObservableCollection<Student>(vocationalSchoolStudents);
        }

        public static ObservableCollection<Student> StudentsWithoutEntranceExam(ObservableCollection<Student> students)
        {
            var studentsWithoutEntranceExam = students.Where(x => (x.AdmissionProcess.AdmissionsProcessResult.Equals("prijatý bez prijímacej skúšky")));

            return new ObservableCollection<Student>(studentsWithoutEntranceExam);
        }

        public static ObservableCollection<Student> StudentsWithEntranceExam(ObservableCollection<Student> students)
        {
            var studentsWithEntranceExam = students.Where(x => (x.AdmissionProcess.AdmissionsProcessResult.Equals("prijatý")));

            return new ObservableCollection<Student>(studentsWithEntranceExam);
        }

        public static ObservableCollection<Student> SlovakNationalityStudents(ObservableCollection<Student> students)
        {
            var slovakStudents = students.Where(x => (x.Nationality.Equals("Slovensko")));

            return new ObservableCollection<Student>(slovakStudents);
        }

        public static ObservableCollection<Student> OtherNationalityStudents(ObservableCollection<Student> students)
        {
            var otherNationalityStudents = students.Where(x => (!x.Nationality.Equals("Slovensko")));

            return new ObservableCollection<Student>(otherNationalityStudents);
        }

        public static ObservableCollection<Student> StudentsParticipatedInCompetitions(ObservableCollection<Student> students)
        {
            var studentsParticipated = students.Where(x => (x.AdmissionProcess.HighSchoolCompetetions != null));

            return new ObservableCollection<Student>(studentsParticipated);
        }

        public static ObservableCollection<Student> StudentsNotParticipatedInCompetitions(ObservableCollection<Student> students)
        {
            var studentsNotParticipated = students.Where(x => (x.AdmissionProcess.HighSchoolCompetetions == null));

            return new ObservableCollection<Student>(studentsNotParticipated);
        }

        public static ObservableCollection<Student> MathGraduates(ObservableCollection<Student> students)
        {
            var mathGraduates = students.Where(x => (x.AdmissionProcess.MathPoints != null));

            return new ObservableCollection<Student>(mathGraduates);
        }

        public static ObservableCollection<Student> StudentsWithoutMathGraduation(ObservableCollection<Student> students)
        {
            var regularGraduates = students.Where(x => (x.AdmissionProcess.MathPoints == null));

            return new ObservableCollection<Student>(regularGraduates);
        }
    }
}
