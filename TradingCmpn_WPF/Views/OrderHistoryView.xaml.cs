using System.Windows.Controls;

namespace TradingCmpn_WPF.Views
{
    public partial class OrderHistoryView : UserControl
    {
        public OrderHistoryView()
        {
            InitializeComponent();

            string connectionString = "Data Source=localhost;Initial Catalog=TradingCompanyVicky_db;Integrated Security=True;TrustServerCertificate=True;";
            IOrderItemDal orderItemDal = new OrderItemDal(connectionString);
            IOrderDal orderDal = new OrderDal(connectionString, orderItemDal);
            IOrderService orderService = new OrderService(orderDal);

            var viewModel = new OrderHistoryViewModel(orderService);
            this.DataContext = viewModel;

            viewModel.LoadOrders();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = (sender as DataGrid).SelectedItem as OrderDto;
            if (selectedOrder != null)
            {
                var viewModel = (OrderHistoryViewModel)this.DataContext;
                viewModel.SelectedOrder = selectedOrder;
            }
        }
    }
}
