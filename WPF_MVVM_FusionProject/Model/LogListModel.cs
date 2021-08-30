using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_FusionProject.ViewModel;

namespace WPF_MVVM_FusionProject.Model
{
    public class LogListModel : Notifier
    {
        public LogListModel(string content)
        {
            this.timeStamp = DateTime.Now.ToString("yyyy. MM. dd. HH:mm:ss");
            this.content = this.timeStamp + "  " + content;
        }

        public LogListModel(string level, string content)
        {
            this.timeStamp = DateTime.Now.ToString("yyyy. MM. dd. HH:mm:ss");
            this.level = level;
            this.content = content;
        }

        private string timeStamp = DateTime.Now.ToString("yyyy. MM. dd. HH:mm:ss");
        public string TimeStamp
        {
            get { return this.timeStamp; }
            set { this.timeStamp = value; Notify("TimeStamp"); }
        }

        private string level = string.Empty;
        public string Level
        {
            get { return this.level; }
            set { this.level = value; Notify("Level"); }
        }

        private string content = string.Empty;
        public string Content
        {
            get { return this.content; }
            set { this.content = value; Notify("Content"); }
        }
    }
}
