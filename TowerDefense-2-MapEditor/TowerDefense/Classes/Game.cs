using System.Windows;
using System.Windows.Media;

namespace DefenseGame
{
    class Game
    {
        public List<Enemy> enemies = new List<Enemy>();
        List<Tower> towers = new List<Tower>();
        DrawingContext dc;
        Path path;

        public Game(int width, int height, DrawingContext dc, List<Point> towers, List<Point> roads)
        {
            this.dc = dc;
            foreach (var tower in towers)
            {
                this.towers.Add(new Tower(tower.X, tower.Y, width, height));
            }

            path = new Path();
            foreach (var road in roads)
            {
                path.AddPoint(road.X, road.Y);
            }
        }

        public void AddEnemy(Vector2D mouse)
        {
            var maxspeed = MainWindow.rnd.Next(1, 3);
            var maxforce = MainWindow.rnd.NextDouble();
            enemies.Add(new Enemy(mouse, maxspeed, maxforce));
        }

        public void TowerUpdate()
        {
            foreach (var tower in towers)
            {
                tower.BulletShot(enemies);
                tower.BulletControl(enemies);
            }
        }

        public void Render(DrawingVisualClass g)
        {
            g.RemoveVisual(MainWindow.visual);

            using (dc = MainWindow.visual.RenderOpen())
            {
                path.Draw(dc);

                foreach (var enemy in enemies)
                {
                    enemy.Follow(path);
                    enemy.Separate(enemies);
                    enemy.Update();
                    enemy.Draw(dc);
                }

                foreach (var tower in towers)
                {
                    tower.Draw(dc);
                }

                dc.Close();
                g.AddVisual(MainWindow.visual);
            }
        }
    }
}
