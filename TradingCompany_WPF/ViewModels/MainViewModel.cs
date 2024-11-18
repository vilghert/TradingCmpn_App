using BusinessLogic.Interface;
using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using TradingCompany_WPF.Views;
using TradingCompany_WPF.Windows;
using System.Windows.Controls;

namespace TradingCompany_WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public ICommand ShowProductCommand { get; }
        public ICommand ShowReviewCommand { get; }
        public ICommand ShowOrderHistoryCommand { get; }
        public ICommand LogoutCommand { get; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainViewModel(IProductService productService, IReviewService reviewService, IAuthService authService, IOrderService orderService, IUserService userService)
        {
            _productService = productService;
            _reviewService = reviewService;
            _authService = authService;
            _orderService = orderService;
            _userService = userService;

            ShowProductCommand = new RelayCommand(async () => await ShowProducts());
            ShowReviewCommand = new RelayCommand(async () =>    await ShowReviews());
            ShowOrderHistoryCommand = new RelayCommand(async () => await ShowOrderHistory());
            LogoutCommand = new RelayCommand(Logout);

            CurrentView = new TextBlock
            {
                Text = "Виберіть вкладку",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private async Task ShowReviews()
        {
            CurrentView = new ReviewView(new ReviewViewModel(_reviewService, _productService, _orderService, _userService, _authService));
            await Task.CompletedTask;
        }


        private async Task ShowProducts()
        {
            CurrentView = new ProductView(new ProductViewModel(_productService));
            await Task.CompletedTask;
        }

        private async Task ShowOrderHistory()
        {
            CurrentView = new OrderHistoryView(new OrderHistoryViewModel(_orderService));
            await Task.CompletedTask;
        }

        private async Task Logout()
        {
            _authService.Logout();

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.Close();

            Application.Current.Shutdown();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}