using System.Windows;
using TradingCmpn_WPF.Services;
using TradingCmpn_WPF.Views;

namespace TradingCmpn_WPF.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginWindow()
        {
            InitializeComponent();

            var connectionString = "Data Source=localhost;Initial Catalog=TradingCompanyVicky_db;Integrated Security=True;TrustServerCertificate=True;";

            var orderItemDal = new OrderItemDal(connectionString);
            var orderDal = new OrderDal(connectionString, orderItemDal);
            var productDal = new ProductDal(connectionString);
            var reviewDal = new ReviewDal(connectionString);
            var userDal = new UserDal(connectionString);

            var orderService = new OrderService(orderDal);
            var productService = new ProductService(productDal);
            var reviewService = new ReviewService(reviewDal);
            var userContext = new UserContext();
            var userService = new UserService(userDal, reviewDal, userContext);

            Action<UserDto> navigateToMain = user =>
            {
                var mainWindow = new MainWindow(user, orderService, productService, reviewService, userService);
                mainWindow.Show();
            };

            _loginViewModel = new LoginViewModel(new AuthService(connectionString), new MessageService(), navigateToMain);
            DataContext = _loginViewModel;

            var loginView = new LoginView();
            Content = loginView;
        }
    }
}
