using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_FusionProject.View;
using WPF_MVVM_FusionProject.ViewModel;

namespace WPF_MVVM_FusionProject.Model
{
    public class UserManageTreeModel : NotifierCollection
    {
        public UserManageTreeModel(int index, int depthCount, string primaryKey, string parentPrimaryKey, string header, bool isTextBoxVisibility, bool isTextBoxFocus)
        {
            this.index = index;
            this.depthCount = depthCount;
            this.primaryKey = primaryKey;
            this.parentPrimaryKey = parentPrimaryKey;
            this.header = header;
            this.isExpanded = false;
            this.inputHeader = string.Empty;
            this.isTextBoxVisibility = isTextBoxVisibility;
            this.isTextBoxFocus = isTextBoxFocus;

            this.commandTreeRootAddClick = new DelegateCommand(TreeRootAddClick);
            this.commandTreeChildAddClick = new DelegateCommand(TreeChildAddClick);
            this.commandTreeUserAddClick = new DelegateCommand(TreeUserAddClick);

            this.commandSaveClick = new DelegateCommand(SaveClick);
            this.commandCancleClick = new DelegateCommand(CancleClick);
            this.commandRenameClick = new DelegateCommand(RenameClick);
            this.commandDeleteClick = new DelegateCommand(DeleteClick);
        }

        public UserManageTreeModel(int depthCount, string primaryKey, string parentPrimaryKey, string header, bool isTextBoxVisibility, bool isTextBoxFocus)
        {
            this.depthCount = depthCount;
            this.primaryKey = primaryKey;
            this.parentPrimaryKey = parentPrimaryKey;
            this.header = header;
            this.isExpanded = false;
            this.inputHeader = string.Empty;
            this.isTextBoxVisibility = isTextBoxVisibility;
            this.isTextBoxFocus = isTextBoxFocus;

            this.commandTreeRootAddClick = new DelegateCommand(TreeRootAddClick);
            this.commandTreeChildAddClick = new DelegateCommand(TreeChildAddClick);
            this.commandTreeUserAddClick = new DelegateCommand(TreeUserAddClick);

            this.commandSaveClick = new DelegateCommand(SaveClick);
            this.commandCancleClick = new DelegateCommand(CancleClick);
            this.commandRenameClick = new DelegateCommand(RenameClick);
            this.commandDeleteClick = new DelegateCommand(DeleteClick);
        }

        private int index = -1;
        public int Index
        {
            get { return this.index; }
            set { this.index = value; NotifyCollection("Index"); }
        }

        private int depthCount;
        public int DepthCount
        {
            get { return this.depthCount; }
            set { this.depthCount = value; NotifyCollection("DepthCount"); }
        }

        private string primaryKey;
        public string PrimaryKey
        {
            get { return this.primaryKey; }
            set { this.primaryKey = value; NotifyCollection("PrimaryKey"); }
        }

        private string parentPrimaryKey;
        public string ParentPrimaryKey
        {
            get { return this.parentPrimaryKey; }
            set { this.parentPrimaryKey = value; NotifyCollection("ParentPrimaryKey"); }
        }

        private string header;
        public string Header
        {
            get { return this.header; }
            set { this.header = value; NotifyCollection("Header"); }
        }

        private string inputHeader;
        public string InputHeader
        {
            get { return this.inputHeader; }
            set { this.inputHeader = value; NotifyCollection("InputHeader"); }
        }

        private bool isTextBoxVisibility = false;
        public bool IsTextBoxVisibility
        {
            get { return this.isTextBoxVisibility; }
            set { this.isTextBoxVisibility = value; NotifyCollection("IsTextBoxVisibility"); }
        }

        private bool isExpanded = false;
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set { this.isExpanded = value; NotifyCollection("IsExpanded"); }
        }

        private bool isTextBoxFocus = false;
        public bool IsTextBoxFocus
        {
            get { return this.isTextBoxFocus; }
            set { this.isTextBoxFocus = value; NotifyCollection("IsTextBoxFocus"); }
        }

