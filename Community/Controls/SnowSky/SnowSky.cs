using Community.Controls.Base;
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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Community.Controls
{
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    public class SnowSky : ControlBase
    {
        private const string CanvasTemplateName = "PART_Canvas";

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(SnowSky), new PropertyMetadata(null));

        private DispatcherTimer _addSnowFlakeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        private Canvas _canvas;

        static SnowSky()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SnowSky), new FrameworkPropertyMetadata(typeof(SnowSky)));
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
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
            _addSnowFlakeTimer.Interval = TimeSpan.FromMilliseconds(300);
            _addSnowFlakeTimer.Tick += _addSnowFlakeTimer_Tick;
            _addSnowFlakeTimer.Start();
        }

        private void _addSnowFlakeTimer_Tick(object sender, EventArgs e)
        {
            AddSnowflake();
        }

        private void AddSnowflake()
        {
            var x = _random.Next(0, (int)_canvas.ActualWidth);
            var y = -10;
            var size = _random.Next(4, 12);
            var translateTransform = new TranslateTransform(x, y);

            var snowflake = new Snowflake
            {
                RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection { translateTransform }
                },
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = size,
                Height = size
            };
            _canvas.Children.Add(snowflake);
            y += (int)(_canvas.ActualHeight + 10);
            var animation = new DoubleAnimation
            {
                To = y,
                Duration = TimeSpan.FromSeconds(_random.Next(3, 8))
            };
            Storyboard.SetTarget(animation, snowflake);
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.Children[0].Y"));

            var story = new Storyboard();
            story.Completed += (sender, e) => { _canvas.Children.Remove(snowflake); snowflake.ClearEvents(); };
            story.Children.Add(animation);
            snowflake.Loaded += (sender, args) => story.Begin();
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
                _addSnowFlakeTimer.Tick -= _addSnowFlakeTimer_Tick;
                Loaded -= _this_Loaded;
            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
