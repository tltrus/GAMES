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
    class Ship
    {
        public double x;
        int xdir;
        double width, height;

        public Ship()
        {
            x = MainWindow.width / 2;
            width = 200;
            height = 150;
        }

        public void SetDir(int dir) => xdir = dir;
        public void Move() => x += xdir * 5;
        public double GetLanchXPos() => x + width / 2;
        public void Draw(DrawingContext dc)
        {
            ImageSource imgSource = new BitmapImage(new Uri("pack://application:,,,/AssemblyName;../../../Media/submarine.png"));
            Rect rect = new Rect()
            {
                X = x,
                Y = MainWindow.height - 125,
                Width = width,
                Height = height
            };
            dc.DrawImage(imgSource, rect);
        }
    }
}
