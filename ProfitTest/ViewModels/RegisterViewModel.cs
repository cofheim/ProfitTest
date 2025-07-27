using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfitTest.Services;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProfitTest.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        private readonly ApiClient _apiClient;
        private readonly NavigationService _navigationService;

        public RegisterViewModel(ApiClient apiClient, NavigationService navigationService)
        {
            _apiClient = apiClient;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task Register(PasswordBox passwordBox)
        {
            if (passwordBox is null || string.IsNullOrEmpty(passwordBox.Password) || string.IsNullOrEmpty(Username))
            {
                ErrorMessage = "Username and password are required.";
                return;
            }
            try
            {
                await _apiClient.RegisterAsync(new Contracts.Requests.Auth.RegisterRequest { UserName = Username, Password = passwordBox.Password });
                _navigationService.NavigateTo<LoginViewModel>();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Registration failed: {ex.Message}";
            }
        }

        private bool CanRegister(PasswordBox passwordBox)
        {
            return !string.IsNullOrEmpty(Username) && passwordBox is not null && !string.IsNullOrEmpty(passwordBox.Password);
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }
    }
} 