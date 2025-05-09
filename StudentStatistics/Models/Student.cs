namespace StudentStatistics.Models
{
    public class Student
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Gender { get; private set; }
        public int? PersonalNumber { get; private set; }
        public string StudyProgram { get; private set; }
        public string Nationality { get; private set; }
        public AdmissionProcess AdmissionProcess { get; private set; }
        public SemesterResults? SemesterResults { get; set; }

        public Student(string name, string surname, string gender, int? personalNumber, string studyProgram, string nationality, AdmissionProcess admissionProcess)
        {
            Name = name;
            Surname = surname;
            Gender = gender;
            PersonalNumber = personalNumber;
            StudyProgram = studyProgram;
            Nationality = nationality;
            AdmissionProcess = admissionProcess;
        }
        
    }
}
