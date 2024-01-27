using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Snake_Wave
{
    internal class Game
    {
        Map map;
        Snake snake;
        Canvas g;
        public static System.Windows.Threading.DispatcherTimer timer;
        int cellsize, imgWidth, imgHeight;

        public Game(Canvas g, int cellsize, int imgWidth, int imgHeight) 
        {
            this.g = g;
            map = new Map(cellsize, imgWidth, imgHeight);
            snake = new Snake(cellsize);
            map.AddSnake(snake);

            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
            this.cellsize = cellsize;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(Control);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

        }

        public void Control(object sender, System.EventArgs e)
        {
            g.Children.Clear();
            map.Drawing(g);
            snake.Drawing(g);

            snake.Update(imgWidth, imgHeight);
            map.CheckEating();

            int X = (int)snake.body[0].X / cellsize;
            int Y = (int)snake.body[0].Y / cellsize;
            snake.direction = map.ChangeSnakeDirection(X, Y);
        }

        public void Start()
        {
            timer.Start();
        }
    }
}
