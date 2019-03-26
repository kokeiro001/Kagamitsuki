using System;
using System.Collections.Generic;
using System.Text;
using Kagamitsuki.Native;

namespace Kagamitsuki
{
    public static class WindowNativeHelper
    {
        /// <summary>
        /// Windowハンドルとタイトルの一覧を取得する
        /// </summary>
        public static IList<(string title, IntPtr hWnd)> GetWindows()
        {
            var list = new List<(string title, IntPtr hWnd)>();

            User32NativeMethods.EnumWindows(new User32NativeMethods.EnumWindowsDelegate((hWnd, lparam) =>
            {
                // ウィンドウのタイトルの長さを取得し、タイトルを持つもののみ結果とする
                int textLen = User32NativeMethods.GetWindowTextLength(hWnd);
                if (textLen > 0)
                {
                    // ウィンドウのタイトルを取得する
                    StringBuilder tsb = new StringBuilder(textLen + 1);
                    User32NativeMethods.GetWindowText(hWnd, tsb, tsb.Capacity);
                    list.Add((tsb.ToString(), hWnd));
                }

                return true;
            }), IntPtr.Zero);
            return list;
        }

        /// <summary>
        /// ウィンドウを最前面に表示する
        /// </summary>
        public static void SetForceForegroundWindow(IntPtr targetHandle)
        {
            // ターゲットとなるハンドルのスレッドIDを取得する
            var targetThreadId = User32NativeMethods.GetWindowThreadProcessId(targetHandle, out var _);

            // 現在アクティブとなっているウィンドウのスレッドIDを取得する
            var currentActiveThreadId = User32NativeMethods.GetWindowThreadProcessId(User32NativeMethods.GetForegroundWindow(), out var _);

            // アクティブ処理
            User32NativeMethods.SetForegroundWindow(targetHandle);
            if (targetThreadId == currentActiveThreadId)
            {
                // 現在アクティブなウィンドウがキャプチャ対象のウィンドウの場合は前面に持ってくる
                User32NativeMethods.BringWindowToTop(targetHandle);
            }
            else
            {
                // 別のプロセスがアクティブな場合は、そのプロセスにアタッチし、入力を奪う
                User32NativeMethods.AttachThreadInput(targetThreadId, currentActiveThreadId, true);
                try
                {
                    // 前面に持ってくる
                    User32NativeMethods.BringWindowToTop(targetHandle);
                }
                finally
                {
                    // アタッチを解除する
                    User32NativeMethods.AttachThreadInput(targetThreadId, currentActiveThreadId, false);
                }
            }
        }

        /// <summary>
        /// ウィンドウの表示領域を設定する
        /// </summary>
        public static void SetWindowBounds(IntPtr windowHandle, System.Drawing.Rectangle bounds)
        {
            var wflags = User32NativeMethods.SWP_NOZORDER | User32NativeMethods.SWP_SHOWWINDOW;
            User32NativeMethods.SetWindowPos(windowHandle, 0,
                bounds.X, bounds.Y, bounds.Width, bounds.Height, 
                wflags);
        }

        /// <summary>
        /// ウィンドウの表示領域を取得する
        /// </summary>
        public static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            var window = new Rectangle();
            User32NativeMethods.GetWindowRect(windowHandle, ref window);
            return window;
        }
    }
}
