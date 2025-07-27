using Microsoft.Extensions.DependencyInjection;
using ProfitTest.ViewModels;
using System.Windows.Controls;
using System.Windows;

namespace ProfitTest.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            var viewModel = App.AppHost.Services.GetRequiredService<LoginViewModel>();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                PasswordBox.Password = viewModel.Password;
            }
        }
    }
} 