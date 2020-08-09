using ATM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ATM
{
    public interface IPageFactory
    {
        Main Main();
        ChoicePage ChoicePage();
        GetMoneyPage GetMoneyPage();
        AddMoneyPage AddMoneyPage();

    }
}
