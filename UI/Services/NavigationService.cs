using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.Common;
using MainModule.ViewModels;

namespace UI.Services;
/// <summary>
/// An implementation of INavigationService that handles UI view creation
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

    public async Task NavigateToAsync<TViewModel>() where TViewModel : IViewModel =>
        await Task.Run(NavigateTo<TViewModel>);
        
    public void NavigateTo<TViewModel>() where TViewModel : IViewModel
    {
        Flags.IsThreadSafe = false;
        CurrentView = _viewModelFactory.Invoke(typeof(TViewModel));
        Flags.IsThreadSafe = true;
    }  
}
