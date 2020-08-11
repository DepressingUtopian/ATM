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
    /// Логика взаимодействия для AddMoneyPage.xaml
    /// </summary>
    public partial class AddMoneyPage : Page
    {
        public AddMoneyPage()
        {
           InitializeComponent();
           MainViewModel.ViewModelMediator.AddMoneyViewModel = new AddMoneyViewModel();
           MainViewModel.ViewModelMediator.AddMoneyViewModel.SetMediator(MainViewModel.ViewModelMediator);
           DataContext = MainViewModel.ViewModelMediator.AddMoneyViewModel;
        }
    }
}
