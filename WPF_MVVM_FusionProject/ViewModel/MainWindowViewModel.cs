using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_FusionProject.DBConncet;
using WPF_MVVM_FusionProject.View;
using WPF_MVVM_FusionProject.ViewModel.LoginWindowViewModel;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class MainWindowViewModel : Notifier
    {
        public MainWindowViewModel()
        {
            manager.Initialize();
            this.commandLoaded = new DelegateCommand(Loaded);
            this.commandDragMove = new DelegateCommand(DragMove);
        }

        public static MySQLManager manager = new MySQLManager();
        public static TitleBarViewModel titleBarViewModel { get; set; } = null;
        public static ContentTitleViewModel contentTitleViewModel { get; set; } = null;
        public static SystemTimeViewModel systemTimeViewModel { get; set; } = null;
        public static UserManageTreeViewModel userManageTreeViewModel { get; set; } = null;
        public static UserManageListViewModel userManageListViewModel { get; set; } = null;
        public static LogListViewModel logListViewModel { get; set; } = null;
        public static WarningMessageBoxViewModel warningMessageBoxViewModel { get; set; } = null;

        private DelegateCommand commandLoaded = null;
        private DelegateCommand commandDragMove = null;

        public DelegateCommand CommandLoaded
        {
            get => this.commandLoaded;
            set => this.commandLoaded = value;
        }

        public DelegateCommand CommandDragMove
        {
            get => this.commandDragMove;
            set => this.commandDragMove = value;
        }

        private void Loaded(object obj)
        {
            MainWindow mainWindow = obj as MainWindow;
            Application.Current.MainWindow = mainWindow;
        }

        private void DragMove(object obj)
        {
            MainWindow mainWindow = obj as MainWindow;
            Point position = Mouse.GetPosition(mainWindow);

            ((MainWindow)Application.Current.MainWindow).DragMove();

            Debug.WriteLine(position);

            //if (((MainWindow)Application.Current.MainWindow).WindowState == WindowState.Maximized)
            //{
            //    ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Normal;
            //}

        }
    }
}
