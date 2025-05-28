using System.ComponentModel;

namespace StudentStatistics.Models
{
    public class SemesterResults : INotifyPropertyChanged
    {
        public double FirstTest { get; private set; }
        public double SecondTest { get; private set; }
        public double? WrittenTestForm { get; private set; }
        public double? OralTestForm { get; private set; }
        public double TestSum { get; private set; }
        public double SemesterSum { get; private set; }
        public double MissingPoints { get; private set; }
        public double TotalPoints { get; private set; }
        public char Grade { get; private set; }
        public double ActivitiesSum { get; private set; }
        public double[] Activities { get; private set; }
        public bool[] Attendance { get; private set; }

        private string _studyGroup;
        public string StudyGroup
        {
            get => _studyGroup;
            set
            {
                _studyGroup = value;
                OnPropertyChanged(nameof(StudyGroup));
            }
        }

        private string _teacher;
        public string Teacher
        {
            get => _teacher;
            set
            {
                _teacher = value;
                OnPropertyChanged(nameof(Teacher));
            }
        }

        public double ExamSum
        {
            get
            {
                double examSum = 0;
                if (WrittenTestForm != null)
                {
                    examSum += (double)WrittenTestForm;
                }
                if (OralTestForm != null)
                {
                    examSum += (double)OralTestForm;
                }
                return examSum;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SemesterResults(
            string studyGroup, double firstTest, double secondTest, double? writtenTestForm, double? oralTestForm, double testSum, double semesterSum,
            double missingPoints, double totalPointsm, char grade, string teacher, double activitiesSum, double[] activities, bool[] attendance
        )
        {
            _studyGroup = studyGroup;
            _teacher = teacher;
            FirstTest = firstTest;
            SecondTest = secondTest;
            WrittenTestForm = writtenTestForm;
            OralTestForm = oralTestForm;
            TestSum = testSum;
            SemesterSum = semesterSum;
            MissingPoints = missingPoints;
            TotalPoints = totalPointsm;
            Grade = grade;
            ActivitiesSum = activitiesSum;
            Activities = activities;
            Attendance = attendance;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
