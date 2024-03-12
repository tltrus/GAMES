using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WpfApp
{
    class Ship
    {
        Vector2D vel;
        bool isBoosting, isBraking;
        int r = 10;
        Shape2D shape;
        public List<Laser> lasers;

        public Ship()
        {
            double x = MainWindow.width / 2;
            double y = MainWindow.height / 2;
            shape = new Shape2D(x, y, Shape2DType.TRIANGLE);
            shape.SetSize(r);

            vel = new Vector2D();
            lasers = new List<Laser>();
        }

        public void Boosting(bool b) => isBoosting = b;
        public void Braking(bool b) => isBraking = b;

        public void Update()
        {
            if (isBoosting)
                Boost();
            if (isBraking)
                Brake();

            shape.Addition(vel);
            vel.Mult(0.99);
        }

        private void Boost()
        {
            var force = shape.GetDirection();
            force.Mult(0.1);
            vel.Add(force);
        }

        private void Brake() => vel.Mult(0.99);

        public void Draw(DrawingContext dc)
        {
            foreach (var l in lasers.ToList())
            {
                l.Draw(dc);
                l.Update();
                if (l.isEdge())
                {
                    lasers.RemoveAt(lasers.IndexOf(l));
                    continue;
                }
            }

            shape.Show(dc);
        }

        public void Edges()
        {
            var pos = shape.GetCentralPoint();

            if (pos.X > MainWindow.width + r)
            {
                shape.Addition(-MainWindow.width - r, 0);
            }
            else if (pos.X < -r)
            {
                shape.Addition(MainWindow.width + r, 0);
            }

            if (pos.Y > MainWindow.height + r)
            {
                shape.Addition(0, -MainWindow.height - r);
            }
            else if (pos.Y < -r)
            {
                shape.Addition(0, MainWindow.height + r);
            }
        }

        public void Rotate(double a) => shape.RotateAroundCenter(a);

        public void Shot()
        {
            var x = shape.GetCentralPoint().X;
            var y = shape.GetCentralPoint().Y;
            var dir = shape.GetDirection();

            lasers.Add(new Laser(x, y, dir));
        }
    }
}
