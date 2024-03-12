using MiscUtils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp
{
    class Laser
    {
        Vector2D pos, vel;
        
        public Laser(double x, double y, Vector2D dir)
        {
            pos = new Vector2D(x, y);
            vel = dir.CopyToVector();
            vel.Mult(10);
        }

        public void Update() => pos.Add(vel);

        public bool isEdge()
        {
            if (pos.X > MainWindow.width || pos.X < 0)
            {
                return true;
            }
            if (pos.Y > MainWindow.height || pos.Y < 0)
            {
                return true;
            }
            return false;
        }

        public void Draw(DrawingContext dc)
        {
            dc.DrawEllipse(Brushes.Yellow, null, new Point(pos.X, pos.Y), 2, 2);
        }
    }
}
