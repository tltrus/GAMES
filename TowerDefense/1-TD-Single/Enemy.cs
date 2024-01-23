using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TD
{
    class Enemy
    {
        public int helth = 2;
        public int size = 4;
        public Point pos { get; set; } = new Point();
        public Point direction;
        public Brush color;

        public Enemy(Point pos)
        {
            this.pos = pos;

            direction = new Point(0, 1);
            color = Brushes.Green;
        }
    }
}
