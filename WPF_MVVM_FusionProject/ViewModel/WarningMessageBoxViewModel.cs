using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_FusionProject.View;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class WarningMessageBoxViewModel : Notifier
    {
        public WarningMessageBoxViewModel()
        {
            MainWindowViewModel.warningMessageBoxViewModel = this;

            this.commandSaveClick = new DelegateCommand(SaveClick);
            this.commandCancleClick = new DelegateCommand(CancleClick);
        }

        private bool isMessageBoxVisibility = false;
        public bool IsMessageBoxVisibility
        {
            get { return this.isMessageBoxVisibility; }
            set { this.isMessageBoxVisibility = value; Notify("IsMessageBoxVisibility"); }
        }

        private bool isMessageBoxResult = false;
        public bool IsMessageBoxResult
        {
            get { return this.isMessageBoxResult; }
            set { this.isMessageBoxResult = value; Notify("IsMessageBoxResult"); }
        }

        private DelegateCommand commandSaveClick = null;
        private DelegateCommand commandCancleClick = null;

        public DelegateCommand CommandSaveClick
        {
            get => this.commandSaveClick;
            set => this.commandSaveClick = value;
        }

        public DelegateCommand CommandCancleClick
        {
            get => this.commandCancleClick;
            set => this.commandCancleClick = value;
        }

        private void SaveClick(object obj)
        {
            IsMessageBoxResult = true;
            WarningMessageBoxView messageBox = obj as WarningMessageBoxView;
            messageBox.Close();
        }

        private void CancleClick(object obj)
        {
            WarningMessageBoxView messageBox = obj as WarningMessageBoxView;
            messageBox.Close();
        }
    }
}
