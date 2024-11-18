using BusinessLogic.Interface;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TradingCmpn_WPF.ViewModels;

public class ReviewViewModel : ViewModelBase
{
    private readonly IReviewService _reviewService;
    private readonly IProductService _productService;
    private ObservableCollection<ReviewDto> _reviews;
    private ObservableCollection<ProductDto> _products;
    private string _reviewText;
    private ProductDto _selectedProduct;

    public ObservableCollection<ReviewDto> Reviews
    {
        get => _reviews;
        set
        {
            _reviews = value;
            OnPropertyChanged(nameof(Reviews));
        }
    }

    public ObservableCollection<ProductDto> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
        }
    }

    public string ReviewText
    {
        get => _reviewText;
        set
        {
            _reviewText = value;
            OnPropertyChanged(nameof(ReviewText));
        }
    }

    public ProductDto SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged(nameof(SelectedProduct));
            _ = LoadReviewsForSelectedProductAsync(); // Завантаження відгуків для вибраного продукту
        }
    }

    public ICommand SubmitReviewCommand { get; }
    public ICommand RefreshReviewsCommand { get; }
    public ICommand LoadAllReviewsCommand { get; }

    public ReviewViewModel(IReviewService reviewService, IProductService productService)
    {
        _reviewService = reviewService;
        _productService = productService;

        SubmitReviewCommand = new RelayCommand(o => SubmitReviewAsync(), o => CanSubmitReview());
        RefreshReviewsCommand = new RelayCommand(o => LoadReviewsForSelectedProductAsync());
        LoadAllReviewsCommand = new RelayCommand(o => LoadAllReviewsAsync());  // Ініціалізуємо команду для завантаження всіх відгуків

        // Завантажуємо продукти під час ініціалізації
        LoadProducts();
    }

    private void LoadProducts()
    {
        var products = _productService.GetAllProducts();
        if (products == null || products.Count == 0)
        {
            Debug.WriteLine("No products found!");
        }
        else
        {
            Debug.WriteLine($"Loaded {products.Count} products.");
        }
        Products = new ObservableCollection<ProductDto>(products);
    }

    // Асинхронне завантаження відгуків для вибраного продукту
    private async Task LoadReviewsForSelectedProductAsync()
    {
        if (SelectedProduct != null)
        {
            var reviews = await _reviewService.GetReviewsForProductAsync(SelectedProduct.Id);
            Reviews = new ObservableCollection<ReviewDto>(reviews);
        }
    }

    private async Task LoadAllReviewsAsync()
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        Reviews = new ObservableCollection<ReviewDto>(reviews);
    }

    private async Task SubmitReviewAsync()
    {
        if (SelectedProduct != null && !string.IsNullOrEmpty(ReviewText))
        {
            await _reviewService.AddReviewAsync(SelectedProduct.Id, ReviewText);
            ReviewText = string.Empty;
            await LoadReviewsForSelectedProductAsync();
        }
    }

    private bool CanSubmitReview()
    {
        return SelectedProduct != null && !string.IsNullOrEmpty(ReviewText);
    }
}
