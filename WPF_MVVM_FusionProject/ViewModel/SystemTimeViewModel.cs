using System;
using System.Windows.Threading;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class SystemTimeViewModel : Notifier
    {
        public SystemTimeViewModel()
        {
            MainWindowViewModel.systemTimeViewModel = this;

            DateTimer();
        }

        public DispatcherTimer Timer = null;

        private string KSTdateTime = DateTime.Now.ToString("yyyy. MM. dd. HH:mm:ss");
        public string KSTDateTime
        {
            get { return this.KSTdateTime; }
            set { this.KSTdateTime = value; Notify("KSTDateTime"); }
        }

        private string UTCdateTime = DateTime.UtcNow.ToString("yyyy. MM. dd. HH:mm:ss");
        public string UTCDateTime
        {
            get { return this.UTCdateTime; }
            set { this.UTCdateTime = value; Notify("UTCDateTime"); }
        }

        private string GPSweek = "001";
        public string GPSWeek
        {
            get { return this.GPSweek; }
            set { this.GPSweek = value; Notify("GPSWeek"); }
        }

        private void DateTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Start();
            Timer.Interval = TimeSpan.FromMilliseconds(1000);
            Timer.Tick += (o, e) =>
            {
                KSTDateTime = DateTime.Now.ToString("yyyy. MM. dd. HH:mm:ss");
                UTCDateTime = DateTime.UtcNow.ToString("yyyy. MM. dd. HH:mm:ss");
            };
        }
    }
}
