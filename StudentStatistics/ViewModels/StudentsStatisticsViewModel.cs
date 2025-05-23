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

namespace StudentStatistics.ViewModels
{
    public class StudentsStatisticsViewModel : INotifyPropertyChanged
    {
        private readonly Router _router;
        private ObservableCollection<Student> _students;

        public AppCloser AppCloser { get; private set; }
        public SolidColorPaint LegendTextColor { get; private set; }
        public ICommand SetTypeTrigger { get; private set; }
        public ICommand GoBackTrigger { get; private set; }
        public ICommand NextPageTrigger {  get; private set; }
        public ICommand PreviousPageTrigger { get; private set; }

        private string _mainHeaderText;
        public string MainHeaderText
        {
            get => _mainHeaderText;
            set
            {
                _mainHeaderText = value;
                OnPropertyChanged(nameof(MainHeaderText));
            }
        }

        private ObservableCollection<Student> _choosenStudents;
        private ObservableCollection<Student> ChoosenStudents
        {
            get => _choosenStudents;
            set
            {
                _choosenStudents = value;
                OnPropertyChanged(nameof(ChoosenStudents));
            }
        }

        private Visibility _typeDecisionVisbility;
        public Visibility TypeDecisionVisbility
        {
            get => _typeDecisionVisbility;
            set
            {
                _typeDecisionVisbility = value;
                OnPropertyChanged(nameof(TypeDecisionVisbility));
            }
        }

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

        private Visibility _previousPageButtonVisibility;
        public Visibility PreviousPageButtonVisibility
        {
            get => _previousPageButtonVisibility;
            set
            {
                _previousPageButtonVisibility = value;
                OnPropertyChanged(nameof(PreviousPageButtonVisibility));
            }
        }

        private IEnumerable<ISeries>? _schoolTypeSeries;
        public IEnumerable<ISeries>? SchoolTypeSeries
        {
            get => _schoolTypeSeries;
            set
            {
                _schoolTypeSeries = value;
                OnPropertyChanged(nameof(SchoolTypeSeries));
            }
        }

        private IEnumerable<ISeries>? _admissionTypeSeries;
        public IEnumerable<ISeries>? AdmissionTypeSeries
        {
            get => _admissionTypeSeries;
            set
            {
                _admissionTypeSeries = value;
                OnPropertyChanged(nameof(AdmissionTypeSeries));
            }
        }

        private IEnumerable<ISeries>? _nationalityTypeSeries;
        public IEnumerable<ISeries>? NationalityTypeSeries
        {
            get => _nationalityTypeSeries;
            set
            {
                _nationalityTypeSeries = value;
                OnPropertyChanged(nameof(NationalityTypeSeries));
            }
        }

        private IEnumerable<ISeries>? _mathGraduationSeries;
        public IEnumerable<ISeries>? MathGraduationSeries
        {
            get => _mathGraduationSeries;
            set
            {
                _mathGraduationSeries = value;
                OnPropertyChanged(nameof(MathGraduationSeries));
            }
        }

        private IEnumerable<ISeries>? _competitionParticipationSeries;
        public IEnumerable<ISeries>? CompetitionParticipationSeries
        {
            get => _competitionParticipationSeries;
            set
            {
                _competitionParticipationSeries = value;
                OnPropertyChanged(nameof(CompetitionParticipationSeries));
            }
        }

        private IEnumerable<ISeries>? _attendanceSeries;
        public IEnumerable<ISeries>? AttendanceSeries
        {
            get => _attendanceSeries;
            set
            {
                _attendanceSeries = value;
                OnPropertyChanged(nameof(AttendanceSeries));
            }
        }

        private IEnumerable<ISeries>? _teachersSeries;
        public IEnumerable<ISeries>? TeachersSeries
        {
            get => _teachersSeries;
            set
            {
                _teachersSeries = value;
                OnPropertyChanged(nameof(TeachersSeries));
            }
        }

