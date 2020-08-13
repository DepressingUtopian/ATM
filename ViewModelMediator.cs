using ATM.View;
using ATM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ATM
{
    public class ViewModelMediator : IMediator
    {
        private ATMViewModel atmViewModel;
        private ChoiceViewModel choiceViewModel;
        private AddMoneyViewModel addMoneyViewModel;
        private GetMoneyViewModel getMoneyViewModel;
        private PickUpViewModel pickUpViewModel;
        public ViewModelMediator()
        {
        }

        public ViewModelMediator(ATMViewModel atmViewModel, ChoiceViewModel choiceViewModel, 
            AddMoneyViewModel addMoneyViewModel, GetMoneyViewModel getMoneyViewModel, PickUpViewModel pickUpViewModel)
        {
            this.atmViewModel = atmViewModel;
            this.choiceViewModel = choiceViewModel;
            this.addMoneyViewModel = addMoneyViewModel;
            this.getMoneyViewModel = getMoneyViewModel;
            this.PickUpViewModel = pickUpViewModel;
        }

        public ChoiceViewModel ChoiceViewModel { get => choiceViewModel; set => choiceViewModel = value; }
        public ATMViewModel ATMViewModel { get => atmViewModel; set => atmViewModel = value; }
        public AddMoneyViewModel AddMoneyViewModel { get => addMoneyViewModel; set => addMoneyViewModel = value; }
        public GetMoneyViewModel GetMoneyViewModel { get => getMoneyViewModel; set => getMoneyViewModel = value; }
        public PickUpViewModel PickUpViewModel { get => pickUpViewModel; set => pickUpViewModel = value; }

        public void Notify(object sender, string _event)
        {
            if (sender is ChoiceViewModel)
            {
                if(_event == "update_main_frame_addPage")
                    ATMViewModel.SetAddMoneyViewCurrent();
                if (_event == "update_main_frame_getPage")
                    ATMViewModel.SetGetMoneyViewCurrent();
            }

            if (sender is AddMoneyViewModel)
            {
                if (_event == "update_count_backnotes_info")
                {
                    (sender as AddMoneyViewModel).CountBanknotesInATM = ATMViewModel.ATM_CoreCapacity - ATMViewModel.CountOfBanknotes;
                }
                if (_event == "go_money_atm")
                {
                    foreach (KeyValuePair<int, int> banknote in (sender as AddMoneyViewModel).AddBanknotes)
                        ATMViewModel.AddBanknotes(banknote.Key, banknote.Value);
                    ATMViewModel.SetChoiceViewCurrent();
                    
                }
            }

            if (sender is GetMoneyViewModel)
            {
                if (_event == "pickup_banknotes")
                {
                    ATMViewModel.PickUpBanknotes((sender as GetMoneyViewModel).Amount, true);
                }
                if (_event == "show_money_solution")
                    ATMViewModel.SetPickUpMoneyPageCurrent();
            }

            if (_event == "go_ChoicePage")
            {
                ATMViewModel.SetChoiceViewCurrent();
            }
        }
    }
}
