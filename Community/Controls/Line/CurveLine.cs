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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    public class CurveLine : Shape
    {
        static CurveLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurveLine), new FrameworkPropertyMetadata(typeof(CurveLine)));
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(PixelLine), new PropertyMetadata(0.0));

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
            set => SetValue(X1Property, value);
        }

        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        private Geometry GenerateMyWeirdGeometry()
        {
            return null;
        }
    }
}
