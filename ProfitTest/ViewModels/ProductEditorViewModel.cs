using CommunityToolkit.Mvvm.Input;
using ProfitTest.Contracts.Responses.Products;
using System;
using System.ComponentModel;

namespace ProfitTest.ViewModels
{
    public class ProductEditModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Guid _id;
        private string _name = string.Empty;
        private decimal _price;
        private DateTime _priceValidFrom = DateTime.UtcNow;
        private DateTime? _priceValidTo;

        public Guid Id { get => _id; set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id))); } }
        public string Name { get => _name; set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); } }
        public decimal Price { get => _price; set { _price = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price))); } }
        public DateTime PriceValidFrom { get => _priceValidFrom; set { _priceValidFrom = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceValidFrom))); } }
        public DateTime? PriceValidTo { get => _priceValidTo; set { _priceValidTo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceValidTo))); } }
    }

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

        public ProductEditorViewModel()
        {
            SaveCommand = new RelayCommand(SaveChanges);
        }
        
        public IRelayCommand SaveCommand { get; }
        
        public void SetProduct(ProductEditModel model)
        {
            Product = model;
            Title = model.Id == Guid.Empty ? "Добавление товара" : "Редактирование товара";
        }

        private void SaveChanges()
        {
            // TODO: Add validation
            SetDialogResult?.Invoke(true);
        }
    }
} 