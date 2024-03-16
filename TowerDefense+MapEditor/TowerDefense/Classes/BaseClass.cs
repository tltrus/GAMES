using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace DefenseGame.Classes
{
    internal class BaseClass
    {
        public class Style
        {
            public Brush? brush;
            public Pen? pen;
            public Size size;
        }
        public Style style;
        public Vector2D pos;

        public void Drawing(DrawingContext dc)
        {
            StreamGeometry streamGeometry = new StreamGeometry();

            dc.DrawEllipse(
                style.brush,
                style.pen,
                new Point(pos.x, pos.y),
                style.size.Width,
                style.size.Height
            );
        }
    }
}
