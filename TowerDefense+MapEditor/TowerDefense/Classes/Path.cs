using System.Windows.Media;
using System.Windows;

namespace DefenseGame
{
    public class Path
    {
        public PointCollection points = new PointCollection();
        public double radius = 15;

        public void AddPoint(double x, double y) => points.Add(new Point(x, y));

        public Vector2D GetStart() => new Vector2D(points[0].X, points[0].Y);

        public Vector2D GetEnd() => new Vector2D(points[points.Count - 1].X, points[points.Count - 1].Y);


        public void Draw(DrawingContext dc)
        {
            if (points.Count <= 0) return;

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], false, true);
                geometryContext.PolyLineTo(points, true, true);
            }
            dc.DrawGeometry(null, new Pen(Brushes.SaddleBrown, radius * 2), streamGeometry);
        }
    }
}
