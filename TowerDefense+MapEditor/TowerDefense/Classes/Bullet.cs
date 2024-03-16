using System.Windows.Media;
using System.Windows;

namespace DefenseGame.Classes
{
    class Bullet : BaseClass
    {
        public double angle;
        public int power;


        public Bullet(double x, double y, double angle)
        {
            pos = new Vector2D(x, y);
            this.angle = angle;

            style = new Style()
            {
                brush = null,
                pen = new Pen(Brushes.White, 1),
                size = new Size(1, 1)
            };

            power = 3;
        }



        public void Update()
        {
            double x1 = pos.x - Math.Cos(angle) * 6;
            double y1 = pos.y - Math.Sin(angle) * 6;

            pos = new Vector2D(x1, y1);
        }

        public bool isOutOfScreen(int gwidth, int gheight)
        {
            // bullet is out of screen
            if (pos.x < 0 || pos.x > gwidth || pos.y < 0 || pos.y > gheight)
                return true;
            else
                return false;
        }

        public bool Hit(Enemy Enemy)
        {
            if (pos.x >= Enemy.pos.x - Enemy.style.size.Width && pos.x <= Enemy.pos.x + Enemy.style.size.Width
                && pos.y >= Enemy.pos.y - Enemy.style.size.Height && pos.y <= Enemy.pos.y + Enemy.style.size.Height)
            {
                return true;
            }
            return false;
        }
    }
}
