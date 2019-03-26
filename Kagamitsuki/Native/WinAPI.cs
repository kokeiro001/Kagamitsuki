using System;
using System.Runtime.InteropServices;

namespace Kagamitsuki.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static class WinAPIEx
    {
        public static System.Drawing.Rectangle ToSystemDrawingRectangle(this Kagamitsuki.Native.Rectangle rect)
        {
            return new System.Drawing.Rectangle(
                rect.Left,
                rect.Top,
                rect.Right - rect.Left,
                rect.Bottom - rect.Top);
        }
    }

    public static class WinAPI
    {
        public static IntPtr CreateLParam(int lowWord, int highWord)
        {
            return (IntPtr)((highWord << 16) | (lowWord & 0xffff));
        }
    }
}
