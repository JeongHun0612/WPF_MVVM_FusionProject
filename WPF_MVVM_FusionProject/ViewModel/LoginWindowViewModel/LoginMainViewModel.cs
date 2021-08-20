using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_FusionProject.DBConncet;
using WPF_MVVM_FusionProject.View.LoginWindowView;

namespace WPF_MVVM_FusionProject.ViewModel.LoginWindowViewModel
{
    public class LoginMainViewModel
    {
        public LoginMainViewModel()
        {
            manager.Initialize();
            this.commandDragMove = new DelegateCommand(DragMove);
        }

        public static MySQLManager manager = new MySQLManager();
        public static LoginViewModel loginViewModel { get; set; } = new LoginViewModel();
        public static MessageBoxViewModel messageBoxViewModel { get; set; } = new MessageBoxViewModel();

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
