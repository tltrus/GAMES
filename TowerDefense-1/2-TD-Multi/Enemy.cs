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
    class Enemy
    {
        public enum Type
        {
            enemy1 = 1, 
            enemy2 = 2, 
            enemy3 = 3, 
            enemy4 = 4
        }
        Type type;
        Random rnd = new Random();

        public int helth = 2;
        public int imgSize = 40;
        public Point pos { get; set; } = new Point();
        public Point direction;
        public Brush color;
        double imgX, imgY, imgWidth, imgHeight;

        public Enemy(Point pos)
        {
            this.pos = pos;

            direction = new Point(0.6, 2);
            color = Brushes.Green;

            // random type
            var enumCount = Enum.GetNames(typeof(Type)).Length;
            this.type = (Type)rnd.Next(1, enumCount);

            // img position
            imgX = pos.X - imgSize * 0.5;
            imgY = pos.Y - imgSize * 0.5;
            imgWidth = imgSize;
            imgHeight = imgSize;
        }

        public void Draw(DrawingContext dc)
        {
            imgX = pos.X - imgSize * 0.5;
            imgY = pos.Y - imgSize * 0.5;

            Rect rect = new Rect()
            {
                X = imgX,
                Y = imgY,
                Width = imgWidth,
                Height = imgHeight
            };

            string uriString = "pack://application:,,,/Media/imgEnemy1.png";
            switch (type)
            {
                case Type.enemy1:
                    uriString = "pack://application:,,,/Media/imgEnemy1.png";
                    break;
                case Type.enemy2:
                    uriString = "pack://application:,,,/Media/imgEnemy2.png";
                    break;
                case Type.enemy3:
                    uriString = "pack://application:,,,/Media/imgEnemy3.png";
                    break;
                case Type.enemy4:
                    uriString = "pack://application:,,,/Media/imgEnemy4.png";
                    break;
            }

            var imgSource = new BitmapImage(new Uri(uriString));
            dc.DrawImage(imgSource, rect);
            //dc.DrawRectangle(null, new Pen(Brushes.Black,1), rect);
        }
    }
}
