using System.Collections.ObjectModel;
using System.Windows.Controls;
using StudentStatistics.Models;
using StudentStatistics.Services;
using StudentStatistics.ViewModels;

namespace StudentStatistics.Views
{
    public partial class StudentsStatisticsView : UserControl
    {
        public StudentsStatisticsView(Router router, ObservableCollection<Student> students)
        {
            InitializeComponent();

            DataContext = new StudentsStatisticsViewModel(router, students);
        }
    }
}
