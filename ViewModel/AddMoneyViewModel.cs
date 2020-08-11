using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM.ViewModel
{
    public class AddMoneyViewModel : BaseComponent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<int, int> addBanknotes;
        private RelayCommand addMoneyCommand;
        private RelayCommand pickModeyCommand;
        private RelayCommand goMoneyToATMCommand;
        private RelayCommand goChoicePage;
        private int countBanknotesInATM = 0;
        private int sumBanknotesValue = 0;
        private int countAddBanknotes = 0;

        public Dictionary<int, int> AddBanknotes { get => addBanknotes;
            set
            {
                addBanknotes = value;
                OnPropertyChanged("AddBanknotes");
            } }

        public RelayCommand AddMoneyCommand {
            get 
            {
                UpdateCountBanknotesInfo();
                return addMoneyCommand ??
                (
                    
                    addMoneyCommand = new RelayCommand(obj =>
                    {
                        int nominal = int.Parse(obj as String);
                        Add(nominal);
                        SumBanknotesValue += nominal;
                        OnPropertyChanged("AddBanknotes");
                    }
                    ,(obj) => (countAddBanknotes < countBanknotesInATM)
                )

              );
            }
        }
        public RelayCommand PickModeyCommand {
            get
            {
                return pickModeyCommand ??
                (
                  pickModeyCommand = new RelayCommand(obj =>
                  {
                      int nominal = int.Parse(obj as String);
                      Pick(nominal);
                      SumBanknotesValue -= nominal;
                      OnPropertyChanged("AddBanknotes");
                  }
                  , (obj) => (AddBanknotes[int.Parse(obj as String)] > 0)
              )

            );
            }
        }

        public RelayCommand GoMoneyToATMCommand {
            get
            {
                return goMoneyToATMCommand ??
                (
                  goMoneyToATMCommand = new RelayCommand(obj =>
                  {
                      this._mediator.Notify(this, "go_money_atm");
                  }
                  , (obj) => (countAddBanknotes > 0)
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
        public int CountBanknotesInATM { get => countBanknotesInATM; set => countBanknotesInATM = value; }
        public int SumBanknotesValue { get => sumBanknotesValue;
            set
            {
                sumBanknotesValue = value;
                OnPropertyChanged("SumBanknotesValue");
            }
        }

  
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public AddMoneyViewModel()
        {
            AddBanknotes = new Dictionary<int, int>() { {10, 0}, { 50, 0}, { 100, 0 },
                        { 200, 0 }, { 500, 0 }, { 1000, 0 }, { 2000, 0 }, { 5000, 0 } };
          
        }

        public void UpdateCountBanknotesInfo()
        {
            this._mediator.Notify(this, "update_count_backnotes_info");
        }

        private void Add(int nomimal)
        {
            if (AddBanknotes.ContainsKey(nomimal) )
            {
                AddBanknotes[nomimal] += 1;
                countAddBanknotes++;
            }
            else
                throw new Exception("В словаре для добавления купюр, не существует номинала: " + nomimal);
        }
        private void Pick(int nomimal)
        {
            if (AddBanknotes.ContainsKey(nomimal))
            {
                AddBanknotes[nomimal] -= 1;
                countAddBanknotes--;
            }
            else
                throw new Exception("В словаре для добавления купюр, не существует номинала: " + nomimal);
        }
    }
}
