using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System;

namespace WpfApp
{
    public enum Shape2DType
    {
        IMAGE
    }

    internal class Shape2D
    {
        private Vector2D centralpoint;
        private Vector2D gunpoint;
        private Vector2D dirpoint;
        private Shape2DType type;
        private int size = 30;
        private List<Vector2D> points;
        private string uriString;
        private BitmapImage imgSource;

        RotateTransform rotation;

        public Shape2D(double x, double y, Shape2DType shapetype)
        {
            centralpoint = new Vector2D(x, y);
            gunpoint = new Vector2D(0, -40);
            dirpoint = new Vector2D(0, -1);
            type = shapetype;
            points = new List<Vector2D>();
            CreateShape();
        }

        public void SetSize(int value)
        {
            size = value;
            CreateShape();
        }

        private void CreateImage()
        {
            double x = centralpoint.X - size;
            double y = centralpoint.Y + size;
            points.Add(new Vector2D(x, y));

            double x2 = centralpoint.X - size;
            double y2 = centralpoint.Y - size;
            points.Add(new Vector2D(x2, y2));

            double x3 = centralpoint.X + size;
            double y3 = centralpoint.Y - size;
            points.Add(new Vector2D(x3, y3));

            double x4 = centralpoint.X + size;
            double y4 = centralpoint.Y + size;
            points.Add(new Vector2D(x4, y4));

            uriString = "pack://application:,,,/Media/spaceship-1.png";
            imgSource = new BitmapImage(new Uri(uriString));

            rotation = new RotateTransform();
        }
        private void CreateShape()
        {
            points.Clear();
            switch (type)
            {
                case Shape2DType.IMAGE:
                    CreateImage();
                    break;
            }
        }

        public Vector2D GetCentralPoint() => centralpoint.CopyToVector();
        public Vector2D GetGunPoint() => gunpoint + centralpoint;
        public Vector2D GetDirection() => dirpoint.CopyToVector();

        public void Addition(Vector2D v)
        {
            foreach (Vector2D point in points)
            {
                point.Add(v);
            }
            centralpoint.Add(v);
        }
        public void Addition(double x, double y) => Addition(new Vector2D(x, y));
        public void RotateAroundCenter(double angle) => Rotate(angle, centralpoint.X, centralpoint.Y);

        private void Rotate(double angle, double x, double y)
        {
            rotation.Angle += angle;

            if (angle != 0)
            {
                var radian = angle * (Math.PI / 180);
                dirpoint.Rotate(radian);
                gunpoint.Rotate(radian);
            }
        }

        public void Show(DrawingContext dc)
        {
            var imgX = points[1].X;
            var imgY = points[1].Y;

            Rect rect = new Rect()
            {
                X = imgX,
                Y = imgY,
                Width = size * 2,
                Height = size * 2
            };
            
            rotation.CenterX = centralpoint.X;
            rotation.CenterY = centralpoint.Y;
            dc.PushTransform(rotation);
            dc.DrawImage(imgSource, rect);

            // corner of Image
            //dc.DrawEllipse(Brushes.Red, null, new Point(points[0].X, points[0].Y), 2, 2);
            //dc.DrawEllipse(Brushes.Red, null, new Point(points[1].X, points[1].Y), 2, 2);
            //dc.DrawEllipse(Brushes.Red, null, new Point(points[2].X, points[2].Y), 2, 2);
            //dc.DrawEllipse(Brushes.Red, null, new Point(points[3].X, points[3].Y), 2, 2);
        }
    }
}
