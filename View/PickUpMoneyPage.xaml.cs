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
    /// Логика взаимодействия для PickUpMoneyPage.xaml
    /// </summary>
    public partial class PickUpMoneyPage : Page
    {
        public PickUpMoneyPage()
        {
            InitializeComponent();
            MainViewModel.ViewModelMediator.PickUpViewModel = new PickUpViewModel();
            MainViewModel.ViewModelMediator.PickUpViewModel.SetMediator(MainViewModel.ViewModelMediator);
            DataContext = MainViewModel.ViewModelMediator.PickUpViewModel;
        }
    }
}
