
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace UI.Common.Resources.Assets.Cursors;

public class CustomCursors
{
    private static readonly Cursor grabCursor = new("/Common/Resources/Assets/Cursors/grab.cur");

    public static Cursor GrabCursor
    {
        get { return grabCursor; }
    }

    private static readonly Cursor grabbingCursor = new(Path.Combine(Environment.CurrentDirectory, "Common/Resources/Assets/Cursors/grabbing.cur"));

    public static Cursor GrabbingCursor
    {
        get { return grabbingCursor; }
    }
}
