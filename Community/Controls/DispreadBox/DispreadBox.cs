﻿using Community.Controls.Base;
using Community.Helpers;
using Community.Themes.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Community.Controls
{
    [TemplatePart(Name = TextBlockBottomTemplateName, Type = typeof(TextBlock))]
    [TemplatePart(Name = TextBlockTopTemplateName, Type = typeof(TextBlock))]
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    [TemplatePart(Name = ClipRect1TemplateName, Type = typeof(RectangleGeometry))]
    [TemplatePart(Name = ClipRect2TemplateName, Type = typeof(RectangleGeometry))]
    //[TemplatePart(Name = GradientStop1TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop2TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop3TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop4TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop5TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop6TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop7TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop8TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop9TemplateName, Type = typeof(GradientStop))]
    //[TemplatePart(Name = GradientStop10TemplateName, Type = typeof(GradientStop))]
    public class DispreadBox : ControlBase
    {
        private const string TextBlockBottomTemplateName = "PART_TextBlockBottom";
        private const string TextBlockTopTemplateName = "PART_TextBlockTop";
        private const string CanvasTemplateName = "PART_Canvas";
        private const string ClipRect1TemplateName = "PART_ClipRect1";
        private const string ClipRect2TemplateName = "PART_ClipRect2";
        //private const string GradientStop1TemplateName = "PART_G1";
        //private const string GradientStop2TemplateName = "PART_G2";
        //private const string GradientStop3TemplateName = "PART_G3";
        //private const string GradientStop4TemplateName = "PART_G4";
        //private const string GradientStop5TemplateName = "PART_G5";
        //private const string GradientStop6TemplateName = "PART_G6";
        //private const string GradientStop7TemplateName = "PART_G7";
        //private const string GradientStop8TemplateName = "PART_G8";
        //private const string GradientStop9TemplateName = "PART_G9";
        //private const string GradientStop10TemplateName = "PART_G10";
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DispreadBox),
                new PropertyMetadata("HEY!"));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(DispreadBox),
                new PropertyMetadata(TimeSpan.FromSeconds(2)));

        public static readonly DependencyProperty GiftsProperty =
            DependencyProperty.Register("Gifts", typeof(ImageSource[]), typeof(DispreadBox),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextBackImageProperty =
            DependencyProperty.Register("TextBackImage", typeof(ImageSource), typeof(DispreadBox),
                new PropertyMetadata(new BitmapImage()));

        private FestvialType festvialType = (FestvialType)(-1);
        private TextBlock _textBlockBottom, _textBlockTop;
        private RectangleGeometry _clipRect1, _clipRect2;
        //private GradientStop _gradientStop1, 
        //    _gradientStop2, 
        //    _gradientStop3, 
        //    _gradientStop4, 
        //    _gradientStop5, 
        //    _gradientStop6, 
        //    _gradientStop7, 
        //    _gradientStop8,
        //    _gradientStop9,
        //    _gradientStop10;

        static DispreadBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DispreadBox), new FrameworkPropertyMetadata(typeof(DispreadBox)));
        }

        public TimeSpan Duration
        {
            get => (TimeSpan)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public int GiftSizeMax;
        public int GiftSizeMin;
        public Canvas _canvas;
        
        public ImageSource[] Gifts
        {
            get => (ImageSource[])GetValue(GiftsProperty);
            set => SetValue(GiftsProperty, value);
        }
        
        public ImageSource TextBackImage
        {
            get => (ImageSource)GetValue(TextBackImageProperty);
            set => SetValue(TextBackImageProperty, value);
        }

        public int SpreadTimeSpan;

        /// <summary>
        /// 整点判断计时器
        /// </summary>
        DispatcherTimer _autoSpreadTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        /// <summary>
        /// 抛出gift
        /// </summary>
        DispatcherTimer _spreadTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        /// <summary>
        /// 一段时间后关闭
        /// </summary>
        DispatcherTimer _closeBoxTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBlockBottom = GetTemplateChild(TextBlockBottomTemplateName) as TextBlock;
            _textBlockTop = GetTemplateChild(TextBlockTopTemplateName) as TextBlock;
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            _clipRect1 = GetTemplateChild(ClipRect1TemplateName) as RectangleGeometry;
            _clipRect2 = GetTemplateChild(ClipRect2TemplateName) as RectangleGeometry;
            
            var center = new Point(FontSize / 2, FontSize / 2);
            if (_textBlockBottom != null && _textBlockTop != null) //&& _rectangleGeometry != null)
            {
                _textBlockTop.Loaded += _textBlockTop_Loaded;
            }
            Hook();
            _closeBoxTimer.Tick += _closeBoxTimer_Tick;
            Loaded += _this_Loaded;
        }

        private  void _textBlockTop_Loaded(object s, RoutedEventArgs e)
        {
            _autoSpreadTimer.Interval = TimeSpan.FromMilliseconds(60 * 60 * 1000 - DateTime.Now.Minute * 60000 - DateTime.Now.Second * 1000 - DateTime.Now.Millisecond);
            _autoSpreadTimer.Start();
        }

        private void _closeBoxTimer_Tick(object sender, EventArgs e)
        {
            CloseTheBox();
            _closeBoxTimer.Stop();
        }

        private void _autoSpreadTimer_Tick(object sender, EventArgs e)
        {
            _autoSpreadTimer.Interval = TimeSpan.FromSeconds(60);
            OpenTheBox(true);
        }

        private void _spreadTimer_Tick(object sender, EventArgs e)
        {
            SpreadGifts();
        }

        private void _this_Loaded(object s, RoutedEventArgs e)
        {
            _spreadTimer.Stop();
            _spreadTimer.Tick += _spreadTimer_Tick;
            OpenTheBox(true);
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

        /// <summary>
        /// 钩子函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION && wParam == WM_MOUSEMOVE)
            {
                POINT point;
                Win32ApiHelper.GetCursorPos(out point);
                Point lt = _textBlockTop.PointToScreen(new Point(0, 0));
                Rect port = new Rect(lt.X, lt.Y, _textBlockTop.ActualWidth, _textBlockTop.ActualHeight);
                if (port.Contains(new Point(point.X, point.Y)) && IsClosing)
                {
                    OpenTheBox();
                }
                else if (!AutoOpen && !IsClosing)
                {
                    CloseTheBox();
                }
            }
            return Win32ApiHelper.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }


        Storyboard _openBoxStoryBoard;
        Storyboard _closeBoxStoryBoard;
        /// <summary>
        /// 是否自动打开
        /// </summary>
        bool AutoOpen = false;
        /// <summary>
        /// 窗口正在关闭
        /// </summary>
        bool IsClosing = true;


        /// <summary>
        /// 打开盒子
        /// </summary>
        public void OpenTheBox(bool Auto = false)
        {
            if (!IsClosing) return;
            IsClosing = false;
            AutoOpen = Auto;
            Begin();
            if (Auto)
            {
                _closeBoxTimer.Interval = Duration;
                _closeBoxTimer.Start();
            }
            else
            {
                _closeBoxTimer.Stop();
            }
        }

        /// <summary>
        /// 关闭盒子
        /// </summary>
        public void CloseTheBox()
        {
            if (IsClosing) return;
            IsClosing = true;
            End();
            _spreadTimer.Stop();
        }

        /// <summary>
        /// 开始动作
        /// </summary>
        public void Begin()
        {
            if (null == _openBoxStoryBoard)
            {
                var textDoubleAnimation = new DoubleAnimation
                {
                    To = 90,
                    Duration = TimeSpan.FromSeconds(3),
                    //EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(textDoubleAnimation, _textBlockTop);
                Storyboard.SetTargetProperty(textDoubleAnimation,
                    new PropertyPath("(TextBlock.Foreground).(Brush.RelativeTransform).(RotateTransform.Angle)"));

                _openBoxStoryBoard = new Storyboard();
                _openBoxStoryBoard.Children.Add(textDoubleAnimation);
            }
            _openBoxStoryBoard.Begin();

            for (int i = 1; i <= 10; i++)
            {
                var to = i <= 5 ? 0 : 1;
                var textColorAnimation = new DoubleAnimation { To = to, Duration = TimeSpan.FromSeconds(1) };
                (GetTemplateChild("PART_G" + i) as GradientStop).BeginAnimation(GradientStop.OffsetProperty, textColorAnimation);
            }
            _spreadTimer.Start();

            SpreadGifts();
        }

        /// <summary>
        /// 结束动作
        /// </summary>
        public void End()
        {
            if (null == _closeBoxStoryBoard)
            {
                var textDoubleAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(3),
                    //EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(textDoubleAnimation, _textBlockTop);
                Storyboard.SetTargetProperty(textDoubleAnimation,
                    new PropertyPath("(TextBlock.Foreground).(Brush.RelativeTransform).(RotateTransform.Angle)"));

                _closeBoxStoryBoard = new Storyboard();
                _closeBoxStoryBoard.Children.Add(textDoubleAnimation);
            }
            _closeBoxStoryBoard.Begin();

            for (int i = 1; i <= 10; i++)
            {
                double? to = 0.5;
                switch (i)
                {
                    case 1:
                        to = 0;
                        break;
                    case 2:
                    case 3:
                        to = 0.3333;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        to = 0.50;
                        break;
                    case 8:
                    case 9:
                        to = 0.6666;
                        break;
                    case 10:
                        to = 1.00;
                        break;
                }
                var gradientStopDoubleAnimation = new DoubleAnimation { To = to, Duration = TimeSpan.FromSeconds(3) };
                (GetTemplateChild("PART_G" + i) as GradientStop).BeginAnimation(GradientStop.OffsetProperty, gradientStopDoubleAnimation);
            }
        }

        /// <summary>
        /// 发撒礼物
        /// </summary>
        private void SpreadGifts()
        {
            if (festvialType != ControlsHelper.FestvialType)
            {
                festvialType = ControlsHelper.FestvialType;
                Gifts = (Application.Current.Resources["TheGifts"] as ImageCollection).ToArray();
            }
            if (Gifts == null || Gifts.Length == 0) return;

            int size = _random.Next(10, 30);
            int tosize = _random.Next(100, 150);
            Duration Duration = TimeSpan.FromSeconds(_random.Next(3, 6));

            Geometry geometry = new RectangleGeometry(new Rect(10, 10, 10, 10));
            
            Image image = new Image()
            {
                Source = Gifts[_random.Next(0, Gifts.Length)],
                Width = size,
                Height = size,
                Stretch = Stretch.Fill,
                RenderTransformOrigin = new Point(0.5, _random.Next(0, 2)/10.0),
                RenderTransform = new RotateTransform { Angle = 0 }
            };
            Canvas.SetTop(image, (_canvas.ActualHeight - image.Height) / 2);
            Canvas.SetLeft(image, (_canvas.ActualWidth - image.Width) / 2);

            double X = Random((int)(0 - 2 * tosize), (int)(_canvas.ActualWidth - tosize), (int)_canvas.ActualWidth,(int)(_canvas.ActualWidth + tosize));
            double Y = Random((int)(0 - 2 * tosize), (int)(_canvas.ActualHeight - tosize), (int)_canvas.ActualHeight, (int)(_canvas.ActualHeight + tosize));
            switch (_random.Next() % 2 == 0)
            {
                case true:
                    Y = _random.Next((int)(0 - tosize), (int)(_canvas.ActualHeight + tosize));
                    break;
                case false:
                    X = _random.Next((int)(0 - tosize), (int)(_canvas.ActualWidth + tosize));
                    break;
            }

            var yAnimation = new DoubleAnimation
            {
                To = Y,
                Duration = Duration
            };
            Storyboard.SetTarget(yAnimation, image);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath(Canvas.TopProperty));

            var xAnimation = new DoubleAnimation
            {
                To = X,
                Duration = Duration
            };
            Storyboard.SetTarget(xAnimation, image);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath(Canvas.LeftProperty));

            var widthAnimation = new DoubleAnimation
            {
                To = tosize,
                Duration = Duration
            };
            Storyboard.SetTarget(widthAnimation, image);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));

            var heightAnimation = new DoubleAnimation
            {
                To = tosize,
                Duration = Duration
            };
            Storyboard.SetTarget(heightAnimation, image);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

            var angleAnimation = new DoubleAnimation
            {
                To = _random.Next(-360 * 3, 360 * 3),
                Duration = Duration
            };
            Storyboard.SetTarget(angleAnimation, image);
            Storyboard.SetTargetProperty(angleAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            var story = new Storyboard();
            story.Completed += (sender, e) => { _canvas.Children.Remove(image); image.ClearEvents(); };
            story.Children.Add(angleAnimation);
            story.Children.Add(xAnimation);
            story.Children.Add(yAnimation);
            story.Children.Add(widthAnimation);
            story.Children.Add(heightAnimation);
            image.Loaded += (sender, args) => story.Begin(this);
            
            _canvas.Children.Add(image);
        }

        public double Random(int a , int b, int c, int d)
        {
            if (b < a || d < c) return 0;
            double x = _random.Next(a, b);
            double y = _random.Next(c, d);
            return _random.Next() % 2 == 0 ? x : y;
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
                Gifts = null;
                TextBackImage = null;
                this._openBoxStoryBoard.Remove();
                this._closeBoxStoryBoard.Remove();
                this.ClearEvents();
                _spreadTimer.Tick -= _spreadTimer_Tick;
                _autoSpreadTimer.Tick -= _autoSpreadTimer_Tick;
                _closeBoxTimer.Tick -= _closeBoxTimer_Tick;
                _textBlockTop.Loaded -= _textBlockTop_Loaded;
                _spreadTimer = null;
                _autoSpreadTimer = null;
                _closeBoxTimer = null;
                Loaded -= _this_Loaded;
            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
