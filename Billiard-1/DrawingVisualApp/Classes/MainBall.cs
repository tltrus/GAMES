using System.Collections.Generic;
using System.Windows.Media;

namespace DrawingVisualApp
{
    internal class MainBall : Ball
    {
        Vector2D mouse = new Vector2D(0, 0);
        Ray ray;
        
        public MainBall(double x, double y, Brush brush) : base(x, y, brush)
        {
             
        }

        public Vector2D GetRayDirection() => ray.dir_vector;

        public void SetPushVelocity(Vector2D push) => velocity.Add(push); 

        public void SetMousePosition(double x, double y)
        {
            mouse.X = x; mouse.Y = y;
        }

        public void Look(List<Boundary> walls, DrawingContext dc)
        {
            // FIRST LINE
            var disp = mouse - position;
            double angle = disp.HeadingRad();
            ray = new Ray(position, angle);

            Vector2D closest = null;
            var record = double.MaxValue;
            int wall_id = 0;
            foreach (var wall in walls)
            {
                var pt = ray.Cast(wall);
                if (pt is null)
                {

                }
                    else
                {
                    var d = Vector2D.Dist(position, pt);
                    if (d < record)
                    {
                        record = d;
                        closest = pt;
                        wall_id = walls.IndexOf(wall);
                    }
                }
            }

            if (!closest.Equals(null))
            {
                // draw first line
                ray.Draw(dc, closest, 0.5);

                // SECOND LINE
                var mirror_line = Vector2D.FromAngle(-angle);
                mirror_line.SetMag(100);
                var mirror_pos = new Vector2D();

                // trick of mirror for horizontal and vertical walls
                if (wall_id == 2 || wall_id == 3)
                    mirror_pos = closest - mirror_line;
                else
                    mirror_pos = closest + mirror_line;


                disp =  mirror_pos - closest;
                angle = disp.HeadingRad();

                var ray2 = new Ray(closest, angle);

                Vector2D closest2 = null;
                var record2 = double.MaxValue;
                foreach (var wall in walls)
                {
                    var pt2 = ray2.Cast(wall);
                    if (pt2 is null)
                    {

                    }
                    else
                    {
                        var d2 = Vector2D.Dist(closest, pt2);
                        if (d2 < record2)
                        {
                            record2 = d2;
                            closest2 = pt2;
                        }
                    }
                }

                // draw second line
                if (!closest2.Equals(null))
                {
                    ray.Draw(dc, closest, closest2, 0.5);
                }
            }
        }
    }
}
