using ATM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ATM.ViewModel
{

    public class ChoiceViewModel : BaseComponent, INotifyPropertyChanged
    {
        private RelayCommand addMoneyOpenPageCommand;
        private RelayCommand getMoneyOpenPageCommand;
        public RelayCommand AddMoneyOpenPageCommand
        {
            get
            {
                return addMoneyOpenPageCommand ??
                (
                    addMoneyOpenPageCommand = new RelayCommand(obj =>
                    {
                        Debug.WriteLine("Notify Click");
                        this._mediator.Notify(this, "update_main_frame_addPage");      
                    }
                    )

                );
            }
        }
   
        public RelayCommand GetMoneyOpenPageCommand
        {
            get
            {
                return getMoneyOpenPageCommand ??
                (
                    getMoneyOpenPageCommand = new RelayCommand(obj =>
                    {
                        this._mediator.Notify(this, "update_main_frame_getPage");
                    }
                    )

                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
