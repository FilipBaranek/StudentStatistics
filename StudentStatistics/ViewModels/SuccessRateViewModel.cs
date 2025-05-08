using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using StudentStatistics.Commands;
using StudentStatistics.Models;
using StudentStatistics.Services;

namespace StudentStatistics.ViewModels
{
    public class SuccessRateViewModel : INotifyPropertyChanged
    {
        private readonly Router _router;
        private string? _semesterFilePath;
        private string? _admissionProcessFilePath;

        public ICommand NewFileGridTrigger { get; private set; }
        public ICommand RemoveFilesTrigger { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        private ObservableCollection<Student> _actualStudents;
        public ObservableCollection<Student> ActualStudents
        {
            get => _actualStudents;
            set
            {
                _actualStudents = value;
                OnPropertyChanged(nameof(ActualStudents));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private Visibility _newFileGridVisibility;
        public Visibility NewFileGridVisibility
        {
            get => _newFileGridVisibility;
            set
            {
                _newFileGridVisibility = value;
                OnPropertyChanged(nameof(NewFileGridVisibility));
            }
        }

        private Visibility _checkmarkSemesterFileVisibility;
        public Visibility CheckmarkSemesterFileVisibility
        {
            get => _checkmarkSemesterFileVisibility;
            set
            {
                _checkmarkSemesterFileVisibility = value;
                OnPropertyChanged(nameof(CheckmarkSemesterFileVisibility));
            }
        }

        private Visibility _checkmarkAdmissionFileVisibility;
        public Visibility CheckmarkAdmissionFileVisibility
        {
            get => _checkmarkAdmissionFileVisibility;
            set
            {
                _checkmarkAdmissionFileVisibility = value;
                OnPropertyChanged(nameof(CheckmarkAdmissionFileVisibility));
            }
        }

        private Visibility _semesterButtonVisibility;
        public Visibility SemesterButtonVisibility
        {
            get => _semesterButtonVisibility;
            set
            {
                _semesterButtonVisibility = value;
                OnPropertyChanged(nameof(SemesterButtonVisibility));
            }
        }

        private Visibility _admissionButtonVisibility;
        public Visibility AdmissionButtonVisibility
        {
            get => _admissionButtonVisibility;
            set
            {
                _admissionButtonVisibility = value;
                OnPropertyChanged(nameof(AdmissionButtonVisibility));
            }
        }

        private Visibility _removeFilesVisibility;
        public Visibility RemoveFilesVisibility
        {
            get => _removeFilesVisibility;
            set
            {
                _removeFilesVisibility = value;
                OnPropertyChanged(nameof(RemoveFilesVisibility));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SuccessRateViewModel(Router router)
        {
            _router = router;
            _newFileGridVisibility = Visibility.Hidden;
            _checkmarkSemesterFileVisibility = Visibility.Collapsed;
            _checkmarkAdmissionFileVisibility = Visibility.Collapsed;
            _semesterButtonVisibility = Visibility.Visible;
            _admissionButtonVisibility = Visibility.Visible;
            _removeFilesVisibility = Visibility.Hidden;
            _students = new ObservableCollection<Student>();
            _actualStudents = new ObservableCollection<Student>();
            _searchText = "";

            NewFileGridTrigger = new RelayCommand(ToggleNewFileGrid);
            OpenFileCommand = new RelayCommand<string>(OpenFile);
            RemoveFilesTrigger = new RelayCommand(RemoveFiles);
            SearchCommand = new RelayCommand(SearchStudent);
        }

        private void OpenFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePath = FileHandler.OpenFile();
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = FileHandler.OpenFile();
            }

            if (_semesterFilePath != null)
            {
                SemesterButtonVisibility = Visibility.Collapsed;
                CheckmarkSemesterFileVisibility = Visibility.Visible;
            }
            if (_admissionProcessFilePath != null)
            {
                AdmissionButtonVisibility = Visibility.Collapsed;
                CheckmarkAdmissionFileVisibility= Visibility.Visible;
            }

            if (_semesterFilePath != null && _admissionProcessFilePath != null)
            {
                Students = DataReader.ReadData(_semesterFilePath);
                ActualStudents = Students;
                RemoveFilesVisibility = Visibility.Visible;
            }
        }

        private void RemoveFiles()
        {
            Students.Clear();
            ActualStudents.Clear();
            //todo obidve skupiny

            SemesterButtonVisibility = Visibility.Visible;
            AdmissionButtonVisibility= Visibility.Visible;
            CheckmarkAdmissionFileVisibility = Visibility.Hidden;
            CheckmarkSemesterFileVisibility = Visibility.Hidden;
            RemoveFilesVisibility = Visibility.Hidden;
        }

        public void SearchStudent()
        {
            ActualStudents = StudentSorter.SearchStudent(SearchText, Students);
        }

        private void ToggleNewFileGrid()
        {
            NewFileGridVisibility = NewFileGridVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
