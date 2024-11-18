using System.Windows;
using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel loginViewModel)
            {
                var passwordBox = (PasswordBox)sender;
                loginViewModel.Password = passwordBox.Password;
            }
        }
    }
}