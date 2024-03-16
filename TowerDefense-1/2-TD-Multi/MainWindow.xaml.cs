using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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


namespace TD_multi
{
    public partial class MainWindow : Window
    {
        DrawingVisual visual;
        DrawingContext dc;

        System.Windows.Threading.DispatcherTimer gameTimer;
        System.Windows.Threading.DispatcherTimer bulletTimer;
        TowerDefense game;
        double width, height;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            width = g.Width;
            height = g.Height;

            game = new TowerDefense(width, height);

            visual = new DrawingVisual();

            gameTimer = new System.Windows.Threading.DispatcherTimer();
            gameTimer.Tick += new EventHandler(GameTimer);
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            bulletTimer = new System.Windows.Threading.DispatcherTimer();
            bulletTimer.Tick += new EventHandler(BulletsTimer);
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            Drawing();
        }

        private void GameTimer(object sender, System.EventArgs e)
        {
            game.Update(gameTimer);
        }

        private void BulletsTimer(object sender, System.EventArgs e)
        {
            game.BulletsUpdate(bulletTimer);

            Drawing();
        }

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                game.Render(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
            bulletTimer.Start();
        }
    }
}
