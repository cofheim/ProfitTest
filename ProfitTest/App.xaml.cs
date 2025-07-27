using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfitTest.Services;
using ProfitTest.ViewModels;
using ProfitTest.Views;
using System;
using System.Net.Http;
using System.Windows;

namespace ProfitTest
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<ApiClient>();
                    services.AddSingleton<NavigationService>();

                    services.AddSingleton<MainView>();
                    services.AddSingleton<MainViewModel>();

                    services.AddTransient<LoginView>();
                    services.AddTransient<LoginViewModel>();

                    services.AddTransient<ProductEditorView>();
                    services.AddTransient<ProductEditorViewModel>();

                    services.AddSingleton<DialogService>();
                    services.AddSingleton<RegisterViewModel>();
                    services.AddSingleton<ProductViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();
            MainWindow = AppHost.Services.GetRequiredService<MainView>();
            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }
}
