using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using StudentStatistics.Commands;
using StudentStatistics.Models;
using StudentStatistics.Services;
using StudentStatistics.Views;

namespace StudentStatistics.ViewModels
{
    public class SuccessRateViewModel : INotifyPropertyChanged
    {
        private readonly Router _router;
        private string? _semesterFilePath;
        private string? _admissionProcessFilePath;
        private ObservableCollection<Student>? _loadedAllStudents;
        private ObservableCollection<Student> _loadedSuccessfullStudents;
        private ObservableCollection<Student> _loadedUnsuccessfullStudents;

        public AppCloser AppCloser { get; private set; }
        public ICommand NewFileGridTrigger { get; private set; }
        public ICommand RemoveFilesTrigger { get; private set; }
        public ICommand RemoveFileTrigger { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand DetailTrigger { get; private set; }

        private bool _successfullStudentsSelected;
        public bool SuccessfullStudentsSelected
        {
            get => _successfullStudentsSelected;
            set
            {
                _successfullStudentsSelected = value;
                OnPropertyChanged(nameof(SuccessfullStudentsSelected));
            }
        }

        private bool _unsuccessfullStudentsSelected;
        public bool UnsuccessfullStudentsSelected
        {
            get => _unsuccessfullStudentsSelected;
            set
            {
                _unsuccessfullStudentsSelected = value;
                OnPropertyChanged(nameof(UnsuccessfullStudentsSelected));
            }
        }

        private bool _allStudentsSelected;
        public bool AllStudentsSelected
        {
            get => _allStudentsSelected;
            set
            {
                _allStudentsSelected = value;
                OnPropertyChanged(nameof(AllStudentsSelected));
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

        private IEnumerable<ISeries>? _stuudentSeries;
        public IEnumerable<ISeries>? StudentSeries
        {
            get => _stuudentSeries;
            set
            {
                _stuudentSeries = value;
                OnPropertyChanged(nameof(StudentSeries));
            }
        }

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

        private ObservableCollection<Student> _successfullStudents;
        public ObservableCollection<Student> SuccessfullStudents
        {
            get => _successfullStudents;
            set
            {
                _successfullStudents = value;
                OnPropertyChanged(nameof(SuccessfullStudents));
            }
        }

        private ObservableCollection<Student> _unsuccessfullStudents;
        public ObservableCollection<Student> UnsuccessfullStudents
        {
            get => _unsuccessfullStudents;
            set
            {
                _unsuccessfullStudents = value;
                OnPropertyChanged(nameof(UnsuccessfullStudents));
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

        private Visibility _wrongFileVisibility;
        public Visibility WrongFileVisibility
        {
            get => _wrongFileVisibility;
            set
            {
                _wrongFileVisibility = value;
                OnPropertyChanged(nameof(WrongFileVisibility));
            }
        }

        private Visibility _removeSemesterFileVisibility;
        public Visibility RemoveSemesterFileVisibility
        {
            get => _removeSemesterFileVisibility;
            set
            {
                _removeSemesterFileVisibility = value;
                OnPropertyChanged(nameof(RemoveSemesterFileVisibility));
            }
        }

        private Visibility _removeAdmissionFileVisibility;
        public Visibility RemoveAdmissionFileVisibility
        {
            get => _removeAdmissionFileVisibility;
            set
            {
                _removeAdmissionFileVisibility = value;
                OnPropertyChanged(nameof(RemoveAdmissionFileVisibility));
            }
        }

        private Visibility _statisticsVisbility;
        public Visibility StatisticsVisibility
        {
            get => _statisticsVisbility;
            set
            {
                _statisticsVisbility = value;
                OnPropertyChanged(nameof(StatisticsVisibility));
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
            _wrongFileVisibility = Visibility.Collapsed;
            _removeSemesterFileVisibility = Visibility.Collapsed;
            _removeAdmissionFileVisibility = Visibility.Collapsed;
            _statisticsVisbility = Visibility.Hidden;
            _searchText = "";
            _loadedSuccessfullStudents = new ObservableCollection<Student>();
            _loadedUnsuccessfullStudents = new ObservableCollection<Student>();
            _students = new ObservableCollection<Student>();
            _successfullStudents = _loadedSuccessfullStudents;
            _unsuccessfullStudents = _loadedUnsuccessfullStudents;

            AppCloser = new AppCloser();
            SuccessfullStudentsSelected = true;
            Students = new ObservableCollection<Student>();
            NewFileGridTrigger = new RelayCommand(ToggleNewFileGrid);
            OpenFileCommand = new RelayCommand<string>(OpenFile);
            RemoveFileTrigger = new RelayCommand<string>(RemoveFile);
            RemoveFilesTrigger = new RelayCommand(RemoveFiles);
            SearchCommand = new RelayCommand(SearchStudent);
            DetailTrigger = new RelayCommand(NavigateToDetailStatistics);
        }

        private void OpenFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePath = FileHandler.OpenFile();

                if (_semesterFilePath != null)
                {
                    RemoveSemesterFileVisibility = Visibility.Visible;
                }
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = FileHandler.OpenFile();

                if (_admissionProcessFilePath != null)
                {
                    RemoveAdmissionFileVisibility = Visibility.Visible;
                }
            }

            SetOpenFileGridVisibility();

            if (_semesterFilePath != null && _admissionProcessFilePath != null)
            {
                _loadedAllStudents = DataReader.ReadData(_semesterFilePath, _admissionProcessFilePath);

                if (_loadedAllStudents != null)
                {
                    _loadedSuccessfullStudents = StudentSorter.SuccessfullStudents(_loadedAllStudents);
                    _loadedUnsuccessfullStudents = StudentSorter.UnsuccessfullStudents(_loadedAllStudents);

                    Students = _loadedAllStudents;
                    SuccessfullStudents = _loadedSuccessfullStudents;
                    UnsuccessfullStudents = _loadedUnsuccessfullStudents;

                    RemoveFilesVisibility = Visibility.Visible;
                    RemoveAdmissionFileVisibility = Visibility.Visible;
                    RemoveSemesterFileVisibility = Visibility.Visible;
                    StatisticsVisibility = Visibility.Visible;

                    UpdatePieChart();
                }
                else
                {
                    FileErrorInfo();
                }
            }
        }

        private void RemoveFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePath = null;

                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
                RemoveSemesterFileVisibility = Visibility.Collapsed;
                SemesterButtonVisibility = Visibility.Visible;
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = null;

                CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
                RemoveAdmissionFileVisibility= Visibility.Collapsed;
                AdmissionButtonVisibility = Visibility.Visible;
            }
        }

        private void RemoveFiles()
        {
            _semesterFilePath = null;
            _admissionProcessFilePath = null;

            SuccessfullStudents.Clear();
            UnsuccessfullStudents.Clear();
            Students.Clear();

            StudentSeries = null;

            RemoveFilesVisibility = Visibility.Hidden;
            StatisticsVisibility = Visibility.Hidden;
            SetOpenFileGridVisibility();
        }

        private async void FileErrorInfo()
        {
            RemoveFiles();

            SemesterButtonVisibility = Visibility.Collapsed;
            AdmissionButtonVisibility = Visibility.Collapsed;
            WrongFileVisibility = Visibility.Visible;

            await Task.Delay(3000);

            WrongFileVisibility = Visibility.Collapsed;
            
            SetOpenFileGridVisibility();
        }

        private void UpdatePieChart()
        {
            StudentSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { SuccessfullStudents.Count }, Name = "Úspešní študenti" },
                new PieSeries<int> { Values = new int[] { UnsuccessfullStudents.Count }, Name = "Neúspešní študenti" }
            };
        }

