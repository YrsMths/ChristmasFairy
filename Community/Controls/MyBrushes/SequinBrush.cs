using Community.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Community.Controls
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Brush))]
    public class SequinBrush : MarkupExtension, INotifyPropertyChanged
    {
        Random rand = new Random();

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        MediaColor _Color;
        public MediaColor Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }

        public int CountX { get; set; } = 1;
        public int CountY { get; set; } = 1;

        int[] pixelValues;
        int[] pixelValues_Spark;

        DispatcherTimer timer;
        Storyboard board = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetProperty = service?.TargetProperty as DependencyProperty;
            var targetObject = service?.TargetObject;
            if (targetObject is FrameworkElement element && targetProperty != null)
            {
                element.SizeChanged += (sender, args) =>
                {
                    Canvas canvas = new Canvas();
                    canvas.Children.Add(Init());

                    Storyboard sb = new Storyboard();
                    foreach (var point in BirdsonHelper.Generate((int)element.ActualWidth, (int)element.ActualHeight))
                    {
                        var pentacle = new Pentacle() { Radius = rand.Next(15, 25), NumofAngle = 6, UnitAngle = 5, Fill= System.Windows.Media.Brushes.White };
                        Canvas.SetLeft(pentacle, point.X);
                        Canvas.SetTop(pentacle, point.Y);

                        DoubleAnimation animation = new DoubleAnimation();//实例化浮点动画
                        pentacle.RenderTransform = new RotateTransform();//设置为旋转动画
                        pentacle.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);//设置旋转的中心
                        animation.From = 0;//动画的起始值
                        animation.To = 360;//动画的结束值
                        animation.Duration = TimeSpan.FromSeconds(3);//动画的播放时间
                        animation.RepeatBehavior = RepeatBehavior.Forever;//设置动画循环播放
                        animation.AutoReverse = false;//设置动画可以进行反转
                        Storyboard.SetTarget(animation, pentacle);//给故事板绑定动画
                        Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.Angle"));//动画的依赖属性
                        sb.Children.Add(animation);//故事板添加动画

                        canvas.Children.Add(pentacle);
                    }
                    sb.Begin();//播放动画

                    canvas.MouseMove += MouseMove;
                    VisualBrush visualBrush = new VisualBrush() { Visual = canvas, Stretch=Stretch.Uniform };
                    element.SetValue(targetProperty, visualBrush);

                    
                };
                element.Unloaded += (sneder, args) =>
                {
                    VisualBrush visualBrush = element.GetValue(targetProperty) as VisualBrush;
                    foreach (FrameworkElement child in (visualBrush?.Visual as Canvas)?.Children)
                    {
                        child.ClearEvents();
                    }
                    element.ClearEvents();
                };
                Image image01 = Init();
                VisualBrush visualBrush01 = new VisualBrush() { Visual = image01 };
                return visualBrush01;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public Image Init()
        {
            int pixelCount = CountX * CountY;
            int[] pixelValues = new int[pixelCount];
            return GeneratePixelValues(ref pixelValues);
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public int Overlay(MediaColor brush, byte a, byte r, byte g, byte b)
        {
            if (null != brush)
                return ((brush.A + a) / 2 << 24) + ((brush.R + r) / 2 << 16) + ((brush.G + g) / 2 << 8) + (brush.B + b) / 2;
            return (a << 24) + (r << 16) + (g << 8) + b;
        }



        /// <summary>
        /// 获取噪点图
        /// </summary>
        /// <returns></returns>
        private Image GeneratePixelValues(ref int[] pixelValues)
        {
            Bitmap pic = new Bitmap(CountX, CountY);
            double Pa = 0.05;//接受输入的Pa
            double Pb = 0.8;//接受输入的Pb
            double P = Pb / (1 - Pa);//为了得到一个概率pixelCount事件

            for (int i = 0; i < CountY; i++)
            {
                for (int j = 0; j < CountX; j++)
                {
                    byte gray = GetNoise(Pa, Pb);

                    Color c = gray == 255 ? System.Drawing.Color.FromArgb(gray, gray, gray) : System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B);

                    byte a = (byte)c.A;
                    byte r = (byte)c.R;
                    byte g = (byte)c.G;
                    byte b = (byte)c.B;

                    pixelValues[j * CountY + i] = Overlay(Color, a, r, g, b);

                    byte a1 = (byte)(pixelValues[j * CountY + i] >> 24);
                    byte r1 = (byte)(pixelValues[j * CountY + i] >> 16);
                    byte g1 = (byte)(pixelValues[j * CountY + i] >> 8);
                    byte b1 = (byte)(pixelValues[j * CountY + i] >> 0);
                    pic.SetPixel(j, i, System.Drawing.Color.FromArgb(255, r, g, b));
                }
            }
            return new Image() { Source = Imaging.CreateBitmapSourceFromHBitmap(pic.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(pic.Width, pic.Height)) };
        }

        /// <summary>
        /// 获取椒盐
        /// </summary>
        /// <param name="Pa"></param>
        /// <param name="Pb"></param>
        /// <returns></returns>
        public byte GetNoise(double Pa, double Pb)
        {
            double P = Pb / (1 - Pa);//为了得到一个概率pixelCount事件
            int gray = 128;
            //int noise = 1;
            double probility = rand.NextDouble();
            if (probility < Pa)
            {
                gray = 255;//有Pa概率 噪声设为最大值
            }
            else
            {
                double temp = rand.NextDouble();
                if (temp < P)//有1 - Pa的几率到达这里，再乘以 P ，刚好等于Pb
                    gray = 0;
            }
            //if (noise != 1)
            //{
            //    gray = noise;
            //}
            //else
            //    gray = 128;
            return (byte)gray;
        }

        #region  INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

         #endregion
    }

    public class Noise
    {
        /// <summary>
        /// 用box muller的方法产生均值为mean，方差为variance的正太分布随机数
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public double GaussNiose(double mean = 0, double variance = 1)
        {
            //double noise = new Noise().GaussNiose();
            //int gray = (int)(255 + noise);//给图像添加高斯噪声噪声
            //if (gray > 255) gray = 255;
            //if (gray < 0) gray = 0;
            //Color color = Color.FromArgb(gray, gray, gray);
            //pic.SetPixel(j, i, color);

            Random ran = new Random(RandomHelper.GetRandomSeed());
            double r1 = ran.NextDouble();
            double r2 = ran.NextDouble();
            double result = Math.Sqrt((-2) * Math.Log(r2)) * Math.Sin(2 * Math.PI * r1);
            return mean + result * variance; ;//返回随机数
        }

        
    }
}
