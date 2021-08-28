using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_FusionProject.Model;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class LogListViewModel : Notifier
    {
        public LogListViewModel()
        {
            MainWindowViewModel.logListViewModel = this;
        }

        private ObservableCollection<LogListModel> logListCollection = new ObservableCollection<LogListModel>();
        public ObservableCollection<LogListModel> LogListCollection
        {
            get { return this.logListCollection; }
            set { this.logListCollection = value; Notify("LogListCollection"); }
        }
    }
}
