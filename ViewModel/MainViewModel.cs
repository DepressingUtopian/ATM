using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ATM.ViewModel
{
    public class MainViewModel : BaseComponent, INotifyPropertyChanged
    {
        private Page currentPage;
        private Page main;
        private PageFactory pageFactory = new PageFactory();
        private static ViewModelMediator viewModelMediator = new ViewModelMediator();

        public Page CurrentPage
        {
            get {  return currentPage;}
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public static ViewModelMediator ViewModelMediator { get => viewModelMediator; set => viewModelMediator = value; }

        public MainViewModel()
        {
            main = pageFactory.Main();
            currentPage = main;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
