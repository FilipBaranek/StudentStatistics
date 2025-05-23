using System.ComponentModel;
using System.Windows.Controls;
using StudentStatistics.Views;

namespace StudentStatistics.Services
{
    public class Router : INotifyPropertyChanged
    {
        private UserControl? _lastView;
        public UserControl? LastView
        {
            get => _lastView;
            set
            {
                _lastView = value;
                OnPropertyChanged(nameof(LastView));
            }
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                LastView = _currentView;
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

        public void NavigateBack()
        {
            if (LastView != null)
            {
                CurrentView = LastView;
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
