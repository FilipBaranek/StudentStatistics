using System.Windows;
using StudentStatistics.ViewModels;


namespace StudentStatistics
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}