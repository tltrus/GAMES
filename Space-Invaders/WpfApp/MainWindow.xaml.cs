using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Based on Coding Challenge #5 Space invaders

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;
        DrawingVisual visual;
        DrawingContext dc;

        Ship Ship;
        List<Missile> drops = new List<Missile>();
        List<Enemy> enemies = new List<Enemy>();
        bool edge;
        Water Water;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            Water = new Water(rnd, width, height);

            Ship = new Ship();
            enemies.Add(new Enemy(width / 2, 30));
            enemies.Add(new Enemy(width / 3, 30));
            enemies.Add(new Enemy(width / 4, 30));
            enemies.Add(new Enemy(width / 5, 30));

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Draw();

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
            {
                Ship.SetDir(0);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                Missile drop = new Missile(Ship.GetLanchXPos(), height);
                drops.Add(drop);
            }
            
            if (e.Key == Key.Left)
            {
                Ship.SetDir(-1);
                Water.Splash(Ship.x, 10);
            }
            else if (e.Key == Key.Right)
            {
                Ship.SetDir(1);
                Water.Splash(Ship.x, 10);
            }
        }

        private void Draw()
        {
            Ship.Move();
            
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {

                foreach(var drop in drops.ToList())
                {
                    if (drop.toDelete) drops.RemoveAt(drops.IndexOf(drop));

                    drop.Show(dc);
                    drop.Move();

                    if (drop.y < 0) drop.Evaporate();

                    for (var j = 0; j < enemies.Count; j++)
                    {
                        if (drop.Hits(enemies[j]))
                        {
                            enemies[j].BlowOff();
                            drop.Evaporate();
                        }
                    }
                }

                edge = false;

                foreach (var enemy in enemies.ToList())
                {
                    enemy.Show(dc);
                    enemy.Move();

                    if (enemy.x > width || enemy.x < 0)
                        edge = true;
                    if (enemy.r < 20)
                        enemies.RemoveAt(enemies.IndexOf(enemy));
                }

                if (edge)
                {
                    enemies.ForEach(a => a.ShiftDown());
                }

                // Ship
                Ship.Draw(dc);

                // Water
                Water.Update();
                Water.Draw(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
