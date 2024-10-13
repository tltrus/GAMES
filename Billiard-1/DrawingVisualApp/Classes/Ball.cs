using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DrawingVisualApp
{
    class Ball
    {
        public Vector2D position, velocity, acceleration;
        double r = 16;
        Brush brush;
        double mass = 6;
        bool stopped;

        public Ball() 
        {
        }


        public Ball(double x, double y, Brush brush)
        {
            position = new Vector2D(x, y);
            acceleration = new Vector2D(0, 0);
            velocity = new Vector2D(0, 0);
            this.brush = brush;
        }

        public bool IsStopped()
        {
            return stopped;
        }


        public void ApplyForce(Vector2D force)
        {
            var f = force.CopyToVector();
            f.Div(mass);
            acceleration.Add(f);
        }
        public void Update()
        {
            velocity.Add(acceleration);
            position.Add(velocity);
            acceleration.Mult(0);
            velocity.Mult(0.97);

            if (Math.Abs(velocity.X) < 0.1 && Math.Abs(velocity.Y) < 0.1) 
                stopped = true;
            else 
                stopped = false;
        }
        public void Collide(Ball other)
        {
            var impactVector = Vector2D.Sub(other.position, position);
            var d = impactVector.Mag();
            if (d < r + other.r)
            {
                // Push the particles out so that they are not overlapping
                var overlap = d - (r + other.r);
                var dir = impactVector.CopyToVector();
                dir.SetMag(overlap * 0.5);
                position.Add(dir);
                other.position.Sub(dir);

                // Correct the distance!
                d = r + other.r;
                impactVector.SetMag(d);

                var mSum = mass + other.mass;
                var vDiff = Vector2D.Sub(other.velocity, velocity);
                // Particle A (this)
                var num = vDiff.Dot(impactVector);
                var den = mSum * d * d;
                var deltaVA = impactVector.CopyToVector();
                deltaVA.Mult((2 * other.mass * num) / den);
                velocity.Add(deltaVA);
                // Particle B (other)
                var deltaVB = impactVector.CopyToVector();
                deltaVB.Mult((-2 * mass * num) / den);
                other.velocity.Add(deltaVB);
            }
        }
        public void Edges(double t_width_start, double t_height_start, double t_width_end, double t_height_end)
        {
            CheckTopEdges(t_width_start, t_height_start, t_width_end, t_height_start); // top left
            CheckBottomEdges(t_width_start, t_height_end, t_width_end, t_height_end); // Bottom left
            CheckLeftEdge(t_width_start, t_height_start, t_width_start, t_height_end); // left
            CheckRightEdge(t_width_end, t_height_start, t_width_end, t_height_end); // right
        }
        private void CheckTopEdges(double x1, double y1, double x2, double y2)
        {
            var x = position.X; 
            var y = position.Y;

            if (x > x1 && x < x2 && y < y1 + r)
            {
                position.Y = y1 + r;
                velocity.Y *= -1;
            }
        }
        private void CheckBottomEdges(double x1, double y1, double x2, double y2)
        {
            var x = position.X;
            var y = position.Y;

            if (x > x1 && x < x2 && y > y1 - r)
            {
                position.Y = y1 - r;
                velocity.Y *= -1;
            }
        }
        private void CheckLeftEdge(double x1, double y1, double x2, double y2)
        {
            var x = position.X;
            var y = position.Y;

            if (x < x1 + r && y > y1 && y < y2)
            {
                position.X = x1 + r;
                velocity.X *= -1;
            }
        }
        private void CheckRightEdge(double x1, double y1, double x2, double y2)
        {
            var x = position.X;
            var y = position.Y;

            if (x > x2 - r && y > y1 && y < y2)
            {
                position.X = x2 - r;
                velocity.X *= -1;
            }
        }

        public void Draw(DrawingContext dc)
        {
            // Circle
            dc.DrawEllipse(brush, null, new Point(position.X, position.Y), r, r);
        }

        public void DrawVector(DrawingContext dc)
        {
            // Vector of direction
            var v = velocity.CopyToVector();
            v.SetMag(r);
            var pv = position + v;
            Point p0 = new Point(position.X, position.Y);
            Point p1 = new Point(pv.X, pv.Y);
            dc.DrawLine(new Pen(Brushes.LightGray, 1), p0, p1);
        }
    }
}
