using System.Windows;
using TradingCmpn_WPF.Windows;

namespace TradingCmpn_WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}