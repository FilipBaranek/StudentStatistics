namespace StudentStatistics.Models
{
    public class AdmissionProcess
    {
        public string _highSchoolType;
        public string? _firstAlternativeStudyProgram;
        public string? SecondAlternativeStudyProgram { get; private set; }
        public string AdmissionsProcessResult { get; private set; }
        public string? HighSchoolCompetetions { get; private set; }
        public int GraduationYear { get; private set; }
        public int? HighSchoolCode { get; private set; }
        public string? HighSchoolName { get; private set; }
        public string? HighSchoolLocation { get; private set; }
        public double ThirdYearAverage { get; private set; }
        public double? ExternGraduationPart { get; private set; }
        public double? ExternPartPercentile { get; private set; }
        public double? ScioPercentile { get; private set; }
        public double? MathPoints {  get; private set; }
        public string Registration {  get; private set; }

        public string? HighSchoolType
        {
            get
            {
                return _highSchoolType != null && _highSchoolType.Equals("zahraničná stredná škola") ? "zahraničná škola" : _highSchoolType;
            }
        }

        public string? AlternativeStudyProgram
        {
            get
            {
                if (_firstAlternativeStudyProgram != null && _firstAlternativeStudyProgram.Equals("informačné a sieťové technológie"))
                {
                    return "IAST";
                }
                return _firstAlternativeStudyProgram;
            }
        }

        public AdmissionProcess(
            string admissionsProcessResult, string? firstAlternativeStudyProgram, string? secondAlternativeStudyProgram, string? highSchoolCompetetions,
            int graduationYear, int? highSchoolCode, string? highSchoolName, string highSchoolType, string? highSchoolLocation, double thirdYearAverage,
            double? externGraduationPart, double? externPartPercentile, double? scioPercentile, double? mathPoints, string registration
        )
        {
            _highSchoolType = highSchoolType;
            _firstAlternativeStudyProgram = firstAlternativeStudyProgram;
            AdmissionsProcessResult = admissionsProcessResult;
            SecondAlternativeStudyProgram = secondAlternativeStudyProgram;
            HighSchoolCompetetions = highSchoolCompetetions;
            GraduationYear = graduationYear;
            HighSchoolCode = highSchoolCode;
            HighSchoolName = highSchoolName;
            HighSchoolLocation = highSchoolLocation;
            ThirdYearAverage = thirdYearAverage;
            ExternGraduationPart = externGraduationPart;
            ExternPartPercentile = externPartPercentile;
            ScioPercentile = scioPercentile;
            MathPoints = MathPoints;
            Registration = registration;
        }


    }
}
