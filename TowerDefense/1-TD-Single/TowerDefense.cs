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

namespace TD
{
    class TowerDefense
    {
        Random rnd = new Random();
        double width, height;
        int enemyStep = 1; // step
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
            towers.Add(new Tower(new Point(width * 0.50, height - 20), 1));

            GenerateEnemy();
        }

        public void Update(System.Windows.Threading.DispatcherTimer timer)
        {
            
            // Рандомная генерация врагов через интервал времени, рассчитанный как разница начала и текущего времени
            TimeSpan interval = DateTime.Now - beginTime;
            if (interval > TimeSpan.FromSeconds(0.2))
            {
                GenerateEnemy();
                beginTime = DateTime.Now;
            }

            foreach (var enemy in enemies.ToList())
            {
                Point newEnemyPos = new Point(
                                            enemy.pos.X + enemy.direction.X,
                                            enemy.pos.Y + enemy.direction.Y
                                        );
                enemy.pos = newEnemyPos;

                // Враг дошел до цели - главной башни
                if (enemy.pos.X == towers[0].pos.X && enemy.pos.Y == towers[0].pos.Y)
                {
                    enemies.RemoveAt(enemies.IndexOf(enemy));
                    continue;
                }

                // Check collision with the walls
                if ((enemy.pos.X < 0) || (enemy.pos.X + enemy.size) > width || (enemy.pos.Y < 0) || (enemy.pos.Y + enemy.size) > height)
                {
                    timer.Stop();
                }

                // После столкновения перерисовываем квадрат у стенки, чтобы он не уходил за пределы поля
                if (enemy.pos.X < 0) enemy.pos = new Point(0, enemy.pos.Y);
                else if ((enemy.pos.X + enemy.size) > width) enemy.pos = new Point(enemy.pos.X - enemy.size, enemy.pos.Y);
                else if (enemy.pos.Y < 0) enemy.pos = new Point(enemy.pos.X, 0);
                else if ((enemy.pos.Y + enemy.size) > height) enemy.pos = new Point(enemy.pos.X, enemy.pos.Y - enemy.size);

                CalculateWayToTarget(enemy);  // Рассчитываем направление врага
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
                    double y = (height - tower.size) - tartgetpos.Y; // отнимаем towersize - высота башни
                    double x = tower.pos.X - tartgetpos.X;
                    double angleRad = Math.Atan2(y, x);

                    // рассчитать конец пушки
                    double x1 = tower.pos.X - Math.Cos(angleRad) * tower.size;
                    double y1 = (height - tower.size) - Math.Sin(angleRad) * tower.size;

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
                        if (bullet.pos.X >= enemy.pos.X - enemy.size && bullet.pos.X <= enemy.pos.X + enemy.size
                            && bullet.pos.Y >= enemy.pos.Y - enemy.size && bullet.pos.Y <= enemy.pos.Y + enemy.size)
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

                    // рассчитать конец пушки
                    double x1 = bullet.pos.X - Math.Cos(bullet.angle) * 10;
                    double y1 = bullet.pos.Y - Math.Sin(bullet.angle) * 10;

                    Point newBulletpos = new Point(x1, y1);
                    bullet.pos = newBulletpos;
                }
            }
        }

        // Расчет маршрута врага до цели
        private void CalculateWayToTarget(Enemy enemy)
        {
            double x = towers[0].pos.X - enemy.pos.X;
            double y = towers[0].pos.Y - enemy.pos.Y;
            int L = (int)Math.Sqrt(x * x + y * y); // Расстояние от врага до цели

            double dirX = x / L;
            double dirY = y / L;

            enemy.direction = new Point(dirX, dirY);
        }

        // Захват цели
        private Point CatchTarget(Tower tower)
        {
            Point pos = new Point(0, 0);

            foreach (var enemy in enemies)
            {
                double x = enemy.pos.X - tower.pos.X;
                double y = enemy.pos.Y - tower.pos.Y;
                int L = (int)Math.Sqrt(x * x + y * y); // Расстояние от врага до цели

                if (L <= tower.rsize) pos = enemy.pos; // Если расстояние от врага до цели меньше радиуса радара, то захватывается цель
            }
            return pos;
        }

        ///////////////////////////////////////////////////////////
 
        // Создание врагов
        private void GenerateEnemy()
        {
            double x = rnd.Next(5, (int)width - 5);
            enemies.Add(new Enemy(new Point(x, 5)));
        }

        ///////////////////////////////////////////////////////////

        public void Render(DrawingContext dc)
        {
            DrawRadar(dc);
            DrawEnemy(dc);
            DrawTower(dc);
            DrawBullet(dc);
        }
        private void DrawRadar(DrawingContext dc)
        {
            foreach (var t in towers)
            {
                Point p = new Point(t.pos.X + t.size * 0.5, t.pos.Y - t.size);
                dc.DrawEllipse(null, new Pen(t.rcolor, 1), p, t.rsize, t.rsize);
            }
        }
        private void DrawEnemy(DrawingContext dc)
        {
            foreach (var e in enemies)
            {
                dc.DrawEllipse(e.color, null, e.pos, e.size, e.size);
            }
        }
        private void DrawTower(DrawingContext dc)
        {
            foreach (var t in towers)
            {
                Rect rect = new Rect()
                {
                    X = t.pos.X,
                    Y = t.pos.Y,
                    Width = t.size,
                    Height = t.size
                };

                dc.DrawRectangle(t.tcolor, null, rect);
            }
        }
        public void DrawBullet(DrawingContext dc)
        {
            foreach (var t in towers)
            {
                foreach (var b in t.bullets)
                {
                    dc.DrawEllipse(b.color, null, b.pos, b.size, b.size);
                }
            }
        }
    }
}
