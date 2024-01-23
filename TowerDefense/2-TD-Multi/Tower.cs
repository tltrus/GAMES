using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TD_multi
{
    class Tower
    {
        public Point pos { get; set; } = new Point();
        public Brush tcolor;
        public int gunAngle;
        public int rsize = 150; // radar radius
        public List<Bullet> bullets;
        Pen rpen;
        double imgX, imgY, imgWidth;
        public double imgHeight;
        int imgSize = 60;

        int type; // тип башни: 1 - главная, 0 - второстепенные
        

        public Tower(Point pos, int type = 0)
        {
            this.pos = pos;
            this.type = type;

            bullets = new List<Bullet>();
            tcolor = Brushes.Blue;

            // for Radar
            rpen = new Pen();
            rpen.Brush = Brushes.LightGray;
            rpen.Thickness = 3;
            rpen.DashStyle = DashStyles.Dash;

            // img position
            imgX = pos.X - imgSize * 0.5;
            imgY = pos.Y - imgSize * 0.5;
            imgWidth = imgSize;
            imgHeight = imgSize;
        }

        public void Draw(DrawingContext dc)
        {
            // radar

            Point p = new Point(pos.X, pos.Y);
            dc.DrawEllipse(null, rpen, p, rsize, rsize);

            // tower

            Rect rect = new Rect()
            {
                X = imgX,
                Y = imgY,
                Width = imgWidth,
                Height = imgHeight
            };

            var imgSource = new BitmapImage(new Uri("pack://application:,,,/Media/imgTower.png"));
            dc.DrawImage(imgSource, rect);

            // bullets

            foreach (var b in bullets)
            {
                b.Draw(dc);
            }
        }
    }
}
