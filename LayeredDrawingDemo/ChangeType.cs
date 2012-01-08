using System;

namespace LayeredDrawingDemo
{
    [Flags]
    public enum ChangeType
    {
        Redraw = 1,
        Resize = 2,
        Scroll = 4,
    }
}