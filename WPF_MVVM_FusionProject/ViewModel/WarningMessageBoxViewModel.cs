using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

        private int messageBoxMode;
        public int MessageBoxMode
        {
            get { return this.messageBoxMode; }
            set 
            { 
                this.messageBoxMode = value; 
                Notify("MessageBoxMode");

                switch (messageBoxMode)
                {
                    case 0:
                        MessageBoxIcon = "/WPF_MVVM_FusionProject;component/Image/warning2.png";
                        IsDangerMode = true;
                        IsWarningMode = false;
                        break;
                    case 1:
                        MessageBoxIcon = "/WPF_MVVM_FusionProject;component/Image/danger.png";
                        IsDangerMode = false;
                        IsWarningMode = true;
                        break;
                }
            }
        }

        private string messageBoxText = string.Empty;
        public string MessageBoxText
        {
            get { return this.messageBoxText; }
            set { this.messageBoxText = value; Notify("MessageBoxText"); }
        }

        private string messageBoxIcon = "/WPF_MVVM_FusionProject;component/Image/warning2.png";
        public string MessageBoxIcon
        {
            get { return this.messageBoxIcon; }
            set { this.messageBoxIcon = value; Notify("MessageBoxIcon"); }
        }

        private bool isDangerMode = false;
        public bool IsDangerMode
        {
            get { return this.isDangerMode; }
            set { this.isDangerMode = value; Notify("IsDangerMode"); }
        }

        private bool isWarningMode = false;
        public bool IsWarningMode
        {
            get { return this.isWarningMode; }
            set { this.isWarningMode = value; Notify("IsWarningMode"); }
        }

        private bool isCancleButtonFocus = true;
        public bool IsCancleButtonFocus
        {
            get { return this.isCancleButtonFocus; }
            set { this.isCancleButtonFocus = value; Notify("IsCancleButtonFocus"); }
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
