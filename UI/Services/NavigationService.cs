using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.ViewModels;

namespace UI.Services;
/// <summary>
/// TODO
/// </summary>
public partial class NavigationService : ObservableObject, INavigationService
{
    [ObservableProperty]
    private IViewModel currentView;

    private readonly Func<Type, IViewModel> _viewModelFactory;

    public NavigationService(Func<Type, IViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : IViewModel
    {
        CurrentView = _viewModelFactory.Invoke(typeof(TViewModel));
    }
}
