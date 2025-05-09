namespace StudentStatistics.Models
{
    public class SemesterResults
    {
        public string StudyGroup { get; private set; }
        public double FirstTest { get; private set; }
        public double SecondTest { get; private set; }
        public double WrittenTestForm { get; private set; }
        public double? OralTestForm { get; private set; }
        public double TestSum { get; private set; }
        public double SemesterSum { get; private set; }
        public double MissingPoints { get; private set; }
        public double TotalPoints { get; private set; }
        public char Grade { get; private set; }
        public double ActivitiesSum { get; private set; }
        public double[] Activities { get; private set; }
        public bool[] Attendance { get; private set; }

        public SemesterResults(
            string studyGroup, double firstTest, double secondTest, double writtenTestForm, double? oralTestForm, double testSum, double semesterSum,
            double missingPoints, double totalPointsm, char grade, double activitiesSum, double[] activities, bool[] attendance
        )
        {
            StudyGroup = studyGroup;
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

    }
}
