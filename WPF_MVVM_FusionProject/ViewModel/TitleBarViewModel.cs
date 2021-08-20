using System;
using System.Diagnostics;
using System.Windows;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class TitleBarViewModel
    {
        public TitleBarViewModel()
        {
            MainWindowViewModel.titleBarViewModel = this;

            this.commandMinimizeClick = new DelegateCommand(MinimizeClick);
            this.commandMaximizeClick = new DelegateCommand(MaximizeClick);
            this.commandExitClick = new DelegateCommand(ExitClick);
        }

        private DelegateCommand commandMinimizeClick = null;
        private DelegateCommand commandMaximizeClick = null;
        private DelegateCommand commandExitClick = null;

        public DelegateCommand CommandMinimizeClick
        {
            get => this.commandMinimizeClick;
            set => this.commandMinimizeClick = value;
        }

        public DelegateCommand CommandMaximizeClick
        {
            get => this.commandMaximizeClick;
            set => this.commandMaximizeClick = value;
        }

        public DelegateCommand CommandExitClick
        {
            get => this.commandExitClick;
            set => this.commandExitClick = value;
        }

        private void MinimizeClick(object obj)
        {
            ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object obj)
        {
            if (((MainWindow)Application.Current.MainWindow).WindowState == WindowState.Normal)
            {
                ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Maximized;
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Normal;
            }
        }

        private void ExitClick(object obj)
        {
            MainWindowViewModel.manager.CloseMySqlConnection();
            Environment.Exit(0);
        }
    }
}
