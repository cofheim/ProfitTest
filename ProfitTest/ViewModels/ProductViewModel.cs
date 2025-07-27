using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProfitTest.Contracts.Requests.Products;
using ProfitTest.Contracts.Responses.Products;
using ProfitTest.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProfitTest.ViewModels
{
    public partial class ProductViewModel : ViewModelBase
    {
        private readonly ApiClient _apiClient;
        private readonly DialogService _dialogService;

        public ObservableCollection<ProductResponse> Products { get; } = new();

        [ObservableProperty]
        private ProductResponse _selectedProduct;

        [ObservableProperty]
        private string _searchQuery;

        public ProductViewModel(ApiClient apiClient, DialogService dialogService)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
            _ = LoadProducts();
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            var products = await _apiClient.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        [RelayCommand]
        private async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await LoadProducts();
                return;
            }

            var products = await _apiClient.SearchByNameAsync(SearchQuery);
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        [RelayCommand]
        private async Task ShowAllProducts()
        {
            SearchQuery = string.Empty;
            await LoadProducts();
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            var editModel = new ProductEditModel();
            var result = _dialogService.ShowProductEditor(editModel);

            if (result == true)
            {
                var request = new CreateProductRequest
                {
                    Name = editModel.Name,
                    Price = editModel.Price,
                    PriceValidFrom = editModel.PriceValidFrom.ToUniversalTime(),
                    PriceValidTo = editModel.PriceValidTo?.ToUniversalTime()
                };
                await _apiClient.CreateProductAsync(request);
                await LoadProducts();
            }
        }

        [RelayCommand]
        private async Task EditProduct()
        {
            if (SelectedProduct == null) return;
            var editModel = new ProductEditModel
            {
                Id = SelectedProduct.Id,
                Name = SelectedProduct.Name,
                Price = SelectedProduct.Price,
                PriceValidFrom = SelectedProduct.PriceValidFrom,
                PriceValidTo = SelectedProduct.PriceValidTo
            };
            var result = _dialogService.ShowProductEditor(editModel);

            if (result == true)
            {
                var request = new UpdateProductRequest
                {
                    Id = editModel.Id,
                    Name = editModel.Name,
                    Price = editModel.Price,
                    PriceValidFrom = editModel.PriceValidFrom.ToUniversalTime(),
                    PriceValidTo = editModel.PriceValidTo?.ToUniversalTime()
                };
                await _apiClient.UpdateProductAsync(editModel.Id, request);
                await LoadProducts();
            }
        }

        [RelayCommand]
        private async Task DeleteProduct()
        {
            await _apiClient.DeleteProductAsync(SelectedProduct.Id);
            await LoadProducts();
        }
    }
} 