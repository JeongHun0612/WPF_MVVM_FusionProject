using System;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using WPF_MVVM_FusionProject.DBConncet;
using WPF_MVVM_FusionProject.View.LoginWindowView;

namespace WPF_MVVM_FusionProject.ViewModel.LoginWindowViewModel
{
    public class LoginViewModel : Notifier
    {
        public LoginViewModel()
        {
            this.commandLoginClick = new DelegateCommand(LoginClick);
            this.commandShowPwClick = new DelegateCommand(ShowPwClick);
            this.commandExitClick = new DelegateCommand(ExitClick);
            //this.commandSignUpClick = new DelegateCommand(SignUpClick);
        }

        private MySQLManager manager = LoginMainViewModel.manager;
        private MainWindow mainWindow = new MainWindow();

        private int timeLeft = 20;
        private int loginFailCount = 0;

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

        private string loginLimitCount = string.Empty;
        public string LoginLimitCount
        {
            get { return this.loginLimitCount; }
            set { this.loginLimitCount = value; Notify("LoginLimitCount"); }
        }

        private string[] showPwImageSource = new string[3] { $"{Config.ImageSource}/eyeFalseNormal.png", $"{Config.ImageSource}/eyeFalseHover.png", $"{Config.ImageSource}/eyeFalsePressed.png" };
        public string[] ShowPwImageSource
        {
            get { return this.showPwImageSource; }
            set { this.showPwImageSource = value; Notify("ShowPwImageSource"); }
        }

        private bool isShowPwCheck = false;
        public bool IsShowPwCheck
        {
            get { return this.isShowPwCheck; }
            set { this.isShowPwCheck = value; Notify("IsShowPwCheck"); }
        }

        private bool isPwPasswordBoxVisibility = true;
        public bool IsPwPasswordBoxVisibility
        {
            get { return this.isPwPasswordBoxVisibility; }
            set { this.isPwPasswordBoxVisibility = value; Notify("IsPwPasswordBoxVisibility"); }
        }

        private bool isPwTextBoxVisibility = false;
        public bool IsPwTextBoxVisibility
        {
            get { return this.isPwTextBoxVisibility; }
            set { this.isPwTextBoxVisibility = value; Notify("IsPwTextBoxVisibility"); }
        }

        private bool loginLimitVisibility = false;
        public bool LoginLimitVisibility
        {
            get { return this.loginLimitVisibility; }
            set { this.loginLimitVisibility = value; Notify("LoginLimitVisibility"); }
        }

        private bool isLoginButtonEnabled = true;
        public bool IsLoginButtonEnabled
        {
            get { return this.isLoginButtonEnabled; }
            set { this.isLoginButtonEnabled = value; Notify("IsLoginButtonEnabled"); }
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

        private bool isPwPasswordBoxFocus = false;
        public bool IsPwPasswordBoxFocus
        {
            get { return this.isPwPasswordBoxFocus; }
            set { this.isPwPasswordBoxFocus = value; Notify("IsPwPasswordBoxFocus"); }
        }

        private DelegateCommand commandSignUpClick = null;
        private DelegateCommand commandLoginClick = null;
        private DelegateCommand commandExitClick = null;
        private DelegateCommand commandShowPwClick = null;

        public DelegateCommand CommandSignUpClick
        {
            get => this.commandSignUpClick;
            set => this.commandSignUpClick = value;
        }

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

        public DelegateCommand CommandShowPwClick
        {
            get => this.commandShowPwClick;
            set => this.commandShowPwClick = value;
        }


        private void LoginClick(object obj)
        {
            LoginInit();

            string tableName = "users";
            string selectQuery = string.Format("SELECT * FROM {0} WHERE user_id = '{1}'", tableName, UserId);

            DataSet userDataSet = manager.Select(selectQuery, tableName);

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(UserPw))
            {
                if (!string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(UserPw))
                {
                    IsPwPasswordBoxFocus = true;
                    IsPwTextBoxFocus = true;
                    PwErrorMessage = "비밀번호를 입력해주세요.";
                }
                else
                {
                    IsIdTextBoxFocus = true;
                    IdErrorMessage = "아이디를 입력해주세요.";
                }
            }
            else
            {
                if (userDataSet.Tables[0].Rows.Count > 0)
                {
                    if (userDataSet.Tables[0].Rows[0]["user_id"].ToString() == UserId && userDataSet.Tables[0].Rows[0]["user_pw"].ToString() == UserPw)
                    {
                        UserId = string.Empty;
                        UserPw = string.Empty;
                        loginFailCount = 0;
                        ((LoginMainView)Application.Current.MainWindow).Close();

                        mainWindow.Show();
                    }
                    else { LoginFail(); }
                }
                else { LoginFail(); }

                if (loginFailCount > 5)
                {
                    if (loginFailCount == 8)
                    {
                        DispatcherTimer timer = new DispatcherTimer();

                        IsLoginButtonEnabled = false;
                        LoginLimitVisibility = true;

                        timer.Start();
                        timer.Interval = TimeSpan.FromMilliseconds(1000);
                        timer.Tick += (o, e) =>
                        {
                            LoginLimitCount = timeLeft.ToString();
                            timeLeft--;

                            if (timeLeft < 0)
                            {
                                timer.Stop();
                                timeLeft = 20;
                                loginFailCount = 0;
                                IsLoginButtonEnabled = true;
                                LoginLimitVisibility = false;
                            }
                        };
                    }
                    else
                    {
                        if (isShowPwCheck) { IsPwTextBoxVisibility = false; }
                        else { IsPwPasswordBoxVisibility = false; }
                        LoginMainViewModel.messageBoxViewModel.ErrorText = "비밀번호 " + loginFailCount + "회 오류입니다.\n\n8회 이상 실패시 로그인이 제한될 수 있습니다.";
                        LoginMainViewModel.messageBoxViewModel.IsMessageBoxVisibility = true;
                    }
                }
            }
        }

