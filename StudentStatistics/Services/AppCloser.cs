using System.Windows;
using System.Windows.Input;
using StudentStatistics.Commands;

namespace StudentStatistics.Services
{
    public class AppCloser
    {
        public ICommand CloseApplicationTrigger { get; private set; }

        public AppCloser()
        {
            CloseApplicationTrigger = new RelayCommand(CloseApplication);
        }

        private void CloseApplication()
        {
            MessageBoxResult result = MessageBox.Show("Naozaj chcete ukončiť aplikáciu?", "Ukončiť aplikáciu", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
