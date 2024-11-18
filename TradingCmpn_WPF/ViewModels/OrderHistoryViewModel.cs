using System.Collections.ObjectModel;
using System.Windows.Input;
using TradingCmpn_WPF.ViewModels;

public class OrderHistoryViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private ObservableCollection<OrderDto> _orders;
    private OrderDto _selectedOrder;
    private string _searchCriteria;
    private decimal _minAmount;
    private decimal _maxAmount;

    public ObservableCollection<OrderDto> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
    }

    public OrderDto SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            _selectedOrder = value;
            OnPropertyChanged(nameof(SelectedOrder));
            LoadOrderItems();
        }
    }

    public ObservableCollection<OrderItemDto> SelectedOrderItems { get; private set; }

    public decimal MinAmount
    {
        get => _minAmount;
        set
        {
            _minAmount = value;
            OnPropertyChanged(nameof(MinAmount));
        }
    }

    public decimal MaxAmount
    {
        get => _maxAmount;
        set
        {
            _maxAmount = value;
            OnPropertyChanged(nameof(MaxAmount));
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand RepeatOrderCommand { get; }

    public bool CanRepeatOrder => SelectedOrder != null;

    public OrderHistoryViewModel(IOrderService orderService)
    {
        _orderService = orderService;
        Orders = new ObservableCollection<OrderDto>(_orderService.GetOrdersForCurrentUser());

        RepeatOrderCommand = new RelayCommand(o => RepeatOrder(), o => CanRepeatOrder);
        SearchCommand = new RelayCommand(o => SearchOrders());
        RefreshCommand = new RelayCommand(o => RefreshOrders());
    }

    private void LoadOrderItems()
    {
        if (SelectedOrder != null)
        {
            SelectedOrderItems = new ObservableCollection<OrderItemDto>(_orderService.GetOrderItems(SelectedOrder.OrderId));
            OnPropertyChanged(nameof(SelectedOrderItems));
        }
    }

    private void SearchOrders()
    {
        if (MinAmount == 0 && MaxAmount == 0)
        {
            Orders = new ObservableCollection<OrderDto>(_orderService.GetOrdersForCurrentUser());
        }
        else
        {
            Orders = new ObservableCollection<OrderDto>(_orderService.SearchOrders(MinAmount, MaxAmount));
        }
    }

    private void RefreshOrders()
    {
        Orders = new ObservableCollection<OrderDto>(_orderService.GetOrdersForCurrentUser());
    }

    private void RepeatOrder()
    {
        if (SelectedOrder != null)
        {
            _orderService.RepeatOrder(SelectedOrder.OrderId);
        }
    }

    public void LoadOrders()
    {
        var ordersList = _orderService.GetAllOrders();
        Orders.Clear();

        foreach (var order in ordersList)
        {
            Orders.Add(order);
        }
    }
}
