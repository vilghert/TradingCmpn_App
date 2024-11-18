using System.Windows;
using TradingCompany_WPF.ViewModels;


namespace TradingCompany_WPF.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}