using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TD_multi
{
    class TowerDefense
    {
        Random rnd = new Random();
        double width, height;
        List<Enemy> enemies;
        List<Tower> towers;
        DateTime beginTime;


        public TowerDefense(double width, double height)
        {
            enemies = new List<Enemy>();
            towers = new List<Tower>();

            beginTime = DateTime.Now;
            this.width = width; 
            this.height = height;

            // towers
            towers.Add(new Tower(new Point(width * 0.50, height - 30), 1));
            towers.Add(new Tower(new Point(width * 0.25, height - 30)));
            towers.Add(new Tower(new Point(width * 0.75, height - 30)));

            GenerateEnemy();
        }
       
        public void Update(System.Windows.Threading.DispatcherTimer timer)
        {
            // Рандомная генерация врагов через интервал времени, рассчитанный как разница начала и текущего времени
            TimeSpan interval = DateTime.Now - beginTime;
            if (interval > TimeSpan.FromSeconds(1))
            {
                GenerateEnemy();
                beginTime = DateTime.Now;
            }

            foreach (var enemy in enemies.ToList())
            {
                var enemyPos_ = new Point(
                                    enemy.pos.X + enemy.direction.X,
                                    enemy.pos.Y + enemy.direction.Y
                                );
                enemy.pos = enemyPos_;
                enemy.direction.X *= -1;

                if ((enemy.pos.Y) > height)
                {
                    enemies.RemoveAt(enemies.IndexOf(enemy));
                    timer.Stop();
                }
            }
        }

        public void BulletsUpdate(System.Windows.Threading.DispatcherTimer timer)
        {
            foreach(var tower in towers)
            {
                Point tartgetpos = CatchTarget(tower);
                if (tartgetpos.X == 0 && tartgetpos.Y == 0) continue;

                if (tower.bullets.Count == 0) // Нужно для формирования пролета одной пули
                {
                    // вычисление угла наклона пушки башни
                    double y = (height - tower.imgHeight) - tartgetpos.Y; // отнимаем towersize - высота башни
                    double x = tower.pos.X - tartgetpos.X;
                    double angleRad = Math.Atan2(y, x);

                    // рассчитать конец пушки
                    double x1 = tower.pos.X - Math.Cos(angleRad) * 1;
                    double y1 = (height - tower.imgHeight*0.5) - Math.Sin(angleRad) * 1;

                    Bullet item = new Bullet(new Point(x1, y1), angleRad);
                    tower.bullets.Add(item);
                }

                // Проверка на попадание пуль
                foreach (var bullet in tower.bullets.ToList())
                {
                    // Удаление пули при вылете за границы экрана
                    if (bullet.pos.X < 0 || bullet.pos.X > width || bullet.pos.Y < 0 || bullet.pos.Y > height)
                    {
                        tower.bullets.Remove(bullet);
                        continue;
                    }

                    // Удаление пули и врага после попадания
                    foreach (var enemy in enemies.ToList())
                    {
                        if (bullet.pos.X >= enemy.pos.X - enemy.imgSize && bullet.pos.X <= enemy.pos.X + enemy.imgSize
                            && bullet.pos.Y >= enemy.pos.Y - enemy.imgSize && bullet.pos.Y <= enemy.pos.Y + enemy.imgSize)
                        {
                            enemy.helth -= bullet.power;
                            tower.bullets.Remove(bullet);
                        }

                        if (enemy.helth <= 0)
                        {
                            tower.bullets.Remove(bullet);
                            enemies.Remove(enemy);
                            continue;
                        }
                    }

                    // рассчитать кончик пушки
                    double x1 = bullet.pos.X - Math.Cos(bullet.angle) * 10;
                    double y1 = bullet.pos.Y - Math.Sin(bullet.angle) * 10;

                    Point newBulletpos = new Point(x1, y1);
                    bullet.pos = newBulletpos;
                }
            }
        }

        // Захват цели
        private Point CatchTarget(Tower tower)
        {
            Point pos = new Point();

            foreach (var enemy in enemies)
            {
                double x = enemy.pos.X - tower.pos.X;
                double y = enemy.pos.Y - tower.pos.Y;
                int dist = (int)Math.Sqrt(x * x + y * y); // Расстояние от врага до цели

                if (dist <= tower.rsize) 
                    pos = enemy.pos; // Если расстояние от врага до цели меньше радиуса радара, то захватывается цель
            }
            return pos;
        }

        // Создание врагов
        private void GenerateEnemy()
        {
            int offset = 40;
            double x = rnd.Next(offset, (int)width - offset);
            var pos = new Point(x, 5);

            enemies.Add(new Enemy(pos));
            
        }

        public void Render(DrawingContext dc)
        {
            foreach (var t in towers)
                t.Draw(dc);

            foreach (var e in enemies)
                e.Draw(dc);
        }
    }
}
