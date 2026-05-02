using PharmoSys.Commands;
using PharmoSys.Core.Store;
using PharmoSys.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmoSys.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public Action CloseAction { get; set; }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
            
            // Seed Admin User (Normally this wouldn't be in the ViewModel constructor, but doing it here for testing ease)
            Task.Run(async () => await _authService.EnsureAdminExistsAsync());
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !IsLoading;
        }

        private async Task LoginAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var user = await _authService.LoginAsync(Username, Password);

                if (user != null)
                {
                    AppSession.CurrentUser = user;
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Login Successful!\nWelcome {user.Username} ({user.Role})", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        CloseAction?.Invoke();
                    });
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                    MessageBox.Show(ErrorMessage, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Database Connection Error: {ex.Message}";
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
