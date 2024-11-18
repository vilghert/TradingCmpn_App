using BusinessLogic.Interface;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace TradingCompany_WPF.ViewModels
{
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public ReviewViewModel(IReviewService reviewService, IProductService productService, IOrderService orderService, IUserService userService, IAuthService authService)
        {
            _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            AddReviewCommand = new RelayCommand(async () => await AddReview());
            RepeatOrderCommand = new RelayCommand(async () => await RepeatOrder());

            LoadDataAsync();
        }

        private List<ReviewDto> _reviews;
        public List<ReviewDto> Reviews
        {
            get => _reviews;
            set
            {
                _reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        private List<ProductDto> _products;
        public List<ProductDto> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ProductDto _selectedProduct;
        public ProductDto SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private string _reviewText;
        public string ReviewText
        {
            get => _reviewText;
            set
            {
                _reviewText = value;
                OnPropertyChanged(nameof(ReviewText));
            }
        }

        public ICommand AddReviewCommand { get; }
        public ICommand RepeatOrderCommand { get; }

        private async void LoadDataAsync()
        {
            await Task.WhenAll(LoadProductsAsync(), LoadReviewsAsync());
        }

        private async Task LoadProductsAsync()
        {
            Products = await _productService.GetAllProductsAsync();
        }

        private async Task LoadReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            Reviews = reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ProductId = r.ProductId,
                UserId = r.UserId,
                ReviewText = r.ReviewText,
                ProductName = Products?.FirstOrDefault(p => p.ProductId == r.ProductId)?.ProductName
            }).ToList();
        }

        private async Task AddReview()
        {
            if (SelectedProduct != null && !string.IsNullOrEmpty(ReviewText))
            {
                var userId = _authService.CurrentUser?.UserId ?? 0;
                if (userId != 0)
                {
                    await _reviewService.AddReviewAsync(SelectedProduct.ProductId, userId, ReviewText);
                    ReviewText = string.Empty;
                    await LoadReviewsAsync();
                }
                else
                {
                    MessageBox.Show("Для додавання відгуку потрібно залогінитись.");
                }
            }
        }

        private async Task RepeatOrder()
        {
            if (SelectedProduct != null)
            {
                await _orderService.RepeatOrderAsync(SelectedProduct.ProductId);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
