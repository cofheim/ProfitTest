using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfitTest.Services;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProfitTest.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        private readonly ApiClient _apiClient;
        private readonly NavigationService _navigationService;

        public LoginViewModel(ApiClient apiClient, NavigationService navigationService)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task Login(PasswordBox passwordBox)
        {
            if (passwordBox is null || string.IsNullOrEmpty(passwordBox.Password) || string.IsNullOrEmpty(Username))
            {
                ErrorMessage = "Username and password are required.";
                return;
            }

            try
            {
                var response = await _apiClient.LoginAsync(new Contracts.Requests.Auth.LoginRequest { UserName = Username, Password = passwordBox.Password });
                _apiClient.SetAuthorizationHeader(response.Token);
                _navigationService.NavigateTo<ProductViewModel>();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
} 