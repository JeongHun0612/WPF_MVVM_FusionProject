using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_FusionProject.DBConncet;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class LoginMainViewModel
    {
        public LoginMainViewModel()
        {
            manager.Initialize();
            this.commandDragMove = new DelegateCommand(DragMove);
        }

        public static MySQLManager manager = new MySQLManager();
        public static LoginViewModel loginViewModel { get; set; } = null;

        private DelegateCommand commandDragMove = null;

        public DelegateCommand CommandDragMove
        {
            get => this.commandDragMove;
            set => this.commandDragMove = value;
        }

        private void DragMove(object obj)
        {
            ((LoginMainView)Application.Current.MainWindow).DragMove();
        }
    }
}
