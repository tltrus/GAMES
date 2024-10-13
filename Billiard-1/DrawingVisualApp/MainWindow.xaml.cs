using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawingVisualApp
{
    // Based on #184 — Elastic Collisions https://thecodingtrain.com/challenges/184-elastic-collisions
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height, t_width_start, t_width_end, t_height_start, t_height_end;

        DrawingVisual visual;
        DrawingContext dc;
        List<Ball> balls = new List<Ball>();
        MainBall mainBall;
        List<Boundary> walls = new List<Boundary>();
        ProgressBar progressBar = new ProgressBar();
         

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            Init();
        }

        private void Init()
        {
            // Walls around field for Ray
            t_width_start = 50;
            t_width_end = width - 50;
            t_height_start = 60;
            t_height_end = height - 60;

            walls.Add(new Boundary(t_width_start, t_height_start, t_width_end, t_height_start)); // top
            walls.Add(new Boundary(t_width_start, t_height_end, t_width_end, t_height_end)); // bottom
            walls.Add(new Boundary(t_width_start, t_height_start, t_width_start, t_height_end));
            walls.Add(new Boundary(t_width_end, t_height_start, t_width_end, t_height_end));

            // main ball
            mainBall = new MainBall(width * 3 / 4, height / 2, Brushes.White);

            for (int i = 0; i < 5; ++i)
            {
                var x = rnd.Next(width);
                var y = rnd.Next(height);
                var brush = Brushes.Yellow;
                var ball = new Ball(x, y, brush);

                balls.Add(ball);
            }
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach (var wall in walls)
                {
                    wall.Show(dc);
                }

                foreach (var ball in balls)
                {
                    ball.Update();
                    ball.Edges(t_width_start, t_height_start, t_width_end, t_height_end);
                    ball.Draw(dc);

                    ball.Collide(mainBall);

                    foreach (var other in balls)
                    {
                        if (ball != other)
                        {
                            ball.Collide(other);
                        }
                    }

                    
                }

                mainBall.Update();
                mainBall.Edges(t_width_start, t_height_start, t_width_end, t_height_end);
                mainBall.Draw(dc);

                if (mainBall.IsStopped())
                    mainBall.Look(walls, dc);
                
                foreach (var b in balls)
                {
                    b.Draw(dc);
                }

                // progressbar
                if (mainBall.IsStopped())
                {
                    progressBar.Update();
                    progressBar.Draw(dc);
                }


                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.GetPosition(g).X;
            var y = e.GetPosition(g).Y;
            mainBall.SetMousePosition(x, y);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!mainBall.IsStopped()) return;

            var x = e.GetPosition(g).X;
            var y = e.GetPosition(g).Y;

            progressBar.SetPosition(x, y);
            progressBar.SetVisible(true);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!mainBall.IsStopped()) return;

            var f = progressBar.GetValue();
            var push = mainBall.GetRayDirection();

            push.SetMag(f);
            mainBall.SetPushVelocity(push);

            progressBar.SetVisible(false);
            progressBar.SetValue(0);
        }

    }
}
