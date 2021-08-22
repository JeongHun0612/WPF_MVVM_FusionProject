using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_FusionProject.Model;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class UserManageTreeViewModel : Notifier
    {
        public UserManageTreeViewModel()
        {
            MainWindowViewModel.userManageTreeViewModel = this;

            PopulateParentGroupTree();
            this.commandSelectedItem = new DelegateCommand(SelectedItem);
            this.commandTreeRootAddClick = new DelegateCommand(TreeRootAddClick);
            this.commandTreeChildAddClick = new DelegateCommand(TreeChildAddClick);
            this.commandTreeUserAddClick = new DelegateCommand(TreeUserAddClick);
        }

        private string[] tableName = new string[3] { "userparentgroup", "usergroup", "users" };

        private ObservableCollection<UserManageTreeModel> parentGroupList = new ObservableCollection<UserManageTreeModel>();
        public ObservableCollection<UserManageTreeModel> ParentGroupList
        {
            get { return this.parentGroupList; }
            set { this.parentGroupList = value; Notify("ParentGroupList"); }
        }

        private ObservableCollection<ComboBoxGroupListModel> userGroupList = new ObservableCollection<ComboBoxGroupListModel>();
        public ObservableCollection<ComboBoxGroupListModel> UserGroupList
        {
            get { return this.userGroupList; }
            set { this.userGroupList = value; Notify("UserGroupList"); }
        }

        private bool isTreeNodeEdit = false;
        public bool IsTreeNodeEdit
        {
            get { return this.isTreeNodeEdit; }
            set { this.isTreeNodeEdit = value; Notify("IsTreeNodeEdit"); }
        }

        private string isEditStatus = string.Empty;
        public string IsEditStatus
        {
            get { return this.isEditStatus; }
            set { this.isEditStatus = value; Notify("IsEditStatus"); }
        }

        private DelegateCommand commandTreeRootAddClick = null;
        private DelegateCommand commandTreeChildAddClick = null;
        private DelegateCommand commandTreeUserAddClick = null;
        private DelegateCommand commandSelectedItem = null;

        public DelegateCommand CommandTreeRootAddClick
        {
            get => this.commandTreeRootAddClick;
            set => this.commandTreeRootAddClick = value;
        }

        public DelegateCommand CommandTreeChildAddClick
        {
            get => this.commandTreeChildAddClick;
            set => this.commandTreeChildAddClick = value;
        }

        public DelegateCommand CommandTreeUserAddClick
        {
            get => this.commandTreeUserAddClick;
            set => this.commandTreeUserAddClick = value;
        }

        public DelegateCommand CommandSelectedItem
        {
            get => this.commandSelectedItem;
            set => this.commandSelectedItem = value;
        }

        private void PopulateParentGroupTree()
        {
            string parentGroupSelectQuery = string.Format("SELECT * FROM {0}", tableName[0]);
            DataSet parentGroupDataSet = MainWindowViewModel.manager.Select(parentGroupSelectQuery, tableName[0]);

            foreach (DataRow item in parentGroupDataSet.Tables[0].Rows)
            {
                string parentGroupHeader = item["parent_group_name"].ToString();
                string parentGroupPrimaryKey = item["parent_group_id"].ToString();
                UserManageTreeModel parentGroupNode = new UserManageTreeModel(0, parentGroupPrimaryKey, string.Empty, parentGroupHeader, false, false);

                ParentGroupList.Add(parentGroupNode);
                UserGroupList.Add(new ComboBoxGroupListModel(parentGroupHeader, true, parentGroupPrimaryKey));
                PopulateUserGroupTree(parentGroupNode, parentGroupPrimaryKey);
            }
        }

        private void PopulateUserGroupTree(UserManageTreeModel parentGroupNode, string parentGroupPrimaryKey)
        {
            string userGroupSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_id = '{1}'", tableName[1], parentGroupPrimaryKey);
            DataSet userGroupDataSet = MainWindowViewModel.manager.Select(userGroupSelectQuery, tableName[1]);

            foreach (DataRow item in userGroupDataSet.Tables[0].Rows)
            {
                string userGroupHeader = item["group_name"].ToString();
                string userGroupPrimaryKey = item["group_id"].ToString();

                UserManageTreeModel userGroupNode = new UserManageTreeModel(1, userGroupPrimaryKey, parentGroupPrimaryKey, userGroupHeader, false, false);

                parentGroupNode.ChildGroupList.Add(userGroupNode);
                UserGroupList.Add(new ComboBoxGroupListModel(userGroupHeader, false, userGroupPrimaryKey, parentGroupPrimaryKey));
                PopulateUserTree(userGroupNode, userGroupPrimaryKey);
            }
        }

        private void PopulateUserTree(UserManageTreeModel userGroupNode, string userGroupPrimaryKey)
        {
            string userSelectQuery = string.Format("SELECT * FROM {0} WHERE group_id = '{1}'", tableName[2], userGroupPrimaryKey);
            DataSet userDataSet = MainWindowViewModel.manager.Select(userSelectQuery, tableName[2]);

            foreach (DataRow item in userDataSet.Tables[0].Rows)
            {
                string userHeader = item["user_name"].ToString();
                string userPrimaryKey = item["id"].ToString();

                UserManageTreeModel userNode = new UserManageTreeModel(2, userPrimaryKey, userGroupPrimaryKey, userHeader, false, false);
                userGroupNode.ChildGroupList.Add(userNode);
            }
        }

        private void SelectedItem(object obj)
        {
            UserManageTreeModel selectedItem = obj as UserManageTreeModel;
            MainWindowViewModel.userManageListViewModel.IsAddMember = false;
            MainWindowViewModel.userManageListViewModel.SelectedItem = selectedItem;

            //MainWindowViewModel.userManageListViewModel.SelectUserListCollection = new ObservableCollection<UserManageListModel>();
            MainWindowViewModel.userManageListViewModel.SelectUserListCollection.Clear();

            ObservableCollection<UserManageListModel> userListCollection = MainWindowViewModel.userManageListViewModel.UserListCollection;
            ObservableCollection<UserManageListModel> selectUserListCollection = MainWindowViewModel.userManageListViewModel.SelectUserListCollection;

            if (selectedItem != null)
            {
                switch (selectedItem.DepthCount)
                {
                    case 0:
                        foreach(UserManageListModel Listitem in userListCollection)
                        {
                            foreach (UserManageTreeModel Treeitem in selectedItem.ChildGroupList)
                            {
                                if(Listitem.UserGroupId == Treeitem.PrimaryKey)
                                {
                                    PopulateUserList(Listitem, selectUserListCollection);
                                }
                            }
                        }
                        break;
                    case 1:
                        foreach (UserManageListModel Listitem in userListCollection)
                        {
                            if (Listitem.UserGroupId == selectedItem.PrimaryKey)
                            {
                                PopulateUserList(Listitem, selectUserListCollection);
                            }
                        }
                        break;
                    case 2:
                        foreach (UserManageListModel Listitem in userListCollection)
                        {
                            if(Listitem.PrimaryKey == selectedItem.PrimaryKey)
                            {
                                PopulateUserList(Listitem, selectUserListCollection);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void PopulateUserList(UserManageListModel Listitem, ObservableCollection<UserManageListModel> selectUserListCollection)
        {
            string primaryKey = Listitem.PrimaryKey;
            string userName = Listitem.UserName;
            string userBirth = Listitem.UserBirth;
            string userId = Listitem.UserId;
            string userPw = Listitem.UserPw;
            string userDepartment = Listitem.UserDepartment;
            string userEmployeeNum = Listitem.UserEmployeeNum;
            string userNumber = Listitem.UserNumber;
            string userGroupName = Listitem.UserGroupName;
            string userGroupId = Listitem.UserGroupId;

            selectUserListCollection.Add(new UserManageListModel(primaryKey, userName, userBirth, userId, userPw, userDepartment, userEmployeeNum, userNumber, userGroupName, userGroupId));
        }

        private void TreeRootAddClick(object obj)
        {
            if (!IsTreeNodeEdit)
            {
                IsEditStatus = "Add";
                IsTreeNodeEdit = true;
                UserManageTreeModel parentGroupNode = new UserManageTreeModel(0, string.Empty, string.Empty, string.Empty, true, true);
                ParentGroupList.Add(parentGroupNode);
            }
        }

        private void TreeChildAddClick(object obj)
        {
            string PrimaryKey = obj as string;
            if (!IsTreeNodeEdit)
            {
                foreach (UserManageTreeModel item in ParentGroupList)
                {
                    if (item.PrimaryKey == PrimaryKey)
                    {
                        IsEditStatus = "Add";
                        IsTreeNodeEdit = true;
                        UserManageTreeModel userGroupNode = new UserManageTreeModel(1, string.Empty, item.PrimaryKey, string.Empty, true, true);
                        item.ChildGroupList.Add(userGroupNode);
                    }
                }
            }
        }

        private void TreeUserAddClick(object obj)
        {
            if (!MainWindowViewModel.userManageListViewModel.IsAddMember)
            {
                MainWindowViewModel.userManageListViewModel.SelectUserListCollection.Insert(0, new UserManageListModel(MainWindowViewModel.userManageTreeViewModel.UserGroupList, 1));
            }
            MainWindowViewModel.userManageListViewModel.IsAddMember = true;
        }
    }
}
