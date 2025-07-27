using ProfitTest.ViewModels;
using System.Windows;

namespace ProfitTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService(typeof(MainViewModel));
        }
    }
}