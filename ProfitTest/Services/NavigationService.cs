using Microsoft.Extensions.DependencyInjection;
using ProfitTest.Views;
using ProfitTest.ViewModels;
using System;
using System.Windows;

namespace ProfitTest.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public event Action CurrentViewModelChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            CurrentViewModel = (ViewModelBase)_serviceProvider.GetService(typeof(TViewModel));
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}