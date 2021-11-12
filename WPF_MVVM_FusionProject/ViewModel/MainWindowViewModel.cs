using System.Diagnostics;
using System.Drawing;
using System.Windows;
using WPF_MVVM_FusionProject.DBConncet;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class MainWindowViewModel : Notifier
    {
        public MainWindowViewModel()
        {
            manager.Initialize();
        }

        public static MySQLManager manager = new MySQLManager();
        public static TitleBarViewModel titleBarViewModel { get; set; } = null;
        public static ContentTitleViewModel contentTitleViewModel { get; set; } = null;
        public static SystemTimeViewModel systemTimeViewModel { get; set; } = null;
        public static UserManageTreeViewModel userManageTreeViewModel { get; set; } = null;
        public static UserManageListViewModel userManageListViewModel { get; set; } = null;
        public static LogListViewModel logListViewModel { get; set; } = null;
        public static WarningMessageBoxViewModel warningMessageBoxViewModel { get; set; } = null;
    }
}
