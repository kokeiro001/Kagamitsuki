using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kagamitsuki.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static class WinAPIEx
    {
        public static System.Drawing.Rectangle ToSystemDrawingRectangle(this Kagamitsuki.Native.Rectangle rect)
        {
            return new System.Drawing.Rectangle(
                rect.left,
                rect.top,
                rect.right - rect.left,
                rect.bottom - rect.top);
        }
    }

    public static class WinAPI
    {
        public static IntPtr CreateLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }
    }
}
