using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UI.Common.Helpers;

namespace UI.Controls.Sliders.CalculatorWindow;
/// <summary>
/// Interaction logic for LeverageSlider.xaml
/// </summary>
public partial class LeverageSlider : Grid
{
    public LeverageSlider()
    {
        InitializeComponent();
    }

    private void ShowGrabCursorHandler(object sender, MouseEventArgs e)
    {
        var senderRef = (Thumb)sender;
        senderRef.Cursor = ResourceAccessHelper.GrabCursorDummy.Cursor;
    }

    private void ShowGrabbingCursorHandler(object sender, DragStartedEventArgs e)
    {
        var senderRef = (Thumb)sender;
        senderRef.Cursor = ResourceAccessHelper.GrabbingCursorDummy.Cursor;
    }
}
