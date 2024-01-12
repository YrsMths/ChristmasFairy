using Community.Controls.Base;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.CrystalBall"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.CrystalBall;assembly=Community.Controls.CrystalBall"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CrystalBall/>
    ///
    /// </summary>
    public class CrystalBall : ControlBase
    {
        private const string GridTemplateName = "PART_Grid";
        private const string ForegroundGridTemplateName = "PART_ForegroundGrid";
        private const string MidgroundGridTemplateName = "PART_MidgroundGrid";
        private const string BackgroundGridTemplateName = "PART_BackgroundGrid";
        private const string CharacterGridTemplateName = "PART_CharacterGrid";

        private Grid _grid;
        private Grid _foregroundGrid;
        private Grid _midgroundGrid;
        private Grid _backgroundGrid;
        private Grid _characterGrid;

        public int ImgTranslate = 30;//图片位移量

        static CrystalBall()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrystalBall), new FrameworkPropertyMetadata(typeof(CrystalBall)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _grid = GetTemplateChild(GridTemplateName) as Grid;
            _foregroundGrid = GetTemplateChild(ForegroundGridTemplateName) as Grid;
            _midgroundGrid = GetTemplateChild(MidgroundGridTemplateName) as Grid;
            _backgroundGrid = GetTemplateChild(BackgroundGridTemplateName) as Grid;
            _characterGrid = GetTemplateChild(CharacterGridTemplateName) as Grid;
            if (_foregroundGrid == null || _midgroundGrid == null || _backgroundGrid == null || _characterGrid == null) return;
            Hook();
            Loaded += _this_Loaded;
            Unloaded += _this_UnLoaded;
            InitShowImage();
        }

        /// <summary>
        /// 初始化加载Image控件
        /// </summary>
        public void InitShowImage()
        {
            _foregroundGrid.Children.Clear();
            _midgroundGrid.Children.Clear();
            _backgroundGrid.Children.Clear();
            _characterGrid.Children.Clear();
            List<KeyValuePair<string, string[]>> strings = new List<KeyValuePair<string, string[]>>();
            foreach (var key in Application.Current.Resources.MergedDictionaries[0].Keys)
            {
                if (key.ToString().Contains("Crystal"))
                {
                    string[] infos = key.ToString().Split('_');
                    if (infos.Length == 5)
                    {
                        strings.Add(new KeyValuePair<string, string[]>(key.ToString(), infos));
                    }
                }
            }
            foreach (var info in strings.OrderBy(x => Convert.ToInt32(x.Value[4])))
            {
                VerticalAlignment vertical = VerticalAlignment.Center;
                Enum.TryParse(info.Value[2], out vertical);
                Image img = new Image
                {
                    Source = Application.Current.Resources[info.Key] as ImageSource,
                    Stretch = Stretch.Uniform,
                    Opacity = 1,
                    VerticalAlignment = vertical
                };
                var trans = new TranslateTransform();
                img.RenderTransform = trans;

                switch (info.Value[1])
                {
                    case "Foreground":
                        for (int  i = 0; i < 2; i ++)
                        {
                            Image foreimg = new Image
                            {
                                Source = Application.Current.Resources[info.Key] as ImageSource,
                                Stretch = Stretch.Uniform,
                                Opacity = i > 0 ? 0.2 : 1,
                                VerticalAlignment = vertical,
                                Effect = new BlurEffect()
                                {
                                    RenderingBias = RenderingBias.Performance,
                                    Radius = i * ImgTranslate
                                }
                            };
                            var foretrans = new TranslateTransform();
                            foreimg.RenderTransform = foretrans;
                            _foregroundGrid.Children.Add(foreimg);
                        }
                        break;
                    case "Character":
                        _characterGrid.Children.Add(img);
                        break;
                    case "Midground":
                        _midgroundGrid.Children.Add(img);
                        break;
                    case "Background":
                        _backgroundGrid.Children.Add(img);
                        break;
                }
            }
        }

        private void MainGrid_MouseMove(POINT point)
        {
            var moveX = point.X / _grid.ActualWidth - 0.5;
            var moveY = point.Y / _grid.ActualHeight - 0.5;

            List<Image> imgs = GetChildObjects<Image>(_foregroundGrid);
            for (int i = 1; i <= imgs.Count; i++)
            {
                TranslateTransform trans = imgs[i - 1].RenderTransform as TranslateTransform;
                trans.X = -moveX * ImgTranslate * Power(i, 0.5);
                trans.Y = -moveY * ImgTranslate * Power(i, 0.3);
            }
            List<Image> midimgs = GetChildObjects<Image>(_midgroundGrid);
            for (int i = 1; i <= midimgs.Count; i ++)
            {
                TranslateTransform trans = midimgs[i - 1].RenderTransform as TranslateTransform;
                trans.X = moveX * ImgTranslate * Power(i, 0.3);
                trans.Y = moveY * ImgTranslate * Power(i, 0.2);
            }
            List<Image> backimgs = GetChildObjects<Image>(_backgroundGrid);
            for (int i = 1; i <= backimgs.Count; i++)
            {
                TranslateTransform trans = backimgs[i - 1].RenderTransform as TranslateTransform;
                trans.X = moveX * ImgTranslate * Power(midimgs.Count + i, 0.5);
                trans.Y = moveY * ImgTranslate * Power(midimgs.Count + i, 0.3);
            }
            List<Image> chaimgs = GetChildObjects<Image>(_characterGrid);
            for (int i = 1; i <= chaimgs.Count; i++)
            {
                TranslateTransform trans = chaimgs[i - 1].RenderTransform as TranslateTransform;
                trans.X = moveX * ImgTranslate * Power(i - 1, 0);
                trans.Y = moveY * ImgTranslate * Power(i - 1, 0);
            }
        }

        private double Power(int i, double weight)
        {
            return (i + 1) * Math.Pow(i, weight);
        }

        /// <summary>
        /// 获得所有子控件
        /// </summary>
        private List<T> GetChildObjects<T>(System.Windows.DependencyObject obj) where T : System.Windows.FrameworkElement
        {
            System.Windows.DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }


        private void _this_Loaded(object s, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(Window.GetWindow(this)).Handle;
            Win32ApiHelper.ClientToScreen(hwnd, ref relativePoint);
        }

        private void _this_UnLoaded(object s, RoutedEventArgs e)
        {
            Unhook();
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
                MainGrid_MouseMove(point);
            }
            return Win32ApiHelper.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }


        /// <summary>
        /// 重写的Dispose方法
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
                Unhook();
                Loaded -= _this_Loaded;
                Unloaded -= _this_UnLoaded;
            }
            //告诉自己已经被释放
            disposed = true;
        }


    }
}
