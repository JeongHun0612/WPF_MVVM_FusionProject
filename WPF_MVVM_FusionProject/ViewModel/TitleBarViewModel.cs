using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class TitleBarViewModel : Notifier
    {
        private Point startPos;

        public TitleBarViewModel()
        {
            MainWindowViewModel.titleBarViewModel = this;
            this.commandDragMove = new DelegateCommand(DragMove);
            this.commandMouseDown= new DelegateCommand(MouseDown);
            this.commandMinimizeClick = new DelegateCommand(MinimizeClick);
            this.commandMaximizeClick = new DelegateCommand(MaximizeClick);
            this.commandExitClick = new DelegateCommand(ExitClick);
        }

        private bool isWindowMaximizeBtnVisibility = false;
        public bool IsWindowMaximizeBtnVisibility
        {
            get { return this.isWindowMaximizeBtnVisibility; }
            set { this.isWindowMaximizeBtnVisibility = value; Notify("IsWindowMaximizeBtnVisibility"); }
        }

        private bool isWindowNormalBtnVisibility = true;
        public bool IsWindowNormalBtnVisibility
        {
            get { return this.isWindowNormalBtnVisibility; }
            set { this.isWindowNormalBtnVisibility = value; Notify("IsWindowNormalBtnVisibility"); }
        }


        private DelegateCommand commandDragMove = null;
        private DelegateCommand commandMouseDown = null;
        private DelegateCommand commandMinimizeClick = null;
        private DelegateCommand commandMaximizeClick = null;
        private DelegateCommand commandExitClick = null;


        public DelegateCommand CommandDragMove
        {
            get => this.commandDragMove;
            set => this.commandDragMove = value;
        }

        public DelegateCommand CommandMouseDown
        {
            get => this.commandMouseDown;
            set => this.commandMouseDown = value;
        }

        public DelegateCommand CommandMinimizeClick
        {
            get => this.commandMinimizeClick;
            set => this.commandMinimizeClick = value;
        }

        public DelegateCommand CommandMaximizeClick
        {
            get => this.commandMaximizeClick;
            set => this.commandMaximizeClick = value;
        }

        public DelegateCommand CommandExitClick
        {
            get => this.commandExitClick;
            set => this.commandExitClick = value;
        }

        private void DragMove(object obj)
        {
            MainWindow mainWindow = obj as MainWindow;

            if (Mouse.LeftButton == MouseButtonState.Pressed && (startPos.X != 0 || startPos.Y != 0))
            {
                Point position = Mouse.GetPosition(mainWindow);

                if (((MainWindow)Application.Current.MainWindow).WindowState == WindowState.Maximized && (Math.Abs(startPos.Y - position.Y) > 2 || Math.Abs(startPos.X - position.X) > 2))
                {
                    Point point = mainWindow.PointToScreen(Mouse.GetPosition(null));

                    ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Normal;
                    IsWindowNormalBtnVisibility = true;
                    IsWindowMaximizeBtnVisibility = false;

                    mainWindow.Left = point.X - mainWindow.ActualWidth / 2; 
                    mainWindow.Top = 0;
                }

                ((MainWindow)Application.Current.MainWindow).DragMove();
            }
        }

        private void MouseDown(object obj)
        {
            MainWindow mainWindow = obj as MainWindow;
            startPos = Mouse.GetPosition(mainWindow);
        }

        private void MinimizeClick(object obj)
        {
            ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object obj)
        {
            startPos = new Point(0, 0);

            if (((MainWindow)Application.Current.MainWindow).WindowState == WindowState.Normal)
            {
                ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Maximized;
                IsWindowNormalBtnVisibility = false;
                IsWindowMaximizeBtnVisibility = true;
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Normal;
                IsWindowNormalBtnVisibility = true;
                IsWindowMaximizeBtnVisibility = false;
            }
        }

        private void ExitClick(object obj)
        {
            MainWindowViewModel.manager.CloseMySqlConnection();
            MainWindowViewModel.systemTimeViewModel.Timer.Stop();

            Environment.Exit(0);
        }
    }
}
