using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace TradingCompany_WPF.ViewModels
{
    public class OrderHistoryViewModel : INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        private ObservableCollection<OrderDto> _orders;
        private string _searchQuery;
        private string _searchStatusMessage;
        private string _selectedSortCriterion;
        private DispatcherTimer _searchDelayTimer;

        public ObservableCollection<OrderDto> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
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
            "Сума (Від найменшої до найбільшої)",
            "Сума (Від найбільшої до найменшої)"
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

        public OrderHistoryViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            SearchCommand = new RelayCommand(async () => await SearchOrders());
            SortCommand = new RelayCommand(async () => await SortOrders());

            LoadOrders();

            _searchDelayTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _searchDelayTimer.Tick += async (sender, args) =>
            {
                _searchDelayTimer.Stop();
                await SearchOrders();
            };
        }

        private async Task LoadOrders()
        {
            var ordersDto = await _orderService.GetAllOrdersAsync();
            Orders = new ObservableCollection<OrderDto>(ordersDto);
            SearchStatusMessage = "Замовлення завантажено.";
        }

        private async Task SearchOrders()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await LoadOrders();
                SearchStatusMessage = "Замовлення завантажено.";
            }
            else
            {
                var orders = await _orderService.GetAllOrdersAsync();

                var filteredOrders = orders
                    .Where(o => o.OrderId.ToString().Contains(SearchQuery) ||
                                (o.Status != null && o.Status.Contains(SearchQuery)))
                    .ToList();

                Orders = new ObservableCollection<OrderDto>(filteredOrders);

                if (filteredOrders.Any())
                {
                    SearchStatusMessage = $"{filteredOrders.Count} замовлень знайдено.";
                }
                else
                {
                    SearchStatusMessage = "Замовлення не знайдено.";
                }
            }
        }

        private async Task SortOrders()
        {
            if (Orders == null || !Orders.Any()) return;

            IEnumerable<OrderDto> sortedOrders = SelectedSortCriterion switch
            {
                "Сума (Від найменшої до найбільшої)" => Orders.OrderBy(o => o.TotalAmount),
                "Сума (Від найбільшої до найменшої)" => Orders.OrderByDescending(o => o.TotalAmount),
                _ => Orders
            };

            Orders = new ObservableCollection<OrderDto>(sortedOrders);

            await Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
