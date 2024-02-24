using System;
using System.Windows;
using System.Windows.Media;

namespace WpfApp
{
    class Missile
    {
        public double x, y;
        int r = 6;
        public bool toDelete;
        Brush brush;
        Pen pen;


        public Missile(double x, double y)
        {
            this.x = x;
            this.y = y;
            brush = Brushes.Black;
            pen = new Pen(Brushes.Yellow, 1);
        }

        public void Show(DrawingContext dc) => dc.DrawEllipse(brush, pen, new Point(x, y), r, r);

        public void Move() => y -= 5;

        public void Evaporate() => toDelete = true;

        public bool Hits(Enemy enemy)
        {
            var d = Dist(x, y, enemy.x, enemy.y);
            if (d < r + enemy.r)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double Dist(double x1, double y1, double x2, double y2) => Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
}
