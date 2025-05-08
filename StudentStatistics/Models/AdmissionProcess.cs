namespace StudentStatistics.Models
{
    public class AdmissionProcess
    {
        public string AdmissionsProcessResult { get; private set; }
        public string? FirstAlternativeStudyProgram { get; private set; }
        public string? SecondAlternativeStudyProgram { get; private set; }
        public string? HighSchoolCompetetions { get; private set; }
        public int GraduationYear { get; private set; }
        public int? HighSchoolCode { get; private set; }
        public string? HighSchoolName { get; private set; }
        public string HighSchoolType { get; private set; }
        public string? HighSchoolLocation { get; private set; }
        public double ThirdYearAverage { get; private set; }
        public double? ExternGraduationPart { get; private set; }
        public double? ExternPartPercentile { get; private set; }
        public double? ScioPercentile { get; private set; }
        public double? MathPoints {  get; private set; }
        public string Registration {  get; private set; }

        public AdmissionProcess(
            string admissionsProcessResult, string? firstAlternativeStudyProgram, string? secondAlternativeStudyProgram, string? highSchoolCompetetions,
            int graduationYear, int? highSchoolCode, string? highSchoolName, string highSchoolType, string? highSchoolLocation, double thirdYearAverage,
            double? externGraduationPart, double? externPartPercentile, double? scioPercentile, double? mathPoints, string registration
        )
        {
            AdmissionsProcessResult = admissionsProcessResult;
            FirstAlternativeStudyProgram = firstAlternativeStudyProgram;
            SecondAlternativeStudyProgram = secondAlternativeStudyProgram;
            HighSchoolCompetetions = highSchoolCompetetions;
            GraduationYear = graduationYear;
            HighSchoolCode = highSchoolCode;
            HighSchoolName = highSchoolName;
            HighSchoolType = highSchoolType;
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
