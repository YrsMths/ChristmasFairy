using Community.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Community.Controls
{
    [TemplatePart(Name = TextBlockTemplateName, Type = typeof(TextBlock))]
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    [TemplatePart(Name = GlowCanvasTemplateName, Type = typeof(Canvas))]
    public class BrokenNeonText : Control
    {
        private const string TextBlockTemplateName = "PART_TextBlock";
        private const string CanvasTemplateName = "PART_Canvas";
        private const string GlowCanvasTemplateName = "PART_GlowCanvas";

        static BrokenNeonText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrokenNeonText), new FrameworkPropertyMetadata(typeof(BrokenNeonText)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BrokenNeonText), new PropertyMetadata(""));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BrokenNeonText), new PropertyMetadata(4.0));

        private double _height;
        private double _width;
        private static Canvas _canvas;
        private static Canvas _glowCanvas;
        private static TextBlock _textBlock;
        private static Dictionary<Path, bool> _paths = new Dictionary<Path, bool>();
        private static Dictionary<Path, Path> _glowPaths = new Dictionary<Path, Path>();
        private static RandomHSBColorCreator randomHSBColorCreator = new RandomHSBColorCreator(66, 67, 90, 100);
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            _glowCanvas = GetTemplateChild(GlowCanvasTemplateName) as Canvas;
            _textBlock = GetTemplateChild(TextBlockTemplateName) as TextBlock;
            Loaded += (s, e) =>
            {
                Init();
                Hook();
            };

            Unloaded += (s, e) =>
            {
                Unhook();
            };
        }
        
        public void Init()
        {
            _textBlock.Inlines.Clear();
            _canvas.Children.Clear();
            _glowCanvas.Children.Clear();
            _paths = new Dictionary<Path, bool>();
            _glowPaths = new Dictionary<Path, Path>();
            for (int i = 0; i < Text.Length; i ++)
            {
                if (Text[i] == 13) continue;
                byte[] bts = Encoding.Unicode.GetBytes(Text[i].ToString());
                if (bts[0].ToString() == "253" && bts[1].ToString() == "255")
                {
                    _textBlock.Inlines.Add(new Run(Text.Substring(i, 2).ToString()));
                    i++;
                }
                else
                {
                    _textBlock.Inlines.Add(new Run(Text[i].ToString()));
                }
            }
            _textBlock.UpdateLayout();
            Point origin = _canvas.PointToScreen(new Point(0, 0));
            Point point = _textBlock.PointToScreen(new Point(0, 0));
            foreach (Inline inline in _textBlock.Inlines)
            {
                if (((System.Windows.Documents.Run)inline).Text.Trim() == "") continue;
                double posLeft = inline.ElementStart.GetCharacterRect(LogicalDirection.Forward).Left;
                double posTop = inline.ElementStart.GetCharacterRect(LogicalDirection.Forward).Top;
                var color = randomHSBColorCreator.Next;
                Path path = new Path()
                {
                    Data = GetGeometry(((System.Windows.Documents.Run)inline).Text.ToString(), new Point()),
                    Stroke = new SolidColorBrush(color),
                    StrokeDashArray = new DoubleCollection() { 50, 2 },
                    StrokeThickness = StrokeThickness,
                    Effect = new BlurEffect() { Radius = 5 },
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeDashCap = PenLineCap.Round
                };
                Path glowPath = new Path()
                {
                    Data = GetGeometry(((System.Windows.Documents.Run)inline).Text.ToString(), new Point()),
                    Stroke = new SolidColorBrush(color) { Opacity = 0.7},
                    StrokeDashArray = new DoubleCollection() { 50, 2 },
                    StrokeThickness = StrokeThickness,
                    Effect = new BlurEffect() { Radius = FontSize },
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    Opacity = 0.7
                };
                Canvas.SetLeft(path, posLeft + point.X - origin.X);
                Canvas.SetTop(path, posTop + point.Y - origin.Y);
                Canvas.SetLeft(glowPath, posLeft + point.X - origin.X);
                Canvas.SetTop(glowPath, posTop + point.Y - origin.Y);
                _canvas.Children.Add(path);
                _glowCanvas.Children.Add(glowPath);
                _paths[path] = false;
                _glowPaths[path] = glowPath;

                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(_random.Next(5, 10)) };
                timer.Tick += delegate { Flash(); timer.Interval = TimeSpan.FromSeconds(_random.Next(5, 10)); };
                timer.Start();
            }
        }

        private static void Flash()
        {
            var plist = _paths.Where(x => x.Value == false).ToList();
            if (plist == null || plist.Count == 0) return;
            Path path = plist[_random.Next(0, plist.Count)].Key;
            int flashTimes = _random.Next(3, 7);
            var flashAnimation = new DoubleAnimation
            {
                To = 1,
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(flashTimes),
                Duration = TimeSpan.FromMilliseconds(20 * flashTimes),
                EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn }
            };
            var glowFlashAnimation = new DoubleAnimation
            {
                To = 0.7,
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(flashTimes),
                Duration = TimeSpan.FromMilliseconds(20 * flashTimes),
                EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn }
            };
            _glowPaths[path].Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, glowFlashAnimation);
            path.Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, flashAnimation);
            
        }
        
        private Geometry GetGeometry(string text, Point point)
        {
            var formattedText = new FormattedText(
                                       text,
                                       CultureInfo.CurrentCulture,
                                       FlowDirection.LeftToRight,
                                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Transparent);
            _height = formattedText.Height;
            _width = formattedText.Width;
            return formattedText.BuildGeometry(point);
        }

        private void MouseOver(POINT point)
        {
            foreach (var child in _canvas.Children)
            {
                var path = child as Path;
                if (path == null) continue;
                Point lt = path.PointToScreen(new Point(0, 0));
                Rect port = new Rect(lt.X, lt.Y, path.ActualWidth, path.ActualHeight);
                if (port.Contains(new Point(point.X, point.Y)))
                {
                    if (_paths[path]) continue; _paths[path] = true;
                    path.Stroke = _glowPaths[path].Stroke = new SolidColorBrush(randomHSBColorCreator.Next);
                    int flashTimes = _random.Next(2, 5);
                    double darkenSecond = _random.Next(20, 50) / 10.0;
                    var flashAnimation = new DoubleAnimation
                    {
                        To = 0.1,
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(flashTimes),
                        Duration = TimeSpan.FromMilliseconds(20 * flashTimes),
                        EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn }
                    };
                    var darkenAnimation = new DoubleAnimation
                    {
                        To = 0.1,
                        AutoReverse = false,
                        BeginTime = TimeSpan.FromSeconds(darkenSecond),
                        Duration = TimeSpan.FromMilliseconds(20)
                    };

                    var glowFlashAnimation = new DoubleAnimation
                    {
                        To = 0,
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(flashTimes),
                        Duration = TimeSpan.FromMilliseconds(20 * flashTimes),
                        EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn }
                    };
                    var glowDarkenAnimation = new DoubleAnimation
                    {
                        To = 0,
                        AutoReverse = false,
                        BeginTime = TimeSpan.FromSeconds(darkenSecond),
                        Duration = TimeSpan.FromMilliseconds(20)
                    };

                    flashAnimation.Completed += (sender, e) =>
                    {
                        _glowPaths[path].Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, glowDarkenAnimation);
                        path.Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, darkenAnimation);
                    };
                    darkenAnimation.Completed += (sender, e) =>
                    {
                        _paths[path] = false;
                        _glowPaths[path].Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, null);
                        path.Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, null);
                        _glowPaths[path].Stroke.Opacity = 0;
                        path.Stroke.Opacity = 0.1;
                    };

                    path.Stroke.Opacity = 1;
                    _glowPaths[path].Stroke.Opacity = 0.7;

                    _glowPaths[path].Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, glowFlashAnimation);
                    path.Stroke.BeginAnimation(SolidColorBrush.OpacityProperty, flashAnimation);
                }
            }
        }
        
        ////////////////////////钩子////////////////////////
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

        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION && wParam == WM_MOUSEMOVE)
            {
                POINT point;
                Win32ApiHelper.GetCursorPos(out point);
                MouseOver(point);
            }
            return Win32ApiHelper.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
    }
}
