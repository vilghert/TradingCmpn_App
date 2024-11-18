using System.Windows;
using TradingCompany_WPF.ViewModels;
using TradingCompany_WPF.Windows;
using TradingCompany_WPF.Views;

namespace TradingCompany_WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string connectionString = "Data Source=LAPTOP-E6KP9TMM\\MSSQLSERVER01;Initial Catalog=TradingCmpn_db;Integrated Security=True;TrustServerCertificate=True;";

            var productDal = new ProductDal(connectionString);
            var reviewDal = new ReviewDal(connectionString);
            var userDal = new UserDal(connectionString);
            var orderDal = new OrderDal(connectionString, new OrderItemDal(connectionString));

            var userContext = new UserContext();

            var authService = new AuthService(connectionString);
            var productService = new ProductService(productDal);
            var reviewService = new ReviewService(reviewDal);
            var orderService = new OrderService(orderDal);

            var userService = new UserService(userDal, reviewDal, userContext);

            var loginViewModel = new LoginViewModel(authService);
            var productViewModel = new ProductViewModel(productService);
            var reviewViewModel = new ReviewViewModel(reviewService, productService, orderService, userService, authService);
            var orderHistoryViewModel = new OrderHistoryViewModel(orderService);
            var mainViewModel = new MainViewModel(productService, reviewService, authService, orderService, userService);

            var loginView = new LoginView();
            var mainView = new MainView();
            var productView = new ProductView(productViewModel);
            var reviewView = new ReviewView(reviewViewModel);
            var orderHistoryView = new OrderHistoryView(orderHistoryViewModel);

            var loginWindow = new LoginWindow(loginView, loginViewModel);

            loginViewModel.OnLoginSuccess += () =>
            {
                // Сховати вікно входу
                loginWindow.Hide();

                // Створення і показ MainWindow
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Opacity = 0;  // Початкове значення для анімації
                mainWindow.Show();

                // Анімація для плавного переходу
                var fadeInAnimation = new System.Windows.Media.Animation.DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                mainWindow.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
            };

            // Показуємо вікно входу
            loginWindow.Show();
        }
    }
}