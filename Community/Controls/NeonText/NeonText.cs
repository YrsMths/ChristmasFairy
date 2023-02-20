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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    [TemplatePart(Name = Path1TemplateName, Type = typeof(Path))]
    [TemplatePart(Name = Path2TemplateName, Type = typeof(Path))]
    [TemplatePart(Name = Path3TemplateName, Type = typeof(Path))]
    public class NeonText : Control
    {
        private const string Path1TemplateName = "PART_Path1";
        private const string Path2TemplateName = "PART_Path2";
        private const string Path3TemplateName = "PART_Path3";

        static NeonText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NeonText), new FrameworkPropertyMetadata(typeof(NeonText)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NeonText), new PropertyMetadata("TEST"));

        //public new static readonly DependencyProperty FontFamilyProperty =
        //    DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ShapeText), new PropertyMetadata(new FontFamily()));

        //public new static readonly DependencyProperty FontStyleProperty =
        //    DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(ShapeText), new PropertyMetadata(null));

        //public new static readonly DependencyProperty FontWeightProperty =
        //    DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(ShapeText), new PropertyMetadata(null));

        //public new static readonly DependencyProperty FontStretchProperty =
        //    DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(ShapeText), new PropertyMetadata(null));

        //public new static readonly DependencyProperty FontSizeProperty =
        //    DependencyProperty.Register("FontSize", typeof(double), typeof(ShapeText), new PropertyMetadata(50.0));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(NeonText), new PropertyMetadata(4.0));
        
        public static readonly DependencyProperty DashProperty =
            DependencyProperty.Register("Dash", typeof(double), typeof(NeonText), new PropertyMetadata(20.0));

        public static readonly DependencyProperty DashIntervalProperty =
            DependencyProperty.Register("DashInterval", typeof(double), typeof(NeonText), new PropertyMetadata(0.0));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(double), typeof(NeonText), new PropertyMetadata(60.0));

        private double _height;
        private double _width;
        private Geometry _textGeometry;
        private Path _path1;
        private Path _path2;
        private Path _path3;

        [Localizability(LocalizationCategory.Text)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        public double Dash
        {
            get { return (double)GetValue(DashProperty); }
            set { SetValue(DashProperty, value); }
        }

        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public double DashInterval
        {
            get { return (double)GetValue(DashIntervalProperty); }
            set { SetValue(DashIntervalProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Geometry DefiningGeometry
        {
            get
            {
                return _textGeometry ?? Geometry.Empty;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _path1 = GetTemplateChild(Path1TemplateName) as Path;
            _path2 = GetTemplateChild(Path2TemplateName) as Path;
            _path3 = GetTemplateChild(Path3TemplateName) as Path;
            Loaded += (s, e) =>
            {
                StartRolling();
            };
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(Math.Min(availableSize.Width, _width), Math.Min(availableSize.Height, _height));
        }

        public void Init()
        {
            RealizeGeometry();
            _path1.Data = _path2.Data = _path3.Data = DefiningGeometry;
            _path1.StrokeDashOffset = 0;
            _path2.StrokeDashOffset = Dash + DashInterval;
            _path3.StrokeDashOffset = 2 * (Dash + DashInterval);

            DoubleCollection dc = new DoubleCollection();
            dc.Add(Dash);
            dc.Add(2 * (Dash + DashInterval) + Interval);
            _path1.StrokeDashArray = _path2.StrokeDashArray = _path3.StrokeDashArray = dc;
        }

        private void StartRolling()
        {
            Init();
            Storyboard storyboard = new Storyboard();
            for (int i = 0; i < 3; i ++)
            {
                DoubleAnimation rollingAnimation = new DoubleAnimation()
                {
                    To = Interval + 2 * DashInterval + 3 * Dash + i * (Dash + DashInterval),
                    Duration = TimeSpan.FromMilliseconds(3000),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = false
                };
                Storyboard.SetTarget(rollingAnimation, GetTemplateChild("PART_Path" + (i + 1)));
                Storyboard.SetTargetProperty(rollingAnimation, new PropertyPath(Path.StrokeDashOffsetProperty));

                storyboard.Children.Add(rollingAnimation);
            }
            storyboard.Begin();
        }

        private void RealizeGeometry()
        {
            var formattedText = new FormattedText(
                                       Text,
                                       CultureInfo.CurrentCulture,
                                       FlowDirection.LeftToRight,
                                       new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Transparent);
            _height = formattedText.Height;
            _width = formattedText.Width;
            _textGeometry = formattedText.BuildGeometry(new Point());
        }
    }
}
