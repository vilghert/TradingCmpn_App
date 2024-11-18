using System.Windows.Controls;
using TradingCompany_WPF.ViewModels;

namespace TradingCompany_WPF.Views
{
    public partial class OrderHistoryView : UserControl
    {
        public OrderHistoryView(OrderHistoryViewModel orderhistoryViewModel)
        {
            InitializeComponent();
            DataContext = orderhistoryViewModel ?? throw new ArgumentNullException(nameof(orderhistoryViewModel));
        }
    }
}