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

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            try
            {
                var response = await _apiClient.LoginAsync(new Contracts.Requests.Auth.LoginRequest { UserName = Username, Password = Password });
                _apiClient.SetAuthorizationHeader(response.Token);
                _navigationService.NavigateTo<ProductViewModel>();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("Invalid credentials") || ex.Message.Contains("400"))
                {
                    ErrorMessage = "Неверный логин или пароль.";
                }
                else
                {
                    ErrorMessage = "Произошла ошибка при входе.";
                }
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