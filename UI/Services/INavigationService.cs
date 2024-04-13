using MainModule.ViewModels;

namespace UI.Services;
/// <summary>
/// defines a navigation service current view and navigation method contract
/// </summary>
public interface INavigationService
{
    IViewModel CurrentView { get; }

    Task NavigateToAsync<TViewModel>() where TViewModel : IViewModel;

    void NavigateTo<TViewModel>() where TViewModel : IViewModel;
}
