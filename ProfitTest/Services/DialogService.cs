using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ProfitTest.Contracts.Requests.Products;
using ProfitTest.Contracts.Responses.Products;
using ProfitTest.Services;
using ProfitTest.ViewModels;
using ProfitTest.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ProfitTest.Services
{
    public class DialogService
    {
        public bool? ShowProductEditor(ProductEditModel editModel)
        {
            var vm = new ProductEditorViewModel();
            vm.SetProduct(editModel);
            
            var dialog = new ProductEditorView
            {
                DataContext = vm,
                Owner = App.AppHost.Services.GetService(typeof(MainView)) as MainView
            };
            
            vm.SetDialogResult = (result) => dialog.DialogResult = result;
            
            return dialog.ShowDialog();
        }
    }
}
