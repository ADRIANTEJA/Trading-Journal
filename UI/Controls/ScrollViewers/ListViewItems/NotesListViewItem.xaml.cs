using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for NotesListViewItem.xaml
/// </summary>
public partial class NotesListViewItem : Border
{
    private readonly IEventAggregator _eventAggregator;

    public NotesListViewItem()
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
    }

    private void OnDeleteNoteButtonClickHandler(object sender, RoutedEventArgs e)
    {
        var contextNote = (AnalysisNote)DataContext;

        _eventAggregator.GetEvent<DeleteNoteClickEvent>().Publish(contextNote.Id);
    }
}
