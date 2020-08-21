using ATM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ATM.ViewModel
{
    public class ATMViewModel :BaseComponent, INotifyPropertyChanged
    {
        private ATMCore core;

        private Page currentPage;
        private Page addMoneyView;
        private Page getMoneyView;
        private Page choiceView;
        private Page pickUpMoneyPage;

        private PageFactory pageFactory = new PageFactory();
        private Dictionary<int, int> lastSolution;



        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public int CountOfBanknotes
        {
            get { return core.CountOfBanknotes; }
        }

        public int ATM_CoreCapacity
        {
            get { return core.Capacity; }
            set {
                core.Capacity = value;
                OnPropertyChanged("ATM_CoreCapacity");
            }
        }
        public int ATM_CoreAmountOfBanknotes
        {
            get { return core.AmountOfBanknotes; }
            set
            {
                core.AmountOfBanknotes = value;
                OnPropertyChanged("ATM_CoreAmountOfBanknotes");
            }
        }

        public int RemainingSpace
        {
            get { return core.RemainingSpace; }
            set 
            {
                core.RemainingSpace = value;
                OnPropertyChanged("RemainingSpace");
            }
        }

        public Dictionary<int, int> Storage
        {
            get 
            {
                return core.Storage;
            }
            set
            {
                core.Storage = value;
                OnPropertyChanged("Storage");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ATMViewModel()
        {
            core = new ATMCore(100);
            addMoneyView = pageFactory.AddMoneyPage();
            getMoneyView = pageFactory.GetMoneyPage();
            choiceView = pageFactory.ChoicePage();
            pickUpMoneyPage = pageFactory.PickUpMoneyPage();
            currentPage = choiceView;
            this.RemainingSpace = core.Capacity - core.CountOfBanknotes;
        }

        public void SetAddMoneyViewCurrent()
        {
            CurrentPage = addMoneyView;
        }

        public void SetGetMoneyViewCurrent()
        {
            CurrentPage = getMoneyView;
        }

        public void SetChoiceViewCurrent()
        {
            CurrentPage = choiceView;
        }

        public void SetPickUpMoneyPageCurrent()
        {
            MainViewModel.ViewModelMediator.PickUpViewModel.PickMoney = lastSolution;
            CurrentPage = pickUpMoneyPage;
        }

        public void AddBanknotes(int nominal, int count)
        {
            core.AddMoney(nominal, count);
            OnPropertyChanged("ATM_CoreAmountOfBanknotes");
            OnPropertyChanged("RemainingSpace");
            OnPropertyChanged("Storage");
        }

        public void PickUpBanknotes(int amount, bool showDialog = false)
        {
           try
           {
                if (showDialog)
                {
                    Dictionary<int, int> solution;
                    core.PickUpMoney(amount, out solution);
                    lastSolution = solution;
                }
                else
                    core.PickUpMoney(amount);
            }
            catch (Exception msg)
            {
               MessageBox.Show(msg.Message);
            }

            OnPropertyChanged("ATM_CoreAmountOfBanknotes");
            OnPropertyChanged("CountOfBanknotes");
            OnPropertyChanged("Storage");
        }
    }
}
