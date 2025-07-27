using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace ProfitTest.ViewModels
{
    public partial class ProductEditModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private DateTime _priceValidFrom = DateTime.UtcNow;

        [ObservableProperty]
        private DateTime? _priceValidTo;
    }
} 