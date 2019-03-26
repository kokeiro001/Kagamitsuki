using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kagamitsuki.SampleConsoleApp
{
    class Program
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan ClickInterval = TimeSpan.FromSeconds(0.1);
        private static readonly string CapturedImageSaveDir = @"I:\Kagamitsuki"; // 自分の都合のいいディレクトリに変えてNE

        static async Task Main(string[] args)
        {
            var windows = WindowNativeHelper.GetWindows();

            var targetWindows = windows
                .Where(x => x.title.EndsWith("Mozilla Firefox"))
                .ToArray();

            if (targetWindows.Length == 0)
            {
                throw new InvalidOperationException("FireFoxを起動してから実行してください～");
            }

            // とりあえず1件だけ処理する
            var targetWindowHandle = targetWindows[0].hWnd;

            var bounds = WindowNativeHelper.GetWindowBounds(targetWindowHandle);
            var captureBounds = new Rectangle(0, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);

            WindowNativeHelper.SetForceForegroundWindow(targetWindowHandle);

            var windowCapture = new WindowCapture(targetWindowHandle, captureBounds);
            var virtualMouse = new VirtualMouseNativeMethods(targetWindowHandle, 0, 0);

            var clickPoints = new Point[] 
            {
                new Point(150, 300),
                new Point(170, 200),
                new Point(200, 250),
            };

            // 画像をキャプチャしつつ、適当な場所をクリックする
            for (int i = 0; i < clickPoints.Length; i++)
            {
                await virtualMouse.ClickAsync(clickPoints[i].X, clickPoints[i].Y, ClickInterval);

                var bitmap = windowCapture.Capture();
                bitmap.Save(Path.Combine(CapturedImageSaveDir, $"{i:D3}.png"));

                await Task.Delay(Interval);
            }
        }
    }
}
