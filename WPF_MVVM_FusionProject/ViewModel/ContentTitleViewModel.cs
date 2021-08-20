using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_FusionProject.ViewModel
{
    public class ContentTitleViewModel
    {
        public ContentTitleViewModel()
        {
            MainWindowViewModel.contentTitleViewModel = this;
        }
    }
}
