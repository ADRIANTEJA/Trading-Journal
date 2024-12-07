using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Utils;
using UI.Controls.ScrollViewers.ScrollableGridViewItems;
using UI.Events;

namespace UI.Windows;
/// <summary>
/// Interaction logic for TradeImageWindow.xaml
/// </summary>
public partial class TradeImageWindow : Window
{
    private readonly IEventAggregator _eventAggragator;

    public TradeImageWindow()
    {
        InitializeComponent();

        _eventAggragator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
        _eventAggragator.GetEvent<TradeImageDeletedEvent>().Subscribe(LoadTradeImagesOnGallery);

        var grid = new Grid();

        grid.ColumnDefinitions.Add(new()
        {
            Width = new(2, GridUnitType.Star)
        });
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e) 
    { 
        if (e.ChangedButton == MouseButton.Left) DragMove(); 
    }

    private void OnTradeImagesWindowLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (TradeImageViewModel)DataContext;
        dataContext.LoadTradeImagesCommand.Execute(dataContext.HomeViewModel.SelectedTrade);

        LoadTradeImagesOnGallery();
    }

    private void AddTradeImageHandler(object sender, RoutedEventArgs e)
    {
        var imagePath = MiscFunctions.GetImagePathFromDisk();

        if (string.IsNullOrEmpty(imagePath)) return; 

        var imageBytes = File.ReadAllBytes(imagePath);

        //updates the UI
        TradeImageContainer tradeImageContainer = new();

        var imageControlRef = (Image)tradeImageContainer.FindName("image_control");

        try
        {
            imageControlRef.Source = MiscFunctions.ByteArrayToBitmapSource(imageBytes);
        }
        catch (ArgumentException) 
        { 
            ErrorHandlers.HandleImageConvertionError();
            return;
        }

        image_gallery.Children.Add(tradeImageContainer);

        //then continous to add the image to the database
        var dataContext = (TradeImageViewModel)DataContext;

        var imageId = 1;
        if (dataContext.Images.Count > 0) imageId = dataContext.Images[dataContext.Images.Count - 1].Id + 1;

        dataContext.AddTradeImageCommand.Execute(new() { Id = imageId, TradeId = 0, Image = imageBytes});

        //it stores the Image Id value in the Tag Property so it can be
        //used to delete the image later
        tradeImageContainer.Tag = imageId;

        //iterates over the grid cells based on how many images
        //are in Images and adds the image at the next empty cell
        int columnsIndex = 0;
        int rowsIndex = 0;

        for (int i = 0; i < dataContext.Images.Count; i++)
        {
            if (columnsIndex == 3)
            {
                if (i == dataContext.Images.Count - 1)
                {
                    Grid.SetColumn(tradeImageContainer, columnsIndex);
                    Grid.SetRow(tradeImageContainer, rowsIndex);
                }

                image_gallery.RowDefinitions.Add(new()
                {
                    Height = new(128)
                });

                rowsIndex++;
                columnsIndex = 0;
            }
            else
            {
                if (i == dataContext.Images.Count - 1)
                {
                    Grid.SetColumn(tradeImageContainer, columnsIndex);
                    Grid.SetRow(tradeImageContainer, rowsIndex);
                }
                columnsIndex++;
            }
        }

        //updates the add button position in the grid
        foreach (var child in image_gallery.Children)
        {
            if (child is AddTradeImageButton button)
            {
                //prevents the add button to spawn beyond image limit
                if (rowsIndex > 1)
                {
                    image_gallery.Children.Remove(button);
                    return;
                }

                Grid.SetColumn(button, columnsIndex);
                Grid.SetRow(button, rowsIndex);
            }
        }
    }

    private void LoadTradeImagesOnGallery()
    {
        var test = image_gallery.Children.Count;

        image_gallery.Children.Clear();

        var dataContext = (TradeImageViewModel)DataContext;

        int columnsIndex = 0;
        int rowsIndex = 0;

        //adds as many images to the grid as there are in
        //Images and creates new rows when needed
        foreach (var image in dataContext.Images)
        {
            TradeImageContainer tradeImageContainer = new();

            var imageControlRef = (Image)tradeImageContainer.FindName("image_control");
            imageControlRef.Source = MiscFunctions.ByteArrayToBitmapSource(image.Image);

            //it stores the Image Id value in the Tag Property so it can be
            //used to delete the image later
            tradeImageContainer.Tag = image.Id;

            image_gallery.Children.Add(tradeImageContainer);

            Grid.SetColumn(tradeImageContainer, columnsIndex);
            Grid.SetRow(tradeImageContainer, rowsIndex);

            if (columnsIndex == 3)
            {
                image_gallery.RowDefinitions.Add(new()
                {
                    Height = new(128)
                });

                rowsIndex++;
                columnsIndex = 0;
            }
            else columnsIndex++;
        }

        //limits the amount of images by "hiding" the add button
        if (rowsIndex > 1) return;

        //sets the add button at the last cell of the grid
        //if the image limit has not been reached
        AddTradeImageButton addTradeImageButton = new()
        {
            Height = double.NaN,
            Margin = new Thickness(5),
        };
        addTradeImageButton.Click += new RoutedEventHandler(AddTradeImageHandler);

        image_gallery.Children.Add(addTradeImageButton);

        Grid.SetRow(addTradeImageButton, rowsIndex);
        Grid.SetColumn(addTradeImageButton, columnsIndex);
    }
}
