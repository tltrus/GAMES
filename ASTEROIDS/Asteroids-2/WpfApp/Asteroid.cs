using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp
{
    class Asteroid
    {
        Vector2D pos, vel;
        int total;
        double r;
        double[] offset;
        List<Vector2D> points;

        public Asteroid(double r)
        {
            var x = MainWindow.rnd.Next((int)MainWindow.width);
            var y = MainWindow.rnd.Next((int)MainWindow.height);
            pos = new Vector2D(x, y);
            this.r = r;

            vel = Vector2D.Random2D(MainWindow.rnd);
            vel.Mult(0.5);
            total = MainWindow.rnd.Next(5, 10);
            offset = new double[total];
            for (var i = 0; i < total; i++)
            {
                offset[i] = MainWindow.rnd.NextDoubleRange(-r * 0.5, r * 0.5);
            }
            points = new List<Vector2D>();

            CreateShape();
        }

        public void Update()
        {
            pos.Add(vel);
            CreateShape();
        }
        public void Edges()
        {
            if (pos.X > MainWindow.width)
            {
                pos.X = 0;
            }
            else if (pos.X < 0)
            {
                pos.X = MainWindow.width;
            }
            if (pos.Y > MainWindow.height)
            {
                pos.Y = 0;
            }
            else if (pos.Y < 0)
            {
                pos.Y = MainWindow.height;
            }
        }

        private void CreateShape()
        {
            points.Clear();

            for (var i = 0; i < total; i++)
            {
                var angle = MiscUtils.Map(i, 0, total, 0, Math.PI * 2);
                r = 30 + offset[i];
                var x = r * Math.Cos(angle);
                var y = r * Math.Sin(angle);
                var v = new Vector2D(pos.X + x, pos.Y + y);
                points.Add(v);
            }
        }

        public void Draw(DrawingContext dc)
        {
            PointCollection dc_points = new PointCollection();
            for (int i = 1; i < points.Count; ++i)
            {
                var x = points[i].X;
                var y = points[i].Y;
                var point = new Point(x, y);
                dc_points.Add(point);
            }

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                var beginPoint = new Point(points[0].X, points[0].Y);
                geometryContext.BeginFigure(beginPoint, true, true);
                geometryContext.PolyLineTo(dc_points, true, true);
            }

            dc.DrawGeometry(null, new Pen(Brushes.White, 3), streamGeometry);
        }
    }
}
