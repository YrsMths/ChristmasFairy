using Community.Themes.Basic;
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
using System.Windows.Shapes;

namespace ZYM.Development.Views.Barbie
{
    /// <summary>
    /// BarbieWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BarbieWindow : BasicWindow
    {
        public BarbieWindow()
        {
            InitializeComponent();
            ImageSource[] Barbies = (Application.Current.Resources["Barbies"] as ImageCollection).ToArray();
            image.Source = Barbies[_random.Next(0, Barbies.Length)];
        }

        Random _random = new Random((int)DateTime.Now.Ticks);

        private void BasicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!image.Source.ToString().Contains("title"))
            {
                var points = new PointCollection();
                Point point = image.TranslatePoint(new Point(), pentacle);
                foreach (var p in pentacle._polygon.Points)
                {
                    points.Add(new Point(p.X - point.X, p.Y - point.Y));
                }
                //绘制路径 剪切图片    
                List<LineSegment> segments = new List<LineSegment>();
                foreach (var item in points)
                {
                    segments.Add(new LineSegment(item, true));
                }
                PathFigure figure = new PathFigure(points.First(), segments, false);
                PathGeometry myPathGeometry = new PathGeometry();
                myPathGeometry.Figures.Add(figure);
                image.Clip = myPathGeometry;
            }
        }
    }
}
