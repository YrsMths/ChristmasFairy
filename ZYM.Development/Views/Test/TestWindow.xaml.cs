using Community.Controls;
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

namespace ZYM.Development.Views.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : BasicWindow
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void BasicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var points = pentacle._polygon.Points;
            ////鼠标绘制的路径     
            //List<LineSegment> segments = new List<LineSegment>();
            //foreach (var item in points)
            //{
            //    segments.Add(new LineSegment(item, true));
            //}
            //PathFigure figure = new PathFigure(points.First(), segments, false);
            //PathGeometry myPathGeometry = new PathGeometry();
            //myPathGeometry.Figures.Add(figure);
            //image.Clip = myPathGeometry;
        }
    }
}
