using Community.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    [TemplatePart(Name = TopImageTemplateName, Type = typeof(Image))]
    [TemplatePart(Name = BottomImageTemplateName, Type = typeof(Image))]
    [TemplatePart(Name = EllipseGeometryTemplateName, Type = typeof(EllipseGeometry))]
    public class WheresWally : Control
    {
        private const string TopImageTemplateName = "PART_TopImage";
        private const string BottomImageTemplateName = "PART_BottomImage";
        private const string EllipseGeometryTemplateName = "PART_EllipseGeometry";
        private const string EllipseGeometry2TemplateName = "PART_EllipseGeometry2";

        public static readonly DependencyProperty BottomImageProperty =
            DependencyProperty.Register("BottomImage", typeof(ImageSource), typeof(WheresWally), new PropertyMetadata(null));

        public static readonly DependencyProperty TopImageProperty =
            DependencyProperty.Register("TopImage", typeof(ImageSource), typeof(WheresWally), new PropertyMetadata(null));
        
        static WheresWally()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WheresWally), new FrameworkPropertyMetadata(typeof(WheresWally)));
        }

        public ImageSource TopImage
        {
            get => (ImageSource)GetValue(TopImageProperty);
            set => SetValue(TopImageProperty, value);
        }

        public ImageSource BottomImage
        {
            get => (ImageSource)GetValue(BottomImageProperty);
            set => SetValue(BottomImageProperty, value);
        }

        Canvas _canvas;
        Image _topImage;
        Image _bottomImage;
        EllipseGeometry _ellipseGeometry;
        EllipseGeometry _ellipseGeometry2;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _topImage = GetTemplateChild(TopImageTemplateName) as Image;
            _bottomImage = GetTemplateChild(BottomImageTemplateName) as Image;
            _ellipseGeometry = GetTemplateChild(EllipseGeometryTemplateName) as EllipseGeometry;
            _ellipseGeometry2 = GetTemplateChild(EllipseGeometry2TemplateName) as EllipseGeometry;
            if (_topImage == null || _bottomImage == null || _ellipseGeometry == null) return;
            Hook();
            
            Loaded += (s, e) =>
            {
                IntPtr hwnd = new WindowInteropHelper(Window.GetWindow(this)).Handle;
                Win32ApiHelper.ClientToScreen(hwnd, ref relativePoint);
            };

            Unloaded += (s, e) =>
            {
                Unhook();
            };
        }

        private const int WM_MOUSEMOVE = 0x0200;
        private const int HC_ACTION = 0;
        private int hMouseHook = 0;
        private const int WH_MOUSE_LL = 14;
        private static Win32ApiHelper.HookProc MouseHookProcedure;

        private void Hook()
        {
            MouseHookProcedure = new Win32ApiHelper.HookProc(MouseHookProc);
            hMouseHook = Win32ApiHelper.SetWindowsHookEx(
                    WH_MOUSE_LL,
                    MouseHookProcedure,
                    Win32ApiHelper.GetModuleHandle(null),
                    0);
        }

        private void Unhook()
        {
            if (hMouseHook != 0)
            {
                bool retMouse = Win32ApiHelper.UnhookWindowsHookEx(hMouseHook);
            }
        }

        POINT relativePoint = new POINT(0, 0);

        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION && wParam == WM_MOUSEMOVE)
            {
                POINT point;
                Win32ApiHelper.GetCursorPos(out point);
                _ellipseGeometry2.Center = 
                    _ellipseGeometry.Center = new Point(point.X - relativePoint.X, point.Y - relativePoint.Y);
            }
            return Win32ApiHelper.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
    }
}
