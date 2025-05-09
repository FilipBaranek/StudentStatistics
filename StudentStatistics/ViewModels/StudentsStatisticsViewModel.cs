using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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
        public ICommand SetTypeTrigger { get; private set; }
        public ICommand GoBackTrigger { get; private set; }

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

        private Visibility _contentVisibility;
        public Visibility ContentVisibility
        {
            get => _contentVisibility;
            set
            {
                _contentVisibility = value;
                OnPropertyChanged(nameof(ContentVisibility));
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public StudentsStatisticsViewModel(Router router, ObservableCollection<Student> students)
        {
            _router = router;
            _mainHeaderText = "";
            _students = students;
            _choosenStudents= new ObservableCollection<Student>();
            _typeDecisionVisbility = Visibility.Visible;
            _contentVisibility= Visibility.Hidden;

            AppCloser = new AppCloser();
            SetTypeTrigger = new RelayCommand<string>(SetStudentType);
            GoBackTrigger = new RelayCommand(GoBack);
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
            ContentVisibility = Visibility.Visible;

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
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithoutEntranceExam(ChoosenStudents).Count }, Name = "Študenti prijatí bez prijímacich skúšiek" },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithEntranceExam(ChoosenStudents).Count }, Name = "Študenti prijatí po absolvovaní prijímacich skúšiek" }
            };

            NationalityTypeSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.SlovakNationalityStudents(ChoosenStudents).Count }, Name = "Slovenskí študenti" },
                new PieSeries<int> { Values = new int[] { StudentSorter.OtherNationalityStudents(ChoosenStudents).Count }, Name = "Študenti z iných krajín" }
            };

            CompetitionParticipationSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsParticipatedInCompetitions(ChoosenStudents).Count }, Name = "Študenti ktorí sa zapojili do stredoškolských súťaží" },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsNotParticipatedInCompetitions(ChoosenStudents).Count }, Name = "Študenti ktorí sa nezapojili do stredoškolských súťaží" }
            };

            MathGraduationSeries = new ISeries[]
            {
                new PieSeries<int> { Values = new int[] { StudentSorter.MathGraduates(ChoosenStudents).Count }, Name = "Študenti s maturitou z matematiky" },
                new PieSeries<int> { Values = new int[] { StudentSorter.StudentsWithoutMathGraduation(ChoosenStudents).Count }, Name = "Študenti bez maturity z matematiky" }
            };
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
