using System.Windows;
using System.Windows.Controls;

namespace TradingCmpn_WPF.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var viewModel = (LoginViewModel)this.DataContext;
            viewModel.Password = passwordBox.Password;
        }
    }
}