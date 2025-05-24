using System.Windows.Controls;
using StudentStatistics.Models;
using StudentStatistics.Services;
using StudentStatistics.ViewModels;

namespace StudentStatistics.Views
{
    public partial class StudentInfoView : UserControl
    {
        public StudentInfoView(Router router, Student student)
        {
            InitializeComponent();

            DataContext = new StudentInfoViewModel(router, student);
        }
    }
}
