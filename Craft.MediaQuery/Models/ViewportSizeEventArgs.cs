namespace Craft.MediaQuery.Models;

public class ViewportSizeEventArgs : EventArgs
{
    public int Height { get; set; }

    public int Width { get; set; }
}
