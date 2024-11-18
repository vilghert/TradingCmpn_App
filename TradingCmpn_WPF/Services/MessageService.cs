using System.Windows;

namespace TradingCmpn_WPF.Services
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}