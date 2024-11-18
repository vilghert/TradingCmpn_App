using System.Windows.Input;
using System.Windows;
using TradingCmpn_WPF.Services;
using TradingCmpn_WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly IMessageService _messageService;
    private readonly Action<UserDto> _navigateToMain;

    private string _username;
    private string _password;
    private string _errorMessage;
    private Visibility _errorMessageVisibility;
    private bool _isPasswordValid;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
            ValidateLogin();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
            ValidatePassword();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            ErrorMessageVisibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(ErrorMessageVisibility));
        }
    }

    public Visibility ErrorMessageVisibility
    {
        get => _errorMessageVisibility;
        set
        {
            _errorMessageVisibility = value;
            OnPropertyChanged(nameof(ErrorMessageVisibility));
        }
    }

    public bool IsPasswordValid
    {
        get => _isPasswordValid;
        set
        {
            _isPasswordValid = value;
            OnPropertyChanged(nameof(IsPasswordValid));
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService, IMessageService messageService, Action<UserDto> navigateToMain)
    {
        _authService = authService;
        _messageService = messageService;
        _navigateToMain = navigateToMain;

        LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
    }

    private void ExecuteLogin(object parameter)
    {
        // Тимчасова авторизація адміністратора без перевірки пароля
        var user = new UserDto
        {
            UserId = 1,
            UserName = "Admin",
            RoleName = "Admin"
        };

        if (user != null)
        {
            _messageService.ShowMessage("Login successful! Welcome, Admin.");
            _navigateToMain(user);
            Application.Current.MainWindow.Close();
        }
        else
        {
            ErrorMessage = "Invalid credentials!";
        }
    }

    private bool CanExecuteLogin(object parameter)
    {
        return true; // Дозволяємо авторизацію без перевірки
    }

    private void ValidatePassword()
    {
        IsPasswordValid = !string.IsNullOrEmpty(Password) && Password.Length >= 3;
        ErrorMessage = IsPasswordValid ? string.Empty : "Password must be at least 3 characters long";
    }

    private void ValidateLogin()
    {
        if (string.IsNullOrEmpty(Username))
        {
            ErrorMessage = "Username cannot be empty";
        }
        else
        {
            ErrorMessage = string.Empty;
        }
    }

    public void Logout()
    {
        _authService.Logout();
        Username = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
    }

    public string CurrentUserRole => _authService.CurrentUserRole;
}