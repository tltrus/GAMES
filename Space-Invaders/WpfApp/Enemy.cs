using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    class Enemy
    {
        public double x, y;
        public int r = 54;
        int xdir = 1;
        Brush brush;
        Pen pen;
        List<string> images;
        int imgNum;


        public Enemy(double x, double y)
        {
            this.x = x;
            this.y = y;
            brush = Brushes.LightBlue;
            pen = new Pen(Brushes.CadetBlue, 2);

            images = new List<string>
            {
                "../../../Media/nlo1.png", "../../../Media/nlo4.png"
            };

            imgNum = MainWindow.rnd.Next(images.Count);
        }

        public void Show(DrawingContext dc)
        {
            ImageSource imgSource = new BitmapImage(new Uri("pack://application:,,,/AssemblyName;" + images[imgNum]));
            Rect rect = new Rect()
            {
                X = x,
                Y = y,
                Width = r,
                Height = r
            };
            dc.DrawImage(imgSource, rect);
        }

        public void Move() => x += xdir;

        public void BlowOff() => r--;

        public void ShiftDown()
        {
            xdir *= -1;
            y += r;
        }


    }
}
