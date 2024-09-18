using CarListApp.Maui.ViewModel;

namespace CarListApp.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage(CarListViewModel carListViewModel)
        {
            InitializeComponent();
            BindingContext = carListViewModel;
        }
    }
}