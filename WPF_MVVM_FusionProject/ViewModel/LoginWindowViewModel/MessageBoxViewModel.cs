using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_FusionProject.ViewModel.LoginWindowViewModel
{
    public class MessageBoxViewModel : Notifier
    {
        public MessageBoxViewModel()
        {
            this.commandExitClick = new DelegateCommand(ExitClick);
        }

        private bool isMessageBoxVisibility = false;
        public bool IsMessageBoxVisibility
        {
            get { return this.isMessageBoxVisibility; }
            set { this.isMessageBoxVisibility = value; Notify("IsMessageBoxVisibility"); }
        }

        private string errorText = string.Empty;
        public string ErrorText
        {
            get { return this.errorText; }
            set { this.errorText = value; Notify("ErrorText"); }
        }

        private DelegateCommand commandExitClick = null;

        public DelegateCommand CommandExitClick
        {
            get => this.commandExitClick;
            set => this.commandExitClick = value;
        }

        private void ExitClick(object obj)
        {
            if (LoginMainViewModel.loginViewModel.IsShowPwCheck)
            {
                LoginMainViewModel.loginViewModel.IsPwTextBoxVisibility = true;
            }
            else
            {
                LoginMainViewModel.loginViewModel.IsPwPasswordBoxVisibility = true;
            }
            IsMessageBoxVisibility = false;
        }
    }
}