        private ObservableCollection<UserManageTreeModel> childGroupList;
        public ObservableCollection<UserManageTreeModel> ChildGroupList
        {
            get
            {
                if (this.childGroupList == null)
                {
                    this.childGroupList = new ObservableCollection<UserManageTreeModel>();
                }
                return this.childGroupList;
            }
            set { this.childGroupList = value; NotifyCollection("ChildGroupList"); }
        }

        private DelegateCommand commandTreeRootAddClick = null;
        private DelegateCommand commandTreeChildAddClick = null;
        private DelegateCommand commandTreeUserAddClick = null;
        private DelegateCommand commandSaveClick = null;
        private DelegateCommand commandCancleClick = null;
        private DelegateCommand commandRenameClick = null;
        private DelegateCommand commandDeleteClick = null;

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

        public DelegateCommand CommandRenameClick
        {
            get => this.commandRenameClick;
            set => this.commandRenameClick = value;
        }

        public DelegateCommand CommandDeleteClick
        {
            get => this.commandDeleteClick;
            set => this.commandDeleteClick = value;
        }

        private void TreeRootAddClick(object obj)
        {
            if (!MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit)
            {
                MainWindowViewModel.userManageTreeViewModel.IsEditStatus = "Add";
                MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = true;
                MainWindowViewModel.userManageTreeViewModel.ParentGroupList.Add(new UserManageTreeModel(0, string.Empty, string.Empty, string.Empty, true, true));
            }
        }

        private void TreeChildAddClick(object obj)
        {
            TreeViewItem treeView = obj as TreeViewItem;
            treeView.IsExpanded = true;
            MainWindowViewModel.userManageTreeViewModel.IsEditStatus = "Add";

            if (!MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit)
            {
                MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = true;
                this.ChildGroupList.Add(new UserManageTreeModel(1, string.Empty, PrimaryKey, string.Empty, true, true));
            }
        }

        private void TreeUserAddClick(object obj)
        {
            UserManageTreeModel selectedItem = MainWindowViewModel.userManageListViewModel.SelectedItem;
            ObservableCollection<UserManageListModel> userListCollection = MainWindowViewModel.userManageListViewModel.UserListCollection;

            if (!MainWindowViewModel.userManageListViewModel.IsAddMember)
            {
                MainWindowViewModel.userManageListViewModel.IsAddMember = true;

                if (selectedItem.DepthCount != 1 || selectedItem.PrimaryKey != PrimaryKey)
                {
                    foreach (UserManageListModel ListItem in userListCollection)
                    {
                        if (ListItem.IsUserListVisibility)
                        {
                            ListItem.IsUserListVisibility = false;
                        }
                    }

                    foreach (UserManageListModel ListItem in userListCollection)
                    {
                        if (ListItem.UserGroupId == PrimaryKey)
                        {
                            ListItem.IsUserListVisibility = true;
                        }
                    }
                }

                foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                {
                    if (!item.IsHeader && PrimaryKey == item.GroupId)
                    {
                        int index = MainWindowViewModel.userManageTreeViewModel.UserGroupList.IndexOf(item);
                        MainWindowViewModel.userManageListViewModel.UserListCollection.Insert(0, new UserManageListModel(MainWindowViewModel.userManageTreeViewModel.UserGroupList, index));
                        break;
                    }
                }
            }
        }

        private void SaveClick(object obj)
        {
            if (IsTextBoxVisibility)
            {
                if (InputHeader != string.Empty)
                {
                    switch (MainWindowViewModel.userManageTreeViewModel.IsEditStatus)
                    {
                        case "Add":
                            AddSave();
                            break;
                        case "Rename":
                            RenameSave();
                            break;
                    }
                }
                else
                {
                    WarningMessageBoxView messageBox = new WarningMessageBoxView();
                    MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "값을 모두 입력해주세요.";
                    MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                    messageBox.ShowDialog();
                    IsTextBoxFocus = true;
                }
            }
        }

        private void CancleClick(object obj)
        {
            if (IsTextBoxVisibility)
            {
                IsTextBoxVisibility = false;
                MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;

                switch (DepthCount)
                {
                    case 0:
                        MainWindowViewModel.userManageTreeViewModel.ParentGroupList.Remove(this);
                        break;
                    case 1:
                        foreach (UserManageTreeModel item in MainWindowViewModel.userManageTreeViewModel.ParentGroupList)
                        {
                            if (item.PrimaryKey == ParentPrimaryKey)
                            {
                                item.ChildGroupList.Remove(this);
                            }
                        }
                        break;
                }
            }
        }

