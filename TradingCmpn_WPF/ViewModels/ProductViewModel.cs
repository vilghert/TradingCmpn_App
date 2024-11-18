using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCmpn_WPF.ViewModels;
using System.Linq;

public class ProductViewModel : ViewModelBase
{
    private readonly IProductService _productService;

    public ObservableCollection<ProductDto> Products { get; }

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged(nameof(SearchQuery));
            UpdateCommandsState();  // Оновлення активності команд
        }
    }

    private string _sortBy;
    public string SortBy
    {
        get => _sortBy;
        set
        {
            _sortBy = value;
            OnPropertyChanged(nameof(SortBy));
            UpdateCommandsState();  // Оновлення активності команд
        }
    }

    private bool _canSearch;
    public bool CanSearch
    {
        get => _canSearch;
        set
        {
            _canSearch = value;
            OnPropertyChanged(nameof(CanSearch));
        }
    }

    private bool _canSort;
    public bool CanSort
    {
        get => _canSort;
        set
        {
            _canSort = value;
            OnPropertyChanged(nameof(CanSort));
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand SortCommand { get; }

    public ProductViewModel(IProductService productService)
    {
        _productService = productService;

        Products = new ObservableCollection<ProductDto>(_productService.GetAllProducts());

        SearchCommand = new RelayCommand(o => ExecuteSearch(), o => CanSearch);
        SortCommand = new RelayCommand(o => ExecuteSort(), o => CanSort);
    }

    private void ExecuteSearch()
    {
        var results = _productService.SearchProducts(SearchQuery);
        UpdateProducts(results);
    }

    private void ExecuteSort()
    {
        var sortedProducts = _productService.SortProducts(SortBy);
        UpdateProducts(sortedProducts);
    }

    private void UpdateProducts(IEnumerable<ProductDto> products)
    {
        Products.Clear();
        foreach (var product in products)
        {
            Products.Add(product);
        }
    }

    // Оновлення активності команд
    private void UpdateCommandsState()
    {
        CanSearch = !string.IsNullOrEmpty(SearchQuery);  // Активуємо Search тільки якщо є текст
        CanSort = !string.IsNullOrEmpty(SortBy);  // Активуємо Sort тільки якщо вибрано критерій сортування
    }
}
