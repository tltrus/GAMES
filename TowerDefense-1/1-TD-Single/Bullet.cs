using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TD
{
    class Bullet
    {
        public Point pos { get; set; } = new Point();
        public double angle;
        public int size = 1;
        public int power = 1;
        public Brush color = Brushes.White;
        public bool catched;

        public Bullet(Point pos, double angle)
        {
            this.pos = pos; 
            this.angle = angle;
        }
    }
}
