using System.ComponentModel;

namespace StudentStatistics.Models
{
    public class Student : INotifyPropertyChanged
    {
        public string FullName => $"{Name} {Surname}";
        public string Gender { get; private set; }
        public string Nationality { get; private set; }
        public AdmissionProcess AdmissionProcess { get; private set; }
        public SemesterResults? SemesterResults { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private int? _personalNumber;
        public int? PersonalNumber
        {
            get => _personalNumber;
            set
            {
                _personalNumber = value;
                OnPropertyChanged(nameof(PersonalNumber));
            }
        }

        private string _studyProgram;
        public string StudyProgram
        {
            get => _studyProgram;
            set
            {
                _studyProgram = value;
                OnPropertyChanged(nameof(StudyProgram));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Student(string name, string surname, string gender, int? personalNumber, string studyProgram, string nationality, AdmissionProcess admissionProcess)
        {
            _name = name;
            _surname = surname;
            _personalNumber = personalNumber;
            _studyProgram = studyProgram;
            Gender = gender;
            Nationality = nationality;
            AdmissionProcess = admissionProcess;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
