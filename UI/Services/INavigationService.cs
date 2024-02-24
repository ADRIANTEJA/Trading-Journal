using MainModule.ViewModels;

namespace UI.Services;
/// <summary>
/// defines a navigation service current view and navigation method contract
/// </summary>
public interface INavigationService
{
    IViewModel CurrentView { get; }

    void NavigateTo<TViewModel>() where TViewModel : IViewModel;
}
