using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MapEditor
{
    public partial class MainWindow : Window
    {
        DrawingVisual visual;
        DrawingContext dc;
        int width, height;
        public static Path path;
        public static List<Point> towers;
        enum Mode
        {
            ROAD,
            TOWER
        }
        Mode mode;

        BitmapImage imgTower;
        Rect rectTower;

        public MainWindow()
        {
            InitializeComponent();

            width = (int)g.Width; height = (int)g.Height;
            visual = new DrawingVisual();

            towers = new List<Point>();
            path = new Path();

            mode = Mode.ROAD;
            lbMode.Content = "Drawing a road";

            imgTower = new BitmapImage(new Uri("pack://application:,,,/Media/imgTower.png"));

            Draw();
        }

        public void Draw()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                // Road
                path.Draw(dc);

                // Towers
                foreach (var tower in towers)
                {
                    rectTower = new Rect()
                    {
                        X = tower.X - 30,
                        Y = tower.Y - 30,
                        Width = 60,
                        Height = 60
                    };

                    dc.DrawImage(imgTower, rectTower);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        void CreateRoad(MouseButtonEventArgs e)
        {
            if (mode == Mode.ROAD)
            {
                var x = e.GetPosition(g).X;
                var y = e.GetPosition(g).Y;

                if (e.ChangedButton == MouseButton.Left)
                    path.AddPoint(x, y);

                if (e.ChangedButton == MouseButton.Right)
                {
                    if (path.points.Count > 0)
                        path.points.RemoveAt(path.points.IndexOf(path.points.Last())); // удаление последнего элемента
                }

                Draw();
            }
        }
        void CreateTower(MouseButtonEventArgs e)
        {
            if (mode == Mode.TOWER)
            {
                if (e.ChangedButton == MouseButton.Left)
                    towers.Add(e.GetPosition(g));

                if (e.ChangedButton == MouseButton.Right)
                {
                    if (towers.Count > 0)
                        towers.RemoveAt(towers.IndexOf(towers.Last())); // remove last element
                }

                Draw();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Files.SaveToFile("road.txt", path.points.ToList<Point>());
            Files.SaveToFile("towers.txt", towers);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            List<Point> points = new List<Point>();
            Files.FileRead("road.txt", points);
            path.points = new PointCollection(points);

            Files.FileRead("towers.txt", towers);
            Draw();
        }

        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CreateRoad(e);
            CreateTower(e);
        }

        private void btnDrawWay_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.ROAD;
            lbMode.Content = "Drawing a road";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            towers.Clear();
            path.points.Clear();
            Draw();
        }



        private void btnDrawTower_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.TOWER;
            lbMode.Content = "Drawing a towers";

        }
    }
}