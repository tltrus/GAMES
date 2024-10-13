using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class ProgressBar
    {
        Point start_position, end_position, current_position;
        double value;
        bool visible;
        double limit;

        public ProgressBar()
        {

        }

        public void SetPosition(double x, double y)
        {
            start_position = new Point(x, y - 15);
            end_position = new Point(x + 50, y - 15);

            current_position = new Point(x, y - 15);
            limit = x + 50;
        }

        public void SetVisible(bool visible) => this.visible = visible;
        public bool isVisible() => visible;
        public void SetValue(double val) => value = val;
        public double GetValue() => value;
        public void Update()
        {
            if (!visible) return;

            if (current_position.X > limit) return;

            current_position.X += 1;
            value++;
        }


        public void Draw(DrawingContext dc)
        {
            if (!visible) return;

            // Drawing background
            dc.DrawLine(new Pen(Brushes.LightGray, 4), start_position, end_position);

            // Draw current
            dc.DrawLine(new Pen(Brushes.Blue, 4), start_position, current_position);
        }
    }
}
