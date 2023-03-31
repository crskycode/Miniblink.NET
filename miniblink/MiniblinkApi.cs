using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Miniblink
{
    internal static class MiniblinkApi
    {
        //
        // The constants.
        //

        internal enum MouseMsg
        {
            MOUSEMOVE = 0x0200,
            LBUTTONDOWN = 0x0201,
            LBUTTONUP = 0x0202,
            LBUTTONDBLCLK = 0x0203,
            RBUTTONDOWN = 0x0204,
            RBUTTONUP = 0x0205,
            RBUTTONDBLCLK = 0x0206,
            MBUTTONDOWN = 0x0207,
            MBUTTONUP = 0x0208,
            MBUTTONDBLCLK = 0x0209,
            MOUSEWHEEL = 0x020A,
        }

        [Flags]
        internal enum MouseFlags
        {
            NONE = 0,
            LBUTTON = 0x01,
            RBUTTON = 0x02,
            SHIFT = 0x04,
            CONTROL = 0x08,
            MBUTTON = 0x10,
        }

        internal enum CursorType
        {
            Pointer,
            Cross,
            Hand,
            IBeam,
            Wait,
            Help,
            EastResize,
            NorthResize,
            NorthEastResize,
            NorthWestResize,
            SouthResize,
            SouthEastResize,
            SouthWestResize,
            WestResize,
            NorthSouthResize,
            EastWestResize,
            NorthEastSouthWestResize,
            NorthWestSouthEastResize,
            ColumnResize,
            RowResize,
            MiddlePanning,
            EastPanning,
            NorthPanning,
            NorthEastPanning,
            NorthWestPanning,
            SouthPanning,
            SouthEastPanning,
            SouthWestPanning,
            WestPanning,
            Move,
            VerticalText,
            Cell,
            ContextMenu,
            Alias,
            Progress,
            NoDrop,
            Copy,
            None,
            NotAllowed,
            ZoomIn,
            ZoomOut,
            Grab,
            Grabbing,
            Custom
        }

        internal enum KeyFlags
        {
            NONE = 0,
            EXTENDED = 0x0100,
            REPEAT = 0x4000,
        }

        //
        // The prototype of function pointers.
        //

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeInitializeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr WkeCreateWebViewDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeDestroyWebViewDelegate(IntPtr webView);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetHandleDelegate(IntPtr webView, IntPtr hWnd);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeOnPaintUpdatedDelegate(IntPtr webView, WkePaintUpdatedCallbackDelegate callback, IntPtr callbackParam);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeResizeDelegate(IntPtr webView, int width, int height);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr WkeGetViewDCDelegate(IntPtr webView);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeLoadURLWDelegate(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] string url);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeLoadHTMLDelegate(IntPtr webView, [In] byte[] html);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeFireMouseEventDelegate(IntPtr webView, MouseMsg message, int x, int y, MouseFlags flags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeFireMouseWheelEventDelegate(IntPtr webView, int x, int y, int delta, MouseFlags flags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeFireKeyUpEventDelegate(IntPtr webView, int virtualKeyCode, KeyFlags flags, bool systemKey);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeFireKeyDownEventDelegate(IntPtr webView, int virtualKeyCode, KeyFlags flags, bool systemKey);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeFireKeyPressEventDelegate(IntPtr webView, int charCode, KeyFlags flags, bool systemKey);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate CursorType WkeGetCursorInfoTypeDelegate(IntPtr webView);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetContextMenuEnabledDelegate(IntPtr webView, bool state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetDragEnableDelegate(IntPtr webView, bool state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetDragDropEnableDelegate(IntPtr webView, bool state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetNavigationToNewWindowEnableDelegate(IntPtr webView, bool state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetCookieJarFullPathDelegate(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkeSetLocalStorageFullPathDelegate(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool WkeReloadDelegate(IntPtr webView);

        //
        // The prototype of callback function pointers.
        //

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void WkePaintUpdatedCallbackDelegate(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);

        //
        // The function pointers.
        //

        internal static WkeInitializeDelegate Initialize;
        internal static WkeCreateWebViewDelegate CreateWebView;
        internal static WkeDestroyWebViewDelegate DestroyWebView;
        internal static WkeSetHandleDelegate SetHandle;
        internal static WkeOnPaintUpdatedDelegate OnPaintUpdated;
        internal static WkeResizeDelegate Resize;
        internal static WkeGetViewDCDelegate GetViewDC;
        internal static WkeLoadURLWDelegate LoadURLW;
        internal static WkeLoadHTMLDelegate LoadHTML;
        internal static WkeFireMouseEventDelegate FireMouseEvent;
        internal static WkeFireMouseWheelEventDelegate FireMouseWheelEvent;
        internal static WkeFireKeyUpEventDelegate FireKeyUpEvent;
        internal static WkeFireKeyDownEventDelegate FireKeyDownEvent;
        internal static WkeFireKeyPressEventDelegate FireKeyPressEvent;
        internal static WkeGetCursorInfoTypeDelegate GetCursorInfoType;
        internal static WkeSetContextMenuEnabledDelegate SetContextMenuEnabled;
        internal static WkeSetDragEnableDelegate SetDragEnable;
        internal static WkeSetDragDropEnableDelegate SetDragDropEnable;
        internal static WkeSetNavigationToNewWindowEnableDelegate SetNavigationToNewWindowEnable;
        internal static WkeSetCookieJarFullPathDelegate SetCookieJarFullPath;
        internal static WkeSetLocalStorageFullPathDelegate SetLocalStorageFullPath;
        internal static WkeReloadDelegate Reload;

        //
        // Dynamic library functions.
        //

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hLibModule);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        //
        // The library handle.
        //

        private static IntPtr Lib;

        //
        // The static constructor.
        //

        static MiniblinkApi()
        {
        }

        internal static void Load()
        {
            string path;

            if (Environment.Is64BitProcess)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miniblink", "miniblink64.dll");
            else
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miniblink", "miniblink32.dll");

            Lib = LoadLibrary(path);
            Debug.Assert(Lib != IntPtr.Zero);

            var pfnInitialize = GetProcAddress(Lib, "wkeInitialize");
            Initialize = Marshal.GetDelegateForFunctionPointer<WkeInitializeDelegate>(pfnInitialize);

            var pfnCreateWebView = GetProcAddress(Lib, "wkeCreateWebView");
            CreateWebView = Marshal.GetDelegateForFunctionPointer<WkeCreateWebViewDelegate>(pfnCreateWebView);

            var pfnDestroyWebView = GetProcAddress(Lib, "wkeDestroyWebView");
            DestroyWebView = Marshal.GetDelegateForFunctionPointer<WkeDestroyWebViewDelegate>(pfnDestroyWebView);

            var pfnSetHandle = GetProcAddress(Lib, "wkeSetHandle");
            SetHandle = Marshal.GetDelegateForFunctionPointer<WkeSetHandleDelegate>(pfnSetHandle);

            var pfnOnPaintUpdated = GetProcAddress(Lib, "wkeOnPaintUpdated");
            OnPaintUpdated = Marshal.GetDelegateForFunctionPointer<WkeOnPaintUpdatedDelegate>(pfnOnPaintUpdated);

            var pfnResize = GetProcAddress(Lib, "wkeResize");
            Resize = Marshal.GetDelegateForFunctionPointer<WkeResizeDelegate>(pfnResize);

            var pfnGetViewDC = GetProcAddress(Lib, "wkeGetViewDC");
            GetViewDC = Marshal.GetDelegateForFunctionPointer<WkeGetViewDCDelegate>(pfnGetViewDC);

            var pfnLoadURLW = GetProcAddress(Lib, "wkeLoadURLW");
            LoadURLW = Marshal.GetDelegateForFunctionPointer<WkeLoadURLWDelegate>(pfnLoadURLW);

            var pfnLoadHTML = GetProcAddress(Lib, "wkeLoadHTML");
            LoadHTML = Marshal.GetDelegateForFunctionPointer<WkeLoadHTMLDelegate>(pfnLoadHTML);

            var pfnFireMouseEvent = GetProcAddress(Lib, "wkeFireMouseEvent");
            FireMouseEvent = Marshal.GetDelegateForFunctionPointer<WkeFireMouseEventDelegate>(pfnFireMouseEvent);

            var pfnFireMouseWheelEvent = GetProcAddress(Lib, "wkeFireMouseWheelEvent");
            FireMouseWheelEvent = Marshal.GetDelegateForFunctionPointer<WkeFireMouseWheelEventDelegate>(pfnFireMouseWheelEvent);

            var pfnFireKeyUpEvent = GetProcAddress(Lib, "wkeFireKeyUpEvent");
            FireKeyUpEvent = Marshal.GetDelegateForFunctionPointer<WkeFireKeyUpEventDelegate>(pfnFireKeyUpEvent);

            var pfnFireKeyDownEvent = GetProcAddress(Lib, "wkeFireKeyDownEvent");
            FireKeyDownEvent = Marshal.GetDelegateForFunctionPointer<WkeFireKeyDownEventDelegate>(pfnFireKeyDownEvent);

            var pfnFireKeyPressEvent = GetProcAddress(Lib, "wkeFireKeyPressEvent");
            FireKeyPressEvent = Marshal.GetDelegateForFunctionPointer<WkeFireKeyPressEventDelegate>(pfnFireKeyPressEvent);

            var pfnGetCursorInfoType = GetProcAddress(Lib, "wkeGetCursorInfoType");
            GetCursorInfoType = Marshal.GetDelegateForFunctionPointer<WkeGetCursorInfoTypeDelegate>(pfnGetCursorInfoType);

            var pfnSetContextMenuEnabled = GetProcAddress(Lib, "wkeSetContextMenuEnabled");
            SetContextMenuEnabled = Marshal.GetDelegateForFunctionPointer<WkeSetContextMenuEnabledDelegate>(pfnSetContextMenuEnabled);

            var pfnSetDragEnable = GetProcAddress(Lib, "wkeSetDragEnable");
            SetDragEnable = Marshal.GetDelegateForFunctionPointer<WkeSetDragEnableDelegate>(pfnSetDragEnable);

            var pfnSetDragDropEnable = GetProcAddress(Lib, "wkeSetDragDropEnable");
            SetDragDropEnable = Marshal.GetDelegateForFunctionPointer<WkeSetDragDropEnableDelegate>(pfnSetDragDropEnable);

            var pfnSetNavigationToNewWindowEnable = GetProcAddress(Lib, "wkeSetNavigationToNewWindowEnable");
            SetNavigationToNewWindowEnable = Marshal.GetDelegateForFunctionPointer<WkeSetNavigationToNewWindowEnableDelegate>(pfnSetNavigationToNewWindowEnable);

            var pfnSetCookieJarFullPath = GetProcAddress(Lib, "wkeSetCookieJarFullPath");
            SetCookieJarFullPath = Marshal.GetDelegateForFunctionPointer<WkeSetCookieJarFullPathDelegate>(pfnSetCookieJarFullPath);

            var pfnSetLocalStorageFullPath = GetProcAddress(Lib, "wkeSetLocalStorageFullPath");
            SetLocalStorageFullPath = Marshal.GetDelegateForFunctionPointer<WkeSetLocalStorageFullPathDelegate>(pfnSetLocalStorageFullPath);

            var pfnReload = GetProcAddress(Lib, "wkeReload");
            Reload = Marshal.GetDelegateForFunctionPointer<WkeReloadDelegate>(pfnReload);
        }
    }
}
