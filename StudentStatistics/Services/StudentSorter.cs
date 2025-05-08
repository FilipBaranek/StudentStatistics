using System.Collections.ObjectModel;
using StudentStatistics.Models;

namespace StudentStatistics.Services
{
    public static class StudentSorter
    {
        public static ObservableCollection<Student> SearchStudent(string searchText, ObservableCollection<Student> students)
        {
            var sortedStudents = students.Where(x => ($"{x.Name} {x.Surname}").Contains(searchText));

            return new ObservableCollection<Student>(sortedStudents);
        }
    }
}
