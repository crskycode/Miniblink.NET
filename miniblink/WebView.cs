using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

//
// https://github.com/weolar/miniblink49
//

namespace Miniblink
{
    public partial class WebView : UserControl
    {
        //
        // Fields
        //

        private IntPtr MyView;
        private MiniblinkApi.WkePaintUpdatedCallbackDelegate OnWkePaintUpdatedCallback;
        private MiniblinkApi.CursorType CursorType;

        //
        // Constructor
        //

        public WebView()
        {
            InitializeComponent();

            if (IsDesignMode)
            {
                // We do nothing in design mode.
                return;
            }

            CreateView();
        }

        //
        // Properties
        //

        private bool IsDesignMode
        {
            get => DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        private bool IsViewCreated
        {
            get => MyView != IntPtr.Zero;
        }

        //
        // Startup
        //

        private void CreateView()
        {
            MyView = MiniblinkApi.CreateWebView();

            if (MyView == IntPtr.Zero)
                return;

            CursorType = MiniblinkApi.CursorType.None;

            // Configure temporary data path here.
            var cookiesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miniblink", "cookies.dat");
            var localStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miniblink", "LocalStorage");

            // Must hold delegate reference.
            OnWkePaintUpdatedCallback = WkeOnPaintUpdated;

            // Bind the view to hwnd because we need a device context that compatible with the window.
            MiniblinkApi.SetHandle(MyView, Handle);

            MiniblinkApi.SetContextMenuEnabled(MyView, false);
            MiniblinkApi.SetDragEnable(MyView, false);
            MiniblinkApi.SetDragDropEnable(MyView, false);
            MiniblinkApi.SetNavigationToNewWindowEnable(MyView, false);
            MiniblinkApi.SetCookieJarFullPath(MyView, cookiesPath);
            MiniblinkApi.SetLocalStorageFullPath(MyView, localStoragePath);

            MiniblinkApi.OnPaintUpdated(MyView, OnWkePaintUpdatedCallback, IntPtr.Zero);

            Debug.WriteLine($"{DateTime.Now} WebView {MyView} created.");
        }

        private void DestroyView()
        {
            if (IsViewCreated)
            {
                MiniblinkApi.OnPaintUpdated(MyView, null, IntPtr.Zero);
                MiniblinkApi.DestroyWebView(MyView);
                Debug.WriteLine($"{DateTime.Now} WebView {MyView} destroyed.");
                MyView = IntPtr.Zero;
            }
        }

        //
        // Shutdown
        //

        protected override void DestroyHandle()
        {
            DestroyView();
            base.DestroyHandle();
        }

        //
        // Handle WebView event
        //

        private void WkeOnPaintUpdated(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            var rc = new Rectangle(x, y, cx, cy);
            Invalidate(rc);
        }

        //
        // Handle window event
        //

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (IsViewCreated)
            {
                MiniblinkApi.Resize(MyView, Width, Height);
            }
        }

        //
        // Handle mouse event
        //

