using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarListApp.Maui.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string? title;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(isNotLoading))]
        bool isLoading;

        public bool isNotLoading => !isLoading;
    }
}
