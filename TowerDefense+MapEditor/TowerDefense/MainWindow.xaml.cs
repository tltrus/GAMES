using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DefenseGame.Classes;

namespace DefenseGame
{
    public partial class MainWindow : Window
    {
        public static Random rnd = new Random();
        public static DrawingVisual visual;
        DrawingContext dc;
        DispatcherTimer ctrlTimer;
        DispatcherTimer drawTimer;
        Game Game;
        int imgWidth, imgHeight;

        public MainWindow()
        {
            InitializeComponent();

            imgWidth = (int)g.Width; imgHeight = (int)g.Height;
            visual = new DrawingVisual();

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "//MapEditor//bin//Debug//net8.0-windows";

            var filetowers = new List<Point>();
            Files.FileRead(path + "/towers.txt", filetowers);
            var fileway = new List<Point>();
            Files.FileRead(path + "/road.txt", fileway);
            Game = new Game(imgWidth, imgHeight, dc, filetowers, fileway);

            // Create a timer for the GameLoop method
            ctrlTimer = new DispatcherTimer();
            ctrlTimer.Tick += new EventHandler(MainGameLoop);
            ctrlTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);

            // Create a timer for the Draw
            drawTimer = new DispatcherTimer();
            drawTimer.Tick += new EventHandler(Draw);
            drawTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Game.Render(g);

            ctrlTimer.Start();
            drawTimer.Start();
        }

        private void MainGameLoop(object sender, System.EventArgs e) => Game.TowerUpdate();
        private void Draw(object sender, System.EventArgs e) => Game.Render(g);

        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mouse = new Vector2D();
            mouse.x = e.GetPosition(g).X;
            mouse.y = e.GetPosition(g).Y;

            Game.AddEnemy(mouse);
        }
    }
}