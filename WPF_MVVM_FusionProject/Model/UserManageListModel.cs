using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_MVVM_FusionProject.View;
using WPF_MVVM_FusionProject.ViewModel;

namespace WPF_MVVM_FusionProject.Model
{
    public class UserManageListModel : NotifierCollection
    {
        #region UserManageListModel 생성자
        public UserManageListModel(ObservableCollection<ComboBoxGroupListModel> userGroupList, int comboBoxSelectedIndex)
        {
            this.primaryKey = string.Empty;
            this.userName = string.Empty;
            this.userBirth = string.Empty;
            this.userId = string.Empty;
            this.tempUserId = string.Empty;
            this.userPw = string.Empty;
            this.userDepartment = string.Empty;
            this.userEmployeeNum = string.Empty;
            this.userNumber = string.Empty;
            this.UserGroupList = userGroupList;
            this.comboBoxSelectedIndex = comboBoxSelectedIndex;
            this.UserGroupName = string.Empty;
            this.UserGroupId = string.Empty;

            this.isReadOnly = false;
            this.isUserListVisibility = true;
            this.isUserPwTextBoxVisibility = true;
            this.isUserPwPasswordBoxVisibility = false;
            this.isGroupListVisibility = true;
            this.isSaveBtnVisibility = true;
            this.isImageEditBtnVisibility = true;
            this.isCancleBtnVisibility = true;
            this.isEditBtnVisibility = false;
            this.isDeleteBtnVisibility = false;
            this.isNameTextBoxFocus = true;

            this.commandUserEditClick = new DelegateCommand(UserEditClick);
            this.commandUserImageEditClick = new DelegateCommand(UserImageEditClick);
            this.commandUserDeleteClick = new DelegateCommand(UserDeleteClick);
            this.commandUserSaveClick = new DelegateCommand(UserSaveClick);
            this.commandUserCancleClick = new DelegateCommand(UserCancleClick);
        }

        public UserManageListModel(string primaryKey, string userName, string userBirth, string userId, string userPw, string userDepartment, string userEmployeeNum, string userNumber, string userGroupName, string userGroupId, BitmapImage profileImage)
        {
            this.primaryKey = primaryKey;
            this.userName = userName;
            this.userBirth = userBirth;
            this.userId = userId;
            this.tempUserId = userId;
            this.userPw = userPw;
            this.userDepartment = userDepartment;
            this.userEmployeeNum = userEmployeeNum;
            this.userNumber = userNumber;
            this.userGroupName = userGroupName;
            this.userGroupId = userGroupId;
            this.profileImage = profileImage;

            this.commandUserEditClick = new DelegateCommand(UserEditClick);
            this.commandUserImageEditClick = new DelegateCommand(UserImageEditClick);
            this.commandUserDeleteClick = new DelegateCommand(UserDeleteClick);
            this.commandUserSaveClick = new DelegateCommand(UserSaveClick);
            this.commandUserCancleClick = new DelegateCommand(UserCancleClick);
        }
        #endregion

        #region UserManageListModel 변수
        private string primaryKey = string.Empty;
        public string PrimaryKey
        {
            get { return this.primaryKey; }
            set { this.primaryKey = value; NotifyCollection("PrimaryKey"); }
        }

