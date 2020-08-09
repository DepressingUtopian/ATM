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

        public ViewModelMediator()
        {
        }

        public ViewModelMediator(ATMViewModel atmViewModel, ChoiceViewModel choiceViewModel)
        {
            this.atmViewModel = atmViewModel;
            this.choiceViewModel = choiceViewModel;
        }

        public ChoiceViewModel ChoiceViewModel { get => choiceViewModel; set => choiceViewModel = value; }
        public ATMViewModel ATMViewModel { get => atmViewModel; set => atmViewModel = value; }

        public void Notify(object sender, string _event)
        {
            if (sender is ChoiceViewModel)
            {
                if(_event == "update_main_frame_addPage")
                    ATMViewModel.SetAddMoneyViewCurrent();
                if (_event == "update_main_frame_getPage")
                    ATMViewModel.SetGetMoneyViewCurrent();
            }
            
        }
    }
}
