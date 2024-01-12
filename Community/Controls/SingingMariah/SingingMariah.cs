using Community.Controls.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Community.Controls
{
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    public class SingingMariah : ControlBase
    {
        private const string CanvasTemplateName = "PART_Canvas";

        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        public static readonly DependencyProperty SourcesProperty =
            DependencyProperty.Register("Sources", typeof(ImageSource[]), typeof(SingingMariah), new PropertyMetadata(null));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(SingingMariah), new PropertyMetadata(null));


        public static readonly DependencyProperty SerialSourcesProperty =
            DependencyProperty.Register("SerialSources", typeof(List<ImageSource[]>), typeof(SingingMariah), new PropertyMetadata(null));

        private Canvas _canvas;

        static SingingMariah()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingingMariah), new FrameworkPropertyMetadata(typeof(SingingMariah)));
        }

        public ImageSource[] Sources
        {
            get => (ImageSource[])GetValue(SourcesProperty);
            set => SetValue(SourcesProperty, value);
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public List<ImageSource[]> SerialSources
        {
            get => (List<ImageSource[]>)GetValue(SerialSourcesProperty);
            set => SetValue(SerialSourcesProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            if (_canvas == null) return;
            Loaded += _this_Loaded;
        }

        private void _this_Loaded(object s, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += delegate { AllIWantForChrismasIsYou(); timer.Interval = TimeSpan.FromSeconds(_random.Next(10, 20)); };
            timer.Start();
        }
        
        private void AllIWantForChrismasIsYou()
        {
            Image image = new Image();
            int x, y;
            switch (0 == _random.Next() % 5)
            {
                case false:
                    if (null == Sources || Sources.Length == 0) return;

                    var RenderTransform = new TransformGroup();
                    RenderTransform.Children.Add(new RotateTransform { Angle = 0 });
                    RenderTransform.Children.Add(new ScaleTransform { ScaleX = 1 });
                    image = new Image()
                    {
                        Source = Sources[_random.Next(0, Sources.Length)],
                        Width = 200,
                        Height = 200,
                        Stretch = Stretch.UniformToFill,
                        RenderTransform = RenderTransform
                    };
                    x = _random.Next(400, (int)_canvas.ActualWidth - 200);
                    y = (int)_canvas.ActualHeight;

                    var showAnimation = new DoubleAnimation
                    {
                        To = y - 180,
                        Duration = TimeSpan.FromMilliseconds(_random.Next(400, 800)),
                        EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    Storyboard.SetTarget(showAnimation, image);
                    Storyboard.SetTargetProperty(showAnimation, new PropertyPath(Canvas.TopProperty));

                    var exitAnimation = new DoubleAnimation
                    {
                        To = y,
                        Duration = TimeSpan.FromMilliseconds(_random.Next(400, 800)),
                    };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    Storyboard.SetTarget(exitAnimation, image);
                    Storyboard.SetTargetProperty(exitAnimation, new PropertyPath(Canvas.TopProperty));

                    var showStory = new Storyboard();
                    var exitStory = new Storyboard();

                    showStory.Completed += (sender, e) =>
                    {
                        Storyboard s = Uhhhh(image);
                        s.Completed += (s1, e1) => exitStory.Begin();
                        s.Begin();
                    };
                    exitStory.Completed += (sender, e) =>
                    {
                        _canvas.Children.Remove(image);
                    };

                    showStory.Children.Add(showAnimation);
                    exitStory.Children.Add(exitAnimation);

                    _canvas.Children.Add(image);
                    image.Loaded += (sender, args) => showStory.Begin();
                    break;
                case true:
                    if (null == SerialSources || SerialSources.Count == 0) return;
                    ImageSource[] Serial = SerialSources[_random.Next(0, SerialSources.Count)];
                    bool moveflag = _random.Next() % 2 == 0;
                    bool outflag = _random.Next() % 2 == 0;
                    x = _random.Next(400, moveflag ? (int)_canvas.ActualWidth - 200 : (int)_canvas.ActualWidth - 200 - 30 * Serial.Length);
                    y = (int)_canvas.ActualHeight;
                    Duration duration = TimeSpan.FromMilliseconds(_random.Next(400, 600));
                    for (int i = 0; i < Serial.Length; i++)
                    {
                        image = new Image()
                        {
                            Source = Serial[i],
                            Width = 200,
                            Stretch = Stretch.UniformToFill
                        };
                        Canvas.SetLeft(image, x);
                        Canvas.SetTop(image, y);
                        _canvas.Children.Add(image);
                        image.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                        var serialAnimation = new DoubleAnimation
                        {
                            To = y - image.DesiredSize.Height,
                            Duration = duration,
                            EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut }
                        };
                        Storyboard.SetTarget(serialAnimation, image);
                        Storyboard.SetTargetProperty(serialAnimation, new PropertyPath(Canvas.TopProperty));
                        
                        var outAnimation = new DoubleAnimation()
                        {
                            To = outflag ? 0 : y,
                            Duration = duration
                        };
                        Storyboard.SetTarget(outAnimation, image);
                        Storyboard.SetTargetProperty(outAnimation, new PropertyPath(outflag ? OpacityProperty : Canvas.TopProperty));

                        var serialStory = new Storyboard() { BeginTime = TimeSpan.FromMilliseconds(i * 400) };
                        var outStory = new Storyboard() { BeginTime = TimeSpan.FromMilliseconds(1000) };
                        

                        serialStory.Children.Add(serialAnimation);
                        outStory.Children.Add(outAnimation);
                        outStory.Completed += (s, e) => _canvas.Children.Remove(image);
                        serialStory.Completed += (s, e) => outStory.Begin();
                        serialStory.Begin();
                        x += moveflag ? -30 : 30;
                    }
                    break;
            }
        }

        private Storyboard Uhhhh(Image image)
        {
            var story = new Storyboard();
            switch (_random.Next() % 3)
            {
                case 0:
                    image.RenderTransformOrigin = new Point(0.5, _random.Next(10, 20) / 10.0f);
                    var angle = _random.Next(10, 20);
                    var angleAnimation = new DoubleAnimation
                    {
                        From = angle,
                        To = -angle,
                        Duration = TimeSpan.FromMilliseconds(_random.Next(200, 400)),
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(_random.Next(3, 6))
                    };
                    Storyboard.SetTarget(angleAnimation, image);
                    Storyboard.SetTargetProperty(angleAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                    story.Children.Add(angleAnimation);
                    break;
                case 1:
                    var y = _random.Next(10, 20);
                    var yAnimation = new DoubleAnimation
                    {
                        From = (int)_canvas.ActualHeight - 200,
                        To = (int)_canvas.ActualHeight - 200 - y,
                        Duration = TimeSpan.FromMilliseconds(_random.Next(50, 100)),
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(5)
                    };
                    Storyboard.SetTarget(yAnimation, image);
                    Storyboard.SetTargetProperty(yAnimation, new PropertyPath(Canvas.TopProperty));
                    story.Children.Add(yAnimation);
                    break;
                case 2:
                    image.RenderTransformOrigin = new Point(0.5, 0.5);
                    bool flag = _random.Next() % 2 == 0;
                    var scaleYAnimation = new DoubleAnimation
                    {
                        From = 1,
                        To = -1,
                        Duration = TimeSpan.FromMilliseconds(_random.Next(200, 400)),
                        AutoReverse = flag,
                        RepeatBehavior = new RepeatBehavior(flag ? 2 : 1)
                    };
                    Storyboard.SetTarget(scaleYAnimation, image);
                    Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)"));
                    story.Children.Add(scaleYAnimation);
                    break;
                default:
                    break;
            }
            return story;
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
                Loaded -= _this_Loaded;
            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
