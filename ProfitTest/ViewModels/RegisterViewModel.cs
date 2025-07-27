using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfitTest.Services;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProfitTest.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly ApiClient _apiClient;
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _username;

        [ObservableProperty]
        private string _errorMessage;
        
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
                ErrorMessage = "Имя пользователя и пароль обязательны.";
                return;
            }
            try
            {
                await _apiClient.RegisterAsync(new Contracts.Requests.Auth.RegisterRequest { UserName = Username, Password = passwordBox.Password });
                _navigationService.NavigateTo<LoginViewModel>();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Ошибка регистрации: {ex.Message}";
            }
        }
        
        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationService.NavigateTo<LoginViewModel>();
        }
    }
} 