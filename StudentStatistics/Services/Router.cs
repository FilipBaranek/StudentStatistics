using System.ComponentModel;
using System.Windows.Controls;
using StudentStatistics.Views;

namespace StudentStatistics.Services
{
    public class Router : INotifyPropertyChanged
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Router()
        {
            _currentView = new SuccessRateView(this);
        }

        public void NavigateTo(UserControl view)
        {
            CurrentView = view;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
