using System;
using System.Windows.Shapes;
using System.Windows.Media;

namespace NLO_killer
{
    /// <summary>
    /// Класс пушки
    /// </summary>
    class Gun
    {
        public int X1, Y1;
        public Line rect;
        public double angleRad; // наклон пушки в радианах

        public double angle // наклон пушки
        {
            get { return angleRad; }   // получаем в радианах
            set { angleRad = value * (Math.PI) / 180; } // вводим в градусах  
        }

        public Gun(int x1, int y1, int x2, int y2)
        {
            X1 = x1; Y1 = y1;

            rect = new Line
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 4
            };
        }
    }
}
