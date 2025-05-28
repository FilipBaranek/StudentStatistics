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
            var unsuccessfullStudents = students.Where(x => (x.SemesterResults != null && x.SemesterResults.Grade.Equals('F')));

            return new ObservableCollection<Student>(unsuccessfullStudents);
        }

        public static ObservableCollection<Student> HasSchoolTypeStudents(string schoolType, ObservableCollection<Student> students)
        {
            var wantedSchoolTypeStudents = students.Where(x => (x.AdmissionProcess._highSchoolType.Equals(schoolType)));

            return new ObservableCollection<Student>(wantedSchoolTypeStudents);
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

        public static ObservableCollection<Student> DidNotGetToExam(ObservableCollection<Student> students)
        {
            var studentsWithoutExam = students.Where(x => (x.SemesterResults != null && (x.SemesterResults.MissingPoints > 0)));

            return new ObservableCollection<Student>(studentsWithoutExam);
        }

        public static ObservableCollection<Student> DidGetToExam(ObservableCollection<Student> students)
        {
            var studentsWithoutExam = students.Where(x => (x.SemesterResults != null && (x.SemesterResults.MissingPoints <= 0)));

            return new ObservableCollection<Student>(studentsWithoutExam);
        }

        public static ObservableCollection<Student> StudentsWithTeacher(string teacherName,  ObservableCollection<Student> students)
        {
            var studentsOfSelectedTeacher = students.Where(x => (x.SemesterResults != null && x.SemesterResults.Teacher.Equals(teacherName)));

            return new ObservableCollection<Student>(studentsOfSelectedTeacher);
        }

        public static ObservableCollection<Student> HasGrade(char grade, ObservableCollection<Student> students)
        {
            var studentsWithWantedGrade = students.Where(x => (x.SemesterResults != null && x.SemesterResults.Grade.Equals(grade)));

            return new ObservableCollection<Student>(studentsWithWantedGrade);
        }

        public static ObservableCollection<Student> StudentsStoppedComing(ObservableCollection<Student> students)
        {
            var studentsStoppedComing = students.Where(x => (StoppedComing(x)));

            return new ObservableCollection<Student>(studentsStoppedComing);
        }

        public static ObservableCollection<Student> StudentsKeptComing(ObservableCollection<Student> students)
        {
            var studentsStoppedComing = students.Where(x => (!StoppedComing(x)));

            return new ObservableCollection<Student>(studentsStoppedComing);
        }

        public static bool StoppedComing(Student student)
        {
            if (student.SemesterResults != null)
            {
                int absent = 0;

                for (int i = 12; i >= 0; --i)
                {
                    if (!student.SemesterResults.Attendance[i])
                    {
                        ++absent;
                    }
                    else
                    {
                        break;
                    }
                }

                if (absent > 3)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
