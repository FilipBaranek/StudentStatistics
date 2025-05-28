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
        private bool? _multipleFiles;
        private string? _teacher;
        private string? _semesterFilePath;
        private string? _admissionProcessFilePath;
        private Student? _selectedStudent;
        private Dictionary<string, string> _semesterFiles;
        private ObservableCollection<Student>? _loadedAllStudents;
        private ObservableCollection<Student> _loadedSuccessfullStudents;
        private ObservableCollection<Student> _loadedUnsuccessfullStudents;

        public AppCloser AppCloser { get; private set; }
        public SolidColorPaint LegendTextColor { get; private set; }
        public ICommand NewFileGridTrigger { get; private set; }
        public ICommand NewMultipleFilesTrigger { get; private set; }
        public ICommand SelectTeacherTrigger { get; private set; }
        public ICommand RemoveFilesTrigger { get; private set; }
        public ICommand RemoveFileTrigger { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SearchTrigger { get; private set; }
        public ICommand DetailTrigger { get; private set; }
        public ICommand DeleteStudentTrigger { get; private set; }
        public ICommand EditStudentTrigger {  get; private set; }
        public ICommand ConfirmEditStudentTrigger { get; private set; }
        public ICommand ConfirmFileOpenTrigger { get; private set; }
        public ICommand NavigateToStudentInfoTrigger { get; private set; }
        public ICommand ExportToCSVCommand { get; private set; }

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
            _confirmFilesVisibility = Visibility.Hidden;
            _searchText = "";
            _semesterFiles = new Dictionary<string, string>();
            _loadedSuccessfullStudents = new ObservableCollection<Student>();
            _loadedUnsuccessfullStudents = new ObservableCollection<Student>();
            _students = new ObservableCollection<Student>();
            _successfullStudents = _loadedSuccessfullStudents;
            _unsuccessfullStudents = _loadedUnsuccessfullStudents;

            AppCloser = new AppCloser();
            LegendTextColor = new SolidColorPaint { Color = new SKColor(255, 179, 0) };
            SuccessfullStudentsSelected = true;
            Students = new ObservableCollection<Student>();
            NewFileGridTrigger = new RelayCommand(OpenSingleFileGrid);
            NewMultipleFilesTrigger = new RelayCommand(OpenMultipleFileGrid);
            OpenFileCommand = new RelayCommand<string>(OpenFile);
            RemoveFileTrigger = new RelayCommand<string>(RemoveFile);
            RemoveFilesTrigger = new RelayCommand(RemoveFiles);
            SearchTrigger = new RelayCommand(SearchStudent);
            DetailTrigger = new RelayCommand(NavigateToDetailStatistics);
            DeleteStudentTrigger = new RelayCommand(RemoveStudent);
            EditStudentTrigger = new RelayCommand(ToggleEditStudent);
            SelectTeacherTrigger = new RelayCommand<string>(SelectTeacher);
            ConfirmFileOpenTrigger = new RelayCommand(ConfirmFileOpen);
            ConfirmEditStudentTrigger = new RelayCommand(EditStudent);
            NavigateToStudentInfoTrigger = new RelayCommand(NavigateToStudentInfo);
            ExportToCSVCommand = new RelayCommand(SaveToCSV);
        }

        private void OpenSingleFileGrid()
        {
            _multipleFiles = false;
            ToggleNewFileGrid();
        }

        private void OpenMultipleFileGrid()
        {
            _multipleFiles = true;
            ToggleNewFileGrid();
        }

        private void OpenFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFilePath = FileHandler.OpenExcelFile();
                
                if (_semesterFilePath != null)
                {
                    TeacherWindowVisibility = Visibility.Visible;
                }
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = FileHandler.OpenCsvFile();
                SetAdmissionFileVisibility();
            }
        }

        private void ConfirmFileOpen()
        {
            ConfirmFilesVisibility = Visibility.Collapsed;
            LoadStudents();
        }

        private void RemoveFile(string fileType)
        {
            if (fileType.Equals("semester"))
            {
                _semesterFiles.Clear();

                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
                RemoveSemesterFileVisibility = Visibility.Collapsed;
                SemesterButtonVisibility = Visibility.Visible;
                ConfirmFilesVisibility = Visibility.Collapsed;
            }
            else if (fileType.Equals("admission"))
            {
                _admissionProcessFilePath = null;

                CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
                RemoveAdmissionFileVisibility= Visibility.Collapsed;
                AdmissionButtonVisibility = Visibility.Visible;
                ConfirmFilesVisibility= Visibility.Collapsed;
            }
        }

        private void RemoveFiles()
        {
            _semesterFiles.Clear();
            _semesterFilePath = null;
            _admissionProcessFilePath = null;
            _loadedAllStudents = null;
            _loadedSuccessfullStudents.Clear();
            _loadedUnsuccessfullStudents.Clear();
            SuccessfullStudents.Clear();
            UnsuccessfullStudents.Clear();
            Students.Clear();

            StudentSeries = null;

            RemoveFilesVisibility = Visibility.Hidden;
            StatisticsVisibility = Visibility.Hidden;
        }

        private async void FileErrorInfo()
        {
            RemoveFiles();

            SemesterButtonVisibility = Visibility.Collapsed;
            AdmissionButtonVisibility = Visibility.Collapsed;
            CheckmarkSemesterFileVisibility = Visibility.Collapsed;
            CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
            RemoveSemesterFileVisibility = Visibility.Collapsed;
            RemoveAdmissionFileVisibility = Visibility.Collapsed;
            WrongFileVisibility = Visibility.Visible;

            await Task.Delay(5000);

            WrongFileVisibility = Visibility.Collapsed;
            SemesterButtonVisibility = Visibility.Visible;
            AdmissionButtonVisibility = Visibility.Visible;
        }

        private void SelectTeacher(string teacher)
        {
            _teacher = teacher;
            TeacherWindowVisibility = Visibility.Hidden;

            if (_semesterFilePath != null && _teacher != null)
            {
                _semesterFiles.Add(_semesterFilePath, _teacher);
                _teacher = null;
                _semesterFilePath = null;

                if (_multipleFiles != null && _multipleFiles == true)
                {
                    SetMultipleSemesterFilesVisibility();
                }
                else
                {
                    SetSemesterFileVisibility();
                }
            }
        }

        private void LoadAdmissionData()
        {
            if (_admissionProcessFilePath != null)
            {
                _loadedAllStudents = DataReader.ReadAdmissionFile(_admissionProcessFilePath);
            }
        }

        private bool LoadSemesterData()
        {
            if (_loadedAllStudents != null && _semesterFiles.Count > 0)
            {
                foreach (KeyValuePair<string, string> file in _semesterFiles)
                {
                    if (!DataReader.ReadSemesterFile(file.Key, file.Value, _loadedAllStudents))
                    {
                        return false;
                    }
                }
                _semesterFiles.Clear();
                
                return true;
            }
            return false;
        }

        private void LoadStudents()
        {
            LoadAdmissionData();

            if (_loadedAllStudents != null && LoadSemesterData())
            {
                _loadedSuccessfullStudents = StudentSorter.SuccessfullStudents(_loadedAllStudents);
                _loadedUnsuccessfullStudents = StudentSorter.UnsuccessfullStudents(_loadedAllStudents);

                Students = _loadedAllStudents;
                SuccessfullStudents = _loadedSuccessfullStudents;
                UnsuccessfullStudents = _loadedUnsuccessfullStudents;

                RemoveFilesVisibility = Visibility.Visible;
                StatisticsVisibility = Visibility.Visible;

                UpdatePieChart();

                _semesterFiles.Clear();
                _admissionProcessFilePath = null;
                
                if (_multipleFiles != null && _multipleFiles == true)
                {
                    SetMultipleSemesterFilesVisibility();
                }
                else
                {
                    SetSemesterFileVisibility();
                }
                SetAdmissionFileVisibility();
            }
            else
            {
                RemoveFiles();
                FileErrorInfo();
            }
        }

        private void SetSemesterFileVisibility()
        {
            if (_semesterFiles.Count > 0)
            {
                RemoveSemesterFileVisibility = Visibility.Visible;
                SemesterButtonVisibility = Visibility.Collapsed;
                CheckmarkSemesterFileVisibility = Visibility.Visible;
            }
            else
            {
                SemesterButtonVisibility = Visibility.Visible;
                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
                RemoveSemesterFileVisibility = Visibility.Collapsed;
            }

            SetConfirmFilesVisibility();
        }

        private async void SetMultipleSemesterFilesVisibility()
        {
            if (_semesterFiles.Count > 0)
            {
                RemoveSemesterFileVisibility = Visibility.Visible;
                SemesterButtonVisibility = Visibility.Collapsed;
                CheckmarkSemesterFileVisibility = Visibility.Visible;

                await Task.Delay(1000);

                SemesterButtonVisibility = Visibility.Visible;
                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
            }
            else
            {
                SemesterButtonVisibility = Visibility.Visible;
                CheckmarkSemesterFileVisibility = Visibility.Collapsed;
                RemoveSemesterFileVisibility = Visibility.Collapsed;
            }

            SetConfirmFilesVisibility();
        }

        private void SetAdmissionFileVisibility()
        {
            if (_admissionProcessFilePath != null)
            {
                RemoveAdmissionFileVisibility = Visibility.Visible;
                AdmissionButtonVisibility = Visibility.Collapsed;
                CheckmarkAdmissionFileVisibility = Visibility.Visible;
            }
            else
            {
                AdmissionButtonVisibility = Visibility.Visible;
                CheckmarkAdmissionFileVisibility = Visibility.Collapsed;
                RemoveAdmissionFileVisibility = Visibility.Collapsed;
            }

            SetConfirmFilesVisibility();
        }

        private void SetConfirmFilesVisibility()
        {
            if (_admissionProcessFilePath != null && _semesterFiles.Count > 0)
            {
                ConfirmFilesVisibility = Visibility.Visible;
            }
        }

        private void ToggleNewFileGrid()
        {
            if (NewFileGridVisibility == Visibility.Hidden)
            {
                RemoveFiles();
                NewFileGridVisibility = Visibility.Visible;
            }
            else
            {
                _multipleFiles = null;
                NewFileGridVisibility = Visibility.Hidden;
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
                    Fill = new SolidColorPaint(SKColors.YellowGreen),
                    Stroke = new SolidColorPaint(SKColors.Black)
                },
                new PieSeries<int> 
                { 
                    Values = new int[] { UnsuccessfullStudents.Count },
                    Name = "Neúspešní študenti",
                    Fill = new SolidColorPaint(new SKColor(255, 248, 77)),
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

        private Student? FindSelectedStudent()
        {
            if (SuccessfullStudentsSelected)
            {
                return StudentSorter.FindSelectedStudent(SuccessfullStudents);
            }
            else if (UnsuccessfullStudentsSelected)
            {
                return StudentSorter.FindSelectedStudent(UnsuccessfullStudents);
            }
            else if (AllStudentsSelected)
            {
                return StudentSorter.FindSelectedStudent(Students);
            }

            return null;
        }

        private ObservableCollection<Student>? FindSelectedTab()
        {
            if (SuccessfullStudentsSelected)
            {
                return SuccessfullStudents;
            }
            else if (UnsuccessfullStudentsSelected)
            {
                return SuccessfullStudents;
            }
            else if (AllStudentsSelected)
            {
                return SuccessfullStudents;
            }

            return null;
        }

        private void RemoveStudent()
        {
            Student? selectedStudent = FindSelectedStudent();
            var selectedTab = FindSelectedTab();

            if (selectedStudent != null && selectedTab != null)
            {
                selectedTab.Remove(selectedStudent);
            }
        }

        private void ToggleEditStudent()
        {
            if (EditStudentVisibility == Visibility.Collapsed)
            {
                EditStudentVisibility = Visibility.Visible;
                LoadEditStudent();
            }
            else
            {
                StudentToEdit = _selectedStudent;
                StudentToEdit = null;
                _selectedStudent = null;

                EditStudentVisibility = Visibility.Collapsed;
            }
        }

        private void LoadEditStudent()
        {
            _selectedStudent = FindSelectedStudent();

            if (_selectedStudent != null)
            {
                Student studentCopy = new Student
                (
                    _selectedStudent.Name,
                    _selectedStudent.Surname,
                    _selectedStudent.Gender,
                    _selectedStudent.PersonalNumber,
                    _selectedStudent.StudyProgram,
                    _selectedStudent.Nationality,
                    _selectedStudent.AdmissionProcess
                );
                studentCopy.SemesterResults = _selectedStudent.SemesterResults;

                StudentToEdit = studentCopy;
            }
        }

        private void EditStudent()
        {
            if (_selectedStudent  != null && StudentToEdit != null && _selectedStudent.SemesterResults != null && StudentToEdit.SemesterResults != null)
            {
                _selectedStudent.Name = StudentToEdit.Name;
                _selectedStudent.Surname = StudentToEdit.Surname;
                _selectedStudent.PersonalNumber = StudentToEdit.PersonalNumber;
                _selectedStudent.SemesterResults.Teacher = StudentToEdit.SemesterResults.Teacher;
                _selectedStudent.SemesterResults.StudyGroup = StudentToEdit.SemesterResults.StudyGroup;

                ToggleEditStudent();
            }
        }

        private void NavigateToDetailStatistics()
        {
            if (_loadedAllStudents != null)
            {
                _router.CurrentView = new StudentsStatisticsView(_router, _loadedAllStudents);
            }
        }

        private void NavigateToStudentInfo()
        {
            Student? selectedStudent = FindSelectedStudent();

            if (selectedStudent != null)
            {
                _router.NavigateTo(new StudentInfoView(_router, selectedStudent));
            }
        }

        private void SaveToCSV()
        {
            var selectedTab = FindSelectedTab();

            if (selectedTab != null)
            {
                FileHandler.ExportToCsv(selectedTab);
            }

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
