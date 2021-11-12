using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class LoginViewModel : Notifier
    {
        public LoginViewModel()
        {
            LoginMainViewModel.loginViewModel = this;

            this.commandLoginClick = new DelegateCommand(LoginClick);
            this.commandExitClick = new DelegateCommand(ExitClick);
            this.commandWarningMessageExitClick = new DelegateCommand(WarningMessageExitClick);
        }

        private int loginFailCount = 0;
        private int loginFailMaxCount = 8;
        private bool isLoginLimit = false;

        private string userId = string.Empty;
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; Notify("UserId"); }
        }

        private string userPw = string.Empty;
        public string UserPw
        {
            get { return this.userPw; }
            set { this.userPw = value; Notify("UserPw"); }
        }

        private string idErrorMessage = string.Empty;
        public string IdErrorMessage
        {
            get { return this.idErrorMessage; }
            set { this.idErrorMessage = value; Notify("IdErrorMessage"); }
        }

        private string pwErrorMessage = string.Empty;
        public string PwErrorMessage
        {
            get { return this.pwErrorMessage; }
            set { this.pwErrorMessage = value; Notify("PwErrorMessage"); }
        }

        private bool isIdTextBoxFocus = false;
        public bool IsIdTextBoxFocus
        {
            get { return this.isIdTextBoxFocus; }
            set { this.isIdTextBoxFocus = value; Notify("IsIdTextBoxFocus"); }
        }

        private bool isPwTextBoxFocus = false;
        public bool IsPwTextBoxFocus
        {
            get { return this.isPwTextBoxFocus; }
            set { this.isPwTextBoxFocus = value; Notify("IsPwTextBoxFocus"); }
        }

        private bool isShowPwCheck = false;
        public bool IsShowPwCheck
        {
            get { return this.isShowPwCheck; }
            set { this.isShowPwCheck = value; Notify("IsShowPwCheck"); }
        }

        private bool isLoginButtonEnabled = true;
        public bool IsLoginButtonEnabled
        {
            get { return this.isLoginButtonEnabled; }
            set { this.isLoginButtonEnabled = value; Notify("IsLoginButtonEnabled"); }
        }

        private bool isLoginWarning = false;
        public bool IsLoginWarning
        {
            get { return this.isLoginWarning; }
            set { this.isLoginWarning = value; Notify("IsLoginWarning"); }
        }

        private string warningErrorMessage = string.Empty;
        public string WarningErrorMessage
        {
            get { return this.warningErrorMessage; }
            set { this.warningErrorMessage = value; Notify("WarningErrorMessage"); }
        }

        private DelegateCommand commandLoginClick = null;
        private DelegateCommand commandExitClick = null;
        private DelegateCommand commandWarningMessageExitClick = null;

        public DelegateCommand CommandLoginClick
        {
            get => this.commandLoginClick;
            set => this.commandLoginClick = value;
        }

        public DelegateCommand CommandExitClick
        {
            get => this.commandExitClick;
            set => this.commandExitClick = value;
        }

        public DelegateCommand CommandWarningMessageExitClick
        {
            get => this.commandWarningMessageExitClick;
            set => this.commandExitClick = value;
        }

        private void LoginClick(object obj)
        {
            IdErrorMessage = string.Empty;
            PwErrorMessage = string.Empty;
            IsIdTextBoxFocus = false;
            IsPwTextBoxFocus = false;

            if (isLoginLimit)
            {
                UserPw = string.Empty;
                IsLoginWarning = true;
                return;
            }

            if (string.IsNullOrEmpty(UserId))
            {
                IsIdTextBoxFocus = true;
                IdErrorMessage = "아이디를 입력해주세요.";
                return;
            }

            if (string.IsNullOrEmpty(UserPw))
            {
                IsPwTextBoxFocus = true;
                PwErrorMessage = "비밀번호를 입력해주세요.";
                return;
            }

            LoginCheck();
        }

        private void LoginCheck()
        {
            string tableName = "users";
            string selectQuery = string.Format("SELECT * FROM {0} WHERE user_id = '{1}'", tableName, UserId);

            DataSet userDataSet = LoginMainViewModel.manager.Select(selectQuery, tableName);

            if (userDataSet.Tables[0].Rows.Count > 0 && userDataSet.Tables[0].Rows[0]["user_id"].ToString() == UserId && userDataSet.Tables[0].Rows[0]["user_pw"].ToString() == UserPw)
            {
                UserId = string.Empty;
                UserPw = string.Empty;
                loginFailCount = 0;

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                ((LoginMainView)Application.Current.MainWindow).Close();
                Application.Current.MainWindow = mainWindow;
            }

            else
            {
                loginFailCount++;
                UserPw = string.Empty;
                PwErrorMessage = "아이디 또는 비밀번호가 틀렸습니다.";

                if (loginFailCount < 5)
                {
                    IsPwTextBoxFocus = true;
                }
                else
                {
                    IsLoginWarning = true;

                    if (loginFailCount == 8)
                    {
                        DispatcherTimer timer = new DispatcherTimer();
                        int loginLimitCount = 5;
                        isLoginLimit = true;

                        timer.Start();
                        timer.Interval = TimeSpan.FromMilliseconds(1000);
                        timer.Tick += (o, e) =>
                        {
                            WarningErrorMessage = string.Format("비밀번호 {0}회 오류입니다.\n\n {1}초 동안 로그인이 제한됩니다.", loginFailCount, loginLimitCount);
                            loginLimitCount--;

                            if (loginLimitCount < 0)
                            {
                                timer.Stop();
                                loginFailCount = 0;
                                isLoginLimit = false;
                                IsLoginWarning = false;
                            }
                        };
                    }
                    else
                    {
                        WarningErrorMessage = string.Format("비밀번호 {0}회 오류입니다.\n\n {1}회 이상 실패시 로그인이 제한될 수 있습니다.", loginFailCount, loginFailMaxCount);
                    }
                }
            }
        }
 
        private void ExitClick(object obj)
        {
            LoginMainViewModel.manager.CloseMySqlConnection();
            ((LoginMainView)Application.Current.MainWindow).Close();
        }

        private void WarningMessageExitClick(object obj)
        {
            IsLoginWarning = false;
            IsPwTextBoxFocus = true;
        }
    }
}