        public void SearchStudent()
        {
            if (SuccessfullStudentsSelected)
            {
                SuccessfullStudents = StudentSorter.SearchStudent(SearchText, _loadedSuccessfullStudents);
            }
            else if (UnsuccessfullStudentsSelected)
            {
                UnsuccessfullStudents = StudentSorter.SearchStudent(SearchText, _loadedUnsuccessfullStudents);
            }
            else if (_loadedAllStudents != null)
            {
                Students = StudentSorter.SearchStudent(SearchText, _loadedAllStudents);
            }
        }

        private void SetOpenFileGridVisibility()
        {
            if (_semesterFilePath != null)
            {
                SemesterButtonVisibility = Visibility.Collapsed;
                CheckmarkSemesterFileVisibility = Visibility.Visible;
            }
            else
            {
                SemesterButtonVisibility = Visibility.Visible;
                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
                RemoveSemesterFileVisibility = Visibility.Collapsed;
            }

            if (_admissionProcessFilePath != null)
            {
                AdmissionButtonVisibility = Visibility.Collapsed;
                CheckmarkAdmissionFileVisibility = Visibility.Visible;
            }
            else
            {
                AdmissionButtonVisibility = Visibility.Visible;
                CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
                RemoveAdmissionFileVisibility = Visibility.Collapsed;
            }
        }

        private void ToggleNewFileGrid()
        {
            NewFileGridVisibility = NewFileGridVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        private void NavigateToDetailStatistics()
        {
            if (_loadedAllStudents != null)
            {
                _router.CurrentView = new StudentsStatisticsView(_router, _loadedAllStudents);
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
