using ProfitTest.ViewModels;
using System.Windows;

namespace ProfitTest.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(MainViewModel));
        }
    }
}