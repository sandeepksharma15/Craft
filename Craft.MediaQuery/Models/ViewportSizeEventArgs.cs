namespace Craft.MediaQuery.Models;

public class ViewportSizeEventArgs : EventArgs
{
    #region Public Properties

    public int Height { get; set; }

    public int Width { get; set; }

    #endregion Public Properties
}