using System;
using System.Drawing;

namespace Kagamitsuki
{
    public class VirtualMouse
    {
        private readonly IntPtr hWnd;
        private readonly Point origin;

        public VirtualMouse(IntPtr hWnd)
            : this(hWnd, 0, 0)
        {
        }

        public VirtualMouse(IntPtr hWnd, int originX, int originY)
        {
            this.hWnd = hWnd;
            origin = new Point(origin.X, origin.Y);
        }

        public void Click(int x, int y)
        {
            MouseNativeHelper.Click(hWnd, origin.X + x, origin.Y + y);
        }

        public void Move(int x, int y)
        {
            MouseNativeHelper.Move(hWnd, origin.X + x, origin.Y + y);
        }
    }
}
