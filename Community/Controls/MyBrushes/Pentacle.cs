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
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.MyBrushes"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.MyBrushes;assembly=Community.Controls.MyBrushes"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:Pentacle/>
    ///
    /// </summary>
    [TemplatePart(Name = PolygonTemplateName, Type = typeof(Polygon))]
    public class Pentacle : Control
    {
        private const string PolygonTemplateName = "PART_Polygon";

        public Polygon _polygon { get; private set; }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Pentacle), new PropertyMetadata(10.0));

        public static readonly DependencyProperty UnitAngleProperty =
            DependencyProperty.Register("UnitAngle", typeof(int), typeof(Pentacle), new PropertyMetadata(45));

        public static readonly DependencyProperty NumofAngleProperty =
            DependencyProperty.Register("NumofAngle", typeof(int), typeof(Pentacle), new PropertyMetadata(5));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(Pentacle), new PropertyMetadata(Brushes.Transparent));
        static Pentacle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pentacle), new FrameworkPropertyMetadata(typeof(Pentacle)));
        }

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public int UnitAngle
        {
            get => (int)GetValue(UnitAngleProperty);
            set => SetValue(UnitAngleProperty, value);
        }

        public int NumofAngle
        {
            get => (int)GetValue(NumofAngleProperty);
            set => SetValue(NumofAngleProperty, value);
        }

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _polygon = GetTemplateChild(PolygonTemplateName) as Polygon;
            _polygon.Points = GetPoints();
        }

        /// <summary>
        /// 画多角芒星,正多角星
        /// </summary>
        /// <param name="r">外接圆半径</param>
        /// <param name="n">角数量</param>
        /// <returns>返回包含正多角星的Canvas</returns>
        public PointCollection GetPoints()
        {
            double x1, x2, y1;
            GetCoordinate(Radius, NumofAngle, out x1, out y1, out x2);
            PointCollection points = new PointCollection();
            //重复N次画出N个三角形斜边
            for (int i = 1; i <= NumofAngle; i++)
            {
                points.Add(RotateAngle(new Point(Radius, Radius), i * 360 / NumofAngle, new Point(x2, y1)));
                points.Add(RotateAngle(new Point(Radius, Radius), i * 360 / NumofAngle, new Point(Radius, 0)));
                //DrawingVisual dv = new DrawingVisual();
                //using (DrawingContext dc = dv.RenderOpen())
                //{
                //    dc.DrawGeometry(Brushes.LightBlue, new Pen(Brushes.BlueViolet, 1), Geometry.Parse(string.Format("M {0},0 L{1},{2} M {0},0 L{3},{2}", r, x1, y1, x2)));
                //}
                ////顺时针旋转
                //dv.Transform = new RotateTransform(i * 360 / n, r, r);

                ////作为图片资源放到图片控件中
                //RenderTargetBitmap rtb = new RenderTargetBitmap(100, 100, 0, 0, PixelFormats.Default);
                //rtb.Render(dv);
                //Image image = new Image() { Source = rtb };
                //canvas.Children.Add(image);
            }
            return points;
        }

        /// <summary>
        /// 获取顶三角形坐标
        /// </summary>
        /// <param name="r">外接圆半径（顶点到中心的距离）</param>
        /// <param name="n">N角星</param>
        /// <param name="x1">左横坐标</param>
        /// <param name="y1">纵坐标</param>
        /// <param name="x2">右横坐标</param>
        private void GetCoordinate(double r, int n, out double x1, out double y1, out double x2)
        {
            //double unitAngle = 0;
            //if (n < 5)
            //{
            //奇数角星锐角30，偶数角星锐角和为45
            //unitAngle = 20;// n % 2 == 1 ? 30 : 45;
            //}
            //else
            //{
            //    //奇数角星锐角和为180，偶数角星锐角和为360
            //    unitAngle = n % 2 == 1 ? 180 / n : 360 / n;
            //}
            double l = Math.PI / 180;    //弧度单位
            double a = Math.Sin(360 / (2 * n) * l),         //多角芒星各角连中心分割所得的夹角的一半
                b = Math.Sin(UnitAngle / 2 * l),           //芒星内角锐角的一半
                c = Math.Sin((180 - 360 / (2 * n) - UnitAngle / 2) * l),  //芒星除了内锐角的其他内角与中心点连线的夹角
                d = Math.Cos((360 / (2 * n)) * l);
            x1 = (a * r * b) / c;         //正弦定理
            y1 = (d * r * b) / c;


            x2 = r - x1;    //x2与x1关于中心点垂线对称,右移r个长度
            x1 += r;        //右移r个长度
            y1 -= r;        //下移r个长度
            //取正数
            x1 = x1 < 0 ? x1 * (-1) : x1;
            y1 = y1 < 0 ? y1 * (-1) : y1;
            x2 = x2 < 0 ? x2 * (-1) : x2;
        }

        /// <summary>
        /// 计算坐标2以坐标1为中心旋转后坐标
        /// </summary>
        /// <param name="p1">坐标1</param>
        /// <param name="ARotate">旋转角度</param>
        /// <param name="p2">坐标2</param>
        /// <param name="p3">旋转后坐标</param>
        /// <returns>运行状态</returns>
        public Point RotateAngle(Point p1, double ARotate, Point p2)
        {
            Point p3 = new Point();
            try
            {
                double Rad = 0;
                Rad = ARotate * Math.Acos(-1) / 180;
                p3.X = (p2.X - p1.X) * Math.Cos(Rad) - (p2.Y - p1.Y) * Math.Sin(Rad) + p1.X;
                p3.Y = (p2.Y - p1.Y) * Math.Cos(Rad) + (p2.X - p1.X) * Math.Sin(Rad) + p1.Y;
            }
            catch (Exception ex)
            {
                p3.X = 999999;
                p3.Y = 999999;
            }
            return p3;
        }
    }
}