        private string userName = string.Empty;
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; NotifyCollection("UserName"); }
        }

        private string userBirth = string.Empty;
        public string UserBirth
        {
            get { return this.userBirth; }
            set { this.userBirth = value; NotifyCollection("UserBirth"); }
        }

        private string userId = string.Empty;
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; NotifyCollection("UserId"); }
        }

        private string tempUserId = string.Empty;
        public string TempUserId
        {
            get { return this.tempUserId; }
            set { this.tempUserId = value; NotifyCollection("TempUserId"); }
        }

        private string userPw = string.Empty;
        public string UserPw
        {
            get { return this.userPw; }
            set { this.userPw = value; NotifyCollection("UserPw"); }
        }

        private string userDepartment = string.Empty;
        public string UserDepartment
        {
            get { return this.userDepartment; }
            set { this.userDepartment = value; NotifyCollection("UserDepartment"); }
        }

        private string userEmployeeNum = string.Empty;
        public string UserEmployeeNum
        {
            get { return this.userEmployeeNum; }
            set { this.userEmployeeNum = value; NotifyCollection("UserEmployeeNum"); }
        }

        private string userNumber = string.Empty;
        public string UserNumber
        {
            get { return this.userNumber; }
            set { this.userNumber = value; NotifyCollection("UserNumber"); }
        }

        private ObservableCollection<ComboBoxGroupListModel> userGroupList = new ObservableCollection<ComboBoxGroupListModel>();
        public ObservableCollection<ComboBoxGroupListModel> UserGroupList
        {
            get { return this.userGroupList; }
            set { this.userGroupList = value; NotifyCollection("UserGroupList"); }
        }

        private int comboBoxSelectedIndex;
        public int ComboBoxSelectedIndex
        {
            get { return this.comboBoxSelectedIndex; }
            set { this.comboBoxSelectedIndex = value; NotifyCollection("ComboBoxSelectedIndex"); }
        }

        private string userGroupName = string.Empty;
        public string UserGroupName
        {
            get { return this.userGroupName; }
            set { this.userGroupName = value; NotifyCollection("UserGroupName"); }
        }

        private string userGroupId = string.Empty;
        public string UserGroupId
        {
            get { return this.userGroupId; }
            set { this.userGroupId = value; NotifyCollection("UserGroupId"); }
        }

        private ImageSource profileImage;
        public ImageSource ProfileImage
        {
            get { return this.profileImage; }
            set { this.profileImage = value; NotifyCollection("ProfileImage"); }
        }

        private bool isReadOnly = true;
        public bool IsReadOnly
        {
            get { return this.isReadOnly; }
            set { this.isReadOnly = value; NotifyCollection("IsReadOnly"); }
        }

        private bool isUserListVisibility = true;
        public bool IsUserListVisibility
        {
            get { return this.isUserListVisibility; }
            set { this.isUserListVisibility = value; NotifyCollection("IsUserListVisibility"); }
        }

        private bool isUserPwTextBoxVisibility = false;
        public bool IsUserPwTextBoxVisibility
        {
            get { return this.isUserPwTextBoxVisibility; }
            set { this.isUserPwTextBoxVisibility = value; NotifyCollection("IsUserPwTextBoxVisibility"); }
        }

        private bool isUserPwPasswordBoxVisibility = true;
        public bool IsUserPwPasswordBoxVisibility
        {
            get { return this.isUserPwPasswordBoxVisibility; }
            set { this.isUserPwPasswordBoxVisibility = value; NotifyCollection("IsUserPwPasswordBoxVisibility"); }
        }

        private bool isGroupListVisibility = false;
        public bool IsGroupListVisibility
        {
            get { return this.isGroupListVisibility; }
            set { this.isGroupListVisibility = value; NotifyCollection("IsGroupListVisibility"); }
        }

        private bool isEditBtnVisibility = true;
        public bool IsEditBtnVisibility
        {
            get { return this.isEditBtnVisibility; }
            set { this.isEditBtnVisibility = value; NotifyCollection("IsEditBtnVisibility"); }
        }

        private bool isImageEditBtnVisibility = false;
        public bool IsImageEditBtnVisibility
        {
            get { return this.isImageEditBtnVisibility; }
            set { this.isImageEditBtnVisibility = value; NotifyCollection("IsImageEditBtnVisibility"); }
        }

        private bool isDeleteBtnVisibility = true;
        public bool IsDeleteBtnVisibility
        {
            get { return this.isDeleteBtnVisibility; }
            set { this.isDeleteBtnVisibility = value; NotifyCollection("IsDeleteBtnVisibility"); }
        }

        private bool isSaveBtnVisibility = false;
        public bool IsSaveBtnVisibility
        {
            get { return this.isSaveBtnVisibility; }
            set { this.isSaveBtnVisibility = value; NotifyCollection("IsSaveBtnVisibility"); }
        }

        private bool isCancleBtnVisibility = false;
        public bool IsCancleBtnVisibility
        {
            get { return this.isCancleBtnVisibility; }
            set { this.isCancleBtnVisibility = value; NotifyCollection("IsCancleBtnVisibility"); }
        }

        private bool isNameTextBoxFocus = false;
        public bool IsNameTextBoxFocus
        {
            get { return this.isNameTextBoxFocus; }
            set { this.isNameTextBoxFocus = value; NotifyCollection("IsNameTextBoxFocus"); }
        }

        private bool isIdTextBoxFocus = false;
        public bool IsIdTextBoxFocus
        {
            get { return this.isIdTextBoxFocus; }
            set { this.isIdTextBoxFocus = value; NotifyCollection("IsIdTextBoxFocus"); }
        }

        string strName, imageName;
        #endregion

        private DelegateCommand commandUserEditClick = null;
        private DelegateCommand commandUserImageEditClick = null;
        private DelegateCommand commandUserDeleteClick = null;
        private DelegateCommand commandUserSaveClick = null;
        private DelegateCommand commandUserCancleClick = null;

        public DelegateCommand CommandUserEditClick
        {
            get => this.commandUserEditClick;
            set => this.commandUserEditClick = value;
        }

        public DelegateCommand CommandUserImageEditClick
        {
            get => this.commandUserImageEditClick;
            set => this.commandUserImageEditClick = value;
        }

        public DelegateCommand CommandUserDeleteClick
        {
            get => this.commandUserDeleteClick;
            set => this.commandUserDeleteClick = value;
        }

        public DelegateCommand CommandUserSaveClick
        {
            get => this.commandUserSaveClick;
            set => this.commandUserSaveClick = value;
        }
        public DelegateCommand CommandUserCancleClick
        {
            get => this.commandUserCancleClick;
            set => this.commandUserCancleClick = value;
        }

        private void UserEditClick(object obj)
        {
            UserGroupList = MainWindowViewModel.userManageTreeViewModel.UserGroupList;

            foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
            {
                if (!item.IsHeader && (item.GroupId == UserGroupId))
                {
                    int index = MainWindowViewModel.userManageTreeViewModel.UserGroupList.IndexOf(item);
                    ComboBoxSelectedIndex = index;
                }
            }

            IsReadOnly = false;
            IsSaveBtnVisibility = true;
            IsCancleBtnVisibility = true;
            IsGroupListVisibility = true;
            IsImageEditBtnVisibility = true;
        }

        private void UserImageEditClick(object obj)
        {
            try
            {
                OpenFileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.png; *.jpg;*.bmp;)|*.png;*.jpg;*.bmp9;";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;

                    if (imageName != string.Empty)
                    {
                        FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                        byte[] imgByteArr = new byte[fs.Length];
                        fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        MemoryStream stream = new MemoryStream();
                        stream.Write(imgByteArr, 0, imgByteArr.Length);
                        stream.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();

                        ProfileImage = bi;

                        //ImageSourceConverter isc = new ImageSourceConverter();
                        //image.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    }
                }
                fldlg = null;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        private void InsertImageData()
        {
            try
            {
                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                byte[] imgByteArr = new byte[fs.Length];

                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                string userImageInsertQuery = "UPDATE users SET user_image = @user_image WHERE id = " + PrimaryKey;

                if (MainWindowViewModel.manager.MySqlImageInsertExecuter(userImageInsertQuery, imgByteArr))
                {
                    Debug.WriteLine("Image added successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void UserDeleteClick(object obj)
        {
            WarningMessageBoxView messageBox = new WarningMessageBoxView();
            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "정말로 삭제하시겠습니까?";
            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 0;
            messageBox.ShowDialog();

            if (MainWindowViewModel.warningMessageBoxViewModel.IsMessageBoxResult)
            {
                string tableName = "users";
                string userDeleteQuery = string.Format("DELETE FROM {0} WHERE id = '{1}'", tableName, PrimaryKey);
                ObservableCollection<UserManageListModel> userListCollection = MainWindowViewModel.userManageListViewModel.UserListCollection;

                if (MainWindowViewModel.manager.MySqlQueryExecuter(userDeleteQuery))
                {
                    foreach (UserManageListModel item in userListCollection)
                    {
                        if (item.PrimaryKey == PrimaryKey)
                        {
                            userListCollection.Remove(item);
                            break;
                        }
                    }
                    MainWindowViewModel.userManageListViewModel.UserListCollection.Remove(this);
                    UserManageTreeDeleteInit();
                }
            }
            else
            {
                Debug.WriteLine("Cancle");
            }
        }

        private void UserSaveClick(object obj)
        {
            ComboBoxGroupListModel selectedItem = obj as ComboBoxGroupListModel;
            ObservableCollection<UserManageListModel> userListCollection = MainWindowViewModel.userManageListViewModel.UserListCollection;

            string tableName = "users";

            if (UserName != string.Empty && UserBirth != string.Empty && UserId != string.Empty && UserPw != string.Empty && UserDepartment != string.Empty && UserEmployeeNum != string.Empty && selectedItem != null && UserNumber != string.Empty)
            {
                string userSelectQuery = string.Format("SELECT * FROM {0} WHERE user_id = '{1}'", tableName, UserId);
                DataSet userDataSet = MainWindowViewModel.manager.Select(userSelectQuery, tableName);

                if (userDataSet.Tables[0].Rows.Count > 0 && (TempUserId != UserId))
                {
                    WarningMessageBoxView messageBox = new WarningMessageBoxView();
                    MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "이미 사용중인 아이디입니다.";
                    MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                    messageBox.ShowDialog();
                    IsIdTextBoxFocus = true;
                }
                else
                {
                    if (IsEditBtnVisibility)
                    {
                        string userUpdateQuery = string.Format("UPDATE {0} SET user_name='{1}', user_birth='{2}', user_id='{3}', user_pw='{4}', user_department='{5}', user_employee_num='{6}', user_number='{7}', group_name='{8}', group_id='{9}' WHERE id = '{10}'", tableName, UserName, UserBirth, UserId, UserPw, UserDepartment, UserEmployeeNum, UserNumber, selectedItem.GroupName, selectedItem.GroupId, PrimaryKey);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(userUpdateQuery))
                        {
                            if (selectedItem.GroupId != UserGroupId)
                            {
                                foreach (UserManageListModel item in userListCollection)
                                {
                                    if (item.PrimaryKey == PrimaryKey)
                                    {
                                        item.UserGroupId = selectedItem.GroupId;
                                        item.UserGroupName = selectedItem.GroupName;
                                    }
                                }

                                IsUserListVisibility = false;
                                UserManageTreeDeleteInit();
                                UserGroupId = selectedItem.GroupId;
                                UserGroupName = selectedItem.GroupName;
                                UserManageTreeAddInit(UserGroupId, PrimaryKey);
                            }
                            TempUserId = UserId;
                            IsReadOnly = true;
                            IsSaveBtnVisibility = false;
                            IsCancleBtnVisibility = false;
                            IsGroupListVisibility = false;
                            IsImageEditBtnVisibility = false;

                            if (imageName != null)
                            {
                                InsertImageData();
                            }
                        }
                    }
                    else
                    {
                        int primaryKey = (MainWindowViewModel.userManageListViewModel.UserListCollection.Count == 0) ? 1 : MainWindowViewModel.userManageListViewModel.LastPrimaryKey + 1;

                        string userInsertQuery = string.Format("INSERT INTO {0}(id, user_name, user_birth, user_id, user_pw, user_department, user_employee_num, user_number, group_name, group_id)" +
                        " VALUES('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", tableName, primaryKey, UserName, UserBirth, UserId, UserPw, UserDepartment, UserEmployeeNum, UserNumber, selectedItem.GroupName, selectedItem.GroupId);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(userInsertQuery))
                        {
                            PrimaryKey = primaryKey.ToString();
                            UserGroupName = selectedItem.GroupName;
                            UserGroupId = selectedItem.GroupId;
                            TempUserId = UserId;

                            IsReadOnly = true;
                            IsEditBtnVisibility = true;
                            IsDeleteBtnVisibility = true;
                            IsSaveBtnVisibility = false;
                            IsCancleBtnVisibility = false;
                            IsGroupListVisibility = false;
                            IsImageEditBtnVisibility = false;
                            IsUserPwTextBoxVisibility = false;
                            IsUserPwPasswordBoxVisibility = true;
                            MainWindowViewModel.userManageListViewModel.IsAddMember = false;
                            MainWindowViewModel.userManageListViewModel.LastPrimaryKey = primaryKey;
                            UserManageTreeAddInit(selectedItem.GroupId, primaryKey.ToString());

                            if (imageName != null)
                            {
                                InsertImageData();
                            }
                        }

                    }
                }
            }
            else
            {
                WarningMessageBoxView messageBox = new WarningMessageBoxView();
                MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "값을 모두 입력해주세요.";
                MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                messageBox.ShowDialog();
            }
        }

        private void UserCancleClick(object obj)
        {
            if (IsEditBtnVisibility)
            {
                string tableName = "users";
                string selectUserQuery = string.Format("SELECT * FROM {0} WHERE id = '{1}'", tableName, PrimaryKey);
                DataSet userDataSet = MainWindowViewModel.manager.Select(selectUserQuery, tableName);

                PrimaryKey = userDataSet.Tables[0].Rows[0]["id"].ToString();
                UserName = userDataSet.Tables[0].Rows[0]["user_name"].ToString();
                UserBirth = userDataSet.Tables[0].Rows[0]["user_birth"].ToString();
                UserId = userDataSet.Tables[0].Rows[0]["user_id"].ToString();
                UserPw = userDataSet.Tables[0].Rows[0]["user_pw"].ToString();
                UserDepartment = userDataSet.Tables[0].Rows[0]["user_department"].ToString();
                UserEmployeeNum = userDataSet.Tables[0].Rows[0]["user_employee_num"].ToString();
                UserNumber = userDataSet.Tables[0].Rows[0]["user_number"].ToString();
                UserGroupName = userDataSet.Tables[0].Rows[0]["group_name"].ToString();
                UserGroupId = userDataSet.Tables[0].Rows[0]["group_id"].ToString();
                ProfileImage = (userDataSet.Tables[0].Rows[0]["user_image"].ToString() != string.Empty) ? MainWindowViewModel.userManageListViewModel.UserImageShow((byte[])userDataSet.Tables[0].Rows[0]["user_image"]) : null;

                IsReadOnly = true;
                IsSaveBtnVisibility = false;
                IsCancleBtnVisibility = false;
                IsGroupListVisibility = false;
                IsImageEditBtnVisibility = false;
            }
            else
            {
                MainWindowViewModel.userManageListViewModel.UserListCollection.Remove(this);
                MainWindowViewModel.userManageListViewModel.IsAddMember = false;
            }
        }

        private void UserManageTreeAddInit(string UserGroupId, string PrimaryKey)
        {
            foreach (UserManageTreeModel item in MainWindowViewModel.userManageTreeViewModel.ParentGroupList)
            {
                foreach (UserManageTreeModel item2 in item.ChildGroupList)
                {
                    if (item2.PrimaryKey == UserGroupId)
                    {
                        item2.ChildGroupList.Insert(0, new UserManageTreeModel(2, PrimaryKey, UserGroupId, UserName, false, false));
                    }
                }
            }
        }

        private void UserManageTreeDeleteInit()
        {
            foreach (UserManageTreeModel item in MainWindowViewModel.userManageTreeViewModel.ParentGroupList)
            {
                foreach (UserManageTreeModel item2 in item.ChildGroupList)
                {
                    foreach (UserManageTreeModel item3 in item2.ChildGroupList)
                    {
                        if (item3.PrimaryKey == PrimaryKey)
                        {
                            item2.ChildGroupList.Remove(item3);
                            break;
                        }
                    }
                }
            }
        }
    }
}
