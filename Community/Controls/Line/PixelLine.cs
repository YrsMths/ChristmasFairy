using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    public class PixelLine : Shape, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty PixelProperty =
            DependencyProperty.Register("Pixel", typeof(int), typeof(PixelLine), new PropertyMetadata(1));
        
        protected override Geometry DefiningGeometry
        {
            get
            {
                return GenerateMyWeirdGeometry();
            }
        }

        public double X1
        {
            get => (double)GetValue(X1Property);
            set
            {
                SetValue(X1Property, value);
                OnPropertyChanged("DefiningGeometry");
            }
        }

        public double X2
        {
            get => (double)GetValue(X2Property);
            set
            {
                SetValue(X2Property, value);
                OnPropertyChanged("DefiningGeometry");
            }
        }

        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set{
                SetValue(Y1Property, value);
                OnPropertyChanged("DefiningGeometry");
            }
}

        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set
            {
                SetValue(Y2Property, value);
            OnPropertyChanged("DefiningGeometry");
            }
}

        public int Pixel
        {
            get => (int)GetValue(PixelProperty);
            set
            {
                SetValue(PixelProperty, value);
                OnPropertyChanged("DefiningGeometry");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            Loaded += delegate
            {
                CompositionTarget.Rendering += delegate
                {
                    
                };
            };
        }

        private Geometry GenerateMyWeirdGeometry()
        {
            StrokeThickness = Pixel;
            double k = (Y2 - Y1) / (X2 - X1);
            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext gc = geom.Open())
            {
                gc.BeginFigure(new Point((int)X1, (int)Y1), false, true);
                for (int i = (int)X1; i <= (int)X2; i += Pixel)
                {
                    int j = (int)(i * k + Y1);
                    gc.LineTo(new Point(i, j), true, true);
                    gc.LineTo(new Point(i + Pixel, j), true, true);
                }
            }
            SnapsToDevicePixels = true;
            return geom;
        }
    }
}
