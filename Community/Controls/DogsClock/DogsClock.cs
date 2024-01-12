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
    [TemplatePart(Name = HourTemplateName, Type = typeof(Canvas))]
    [TemplatePart(Name = HourTemplateName, Type = typeof(Canvas))]
    [TemplatePart(Name = HourTemplateName, Type = typeof(Canvas))]
    public class DogsClock : ControlBase
    {
        private const string CanvasTemplateName = "PART_Canvas";
        private const string HourTemplateName = "PART_HourTextBlock";
        private const string MinuteTemplateName = "PART_MinuteTextBlock";
        private const string SecondTemplateName = "PART_SecondTextBlock";
        static DogsClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DogsClock), new FrameworkPropertyMetadata(typeof(DogsClock)));
        }

        private Canvas _canvas;
        private TextBlock _hour;
        private TextBlock _minute;
        private TextBlock _second;

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(DogsClock),
                new PropertyMetadata(100));

        public int Top
        {
            get => (int)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
            _hour = GetTemplateChild(HourTemplateName) as TextBlock;
            _minute = GetTemplateChild(MinuteTemplateName) as TextBlock;
            _second = GetTemplateChild(SecondTemplateName) as TextBlock;
            if (_canvas == null) return;
            Loaded += (s, e) =>
            {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
                timer.Tick += delegate 
                {
                    if (_hour.Text != DateTime.Now.ToString("HH")) 
                    if(_minute.Text != DateTime.Now.ToString("mm")) 
                    if(_second.Text != DateTime.Now.ToString("ss")) 
                    _hour.Text = DateTime.Now.ToString("HH");
                    _minute.Text = DateTime.Now.ToString("mm");
                    _second.Text = DateTime.Now.ToString("ss");
                };
                timer.Start();
            };
        }

        private void IMHERE(Image image, TimeType timeType, int i = 0)
        {
            var shakeAnimation = new DoubleAnimation
            {
                To = Top + 10,
                Duration = TimeSpan.FromMilliseconds((int)timeType * 300),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior((int)timeType)
            };
            Storyboard.SetTarget(shakeAnimation, image);
            Storyboard.SetTargetProperty(shakeAnimation, new PropertyPath(Canvas.TopProperty));
        }
    }

    enum TimeType
    {
        Second = 1,
        Minute = 2,
        Hour = 3,
    }
}