        private static MiniblinkApi.MouseFlags GetWkeMouseEventFlags(MouseEventArgs e)
        {
            var flags = MiniblinkApi.MouseFlags.NONE;

            if (e.Button.HasFlag(MouseButtons.Left))
                flags |= MiniblinkApi.MouseFlags.LBUTTON;
            if (e.Button.HasFlag(MouseButtons.Right))
                flags |= MiniblinkApi.MouseFlags.RBUTTON;
            if (e.Button.HasFlag(MouseButtons.Middle))
                flags |= MiniblinkApi.MouseFlags.MBUTTON;

            if (ModifierKeys.HasFlag(Keys.Control))
                flags |= MiniblinkApi.MouseFlags.CONTROL;
            if (ModifierKeys.HasFlag(Keys.Shift))
                flags |= MiniblinkApi.MouseFlags.SHIFT;

            return flags;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsViewCreated)
            {
                var flags = GetWkeMouseEventFlags(e);
                MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.MOUSEMOVE, e.X, e.Y, flags);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (IsViewCreated)
            {
                var flags = GetWkeMouseEventFlags(e);

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.LBUTTONDOWN, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Right:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.RBUTTONDOWN, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Middle:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.MBUTTONDOWN, e.X, e.Y, flags);
                        break;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsViewCreated)
            {
                var flags = GetWkeMouseEventFlags(e);

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.LBUTTONUP, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Right:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.RBUTTONUP, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Middle:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.MBUTTONUP, e.X, e.Y, flags);
                        break;
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (IsViewCreated)
            {
                var flags = GetWkeMouseEventFlags(e);

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.LBUTTONDBLCLK, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Right:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.RBUTTONDBLCLK, e.X, e.Y, flags);
                        break;
                    case MouseButtons.Middle:
                        MiniblinkApi.FireMouseEvent(MyView, MiniblinkApi.MouseMsg.MBUTTONDBLCLK, e.X, e.Y, flags);
                        break;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (IsViewCreated)
            {
                var flags = GetWkeMouseEventFlags(e);
                MiniblinkApi.FireMouseWheelEvent(MyView, e.X, e.Y, e.Delta, flags);
            }
        }

        //
        // Handle keyboard event
        //

        private static bool IsExtendedKey(Keys key)
        {
            switch (key)
            {
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Home:
                case Keys.End:
                case Keys.Prior:
                case Keys.Next:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
                default:
                    return false;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsViewCreated)
            {
                var flags = MiniblinkApi.KeyFlags.REPEAT;

                if (IsExtendedKey(e.KeyCode))
                {
                    flags |= MiniblinkApi.KeyFlags.EXTENDED;
                }

                if (MiniblinkApi.FireKeyDownEvent(MyView, e.KeyValue, flags, false))
                {
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (IsViewCreated)
            {
                var flags = MiniblinkApi.KeyFlags.REPEAT;

                if (IsExtendedKey(e.KeyCode))
                {
                    flags |= MiniblinkApi.KeyFlags.EXTENDED;
                }

                if (MiniblinkApi.FireKeyUpEvent(MyView, e.KeyValue, flags, false))
                {
                    e.Handled = true;
                }
            }

            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (IsViewCreated)
            {
                if (MiniblinkApi.FireKeyPressEvent(MyView, e.KeyChar, MiniblinkApi.KeyFlags.REPEAT, false))
                {
                    e.Handled = true;
                }
            }

            base.OnKeyPress(e);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        //
        // Handle other event
        //

        protected override void WndProc(ref Message msg)
        {
            if (IsDesignMode || !IsViewCreated)
            {
                base.WndProc(ref msg);
                return;
            }

            switch (msg.Msg)
            {
                case Win32Api.WM_PAINT:
                {
                    var dc = MiniblinkApi.GetViewDC(MyView);
                    var ps = new PAINTSTRUCT();

                    Win32Api.BeginPaint(msg.HWnd, ref ps);

                    int x = ps.rcPaint.Left;
                    int y = ps.rcPaint.Top;
                    int w = ps.rcPaint.Right - ps.rcPaint.Left;
                    int h = ps.rcPaint.Bottom - ps.rcPaint.Top;

                    Win32Api.BitBlt(ps.hdc, x, y, w, h, dc, x, y, Win32Api.SRCCOPY);

                    Win32Api.EndPaint(msg.HWnd, ref ps);

                    return;
                }
                case Win32Api.WM_SETCURSOR:
                {
                    var cursor = MiniblinkApi.GetCursorInfoType(MyView);

                    if (cursor != CursorType)
                    {
                        switch (cursor)
                        {
                            case MiniblinkApi.CursorType.Pointer:
                                if (Cursor != Cursors.Arrow)
                                    Cursor = Cursors.Arrow;
                                break;
                            case MiniblinkApi.CursorType.Hand:
                                if (Cursor != Cursors.Hand)
                                    Cursor = Cursors.Hand;
                                break;
                            case MiniblinkApi.CursorType.IBeam:
                                if (Cursor != Cursors.IBeam)
                                    Cursor = Cursors.IBeam;
                                break;
                            default:
                                if (Cursor != Cursors.Arrow)
                                    Cursor = Cursors.Arrow;
                                break;
                        }

                        CursorType = cursor;
                    }

                    break;
                }
            }

            base.WndProc(ref msg);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            // if (IsDesignMode)
            // {
            //     if (Parent != null)
            //     {
            //         if (!IsViewCreated)
            //         {
            //             CreateView();
            //         }
            //     }
            //     else
            //     {
            //         DestroyView();
            //     }
            // }

            base.OnParentChanged(e);
        }

        //
        // Methods
        //

        public void LoadUrl(string url)
        {
            if (IsViewCreated)
            {
                MiniblinkApi.LoadURLW(MyView, url);
            }
        }

        public void LoadHtml(string html)
        {
            if (IsViewCreated)
            {
                var data = Encoding.UTF8.GetBytes(html);

                // Make string null-terminated
                var buffer = new byte[data.Length + 1];
                // Copy string
                Array.Copy(data, buffer, data.Length);

                MiniblinkApi.LoadHTML(MyView, buffer);
            }
        }

        public void Reload()
        {
            if (IsViewCreated)
            {
                MiniblinkApi.Reload(MyView);
            }
        }
    }
}
