using System.Windows.Media;
using System.Windows;
using DefenseGame.Classes;

namespace DefenseGame
{
    class Enemy : BaseClass
    {
        public Vector2D velocity;
        public Vector2D acceleration;
        Vector2D normal, normalPoint;
        Vector2D predictpos;
        Vector2D target;
        public double r = 8;
        double maxspeed, maxforce;
        Brush brush;
        public int Health;


        public Enemy(Vector2D loc, double ms, double mf)
        {
            pos = loc.CopyToVector();
            maxforce = mf;
            maxspeed = ms;

            acceleration = new Vector2D();
            velocity = new Vector2D(maxspeed, 0);
            normalPoint = new Vector2D();

            brush = Brushes.Orange;

            Health = MainWindow.rnd.Next(1, 3);

            byte R = (byte)MainWindow.rnd.Next(255);
            byte G = (byte)MainWindow.rnd.Next(255);
            byte B = (byte)MainWindow.rnd.Next(255);

            style = new Style()
            {
                brush = new SolidColorBrush(Color.FromRgb(R, G, B)),
                pen = null,
                size = new Size(r, r)
            };
        }

        // Based on Codding Challange

        public void Separate(List<Enemy> cars)
        {
            double desiredseparation = r * 2;
            Vector2D sum = new Vector2D();
            double count = 0;
            // For every boid in the system, check if it's too close
            for (int i = 0; i < cars.Count; i++)
            {
                double d = Vector2D.Dist(pos, cars[i].pos);
                // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
                if ((d > 0) && (d < desiredseparation))
                {
                    // Calculate vector pointing away from neighbor
                    Vector2D diff = Vector2D.Sub(pos, cars[i].pos);
                    diff.Normalize();
                    diff.Div(d); // Weight by distance
                    sum.Add(diff);
                    count++; // Keep track of how many
                }
            }
            // Average -- divide by how many
            if (count > 0)
            {
                sum.Div(count);
                // Our desired vector is the average scaled to maximum speed
                sum.Normalize();
                sum.Mult(maxspeed);
                // Implement Reynolds: Steering = Desired - Velocity
                Vector2D steer = Vector2D.Sub(sum, velocity);
                steer.Limit(maxforce);
                ApplyForce(steer);
            }
        }

        public void Follow(Path p)
        {
            // Predict location 50
            Vector2D predict = velocity.CopyToVector();
            predict.Normalize();
            predict.Mult(25);
            predictpos = Vector2D.Add(pos, predict);

            normal = new Vector2D();
            target = new Vector2D();
            double worldRecord = 1000000;

            // Look at the line segment
            for (int i = 0; i < p.points.Count; i++)
            {
                Vector2D a = new Vector2D(p.points[i].X, p.points[i].Y);
                Vector2D b = new Vector2D(p.points[(i + 1) % p.points.Count].X, p.points[(i + 1) % p.points.Count].Y);

                // Get the normal point to that line
                normalPoint = GetNormalPoint(predictpos, a, b);

                // Check if normal is on line segment
                var dir = Vector2D.Sub(b, a);
                // If it's not within the line segment, consider the normal to just be the end of the line segment (point b)
                //if (da + db > line.mag()+1) {
                if (normalPoint.x < Math.Min(a.x, b.x) || normalPoint.x > Math.Max(a.x, b.x) || normalPoint.y < Math.Min(a.y, b.y) || normalPoint.y > Math.Max(a.y, b.y))
                {
                    normalPoint = b.CopyToVector();
                    // If we're at the end we really want the next line segment for looking ahead
                    a = new Vector2D(p.points[(i + 1) % p.points.Count].X, p.points[(i + 1) % p.points.Count].Y);
                    b = new Vector2D(p.points[(i + 2) % p.points.Count].X, p.points[(i + 2) % p.points.Count].Y); // Path wraps around
                    dir = Vector2D.Sub(b, a);
                }

                // How far away are we from the path?
                double d = Vector2D.Dist(predictpos, normalPoint);
                // Only if the distance is greater than the path's radius do we bother to steer
                if (d < worldRecord)
                {
                    worldRecord = d;
                    normal = normalPoint.CopyToVector();

                    // Find target point a little further ahead of normal
                    //dir = Vector2D.Sub(b, a);
                    dir.Normalize();
                    dir.Mult(25);  // This could be based on velocity instead of just an arbitrary 10 pixels
                    target = normalPoint.CopyToVector();
                    target.Add(dir);
                }
            }

            // Only if the distance is greater than the path's radius do we bother to steer
            if (worldRecord > p.radius)
            {
                Seek(target);
            }
        }

        // A function to get the normal point from a point (p) to a line segment (a-b)
        // This function could be optimized to make fewer new Vector objects
        Vector2D GetNormalPoint(Vector2D p, Vector2D a, Vector2D b)
        {
            // Vector from a to p (predictLoc)
            Vector2D ap = Vector2D.Sub(p, a);
            // Vector from a to b
            Vector2D ab = Vector2D.Sub(b, a);

            // Скалярное произведение дает проекцию вектора а на вектор b
            // Вектор b сначала нужно нормализовать, чтобы получить корректные значения
            ab.Normalize();     // Normalize the line
            ab.Mult(ap.Dot(ab));

            Vector2D normalPoint = Vector2D.Add(a, ab);
            return normalPoint.CopyToVector();
        }

        // A method that calculates and applies a steering force towards a target
        // STEER = DESIRED MINUS VELOCITY
        public void Seek(Vector2D target)
        {
            // Вектор до цели
            Vector2D desired = Vector2D.Sub(target, pos);

            // If the magnitude of desired equals 0, skip out of here
            // (We could optimize this to check if x and y are 0 to avoid mag() square root
            if (desired.Mag() == 0) return;

            desired.Normalize();
            desired.Mult(maxspeed);

            // Вектор руления
            Vector2D steer = Vector2D.Sub(desired, velocity);
            steer.Limit(maxforce);

            ApplyForce(steer);
        }

        public void ApplyForce(Vector2D force) => acceleration.Add(force);

        public void Update()
        {
            velocity.Add(acceleration);
            velocity.Limit(maxspeed);
            pos.Add(velocity);
            acceleration.Mult(0);
        }

        public void Draw(DrawingContext dc)
        {
            Point p = new Point();

            p.X = pos.x;
            p.Y = pos.y;
            dc.DrawEllipse(brush, null, p, style.size.Width, style.size.Height);
        }
    }
}
