namespace StudentStatistics.Models
{
    public class SemesterResults
    {
        private const int _minimalPoints = 30;

        public double FirstTest { get; private set; }
        public double SecondTest { get; private set; }
        public double WrittenTestForm { get; private set; }
        public double TheoreticalTestForm { get; private set; }
        public double[] Activities { get; private set; }

        public double ActivitiesSum
        {
            get => GetActivitiesSum();
        }

        public double TestSum
        {
            get => FirstTest + SecondTest;
        }

        public double SemesterSum
        {
            get => ActivitiesSum + TestSum;
        }

        public double MissingPoints
        {
            get
            {
                if (SemesterSum < _minimalPoints)
                {
                    return _minimalPoints - SemesterSum;
                }

                return 0;
            }
        }

        public double Exam
        {
            get => WrittenTestForm + TheoreticalTestForm;
        }

        public double TotalPoints
        {
            get => SemesterSum + Exam;
        }

        public char Grade
        {
            get => GetGrade();
        }

        public SemesterResults(double firstTest, double secondTest, double writtenTestForm, double theoreticalTestForm, double[] activities)
        {
            FirstTest = firstTest;
            SecondTest = secondTest;
            WrittenTestForm = writtenTestForm;
            TheoreticalTestForm = theoreticalTestForm;
            Activities = activities;
        }

        private double GetActivitiesSum()
        {
            double sum = 0;

            foreach (double activityPoints in Activities)
            {
                sum += activityPoints;
            }

            return sum;
        }

        private char GetGrade()
        {
            switch (TotalPoints)
            {
                case > 92:
                    return 'A';
                case > 84:
                    return 'B';
                case > 76:
                    return 'C';
                case > 68:
                    return 'D';
                case > 60:
                    return 'E';
                default:
                    return 'F';
            }
        }

    }
}
