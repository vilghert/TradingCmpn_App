using System.Windows;
using TradingCompany_WPF.Views;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginView loginView, LoginViewModel loginViewModel)
        {
            InitializeComponent();
            Content = loginView;
            DataContext = loginViewModel;
        }
    }
}