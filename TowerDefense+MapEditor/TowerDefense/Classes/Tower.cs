using System.Windows.Media;
using System.Windows;
using DefenseGame.Classes;
using System.Windows.Media.Imaging;

namespace DefenseGame
{
    class Tower : BaseClass
    {
        private int width, height;
        public int radarR = 150;    // Radar radius
        public List<Bullet> bullets = new List<Bullet> { };
        BitmapImage imgTower;

        public Tower(double x, double y, int width, int height)
        {
            pos = new Vector2D(x, y);

            this.width = width;
            this.height = height;

            imgTower = new BitmapImage(new Uri("pack://application:,,,/Media/imgTower.png"));
        }


        // Захват цели
        private Vector2D CatchTarget(List<Enemy> enemies)
        {
            Vector2D pos = new Vector2D(0, 0);

            foreach (var enemy in enemies)
            {
                // Расстояние от врага до цели
                double dist = Vector2D.Dist(enemy.pos, this.pos);

                // Если расстояние от врага до цели меньше радиуса радара, то захватывается цель
                if (dist <= radarR)
                    pos = enemy.pos.CopyToVector();
            }
            return pos;
        }

        public void BulletShot(List<Enemy> enemies)
        {
            Vector2D tartgetPos = CatchTarget(enemies);

            if (tartgetPos.x == 0 && tartgetPos.y == 0)
            {
                bullets.Clear();
            }
            else
            {
                if (bullets.Count == 0) // Нужно для формирования пролета одной пули
                {
                    // вычисление угла наклона пушки башни
                    double y = (pos.y - 60) - tartgetPos.y; // отнимаем towerSize - высота башни
                    double x = pos.x - tartgetPos.x;
                    double angleRad = Math.Atan2(y, x);

                    //double angleDec = angleRad * 180 / Math.PI;
                    //gunAngle = (int)angleDec;

                    // рассчитать кончик пушки
                    double x1 = pos.x - Math.Cos(angleRad) * 60;
                    double y1 = (pos.y - 60) - Math.Sin(angleRad) * 60;

                    Bullet item = new Bullet(x1, y1, angleRad);
                    bullets.Add(item);
                }
            }
        }

        public void BulletControl(List<Enemy> enemies)
        {
            foreach (var bullet in bullets.ToList())
            {
                // Удаление пули при вылете за границы экрана
                if (bullet.isOutOfScreen(width, height))
                {
                    bullets.Remove(bullet);
                    continue;
                }

                // Удаление пули и врага после попадания
                foreach (var enemy in enemies.ToList())
                {
                    if (bullet.Hit(enemy))
                    {
                        enemy.Health -= bullet.power;
                        if (enemy.style.size.Height > 2)
                        {
                            enemy.style.size.Width--;
                            enemy.style.size.Height--;
                        }
                        bullets.Remove(bullet);
                    }

                    if (enemy.Health <= 0)
                    {
                        bullets.Remove(bullet);
                        enemies.Remove(enemy);
                        continue;
                    }
                }

                bullet.Update();
            }
        }

        public void Draw(DrawingContext dc)
        {
            DrawRadar(dc);
            DrawBullet(dc);

            var rectTower = new Rect()
            {
                X = pos.x - 30,
                Y = pos.y - 30,
                Width = 60,
                Height = 60
            };

            dc.DrawImage(imgTower, rectTower);
        }

        void DrawRadar(DrawingContext dc)
        {
            var rpen = new Pen();
            rpen.Brush = Brushes.LightGray;
            rpen.Thickness = 1;
            rpen.DashStyle = DashStyles.Dash;

            dc.DrawEllipse(null, rpen, new Point(pos.x, pos.y), radarR, radarR);
        }

        public void DrawBullet(DrawingContext dc)
        {
            foreach (var bullet in bullets)
            {
                bullet.Drawing(dc);
            }
        }
    }
}
