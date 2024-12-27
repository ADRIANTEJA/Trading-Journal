using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Events;
using UI.Windows;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for AccountsListViewItem.xaml
/// </summary>
public partial class AccountsListViewItem : Border
{
    private readonly IEventAggregator _eventAggregator;

    public AccountsListViewItem()
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();

        _eventAggregator.GetEvent<ChangeAccountClickEvent>().Subscribe(SelectAccountClickHandler);
        _eventAggregator.GetEvent<DeleteAccountClickEvent>().Subscribe(DeleteAccountClickHandler);
        _eventAggregator.GetEvent<SelectedAccountUpdatedEvent>().Subscribe(SelectedAccountUpdatedHandler);
    }

    private void DeleteAccountClickHandler()
    {
        event_receptor_helper.MouseDown -= ChangeSelectedAccountClickHandler;
        event_receptor_helper.MouseDown += AccountDeletedClickHandler;
        event_receptor_helper.MouseEnter += OnAccountItemMouseEnterHandler;
        event_receptor_helper.MouseLeave += OnAccountItemMouseLeaveHandler;
        event_receptor_helper.Cursor = Cursors.Hand;

        BorderThickness = new(5);
    }

    private void AccountDeletedClickHandler(object sender, MouseButtonEventArgs e)
    {
        var contextAccount = (Account)DataContext;

        var deleteAccountWarningWindow = new DeleteAccountWarningWindow(contextAccount.Id);
        deleteAccountWarningWindow.ShowDialog();
    }

    private void SelectAccountClickHandler()
    {
        event_receptor_helper.MouseDown -= AccountDeletedClickHandler;
        event_receptor_helper.MouseDown += ChangeSelectedAccountClickHandler;
        event_receptor_helper.MouseEnter += OnAccountItemMouseEnterHandler;
        event_receptor_helper.MouseLeave += OnAccountItemMouseLeaveHandler;
        event_receptor_helper.Cursor = Cursors.Hand;

        BorderThickness = new(5);
    }

    private void ChangeSelectedAccountClickHandler(object sender, MouseButtonEventArgs e)
    {
        var contextAccount = (Account)DataContext;

        _eventAggregator.GetEvent<SelectedAccountChangedEvent>().Publish(contextAccount.Id);
    }

    private void SelectedAccountUpdatedHandler()
    {
        event_receptor_helper.MouseDown -= ChangeSelectedAccountClickHandler;
        event_receptor_helper.MouseEnter -= OnAccountItemMouseEnterHandler;
        event_receptor_helper.MouseLeave -= OnAccountItemMouseLeaveHandler;
        event_receptor_helper.Cursor = Cursors.Arrow;

        BorderThickness = new(1);
    }

    private void OnAccountItemMouseEnterHandler(object sender, MouseEventArgs e) => BorderThickness = new(3);

    private void OnAccountItemMouseLeaveHandler(object sender, MouseEventArgs e) => BorderThickness = new(5);

    private void OnAccountsListViewItemUnloadedHandler(object sender, RoutedEventArgs e)
    {
        event_receptor_helper.MouseDown -= AccountDeletedClickHandler;
        event_receptor_helper.MouseDown -= ChangeSelectedAccountClickHandler;
        event_receptor_helper.MouseEnter -= OnAccountItemMouseEnterHandler;
        event_receptor_helper.MouseLeave -= OnAccountItemMouseLeaveHandler;
    }
}
