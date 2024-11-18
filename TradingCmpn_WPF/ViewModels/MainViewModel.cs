using BusinessLogic.Interface;
using System.Windows.Input;

namespace TradingCmpn_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly UserDto _currentUser;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        private ViewModelBase _currentViewModel;
        private OrderHistoryViewModel _orderHistoryViewModel;
        private ProductViewModel _productViewModel;
        private ReviewViewModel _reviewViewModel;

        public ICommand ShowOrderHistoryViewCommand { get; }
        public ICommand ShowProductViewCommand { get; }
        public ICommand ShowReviewViewCommand { get; }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel(UserDto currentUser, IOrderService orderService, IProductService productService, IReviewService reviewService, IUserService userService)
        {
            _currentUser = currentUser;
            _orderService = orderService;
            _productService = productService;
            _reviewService = reviewService;
            _userService = userService;

            _orderHistoryViewModel = new OrderHistoryViewModel(orderService);
            _productViewModel = new ProductViewModel(productService);
            _reviewViewModel = new ReviewViewModel(reviewService, productService);

            CurrentViewModel = _orderHistoryViewModel;

            ShowOrderHistoryViewCommand = new RelayCommand(o => ShowOrderHistoryView());
            ShowProductViewCommand = new RelayCommand(o => ShowProductView());
            ShowReviewViewCommand = new RelayCommand(o => ShowReviewView());
        }


        private void ShowOrderHistoryView()
        {
            CurrentViewModel = _orderHistoryViewModel;
        }

        private void ShowProductView()
        {
            CurrentViewModel = _productViewModel;
        }

        private void ShowReviewView()
        {
            CurrentViewModel = _reviewViewModel;
        }
    }
}
