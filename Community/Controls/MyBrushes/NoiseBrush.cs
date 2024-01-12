using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows;
using System.Drawing;
using System.Windows.Data;

namespace Community.Controls
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Brush))]
    public class NoiseBrush : MarkupExtension, INotifyPropertyChanged
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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetProperty = service?.TargetProperty as DependencyProperty;
            var targetObject = service?.TargetObject;
            if (targetObject is FrameworkElement element && targetProperty != null)
            {
                element.SizeChanged += (sender, args) =>
                {
                    Storyboard board = null;
                    CountX = (int)Math.Ceiling(element.ActualWidth);
                    CountY = (int)Math.Ceiling(element.ActualHeight);
                    Image image = Init();
                    List<BitmapFrame> frameList = new List<BitmapFrame>();
                    for (int i = 0; i < 40; i++)
                    {
                        frameList.Add(BitmapFrame.Create(Init().Source as BitmapSource));
                    }
                    ObjectAnimationUsingKeyFrames objKeyAnimate = new ObjectAnimationUsingKeyFrames();
                    objKeyAnimate.Duration = new Duration(TimeSpan.FromSeconds(1));
                    foreach (var item in frameList)
                    {
                        DiscreteObjectKeyFrame k1_img1 = new DiscreteObjectKeyFrame(item);
                        objKeyAnimate.KeyFrames.Add(k1_img1);
                    }
                    board = new Storyboard();
                    board.RepeatBehavior = RepeatBehavior.Forever;
                    board.FillBehavior = FillBehavior.HoldEnd;
                    board.AutoReverse = true;
                    board.Children.Add(objKeyAnimate);
                    Storyboard.SetTarget(objKeyAnimate, image);
                    Storyboard.SetTargetProperty(objKeyAnimate, new PropertyPath(Image.SourceProperty));
                    VisualBrush visualBrush = new VisualBrush() { Visual = image };
                    element.SetValue(targetProperty, visualBrush);
                    board.Begin(image);
                };
                return null;
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
            double Pa = 0.3;//接受输入的Pa
            double Pb = 0.1;//接受输入的Pb
            double P = Pb / (1 - Pa);//为了得到一个概率pixelCount事件

            for (int i = 0; i < CountY; i++)
            {
                for (int j = 0; j < CountX; j++)
                {
                    byte gray = GetNoise(Pa, Pb);
                    Color c = System.Drawing.Color.FromArgb(gray, gray, gray);

                    byte a = (byte)c.A;
                    byte r = (byte)c.R;
                    byte g = (byte)c.G;
                    byte b = (byte)c.B;

                    pixelValues[j * CountY + i] = Overlay((MediaColor)Color, a, r, g, b);

                    byte a1 = (byte)(pixelValues[j * CountY + i] >> 24);
                    byte r1 = (byte)(pixelValues[j * CountY + i] >> 16);
                    byte g1 = (byte)(pixelValues[j * CountY + i] >> 8);
                    byte b1 = (byte)(pixelValues[j * CountY + i] >> 0);
                    pic.SetPixel(j, i, System.Drawing.Color.FromArgb(255, r1, g1, b1));
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

    public class ClrBindingExchanger<T> : DependencyObject
    {
        private readonly object _owner;
        private readonly DependencyProperty _attachedProperty;
        private readonly Action<object, object> _valueChangeCallback;

        public ClrBindingExchanger(object owner, DependencyProperty attachedProperty,
            Action<object, object> valueChangeCallback = null)
        {
            _owner = owner;
            _attachedProperty = attachedProperty;
            _valueChangeCallback = valueChangeCallback;
        }

        public object GetValue()
        {
            return GetValue(_attachedProperty);
        }

        public void SetValue(object value)
        {
            if (value is Binding binding)
            {
                BindingOperations.SetBinding(this, _attachedProperty, binding);
            }
            else
            {
                SetValue(_attachedProperty, (T)value);
            }
        }

        public static void ValueChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ClrBindingExchanger<T>)d)._valueChangeCallback?.Invoke(e.OldValue, e.NewValue);
        }
    }
}
