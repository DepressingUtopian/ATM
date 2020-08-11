using ATM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATM.View
{
    /// <summary>
    /// Логика взаимодействия для GetMoneyPage.xaml
    /// </summary>
    public partial class GetMoneyPage : Page
    {
        public GetMoneyPage()
        {
            InitializeComponent();

            MainViewModel.ViewModelMediator.GetMoneyViewModel = new GetMoneyViewModel();
            MainViewModel.ViewModelMediator.GetMoneyViewModel.SetMediator(MainViewModel.ViewModelMediator);
            DataContext = MainViewModel.ViewModelMediator.GetMoneyViewModel;
        }
    }
}
