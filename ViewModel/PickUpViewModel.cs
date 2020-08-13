using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ATM.ViewModel
{
    public class PickUpViewModel : BaseComponent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Dictionary<int, int> pickMoney;
        private RelayCommand goChoicePage;

        public Dictionary<int, int> PickMoney { 
            get => pickMoney;
            set
            {
                pickMoney = value;
                OnPropertyChanged("PickMoney");
            } 
        }

        public RelayCommand GoChoicePage
        {
            get
            {
                return goChoicePage ??
                (
                  goChoicePage = new RelayCommand(obj =>
                  {
                      this._mediator.Notify(this, "go_ChoicePage");
                  }
                )

            );
            }
        }

        public PickUpViewModel()
        {
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
