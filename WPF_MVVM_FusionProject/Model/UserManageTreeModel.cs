using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using WPF_MVVM_FusionProject.ViewModel;

namespace WPF_MVVM_FusionProject.Model
{
    public class UserManageTreeModel : NotifierCollection
    {
        public UserManageTreeModel(int depthCount, string primaryKey, string parentPrimaryKey, string header, bool isTextBoxVisibility, bool isTextBoxFocus)
        {
            this.depthCount = depthCount;
            this.primaryKey = primaryKey;
            this.parentPrimaryKey = parentPrimaryKey;
            this.header = header;
            this.isTextBoxVisibility = isTextBoxVisibility;
            this.isTextBoxFocus = isTextBoxFocus;

            this.commandSaveClick = new DelegateCommand(SaveClick);
            this.commandRenameClick = new DelegateCommand(RenameClick);
            this.commandDeleteClick = new DelegateCommand(DeleteClick);
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

        private DelegateCommand commandSaveClick = null;
        private DelegateCommand commandRenameClick = null;
        private DelegateCommand commandDeleteClick = null;

        public DelegateCommand CommandSaveClick
        {
            get => this.commandSaveClick;
            set => this.commandSaveClick = value;
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
                    MessageBox.Show("값을 입력해주세요.");
                    IsTextBoxFocus = true;
                }
            }
        }

        private void RenameClick(object obj)
        {
            if (!MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit)
            {
                MainWindowViewModel.userManageTreeViewModel.IsTreeNodeEdit = true;
                MainWindowViewModel.userManageTreeViewModel.IsEditStatus = "Rename";
                IsTextBoxVisibility = true;
                IsTextBoxFocus = true;
                InputHeader = Header;
            }
        }

        private void DeleteClick(object obj)
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
                        MessageBox.Show("동일한 이름이 있습니다.");
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
                        MessageBox.Show("동일한 이름이 있습니다.");
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
                                if (!item.IsHeader && (item.ParentGroupId == ParentPrimaryKey))
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
                            MessageBox.Show("동일한 이름이 있습니다.");
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
                            MessageBox.Show("동일한 이름이 있습니다.");
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