        private IEnumerable<ISeries>? _semesterSuccesSeries;
        public IEnumerable<ISeries>? SemesterSuccessSeries
        {
            get => _semesterSuccesSeries;
            set
            {
                _semesterSuccesSeries = value;
                OnPropertyChanged(nameof(SemesterSuccessSeries));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public StudentsStatisticsViewModel(Router router, ObservableCollection<Student> students)
        {
            _router = router;
            _mainHeaderText = "";
            _students = students;
            _choosenStudents= new ObservableCollection<Student>();
            _typeDecisionVisbility = Visibility.Visible;
            _firstPageVisibility= Visibility.Hidden;
            _secondPageVisibility= Visibility.Collapsed;
            _nextPageButtonVisibility= Visibility.Hidden;
            _previousPageButtonVisibility= Visibility.Hidden;

            AppCloser = new AppCloser();
            LegendTextColor = new SolidColorPaint { Color = new SKColor(255, 179, 0) };
            SetTypeTrigger = new RelayCommand<string>(SetStudentType);
            GoBackTrigger = new RelayCommand(GoBack);
            NextPageTrigger = new RelayCommand(NextPage);
            PreviousPageTrigger = new RelayCommand(PreviousPage);
        }

        private void GoBack()
        {
            _router.NavigateBack();
        }

        public void SetStudentType(string type)
        {
            if (type.Equals("successful"))
            {
                ChoosenStudents = StudentSorter.SuccessfullStudents(_students);
                MainHeaderText = "Úspešní študenti";
            }
            else if (type.Equals("unsuccessful"))
            {
                ChoosenStudents = StudentSorter.UnsuccessfullStudents(_students);
                MainHeaderText = "Neúspešní študenti";
            }

            TypeDecisionVisbility = Visibility.Hidden;
            FirstPageVisibility = Visibility.Visible;
            NextPageButtonVisibility = Visibility.Visible;

            LoadGraphs();
        }

        private void LoadGraphs()
        {
            SchoolTypeSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.GrammarSchoolStudents(ChoosenStudents).Count }, Name = "Študenti gymnázia" },
                new PieSeries<int> { Values = new int[] { StudentSorter.VocationalSchoolStudents(ChoosenStudents).Count }, Name = "Študenti odborných škôl" }
            };

            AdmissionTypeSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithoutEntranceExam(ChoosenStudents).Count }, Name = "Bez prijímacich skúšiek", Fill = new SolidColorPaint(SKColors.GreenYellow) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithEntranceExam(ChoosenStudents).Count }, Name = "S prijímacimi skúškami", Fill = new SolidColorPaint(SKColors.LightBlue) }
            };

            NationalityTypeSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.SlovakNationalityStudents(ChoosenStudents).Count }, Name = "Slovenskí študenti", Fill = new SolidColorPaint(SKColors.White) },
                new PieSeries<int> { Values = new int[] { StudentSorter.OtherNationalityStudents(ChoosenStudents).Count }, Name = "Študenti z iných krajín", Fill = new SolidColorPaint(SKColors.Black) }
            };

            CompetitionParticipationSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsParticipatedInCompetitions(ChoosenStudents).Count }, Name = "Zapojili sa do súťaží", Fill = new SolidColorPaint(SKColors.HotPink) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsNotParticipatedInCompetitions(ChoosenStudents).Count }, Name = "Nezapojili sa do súťaží", Fill = new SolidColorPaint(SKColors.Yellow) }
            };

            MathGraduationSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.MathGraduates(ChoosenStudents).Count }, Name = "S maturitou z matematiky", Fill = new SolidColorPaint(SKColors.LightBlue) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithoutMathGraduation(ChoosenStudents).Count }, Name = "Bez maturity z matematiky", Fill = new SolidColorPaint(SKColors.Orange) }
            };

            AttendanceSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsStoppedComing(ChoosenStudents).Count }, Name = "Prestali chodiť na cvičenia", Fill = new SolidColorPaint(SKColors.Turquoise) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsKeptComing(ChoosenStudents).Count }, Name = "Chodili na cvičenia", Fill = new SolidColorPaint(SKColors.Red) }
            };

            SemesterSuccessSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.DidGetToExam(ChoosenStudents).Count }, Name = "Boli na skúške", Fill = new SolidColorPaint(SKColors.DarkRed) },
                new PieSeries<int> { Values = new int[] { StudentSorter.DidNotGetToExam(ChoosenStudents).Count }, Name = "Neboli na skúške", Fill = new SolidColorPaint(SKColors.DarkBlue) }
            };

            TeachersSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithTeacher("doc. PeaDr. Dalibor Gonda PhD.", ChoosenStudents).Count }, Name = "doc. PeaDr. Dalibor Gonda PhD.", Fill = new SolidColorPaint(SKColors.Turquoise) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithTeacher("RNDr. Ida Stankovianska CSc.", ChoosenStudents).Count }, Name = "RNDr. Ida Stankovianska CSc.", Fill = new SolidColorPaint(SKColors.DarkBlue) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithTeacher("Ing. Milan Straka PhD.", ChoosenStudents).Count }, Name = "Ing. Milan Straka PhD.", Fill = new SolidColorPaint(SKColors.Red) },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithTeacher("Ing. Maroš Janovec PhD.", ChoosenStudents).Count }, Name = "Ing. Maroš Janovec PhD.", Fill = new SolidColorPaint(SKColors.DarkRed) }
            };
        }

        private void NextPage()
        {
            FirstPageVisibility = Visibility.Collapsed;
            SecondPageVisibility = Visibility.Visible;
            NextPageButtonVisibility = Visibility.Hidden;
            PreviousPageButtonVisibility = Visibility.Visible;
        }

        private void PreviousPage()
        {
            FirstPageVisibility = Visibility.Visible;
            SecondPageVisibility = Visibility.Collapsed;
            NextPageButtonVisibility = Visibility.Visible;
            PreviousPageButtonVisibility = Visibility.Hidden;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
