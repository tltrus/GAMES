using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace WpfApp
{
    public enum Shape2DType
    {
        ROMB,
        TRIANGLE,
        ARROW
    }

    internal class Shape2D
    {
        private Vector2D centralpoint;
        private Shape2DType type;
        private int size = 5;
        private List<Vector2D> points;
        private int sign;
        private SolidColorBrush brush;

        public Shape2D(double x, double y, Shape2DType shapetype)
        {
            centralpoint = new Vector2D(x, y);
            type = shapetype;
            points = new List<Vector2D>();
            sign = 1; // Randoms.rnd.Next(-1, 2);
            brush = Brushes.White;
            CreateShape();
        }

        public void SetSize(int value)
        {
            size = value;
            CreateShape();
        }

        public void SetBrush(Color color) => brush = new SolidColorBrush(color);

        public void SetBrush(SolidColorBrush brush) => this.brush = brush;

        public void SetOpacity(double opacity)
        {
            Color color = brush.Color;
            color.A = (byte)opacity;
            brush = new SolidColorBrush(color);
        }

        private void CreateRomb()
        {
            double x = centralpoint.X - (double)size;
            double y = centralpoint.Y;
            points.Add(new Vector2D(x, y));
            double x2 = centralpoint.X;
            double y2 = centralpoint.Y - (double)size;
            points.Add(new Vector2D(x2, y2));
            double x3 = centralpoint.X + (double)size;
            double y3 = centralpoint.Y;
            points.Add(new Vector2D(x3, y3));
            double x4 = centralpoint.X;
            double y4 = centralpoint.Y + (double)size;
            points.Add(new Vector2D(x4, y4));
        }

        private void CreateTriangle()
        {
            double x = centralpoint.X - (double)size;
            double y = centralpoint.Y + (double)size;
            points.Add(new Vector2D(x, y));
            double x2 = centralpoint.X;
            double y2 = centralpoint.Y - (double)size - 10;
            points.Add(new Vector2D(x2, y2));
            double x3 = centralpoint.X + (double)size;
            double y3 = centralpoint.Y + (double)size;
            points.Add(new Vector2D(x3, y3));
        }

        private void CreateArrow()
        {
            double x = centralpoint.X - (double)size;
            double y = centralpoint.Y + (double)(size * 2);
            points.Add(new Vector2D(x, y));
            double x2 = centralpoint.X;
            double y2 = centralpoint.Y - (double)size;
            points.Add(new Vector2D(x2, y2));
            double x3 = centralpoint.X + (double)size;
            double y3 = centralpoint.Y + (double)(size * 2);
            points.Add(new Vector2D(x3, y3));
            double x4 = centralpoint.X;
            double y4 = centralpoint.Y + (double)size;
            points.Add(new Vector2D(x4, y4));
        }

        private void CreateShape()
        {
            points.Clear();
            switch (type)
            {
                case Shape2DType.ROMB:
                    CreateRomb();
                    break;
                case Shape2DType.TRIANGLE:
                    CreateTriangle();
                    break;
                case Shape2DType.ARROW:
                    CreateArrow();
                    break;
            }
        }

        public List<Vector2D> GetPoints() => points;

        public Vector2D GetCentralPoint() => centralpoint.CopyToVector();

        public Vector2D GetDirection()
        {
            Vector2D vector2D = Vector2D.Sub(points[1], centralpoint);
            vector2D.Normalize();
            return vector2D.CopyToVector();
        }

        public void Addition(Vector2D v)
        {
            foreach (Vector2D point in points)
            {
                point.Add(v);
            }

            centralpoint.Add(v);
        }

        public void Addition(double x, double y) => Addition(new Vector2D(x, y));

        private void Rotate(double angle, double x, double y)
        {
            double[,] matrixA = ListToArr(points);
            Matrix2D matrix2D = new Matrix2D();
            matrix2D.Translate(0.0 - x, 0.0 - y);
            Matrix2D matrix2D2 = new Matrix2D();
            matrix2D2.Rotate(angle);
            Matrix2D matrix2D3 = new Matrix2D();
            matrix2D3.Translate(x, y);
            Matrix2D matrix2D4 = matrix2D * matrix2D2 * matrix2D3;
            double[,] matrixB = matrix2D4.ToArray();
            double[,] arr = MatrixBase.Mult(matrixA, matrixB);
            points = ArrToList(arr);
        }

        public void RotateAroundCenter(double angle)
        {
            Rotate(angle * (double)sign, centralpoint.X, centralpoint.Y);
        }

        private double[,] ListToArr(List<Vector2D> list)
        {
            int count = list.Count;
            int num = 3;
            double[,] array = new double[count, num];
            for (int i = 0; i < list.Count; i++)
            {
                array[i, 0] = list[i].X;
                array[i, 1] = list[i].Y;
                array[i, 2] = 1.0;
            }

            return array;
        }

        private List<Vector2D> ArrToList(double[,] arr)
        {
            List<Vector2D> list = new List<Vector2D>();
            for (int i = 0; i < arr.GetUpperBound(0) + 1; i++)
            {
                double x = arr[i, 0];
                double y = arr[i, 1];
                double num = arr[i, 2];
                list.Add(new Vector2D(x, y));
            }

            return list;
        }

        public void Show(DrawingContext dc)
        {
            PointCollection pointCollection = new PointCollection();
            for (int i = 1; i < points.Count; i++)
            {
                double x = points[i].X;
                double y = points[i].Y;
                Point value = new Point(x, y);
                pointCollection.Add(value);
            }

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                Point startPoint = new Point(points[0].X, points[0].Y);
                streamGeometryContext.BeginFigure(startPoint, isFilled: true, isClosed: true);
                streamGeometryContext.PolyLineTo(pointCollection, isStroked: true, isSmoothJoin: true);
            }

            dc.DrawGeometry(brush, null, streamGeometry);
            //dc.DrawEllipse(Brushes.Red, null, new Point(centralpoint.X, centralpoint.Y), 2.0, 2.0);
        }
    }
}