        private void ShowPwClick(object obj)
        {
            isShowPwCheck = !isShowPwCheck;
            if (isShowPwCheck)
            {
                IsPwPasswordBoxVisibility = false;
                IsPwTextBoxVisibility = true;
                ShowPwImageSource = new string[3] { $"{Config.ImageSource}/eyeTrueNormal.png", $"{Config.ImageSource}/eyeTrueHover.png", $"{Config.ImageSource}/eyeTruePressed.png" };
            }
            else
            {
                IsPwPasswordBoxVisibility = true;
                IsPwTextBoxVisibility = false;
                ShowPwImageSource = new string[3] { $"{Config.ImageSource}/eyeFalseNormal.png", $"{Config.ImageSource}/eyeFalseHover.png", $"{Config.ImageSource}/eyeFalsePressed.png" };
            }
        }

        private void ExitClick(object obj)
        {
            // 모두 종료
            manager.CloseMySqlConnection();
            Environment.Exit(0);
        }

        private void LoginInit()
        {
            IdErrorMessage = string.Empty;
            PwErrorMessage = string.Empty;
            IsIdTextBoxFocus = false;
            IsPwTextBoxFocus = false;
            IsPwPasswordBoxFocus = false;
        }

        private void LoginFail()
        {
            loginFailCount++;
            UserPw = string.Empty;
            IsPwPasswordBoxFocus = true;
            IsPwTextBoxFocus = true;
            PwErrorMessage = "아이디 또는 비밀번호가 틀렸습니다.";
        }

        //private void SignUpClick(object obj)
        //{
        //    string tableName = "users";
        //    string selectQuery = string.Format("SELECT * FROM {0} WHERE userId = '{1}'", tableName, UserId);

        //    DataSet userDataSet = manager.Select(selectQuery, tableName);

        //    if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(UserPw))
        //    {
        //        MessageBox.Show("값을 모두 입력해주세요.");
        //    }
        //    else
        //    {
        //        if (userDataSet.Tables[0].Rows.Count > 0)
        //        {
        //            MessageBox.Show("이미 사용중인 아이디입니다.");
        //        }
        //        else
        //        {
        //            string insertQuery = string.Format("INSERT INTO {0} VALUES('{1}','{2}')", tableName, UserId, UserPw);
        //            if (manager.MySqlQueryExecuter(insertQuery))
        //            {
        //                MessageBox.Show("회원가입에 성공하셨습니다!");

        //            }
        //            else
        //            {
        //                MessageBox.Show("회원가입에 실패하셨습니다!");
        //            }
        //        }
        //    }
        //}
    }
}
