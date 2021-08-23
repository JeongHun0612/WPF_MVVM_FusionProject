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

        private DelegateCommand commandSelectedItem = null;

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

                UserManageTreeModel userNode = new UserManageTreeModel(2, primaryKey, userGroupPrimaryKey, userName, false, false);
                userGroupNode.ChildGroupList.Add(userNode);
                userGroupNode.UserListCollection.Add(new UserManageListModel(primaryKey, userName, userBirth, userId, userPw, userDepartment, userEmployeeNum, userNumber, userGroupName, userGroupId));
            }
        }

        private void SelectedItem(object obj)
        {
            UserManageTreeModel selectedItem = obj as UserManageTreeModel;
            MainWindowViewModel.userManageListViewModel.IsAddMember = false;
            MainWindowViewModel.userManageListViewModel.SelectedItem = selectedItem;

            //MainWindowViewModel.userManageListViewModel.SelectUserListCollection = new ObservableCollection<UserManageListModel>();
            //MainWindowViewModel.userManageListViewModel.SelectUserListCollection.Clear();

            ObservableCollection<UserManageListModel> userListCollection = MainWindowViewModel.userManageListViewModel.UserListCollection;
            ObservableCollection<UserManageListModel> selectUserListCollection = MainWindowViewModel.userManageListViewModel.SelectUserListCollection;

            foreach (UserManageListModel ListItem in selectUserListCollection)
            {
                if (ListItem.IsUserListVisibility)
                {
                    ListItem.IsUserListVisibility = false;
                }
            }

            if (selectedItem != null)
            {
                switch (selectedItem.DepthCount)
                {
                    case 0:
                        //foreach(UserManageTreeModel TreeItem in selectedItem.ChildGroupList)
                        //{
                        //    if(TreeItem.ChildGroupList.Count != 0)
                        //    {
                        //        foreach (UserManageListModel ListItem in userListCollection)
                        //        {
                        //            if (ListItem.UserGroupId == TreeItem.PrimaryKey)
                        //            {
                        //                PopulateUserList(ListItem, selectUserListCollection);
                        //            }
                        //        }
                        //    }
                        //}

                        foreach (UserManageTreeModel TreeItem in selectedItem.ChildGroupList)
                        {
                            //selectUserListCollection = TreeItem.UserListCollection;

                            //selectUserListCollection = TreeItem.UserListCollection;
                            //if (TreeItem.UserListCollection.Count != 0)
                            //{
                            //    foreach (UserManageListModel ListItem in TreeItem.UserListCollection)
                            //    {
                            //        PopulateUserList(ListItem, selectUserListCollection);
                            //    }
                            //}

                            //if (TreeItem.ChildGroupList.Count != 0)
                            //{
                            //    foreach (UserManageListModel ListItem in userListCollection)
                            //    {
                            //        if (ListItem.UserGroupId == TreeItem.PrimaryKey)
                            //        {
                            //            PopulateUserList(ListItem, selectUserListCollection);
                            //        }
                            //    }
                            //}
                        }
                        break;
                    case 1:
                        //MainWindowViewModel.userManageListViewModel.SelectUserListCollection = selectedItem.UserListCollection;
                        foreach(UserManageListModel ListItem in selectUserListCollection)
                        {
                            if(ListItem.UserGroupId == selectedItem.PrimaryKey)
                            {
                                ListItem.IsUserListVisibility = true;
                            }
                        }

                        //foreach (UserManageListModel ListItem in selectedItem.UserListCollection)
                        //{
                        //    ListItem.UserName = "Test";
                        //    //PopulateUserList(ListItem, selectUserListCollection);
                        //}

                        //if (selectedItem.ChildGroupList.Count != 0)
                        //{
                        //    foreach (UserManageListModel ListItem in userListCollection)
                        //    {
                        //        if (ListItem.UserGroupId == selectedItem.PrimaryKey)
                        //        {
                        //            PopulateUserList(ListItem, selectUserListCollection);
                        //        }
                        //    }
                        //}

                        break;
                    case 2:
                        foreach (UserManageListModel ListItem in userListCollection)
                        {
                            if (ListItem.PrimaryKey == selectedItem.PrimaryKey)
                            {
                                PopulateUserList(ListItem, selectUserListCollection);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void PopulateUserList(UserManageListModel ListItem, ObservableCollection<UserManageListModel> selectUserListCollection)
        {
            string primaryKey = ListItem.PrimaryKey;
            string userName = ListItem.UserName;
            string userBirth = ListItem.UserBirth;
            string userId = ListItem.UserId;
            string userPw = ListItem.UserPw;
            string userDepartment = ListItem.UserDepartment;
            string userEmployeeNum = ListItem.UserEmployeeNum;
            string userNumber = ListItem.UserNumber;
            string userGroupName = ListItem.UserGroupName;
            string userGroupId = ListItem.UserGroupId;

            selectUserListCollection.Add(new UserManageListModel(primaryKey, userName, userBirth, userId, userPw, userDepartment, userEmployeeNum, userNumber, userGroupName, userGroupId));
        }
    }
}
