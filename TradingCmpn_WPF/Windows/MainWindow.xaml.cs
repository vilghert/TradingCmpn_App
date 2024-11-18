using BusinessLogic.Interface;
using System.Windows;
using TradingCmpn_WPF.ViewModels;

namespace TradingCmpn_WPF.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow(UserDto currentUser, IOrderService orderService, IProductService productService, IReviewService reviewService, IUserService userService)
        {
            InitializeComponent();

            var mainViewModel = new MainViewModel(currentUser, orderService, productService, reviewService, userService);
            DataContext = mainViewModel;
        }

    }
}