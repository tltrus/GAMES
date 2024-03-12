using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


// Based on: Coding Challenge #46 Asteroids

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static double width, height;
        DrawingVisual visual;
        DrawingContext dc;

        List<Asteroid> asteroids;
        Ship ship;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = g.Width;
            height = g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Setup();

            timer.Start();
        }

        private void Setup()
        {
            ship = new Ship();
            asteroids = new List<Asteroid>();

            for (int i = 0; i < 5; ++i)
                asteroids.Add(new Asteroid(30));
        }

        private void timerTick(object sender, EventArgs e) => Draw();

        private void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach (var a in asteroids)
                {
                    a.Draw(dc);
                    a.Update();
                    a.Edges();
                }

                ship.Update();
                ship.Edges();
                ship.Draw(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ship.Boosting(false);
            ship.Braking(false);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ship.Shot();
            }

            if (e.Key == Key.Left)
            {
                ship.Rotate(-10);
            } else
            if (e.Key == Key.Right)
            {
                ship.Rotate(10);
            } else 
            if (e.Key == Key.Up)
            {
                ship.Boosting(true);
            } else 
            if (e.Key == Key.Down)
            {
                ship.Braking(true);
            }
        }
    }
}
