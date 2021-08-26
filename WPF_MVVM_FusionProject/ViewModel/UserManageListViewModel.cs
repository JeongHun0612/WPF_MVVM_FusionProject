using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_MVVM_FusionProject.Model;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class UserManageListViewModel : Notifier
    {
        public UserManageListViewModel()
        {
            MainWindowViewModel.userManageListViewModel = this;

            PopulateUserList();
            this.commandAddMemberClick = new DelegateCommand(AddMemberClick);
        }

        private int lastPrimaryKey = 0;
        public int LastPrimaryKey
        {
            get { return this.lastPrimaryKey; }
            set { this.lastPrimaryKey = value; Notify("LastPrimaryKey"); }
        }

        private bool isAddMember = false;
        public bool IsAddMember
        {
            get { return this.isAddMember; }
            set { this.isAddMember = value; Notify("IsAddMember"); }
        }

        private UserManageTreeModel selectedItem;
        public UserManageTreeModel SelectedItem
        {
            get { return this.selectedItem; }
            set { this.selectedItem = value; Notify("SelectedItem"); }
        }

        private ObservableCollection<UserManageListModel> userListCollection = new ObservableCollection<UserManageListModel>();
        public ObservableCollection<UserManageListModel> UserListCollection
        {
            get { return this.userListCollection; }
            set { this.userListCollection = value; Notify("UserListCollection"); }
        }

        private DelegateCommand commandAddMemberClick = null;
        public DelegateCommand CommandAddMemberClick
        {
            get => this.commandAddMemberClick;
            set => this.commandAddMemberClick = value;
        }

        private void PopulateUserList()
        {
            string tableName = "users";
            string selectUserQuery = string.Format("SELECT * FROM {0}", tableName);
            DataSet userDataSet = MainWindowViewModel.manager.Select(selectUserQuery, tableName);

            foreach (DataRow item in userDataSet.Tables[0].Rows)
            {
                string primaryKey = item["id"].ToString();
                string userName = item["user_name"].ToString();
                string userBirth = item["user_birth"].ToString();
                string userId = item["user_id"].ToString();
                string userPw = item["user_pw"].ToString();
                string userDepartment = item["user_department"].ToString();
                string userEmployeeNum = item["user_employee_num"].ToString();
                string userNumber = item["user_number"].ToString();
                string userGroupName = item["group_name"].ToString();
                string userGroupId = item["group_id"].ToString();
                byte[] blob = (item["user_image"].ToString() != string.Empty) ? (byte[])item["user_image"] : null;

                UserListCollection.Add(new UserManageListModel(primaryKey, userName, userBirth, userId, userPw, userDepartment, userEmployeeNum, userNumber, userGroupName, userGroupId, UserImageShow(blob)));
                LastPrimaryKey = int.Parse(primaryKey);
            }
        }

        public BitmapImage UserImageShow(byte[] blob)
        {
            if(blob == null)
            {
                return null;
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;

                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }
        }

        private void AddMemberClick(object obj)
        {
            if (!isAddMember)
            {
                isAddMember = true;
                if (SelectedItem != null)
                {
                    if (SelectedItem.DepthCount == 0)
                    {
                        foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                        {
                            if (item.IsHeader && (SelectedItem.PrimaryKey == item.GroupId))
                            {
                                int index = MainWindowViewModel.userManageTreeViewModel.UserGroupList.IndexOf(item);
                                UserListCollection.Insert(0, new UserManageListModel(MainWindowViewModel.userManageTreeViewModel.UserGroupList, index + 1));
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                        {
                            string SelectedItemPrimaryKey = (SelectedItem.DepthCount == 1) ? SelectedItem.PrimaryKey : SelectedItem.ParentPrimaryKey;

                            if (!item.IsHeader && SelectedItemPrimaryKey == item.GroupId)
                            {
                                int index = MainWindowViewModel.userManageTreeViewModel.UserGroupList.IndexOf(item);
                                UserListCollection.Insert(0, new UserManageListModel(MainWindowViewModel.userManageTreeViewModel.UserGroupList, index));
                                break;
                            }
                        }
                    }

                }
                else
                {
                    UserListCollection.Insert(0, new UserManageListModel(MainWindowViewModel.userManageTreeViewModel.UserGroupList, 1));
                }
            }
        }
    }
}
