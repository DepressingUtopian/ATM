using ATM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class PageFactory : IPageFactory
    {
        public AddMoneyPage AddMoneyPage()
        {
            return new AddMoneyPage();
        }

        public ChoicePage ChoicePage()
        {
            return new ChoicePage();
        }

        public GetMoneyPage GetMoneyPage()
        {
            return new GetMoneyPage();
        }

        public PickUpMoneyPage PickUpMoneyPage()
        {
            return new PickUpMoneyPage();
        }

        public Main Main()
        {
            return new Main();
        }
    }
}
