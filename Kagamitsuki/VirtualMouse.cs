using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Kagamitsuki
{
    public class VirtualMouseNativeMethods
    {
        private readonly IntPtr hWnd;
        private readonly Point origin;

        public VirtualMouseNativeMethods(IntPtr hWnd)
            : this(hWnd, 0, 0)
        {
        }

        public VirtualMouseNativeMethods(IntPtr hWnd, int originX, int originY)
        {
            this.hWnd = hWnd;
            origin = new Point(origin.X, origin.Y);
        }

        public void Click(int x, int y)
        {
            MouseNativeHelper.Click(hWnd, origin.X + x, origin.Y + y);
        }

        public async Task ClickAsync(int x, int y, TimeSpan delayButtonUp)
        {
            await MouseNativeHelper.ClickAsync(hWnd, origin.X + x, origin.Y + y, delayButtonUp);
        }

        public void Move(int x, int y)
        {
            MouseNativeHelper.Move(hWnd, origin.X + x, origin.Y + y);
        }
    }
}
