using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace TradingCompany_WPF.ViewModels 
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;
        private ObservableCollection<ProductDto> _products;
        private string _searchQuery;
        private string _searchStatusMessage;
        private string _selectedSortCriterion;
        private DispatcherTimer _searchDelayTimer;

        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));

                SearchStatusMessage = "Шукаємо...";

                _searchDelayTimer?.Stop();
                _searchDelayTimer.Start();
            }
        }

        public string SearchStatusMessage
        {
            get => _searchStatusMessage;
            set
            {
                _searchStatusMessage = value;
                OnPropertyChanged(nameof(SearchStatusMessage));
            }
        }

        public ObservableCollection<string> SortCriteria { get; } = new ObservableCollection<string>
    {
        "Назва (Алфавітний порядок)",
        "Ціна (Від найнижчої до найвищої)",
        "Ціна (Від найвищої до найнижчої)"
    };

        public string SelectedSortCriterion
        {
            get => _selectedSortCriterion;
            set
            {
                _selectedSortCriterion = value;
                OnPropertyChanged(nameof(SelectedSortCriterion));
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand SortCommand { get; }

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;

            SearchCommand = new RelayCommand(async () => await SearchProducts());
            SortCommand = new RelayCommand(async () => await SortProducts());

            LoadProducts();

            _searchDelayTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _searchDelayTimer.Tick += async (sender, args) =>
            {
                _searchDelayTimer.Stop();
                await SearchProducts();
            };
        }

        private async Task LoadProducts()
        {
            var productsDto = await _productService.GetAllProductsAsync();
            Products = new ObservableCollection<ProductDto>(productsDto);
            SearchStatusMessage = "Продукти завантажено.";
        }

        private async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await LoadProducts();
                SearchStatusMessage = "Продукти завантажено.";
                return;
            }

            var filteredProducts = await _productService.SearchProductsAsync(SearchQuery);

            if (filteredProducts.Any())
            {
                Products = new ObservableCollection<ProductDto>(filteredProducts);
                SearchStatusMessage = $"{filteredProducts.Count} продуктів знайдено.";
            }
            else
            {
                Products = new ObservableCollection<ProductDto>();
                SearchStatusMessage = "Продукти не знайдено.";
            }
        }

        private async Task SortProducts()
        {
            if (Products == null || !Products.Any()) return;

            IEnumerable<ProductDto> sortedProducts = SelectedSortCriterion switch
            {
                "Назва (Алфавітний порядок)" => Products.OrderBy(p => p.ProductName),
                "Ціна (Від найнижчої до найвищої)" => Products.OrderBy(p => p.Price),
                "Ціна (Від найвищої до найнижчої)" => Products.OrderByDescending(p => p.Price),
                _ => Products
            };

            Products = new ObservableCollection<ProductDto>(sortedProducts);

            await Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}