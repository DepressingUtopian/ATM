using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        private PageFactory pageFactory = new PageFactory();


        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
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

        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ATMViewModel()
        {
            core = new ATMCore(10);
            addMoneyView = pageFactory.AddMoneyPage();
            getMoneyView = pageFactory.GetMoneyPage();
            choiceView = pageFactory.ChoicePage();
            currentPage = choiceView;
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
    }
}
