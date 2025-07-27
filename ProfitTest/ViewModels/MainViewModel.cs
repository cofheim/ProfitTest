using CommunityToolkit.Mvvm.Input;
using ProfitTest.Contracts.Requests.Products;
using ProfitTest.Contracts.Responses.Products;
using ProfitTest.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using ProfitTest.ViewModels;

namespace ProfitTest.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private ViewModelBase _currentViewModel;

        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _navigationService.NavigateTo<LoginViewModel>();
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModel = _navigationService.CurrentViewModel;
        }
    }
} 