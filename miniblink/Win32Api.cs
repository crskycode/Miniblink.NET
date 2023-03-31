using System;
using System.Runtime.InteropServices;

namespace Miniblink
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
    }

    internal static class Win32Api
    {
        internal const int WM_SETFOCUS = 7;
        internal const int WM_KILLFOCUS = 8;
        internal const int WM_PAINT = 15;
        internal const int WM_SETCURSOR = 32;
        internal const int SRCCOPY = 0x00CC0020;

        [DllImport("user32.dll")]
        internal static extern void BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        internal static extern void EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        internal static extern void BitBlt(IntPtr hDC, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
    }
}
