using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using StudentStatistics.Commands;
using StudentStatistics.Models;
using StudentStatistics.Services;

namespace StudentStatistics.ViewModels
{
    public class StudentInfoViewModel : INotifyPropertyChanged
    {
        private readonly Router _router;

        public string Attendance { get; private set; }
        public string Competitions { get; private set; }
        public Student Student { get; private set; }
        public AppCloser AppCloser { get; private set; }
        public ICommand GoBackTrigger { get; private set; }
        public ICommand NextPageTrigger { get; private set; }
        public ICommand PreviousPageTrigger {  get; private set; }

        private Visibility _firstPageVisibility;
        public Visibility FirstPageVisibility
        {
            get => _firstPageVisibility;
            set
            {
                _firstPageVisibility = value;
                OnPropertyChanged(nameof(FirstPageVisibility));
            }
        }

        private Visibility _nextPageButtonVisibility;
        public Visibility NextPageButtonVisibility
        {
            get => _nextPageButtonVisibility;
            set
            {
                _nextPageButtonVisibility = value;
                OnPropertyChanged(nameof(NextPageButtonVisibility));
            }
        }

        private Visibility _secondPageVisibility;
        public Visibility SecondPageVisibility
        {
            get => _secondPageVisibility;
            set
            {
                _secondPageVisibility = value;
                OnPropertyChanged(nameof(SecondPageVisibility));
            }
        }

        private Visibility _previousePageVisbility;
        public Visibility PreviousPageButtonVisibility
        {
            get => _previousePageVisbility;
            set
            {
                _previousePageVisbility = value;
                OnPropertyChanged(nameof(PreviousPageButtonVisibility));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public StudentInfoViewModel(Router router, Student student)
        {
            _router = router;
            _firstPageVisibility = Visibility.Visible;
            _nextPageButtonVisibility = Visibility.Visible;
            _secondPageVisibility = Visibility.Collapsed;
            _previousePageVisbility = Visibility.Collapsed;

            Student = student;
            Attendance = StudentSorter.StoppedComing(student) ? "Prestal chodiť" : "Aktívne chodil";
            Competitions = student.AdmissionProcess.HighSchoolCompetetions != null ? "Áno" : "Nie";

            AppCloser = new AppCloser();
            GoBackTrigger = new RelayCommand(GoBack);
            NextPageTrigger = new RelayCommand(NextPage);
            PreviousPageTrigger = new RelayCommand(PreviousPage);
        }

        private void NextPage()
        {
            FirstPageVisibility = Visibility.Collapsed;
            NextPageButtonVisibility = Visibility.Collapsed;
            SecondPageVisibility = Visibility.Visible;
            PreviousPageButtonVisibility = Visibility.Visible;
        }

        private void PreviousPage()
        {
            FirstPageVisibility = Visibility.Visible;
            NextPageButtonVisibility = Visibility.Visible;
            SecondPageVisibility = Visibility.Collapsed;
            PreviousPageButtonVisibility = Visibility.Collapsed;
        }

        private void GoBack()
        {
            _router.NavigateBack();
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
