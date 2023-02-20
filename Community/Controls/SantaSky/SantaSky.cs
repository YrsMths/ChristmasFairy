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

namespace Community.Controls
{
    [TemplatePart(Name = GridTemplateName, Type = typeof(Grid))]
    [TemplatePart(Name = CanvasTemplateName, Type = typeof(Canvas))]
    public class SantaSky : Control
    {
        private const string GridTemplateName = "PART_GridLineContainer";

        private const string CanvasTemplateName = "PART_CanvasPartContainer";

        private Grid _grid;
        private Canvas _canvas;

        public static readonly DependencyProperty DeerCountProperty =
            DependencyProperty.Register("DeerCount", typeof(int), typeof(SantaSky), new UIPropertyMetadata(48));

        public static readonly DependencyProperty SantaCountProperty =
            DependencyProperty.Register("SantaCount", typeof(int), typeof(SantaSky), new UIPropertyMetadata(4));

        public static readonly DependencyProperty DeerImgProperty =
            DependencyProperty.Register("DeerImg", typeof(ImageSource), typeof(SantaSky), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SantaImgProperty =
            DependencyProperty.Register("SantaImg", typeof(ImageSource), typeof(SantaSky), new UIPropertyMetadata(null));

        public static readonly DependencyProperty PartSizeMinProperty =
            DependencyProperty.Register("PartSizeMin", typeof(int), typeof(SantaSky), new UIPropertyMetadata(40));

        public static readonly DependencyProperty PartSizeMaxProperty =
            DependencyProperty.Register("PartSizeMax", typeof(int), typeof(SantaSky), new UIPropertyMetadata(80));

        public static readonly DependencyProperty PartVMinProperty =
            DependencyProperty.Register("PartVMin", typeof(int), typeof(SantaSky), new UIPropertyMetadata(10));

        public static readonly DependencyProperty PartVMaxProperty =
            DependencyProperty.Register("PartVMax", typeof(int), typeof(SantaSky), new UIPropertyMetadata(20));

        public static readonly DependencyProperty PartRVMinProperty =
            DependencyProperty.Register("PartRVMin", typeof(int), typeof(SantaSky), new UIPropertyMetadata(90));

        public static readonly DependencyProperty PartRVMaxProperty =
            DependencyProperty.Register("PartRVMax", typeof(int), typeof(SantaSky), new UIPropertyMetadata(360));

        public static readonly DependencyProperty LineRateProperty =
            DependencyProperty.Register("LineRate", typeof(int), typeof(SantaSky), new UIPropertyMetadata(2));

        private readonly Random _random = new Random();
        private PartInfo[] _santas;
        private PartInfo[] _deers;

        static SantaSky()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SantaSky), new FrameworkPropertyMetadata(typeof(SantaSky)));
        }

        public SantaSky()
        {
            Loaded += delegate
            {
                CompositionTarget.Rendering += delegate
                {
                    PartRoamAnimation();
                    AddOrRemovePartLine();
                    MovePartLine();
                };
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _grid = GetTemplateChild(GridTemplateName) as Grid;
            _canvas = GetTemplateChild(CanvasTemplateName) as Canvas;
        }
        
        public int SantaCount
        {
            get => (int)GetValue(SantaCountProperty);
            set => SetValue(SantaCountProperty, value);
        }

        public int DeerCount
        {
            get => (int)GetValue(DeerCountProperty);
            set => SetValue(DeerCountProperty, value);
        }
        
        public ImageSource SantaImg
        {
            get => (ImageSource)GetValue(SantaImgProperty);
            set => SetValue(SantaImgProperty, value);
        }

        public ImageSource DeerImg
        {
            get => (ImageSource)GetValue(DeerImgProperty);
            set => SetValue(DeerImgProperty, value);
        }

        public int PartSizeMin
        {
            get => (int)GetValue(PartSizeMinProperty);
            set => SetValue(PartSizeMinProperty, value);
        }

        public int PartSizeMax
        {
            get => (int)GetValue(PartSizeMaxProperty);
            set => SetValue(PartSizeMaxProperty, value);
        }

        public int PartVMin
        {
            get => (int)GetValue(PartVMinProperty);
            set => SetValue(PartVMinProperty, value);
        }

        public int PartVMax
        {
            get => (int)GetValue(PartVMaxProperty);
            set => SetValue(PartVMaxProperty, value);
        }


        public int PartRVMin
        {
            get => (int)GetValue(PartRVMinProperty);
            set => SetValue(PartRVMinProperty, value);
        }

        public int PartRVMax
        {
            get => (int)GetValue(PartRVMaxProperty);
            set => SetValue(PartRVMaxProperty, value);
        }

        public int LineRate
        {
            get => (int)GetValue(LineRateProperty);
            set => SetValue(LineRateProperty, value);
        }



        public void InitPart()
        {
            //清空part容器
            _santas = new PartInfo[SantaCount];
            _deers = new PartInfo[DeerCount];

            _canvas.Children.Clear();
            _grid.Children.Clear();
            //生成星星
            for (var i = 0; i < SantaCount + DeerCount; i++)
            {
                double size = _random.Next(PartSizeMin, PartSizeMax + 1); //星星尺寸
                var partInfo = new PartInfo
                {
                    X = _random.Next(0, (int)_canvas.ActualWidth),
                    XV = (double)_random.Next(-PartVMax, PartVMax) / 60,
                    XT = _random.Next(6, 301), //帧
                    Y = _random.Next(0, (int)_canvas.ActualHeight),
                    YV = (double)_random.Next(-PartVMax, PartVMax) / 60,
                    YT = _random.Next(6, 301), //帧
                    Lined = false,
                    PartLines = new Dictionary<PartInfo, Line>()
                };
                var part = new Image
                {
                    Source = i < SantaCount ? SantaImg : DeerImg,
                    Width = size,
                    Height = size,
                    Stretch = Stretch.Fill,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform { Angle = 0 }
                };
                Canvas.SetLeft(part, partInfo.X);
                Canvas.SetTop(part, partInfo.Y);
                partInfo.PartRef = part;
                //设置part旋转动画
                SetPartRotateAnimation(part);
                //添加到容器
                if (i < SantaCount) _santas[i] = partInfo;
                else _deers[i - SantaCount] = partInfo;

                _canvas.Children.Add(part);
            }
        }

        private void SetPartRotateAnimation(Image image)
        {
            double v = _random.Next(PartRVMin, PartRVMax + 1); //速度
            double a = _random.Next(0, 360 * 5); //角度
            var t = a / v; //时间
            var dur = new Duration(new TimeSpan(0, 0, 0, 0, (int)(t * 1000)));

            var sb = new Storyboard
            {
                Duration = dur
            };
            //动画完成事件 再次设置此动画
            sb.Completed += (S, E) => { SetPartRotateAnimation(image); };

            var da = new DoubleAnimation
            {
                To = a,
                Duration = dur
            };
            Storyboard.SetTarget(da, image);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            sb.Children.Add(da);
            sb.Begin(this);
        }

        private void SetPartRotateAnimation(Path part)
        {
            double v = _random.Next(PartRVMin, PartRVMax + 1); //速度
            double a = _random.Next(0, 360 * 5); //角度
            var t = a / v; //时间
            var dur = new Duration(new TimeSpan(0, 0, 0, 0, (int)(t * 1000)));

            var sb = new Storyboard
            {
                Duration = dur
            };
            //动画完成事件 再次设置此动画
            sb.Completed += (S, E) => { SetPartRotateAnimation(part); };

            var da = new DoubleAnimation
            {
                To = a,
                Duration = dur
            };
            Storyboard.SetTarget(da, part);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            sb.Children.Add(da);
            sb.Begin(this);
        }
        private SolidColorBrush GetRandomColorBursh()
        {
            var r = (byte)_random.Next(128, 256);
            var g = (byte)_random.Next(128, 256);
            var b = (byte)_random.Next(128, 256);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }
        /// <summary>
        ///     Part漫游动画
        /// </summary>
        private void PartRoamAnimation()
        {
            if (_santas == null || _deers == null)
                return;

            foreach (var partInfo in _santas.Concat(_deers))
            {
                //X轴运动
                if (partInfo.XT > 0)
                {
                    //运动时间大于0,继续运动
                    if (partInfo.X >= _canvas.ActualWidth || partInfo.X <= 0)
                        //碰到边缘,速度取反向
                        partInfo.XV = -partInfo.XV;
                    //位移加,时间减
                    partInfo.X += partInfo.XV;
                    partInfo.XT--;
                    Canvas.SetLeft(partInfo.PartRef, partInfo.X);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    partInfo.XV = (double)_random.Next(-PartVMax, PartVMax) / 60;
                    partInfo.XT = _random.Next(100, 1001);
                }

                //Y轴运动
                if (partInfo.YT > 0)
                {
                    //运动时间大于0,继续运动
                    if (partInfo.Y >= _canvas.ActualHeight || partInfo.Y <= 0)
                        //碰到边缘,速度取反向
                        partInfo.YV = -partInfo.YV;
                    //位移加,时间减
                    partInfo.Y += partInfo.YV;
                    partInfo.YT--;
                    Canvas.SetTop(partInfo.PartRef, partInfo.Y);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    partInfo.YV = (double)_random.Next(-PartVMax, PartVMax) / 60;
                    partInfo.YT = _random.Next(100, 1001);
                }
            }
        }
        /// <summary>
        ///     添加或者移除santa & deer之间的连线
        /// </summary>
        private void AddOrRemovePartLine()
        {
            //没有part 直接返回
            if (_santas == null || SantaCount != _santas.Length || _deers == null || DeerCount != _deers.Length)
                return;

            //生成santa & deer间的连线
            for (var i = 0; i < SantaCount; i++)
                for (var j = 0; j < DeerCount; j++)
                {
                    var santa = _santas[i];
                    var x1 = santa.X + santa.PartRef.Width / 2;
                    var y1 = santa.Y + santa.PartRef.Height / 2;
                    var deer  = _deers[j];
                    var x2 = deer.X + deer.PartRef.Width / 2;
                    var y2 = deer.Y + deer.PartRef.Height / 2;
                    var s = Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)); //两个星星间的距离
                    var threshold = santa.PartRef.Width * LineRate + deer.PartRef.Width * LineRate;
                    if (s <= threshold)
                    {
                        if (!santa.PartLines.ContainsKey(deer) && !deer.Lined)
                        {
                            deer.Lined = true;
                            var line = new Line
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                Stroke = GetPartLineBrush(),
                            };
                            santa.PartLines.Add(deer, line);
                            _grid.Children.Add(line);
                        }
                    }
                    else
                    {
                        if (santa.PartLines.ContainsKey(deer))
                        {
                            deer.Lined = false;
                            _grid.Children.Remove(santa.PartLines[deer]);
                            santa.PartLines.Remove(deer);
                        }
                    }
                }
        }

        /// <summary>
        ///     移动part之间的连线
        /// </summary>
        private void MovePartLine()
        {
            //没有part 直接返回
            if (_santas == null || _deers == null)
                return;

            foreach (var santa in _santas)
                foreach (var partLine in santa.PartLines)
                {
                    var line = partLine.Value;
                    line.X1 = santa.X + santa.PartRef.Width / 2;
                    line.Y1 = santa.Y + santa.PartRef.Height / 2;
                    line.X2 = partLine.Key.X + partLine.Key.PartRef.Width / 2;
                    line.Y2 = partLine.Key.Y + partLine.Key.PartRef.Height / 2;
                }
        }

        /// <summary>
        ///     获取part连线颜色画刷
        /// </summary>
        /// <returns>LinearGradientBrush</returns>
        private LinearGradientBrush GetPartLineBrush()
        {
            return new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Offset = 0, Color = Color.FromRgb(0, 0, 0) },
                    new GradientStop { Offset = 0.4, Color = Color.FromRgb(0, 0, 0) },
                    new GradientStop { Offset = 0.6, Color = Color.FromRgb(0, 0, 0) },
                    new GradientStop { Offset = 1, Color = Color.FromRgb(0, 0, 0) }
                }
            };
        }
    }

    /// <summary>
    ///     星星
    /// </summary>
    internal class PartInfo
    {
        /// <summary>
        ///     X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     X轴速度(单位距离/帧)
        /// </summary>
        public double XV { get; set; }

        /// <summary>
        ///     X坐标以X轴速度运行的时间(帧)
        /// </summary>
        public int XT { get; set; }

        /// <summary>
        ///     Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        ///     Y轴速度(单位距离/帧)
        /// </summary>
        public double YV { get; set; }

        /// <summary>
        ///     Y坐标以Y轴速度运行的时间(帧)
        /// </summary>
        public int YT { get; set; }
        
        /// <summary>
        ///     对星星的引用
        /// </summary>
        public Image PartRef { get; set; }

        /// <summary>
        /// 存在连线
        /// </summary>
        public bool Lined { get; set; } 

        public Dictionary<PartInfo, Line> PartLines { get; set; }
    }
}
