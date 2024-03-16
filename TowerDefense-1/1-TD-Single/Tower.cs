using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TD
{
    class Tower
    {
        public Point pos { get; set; } = new Point();
        public Brush tcolor, rcolor;
        public int gunAngle;
        public int size = 10;
        public int rsize = 150; // radar radius
        public List<Bullet> bullets;
        private int type; // тип башни: 1 - главная, 0 - второстепенные

        public Tower(Point pos, int type = 0)
        {
            this.pos = pos;
            this.type = type;

            bullets = new List<Bullet>();
            tcolor = Brushes.Blue;
            rcolor = Brushes.Yellow;
        }
    }
}
