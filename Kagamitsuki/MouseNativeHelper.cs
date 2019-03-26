using System;
using System.Threading.Tasks;
using Kagamitsuki.Native;

namespace Kagamitsuki
{
    public static class MouseNativeHelper
    {
        public static void Click(IntPtr windowHandle, int x, int y)
        {
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_LBUTTONDOWN, new IntPtr(User32NativeMethods.MK_LBUTTON), WinAPI.CreateLParam(x, y));
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_LBUTTONUP, new IntPtr(User32NativeMethods.MK_LBUTTON), WinAPI.CreateLParam(x, y));
        }

        public static async Task ClickAsync(IntPtr windowHandle, int x, int y, TimeSpan delayButtonUp)
        {
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_LBUTTONDOWN, new IntPtr(User32NativeMethods.MK_LBUTTON), WinAPI.CreateLParam(x, y));
            await Task.Delay(delayButtonUp);
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_LBUTTONUP, new IntPtr(User32NativeMethods.MK_LBUTTON), WinAPI.CreateLParam(x, y));
        }

        public static void Move(IntPtr windowHandle, int x, int y)
        {
            User32NativeMethods.SendMessage(windowHandle, User32NativeMethods.WM_MOUSEMOVE, IntPtr.Zero, WinAPI.CreateLParam(x, y));
        }
    }
}
