using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class GetMoneyViewModel : BaseComponent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int amount = 0;
        private int amountOfBanknotes = 1000;
        private RelayCommand nominalClickButton;
        private RelayCommand pickupClickButton;
        private RelayCommand goChoicePage;

        public RelayCommand NominalClickButton {
            get
            {
                return nominalClickButton ??
                (

                    nominalClickButton = new RelayCommand(obj =>
                    {
                        int nominal = int.Parse(obj as String);
                        Amount = nominal;
                    }
                     , (obj) => (amountOfBanknotes > 0)
                )

              );
            }
        }

        public RelayCommand PickupClickButton
        {
            get
            {
                return pickupClickButton ??
                (

                    pickupClickButton = new RelayCommand(obj =>
                    {
                        this._mediator.Notify(this, "pickup_banknotes");
                        this._mediator.Notify(this, "go_ChoicePage");
                        Amount = 0;
                    }
                )

              );
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

        public int Amount { 
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
