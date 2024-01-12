using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Helpers
{
    public class BirdsonHelper
    {
        private static Random random = new Random();
        private const double MinDistance = 50; // 最小距离，可以根据需要调整
        private const int MaxIterations = 1000; // 最大迭代次数，可以根据需要调整

        public static List<Point> Generate(int width, int height)
        {
            List<Point> points = new List<Point>();
            Generate(width, height, points, new Rectangle(0, 0, width, height), 0);
            return points;
        }

        private static void Generate(int width, int height, List<Point> points, Rectangle bounds, int iterations)
        {
            if (iterations > MaxIterations)
                return;

            double x = random.NextDouble() * bounds.Width;
            double y = random.NextDouble() * bounds.Height;
            Point point = new Point((int)x, (int)y);

            if (IsInBounds(point, bounds) && DistanceCheck(points, point))
            {
                points.Add(point);
                Generate(width, height, points, bounds, iterations + 1);
            }
        }

        private static bool IsInBounds(Point point, Rectangle bounds)
        {
            return point.X >= bounds.Left && point.X <= bounds.Right && point.Y >= bounds.Top && point.Y <= bounds.Bottom;
        }

        private static bool DistanceCheck(List<Point> points, Point point)
        {
            foreach (var existingPoint in points)
            {
                double distance = Math.Sqrt((point.X - existingPoint.X) * (point.X - existingPoint.X) + (point.Y - existingPoint.Y) * (point.Y - existingPoint.Y));
                if (distance < MinDistance)
                    return false; // 如果新点与现有点之间的距离小于最小距离，则返回false，不满足条件。
            }
            return true; // 所有点都满足条件。
        }
    }
}
