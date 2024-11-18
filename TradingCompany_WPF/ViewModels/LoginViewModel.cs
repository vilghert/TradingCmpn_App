using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace TradingCompany_WPF.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isAuthenticated;
        private string _passwordHint;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                    PasswordHint = ValidatePassword(_password);
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordHint
        {
            get => _passwordHint;
            set
            {
                if (_passwordHint != value)
                {
                    _passwordHint = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public event Action OnLoginSuccess;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new RelayCommand(async () => await Login(), CanLogin);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task Login()
        {
            var user = await _authService.AuthenticateAsync(Username, Password);
            if (user != null)
            {
                IsAuthenticated = true;
                OnLoginSuccess?.Invoke();
            }
            else
            {
                ErrorMessage = "Невірний логін або пароль";
            }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password) &&
                   ValidateUsername(Username) == null &&
                   ValidatePassword(Password) == null;
        }

        private string ValidateUsername(string username)
        {
            var invalidCharacters = new[] { '@', '!', '?', '#', '$', '%', '^', '&', '*' };
            foreach (var ch in invalidCharacters)
            {
                if (username.StartsWith(ch.ToString()))
                {
                    return "Неприпустимий символ на початку імені користувача";
                }
            }
            return null;
        }

        private string ValidatePassword(string password)
        {
            if (password.Length < 3)
            {
                return "Пароль має містити мінімум 3 символи";
            }
            return null;
        }
    }
}