        private void RenameClick(object obj)
        {
            IsTextBoxFocus = false;
            if (!MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit)
            {
                MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = true;
                MainWindowViewModel.userManageTreeViewModel.IsEditStatus = "Rename";
                IsTextBoxVisibility = true;
                if (IsTextBoxVisibility)
                {
                    IsTextBoxFocus = true;
                    InputHeader = Header;
                }
            }
        }

        private void DeleteClick(object obj)
        {
            WarningMessageBoxView messageBox = new WarningMessageBoxView();
            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "정말로 삭제하시겠습니까?";
            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 0;
            messageBox.ShowDialog();

            if (MainWindowViewModel.warningMessageBoxViewModel.IsMessageBoxResult)
            {
                switch (DepthCount)
                {
                    case 0:
                        if (MainWindowViewModel.userManageTreeViewModel.ParentGroupList.Remove(this))
                        {
                            foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                            {
                                if (item.IsHeader && (item.GroupId == PrimaryKey))
                                {
                                    MainWindowViewModel.userManageTreeViewModel.UserGroupList.Remove(item);
                                    break;
                                }
                            }
                            string tableName = "userparentgroup";
                            string parentGroupDeleteQuery = string.Format("DELETE FROM {0} WHERE parent_group_id = '{1}'", tableName, PrimaryKey);
                            MainWindowViewModel.manager.MySqlQueryExecuter(parentGroupDeleteQuery);
                        }
                        break;
                    case 1:
                        foreach (UserManageTreeModel item in MainWindowViewModel.userManageTreeViewModel.ParentGroupList)
                        {
                            if (item.PrimaryKey == ParentPrimaryKey)
                            {
                                item.ChildGroupList.Remove(this);
                                foreach (ComboBoxGroupListModel item2 in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                                {
                                    if (!item2.IsHeader && (item2.GroupId == PrimaryKey))
                                    {
                                        MainWindowViewModel.userManageTreeViewModel.UserGroupList.Remove(item2);
                                        break;
                                    }
                                }
                                string tableName = "usergroup";
                                string parentGroupDeleteQuery = string.Format("DELETE FROM {0} WHERE group_id = '{1}'", tableName, PrimaryKey);
                                MainWindowViewModel.manager.MySqlQueryExecuter(parentGroupDeleteQuery);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        // 수정
        private void AddSave()
        {
            string tableName = string.Empty;

            switch (DepthCount)
            {
                case 0:
                    tableName = "userparentgroup";
                    string parentGroupNameSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_name = '{1}'", tableName, InputHeader);
                    DataSet parentGroupNameDataSet = MainWindowViewModel.manager.Select(parentGroupNameSelectQuery, tableName);
                    if (parentGroupNameDataSet.Tables[0].Rows.Count > 0)
                    {
                        WarningMessageBoxView messageBox = new WarningMessageBoxView();
                        MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "동일한 이름이 있습니다.";
                        MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        string parentGroupInsertQuery = string.Format("INSERT INTO {0}(parent_group_name) VALUES ('{1}')", tableName, InputHeader);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(parentGroupInsertQuery))
                        {
                            string parentGroupSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_name = '{1}'", tableName, InputHeader);
                            DataSet parentGroupDataSet = MainWindowViewModel.manager.Select(parentGroupSelectQuery, tableName);
                            Header = InputHeader;
                            PrimaryKey = parentGroupDataSet.Tables[0].Rows[0]["parent_group_id"].ToString();
                            MainWindowViewModel.userManageTreeViewModel.UserGroupList.Add(new ComboBoxGroupListModel(InputHeader, true, PrimaryKey));

                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                        }
                    }
                    break;
                case 1:
                    tableName = "usergroup";
                    string userGroupNameSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_id = '{1}' AND group_name = '{2}'", tableName, ParentPrimaryKey, InputHeader);
                    DataSet userGroupNameDataSet = MainWindowViewModel.manager.Select(userGroupNameSelectQuery, tableName);
                    if (userGroupNameDataSet.Tables[0].Rows.Count > 0)
                    {
                        WarningMessageBoxView messageBox = new WarningMessageBoxView();
                        MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "동일한 이름이 있습니다.";
                        MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        int index = 0;
                        string userGroupInsertQuery = string.Format("INSERT INTO {0}(group_name, parent_group_id) VALUES ('{1}', '{2}')", tableName, InputHeader, ParentPrimaryKey);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(userGroupInsertQuery))
                        {
                            string userGroupSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_id = '{1}' AND group_name = '{2}'", tableName, ParentPrimaryKey, InputHeader);
                            DataSet userGroupDataSet = MainWindowViewModel.manager.Select(userGroupSelectQuery, tableName);
                            Header = InputHeader;
                            PrimaryKey = userGroupDataSet.Tables[0].Rows[0]["group_id"].ToString();

                            foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                            {
                                if (item.IsHeader && (item.GroupId == ParentPrimaryKey))
                                {
                                    index = MainWindowViewModel.userManageTreeViewModel.UserGroupList.IndexOf(item);
                                }
                            }

                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                            MainWindowViewModel.userManageTreeViewModel.UserGroupList.Insert(index + 1, new ComboBoxGroupListModel(InputHeader, false, PrimaryKey, ParentPrimaryKey));
                        }
                    }
                    break;
            }
        }

        // 수정
        private void RenameSave()
        {
            string tableName = string.Empty;

            switch (DepthCount)
            {
                case 0:
                    tableName = "userparentgroup";
                    string parentGroupNameSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_name = '{1}'", tableName, InputHeader);
                    DataSet parentGroupNameDataSet = MainWindowViewModel.manager.Select(parentGroupNameSelectQuery, tableName);
                    if (parentGroupNameDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (InputHeader == Header)
                        {
                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                        }
                        else
                        {
                            WarningMessageBoxView messageBox = new WarningMessageBoxView();
                            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "동일한 이름이 있습니다.";
                            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                            messageBox.ShowDialog();
                            InputHeader = Header;
                        }
                    }
                    else
                    {
                        string parentGroupUpdateQuery = string.Format("UPDATE {0} SET parent_group_name = '{1}' WHERE parent_group_id = '{2}'", tableName, InputHeader, PrimaryKey);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(parentGroupUpdateQuery))
                        {
                            foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                            {
                                if (item.IsHeader && (item.GroupId == PrimaryKey))
                                {
                                    item.GroupName = InputHeader;
                                }
                            }
                            Header = InputHeader;
                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                        }
                    }
                    break;
                case 1:
                    tableName = "usergroup";
                    string userGroupNameSelectQuery = string.Format("SELECT * FROM {0} WHERE parent_group_id = '{1}' AND group_name = '{2}'", tableName, ParentPrimaryKey, InputHeader);
                    DataSet userGroupNameDataSet = MainWindowViewModel.manager.Select(userGroupNameSelectQuery, tableName);
                    if (userGroupNameDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (InputHeader == Header)
                        {
                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                        }
                        else
                        {
                            WarningMessageBoxView messageBox = new WarningMessageBoxView();
                            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxText = "동일한 이름이 있습니다.";
                            MainWindowViewModel.warningMessageBoxViewModel.MessageBoxMode = 1;
                            messageBox.ShowDialog();
                            InputHeader = Header;
                        }
                    }
                    else
                    {
                        string userGroupUpdateQuery = string.Format("UPDATE {0} SET group_name = '{1}' WHERE group_id = '{2}'", tableName, InputHeader, PrimaryKey);
                        if (MainWindowViewModel.manager.MySqlQueryExecuter(userGroupUpdateQuery))
                        {
                            foreach (ComboBoxGroupListModel item in MainWindowViewModel.userManageTreeViewModel.UserGroupList)
                            {
                                if (!item.IsHeader && (item.GroupId == PrimaryKey))
                                {
                                    item.GroupName = InputHeader;
                                }
                            }
                            Header = InputHeader;
                            IsTextBoxVisibility = false;
                            MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = false;
                        }
                    }
                    break;
            }
        }
    }
}
