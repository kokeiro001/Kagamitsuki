using System;
using System.Drawing;
using Kagamitsuki.Native;

using Drawing = System.Drawing;

namespace Kagamitsuki
{
    public class WindowCapture
    {
        readonly IntPtr windowHandle;
        Graphics writerBitmapGraphcis = null;

        /// <summary>キャプチャ対象のウィンドウ内におけるキャプチャ範囲</summary>
        public Drawing.Rectangle CaptureBoundsInWindow { get; private set; }

        public Bitmap WriterBitmap { get; private set; }


        /// <summary>キャプチャした画像を保持するためのBitmapインスタンスが有効な状態であるか取得する</summary>
        private bool EnableWriterBitmap => 
                       WriterBitmap != null &&
                       WriterBitmap.Width == CaptureBoundsInWindow.Width &&
                       WriterBitmap.Height == CaptureBoundsInWindow.Height;


        public WindowCapture(IntPtr windowHandle)
            : this(windowHandle, WindowNativeHelper.GetWindowBounds(windowHandle).ToSystemDrawingRectangle())
        {
        }

        public WindowCapture(IntPtr windowHandle, Drawing.Rectangle range)
        {
            this.windowHandle = windowHandle;
            CaptureBoundsInWindow = range;
            UpdateWriterBitmap();
        }

        private void UpdateWriterBitmap()
        {
            writerBitmapGraphcis?.Dispose();
            WriterBitmap?.Dispose();

            WriterBitmap = new Bitmap(CaptureBoundsInWindow.Width, CaptureBoundsInWindow.Height);
            writerBitmapGraphcis = Graphics.FromImage(WriterBitmap);
        }

        public Bitmap Capture()
        {
            if (!EnableWriterBitmap)
            {
                UpdateWriterBitmap();
            }

            IntPtr windowDC = User32.GetWindowDC(windowHandle);
            IntPtr hDC = writerBitmapGraphcis.GetHdc();
            Gdi32.BitBlt(hDC,
                   0,
                   0,
                   WriterBitmap.Width, WriterBitmap.Height,
                   windowDC,
                   CaptureBoundsInWindow.X,
                   CaptureBoundsInWindow.Y,
                   (int)Gdi32.TernaryRasterOperations.SRCCOPY);
            writerBitmapGraphcis.ReleaseHdc(hDC);
            User32.ReleaseDC(windowHandle, windowDC);

            return WriterBitmap;
        }
    }
}
