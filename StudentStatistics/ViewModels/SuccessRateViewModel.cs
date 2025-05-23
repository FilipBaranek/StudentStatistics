using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using StudentStatistics.Commands;
using StudentStatistics.Models;
using StudentStatistics.Services;
using StudentStatistics.Views;

namespace StudentStatistics.ViewModels
{
    public class SuccessRateViewModel : INotifyPropertyChanged
    {
        private readonly Router _router;
        private string? _teacher;
        private string? _semesterFilePath;
        private string? _admissionProcessFilePath;
        private List<string?> _semesterFilePaths;
        private ObservableCollection<Student>? _loadedAllStudents;
        private ObservableCollection<Student> _loadedSuccessfullStudents;
        private ObservableCollection<Student> _loadedUnsuccessfullStudents;

        public AppCloser AppCloser { get; private set; }
        public SolidColorPaint LegendTextColor { get; private set; }
        public ICommand NewFileGridTrigger { get; private set; }
        public ICommand SelectTeacherTrigger { get; private set; }
        public ICommand RemoveFilesTrigger { get; private set; }
        public ICommand RemoveFileTrigger { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand OpenMultipleFilesTrigger { get; private set; }
        public ICommand SearchTrigger { get; private set; }
        public ICommand DetailTrigger { get; private set; }
        public ICommand DeleteStudentTrigger { get; private set; }
        public ICommand EditStudentTrigger {  get; private set; }
        public ICommand ConfirmFileOpenTrigger { get; private set; }

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

        private Student? _studentToEdit;
        public Student? StudentToEdit
        {
            get => _studentToEdit;
            set
            {
                _studentToEdit = value;
                OnPropertyChanged(nameof(StudentToEdit));
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

        private Visibility _teacherWindowVisibility;
        public Visibility TeacherWindowVisibility
        {
            get => _teacherWindowVisibility;
            set
            {
                _teacherWindowVisibility = value;
                OnPropertyChanged(nameof(TeacherWindowVisibility));
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

        private Visibility _editStudentVisibility;
        public Visibility EditStudentVisibility
        {
            get => _editStudentVisibility;
            set
            {
                _editStudentVisibility = value;
                OnPropertyChanged(nameof(EditStudentVisibility));
            }
        }

        private Visibility _confirmFilesVisibility;
        public Visibility ConfirmFilesVisibility
        {
            get => _confirmFilesVisibility;
            set
            {
                _confirmFilesVisibility = value;
                OnPropertyChanged(nameof(ConfirmFilesVisibility));
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
            _editStudentVisibility = Visibility.Collapsed;
            _teacherWindowVisibility = Visibility.Hidden;
            _confirmFilesVisibility = Visibility.Visible;
            _searchText = "";
            _semesterFilePaths = new List<string?>();
            _loadedSuccessfullStudents = new ObservableCollection<Student>();
            _loadedUnsuccessfullStudents = new ObservableCollection<Student>();
            _students = new ObservableCollection<Student>();
            _successfullStudents = _loadedSuccessfullStudents;
            _unsuccessfullStudents = _loadedUnsuccessfullStudents;

            AppCloser = new AppCloser();
            LegendTextColor = new SolidColorPaint { Color = new SKColor(255, 179, 0) };
            SuccessfullStudentsSelected = true;
            Students = new ObservableCollection<Student>();
            NewFileGridTrigger = new RelayCommand(ToggleNewFileGrid);
            OpenFileCommand = new RelayCommand<string>(OpenFile);
            RemoveFileTrigger = new RelayCommand<string>(RemoveFile);
            RemoveFilesTrigger = new RelayCommand(RemoveFiles);
            SearchTrigger = new RelayCommand(SearchStudent);
            DetailTrigger = new RelayCommand(NavigateToDetailStatistics);
            DeleteStudentTrigger = new RelayCommand(RemoveStudent);
            EditStudentTrigger = new RelayCommand(ToggleEditStudent);
            SelectTeacherTrigger = new RelayCommand<string>(SelectTeacher);
            ConfirmFileOpenTrigger = new RelayCommand(ConfirmFileOpen);
            OpenMultipleFilesTrigger = new RelayCommand<string>(OpenMultipleFile);
        }

        private void OpenFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePath = FileHandler.OpenExcelFile();

                if (_semesterFilePath != null)
                {
                    RemoveSemesterFileVisibility = Visibility.Visible;
                }
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = FileHandler.OpenCsvFile();

                if (_admissionProcessFilePath != null)
                {
                    RemoveAdmissionFileVisibility = Visibility.Visible;
                }
            }

            SetOpenFileGridVisibility();
        }

        private async void OpenMultipleFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePaths.Add(FileHandler.OpenExcelFile());

                if (_semesterFilePath != null)
                {
                    RemoveSemesterFileVisibility = Visibility.Visible;
                }

                SemesterButtonVisibility = Visibility.Collapsed;
                CheckmarkSemesterFileVisibility = Visibility.Visible;

                await Task.Delay(1000);

                SemesterButtonVisibility = Visibility.Visible;
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = FileHandler.OpenCsvFile();

                if (_admissionProcessFilePath != null)
                {
                    RemoveAdmissionFileVisibility = Visibility.Visible;
                    AdmissionButtonVisibility = Visibility.Collapsed;
                }
            }
        }

        private void ConfirmFileOpen()
        {
            if (_admissionProcessFilePath != null && _semesterFilePath != null)
            {
                ConfirmFilesVisibility = Visibility.Collapsed;
                TeacherWindowVisibility = Visibility.Visible;
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
                ConfirmFilesVisibility = Visibility.Visible;
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = null;

                CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
                RemoveAdmissionFileVisibility= Visibility.Collapsed;
                AdmissionButtonVisibility = Visibility.Visible;
                ConfirmFilesVisibility = Visibility.Visible;
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
            ConfirmFilesVisibility= Visibility.Visible;
            SetOpenFileGridVisibility();
        }

        private async void FileErrorInfo()
        {
            RemoveFiles();

            SemesterButtonVisibility = Visibility.Collapsed;
            AdmissionButtonVisibility = Visibility.Collapsed;
            WrongFileVisibility = Visibility.Visible;

            await Task.Delay(5000);

            WrongFileVisibility = Visibility.Collapsed;
            
            SetOpenFileGridVisibility();
        }

        private void SelectTeacher(string teacher)
        {
            _teacher = teacher;

            LoadStudents(_teacher);
            TeacherWindowVisibility = Visibility.Hidden;

            _teacher = null;
        }

        private void LoadStudents(string teacher)
        {
            if (_semesterFilePath != null && _admissionProcessFilePath != null)
            {
                _loadedAllStudents = DataReader.ReadData(_semesterFilePath, _admissionProcessFilePath, teacher);
                _admissionProcessFilePath = null;
                _semesterFilePath = null;

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

        private void UpdatePieChart()
        {
            StudentSeries = new ISeries[]
            {
                new PieSeries<int> 
                {
                    Values = new int[] { SuccessfullStudents.Count },
                    Name = "Úspešní študenti",
                    Fill = new SolidColorPaint(new SKColor(255, 248, 77)),
                    Stroke = new SolidColorPaint(SKColors.Black)
                },
                new PieSeries<int> 
                { 
                    Values = new int[] { UnsuccessfullStudents.Count },
                    Name = "Neúspešní študenti",
                    Fill = new SolidColorPaint(SKColors.YellowGreen),
                    Stroke = new SolidColorPaint(SKColors.Black)
                }
            };
        }

        private void SearchStudent()
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

        private Student? FindSelectedStudent(ref ObservableCollection<Student> selectedStudentTab)
        {
            if (SuccessfullStudentsSelected)
            {
                selectedStudentTab = SuccessfullStudents;
                return StudentSorter.FindSelectedStudent(SuccessfullStudents);
            }
            else if (UnsuccessfullStudentsSelected)
            {
                selectedStudentTab = UnsuccessfullStudents;
                return StudentSorter.FindSelectedStudent(UnsuccessfullStudents);
            }
            else if (AllStudentsSelected)
            {
                selectedStudentTab = Students;
                return StudentSorter.FindSelectedStudent(Students);
            }

            return null;
        }

        private void RemoveStudent()
        {
            ObservableCollection<Student> selectedStudentTab = new ObservableCollection<Student>();
            Student? selectedStudent = FindSelectedStudent(ref selectedStudentTab);

            if (selectedStudent != null)
            {
                selectedStudentTab.Remove(selectedStudent);
            }
        }

        private void ToggleEditStudent()
        {
            //ObservableCollection<Student> selectedStudentTab = new ObservableCollection<Student>();
            //StudentToEdit = FindSelectedStudent(ref selectedStudentTab);

            //if (EditStudentVisibility == Visibility.Collapsed && StudentToEdit != null)
            //{
            //    EditStudentVisibility = Visibility.Visible;
            //}
            //else
            //{
            //    EditStudentVisibility = Visibility.Collapsed;
            //}

            if (EditStudentVisibility == Visibility.Collapsed)
            {
                EditStudentVisibility = Visibility.Visible;
            }
            else
            {
                EditStudentVisibility = Visibility.Collapsed;
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
