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

            User32.EnumWindows(new User32.EnumWindowsDelegate((hWnd, lparam)=>
            {
                //ウィンドウのタイトルの長さを取得し、タイトルを持つもののみ結果とする
                int textLen = User32.GetWindowTextLength(hWnd);
                if (textLen > 0)
                {
                    //ウィンドウのタイトルを取得する
                    StringBuilder tsb = new StringBuilder(textLen + 1);
                    User32.GetWindowText(hWnd, tsb, tsb.Capacity);
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
            var targetThreadId = User32.GetWindowThreadProcessId(targetHandle, out var _);

            // 現在アクティブとなっているウィンドウのスレッドIDを取得する
            var currentActiveThreadId = User32.GetWindowThreadProcessId(User32.GetForegroundWindow(), out var _);

            // アクティブ処理
            User32.SetForegroundWindow(targetHandle);
            if (targetThreadId == currentActiveThreadId)
            {
                // 現在アクティブなウィンドウがキャプチャ対象のウィンドウの場合は前面に持ってくる
                User32.BringWindowToTop(targetHandle);
            }
            else
            {
                // 別のプロセスがアクティブな場合は、そのプロセスにアタッチし、入力を奪う
                User32.AttachThreadInput(targetThreadId, currentActiveThreadId, true);
                try
                {
                    // 前面に持ってくる
                    User32.BringWindowToTop(targetHandle);
                }
                finally
                {
                    // アタッチを解除する
                    User32.AttachThreadInput(targetThreadId, currentActiveThreadId, false);
                }
            }
        }

        /// <summary>
        /// ウィンドウの表示領域を設定する
        /// </summary>
        public static void SetWindowBounds(IntPtr hWnd, System.Drawing.Rectangle bounds)
        {
            User32.SetWindowPos(hWnd, 0,
                bounds.X, bounds.Y, bounds.Width, bounds.Height, 
                (User32.SWP_NOZORDER | User32.SWP_SHOWWINDOW));
        }

        /// <summary>
        /// ウィンドウの表示領域を取得する
        /// </summary>
        public static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            var window = new Rectangle();
            User32.GetWindowRect(windowHandle, ref window);
            return window;
        }
    }
}
