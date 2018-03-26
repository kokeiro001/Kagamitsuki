using System;
using System.Threading.Tasks;
using Kagamitsuki.Native;

namespace Kagamitsuki
{
    public static class MouseNativeHelper
    {
        public static void Click(IntPtr windowHandle, int x, int y)
        {
            User32.SendMessage(windowHandle, User32.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
            User32.SendMessage(windowHandle, User32.WM_LBUTTONDOWN, new IntPtr(User32.MK_LBUTTON), WinAPI.CreateLParam(x, y));
            User32.SendMessage(windowHandle, User32.WM_LBUTTONUP, new IntPtr(User32.MK_LBUTTON), WinAPI.CreateLParam(x, y));
        }

        public static async Task ClickAsync(IntPtr windowHandle, int x, int y, TimeSpan delayButtonUp)
        {
            User32.SendMessage(windowHandle, User32.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
            User32.SendMessage(windowHandle, User32.WM_LBUTTONDOWN, new IntPtr(User32.MK_LBUTTON), WinAPI.CreateLParam(x, y));
            await Task.Delay(delayButtonUp);
            User32.SendMessage(windowHandle, User32.WM_LBUTTONUP, new IntPtr(User32.MK_LBUTTON), WinAPI.CreateLParam(x, y));
        }

        public static void Move(IntPtr windowHandle, int x, int y)
        {
            User32.SendMessage(windowHandle, User32.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
        }
    }
}
