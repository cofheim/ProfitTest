using Microsoft.Extensions.DependencyInjection;
using ProfitTest.ViewModels;
using System.Windows.Controls;

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
    }
} 