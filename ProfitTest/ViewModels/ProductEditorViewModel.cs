using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ProfitTest.ViewModels
{
    public partial class ProductEditorViewModel : ViewModelBase
    {
        private ProductEditModel _product = new();
        public ProductEditModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Action<bool?>? SetDialogResult { get; set; }
        
        public void SetProduct(ProductEditModel model)
        {
            Product = model;
            Title = model.Id == Guid.Empty ? "Добавление товара" : "Редактирование товара";
        }

        [RelayCommand]
        private void Save()
        {
            SetDialogResult?.Invoke(true);
        }
    }
} 