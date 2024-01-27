using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake_Wave
{
    internal class Map
    {
        Random rnd = new Random();
        
        public static int[,] map;           
        private int cellSize;               
        private int imgWidth, imgHeight;    
        private int mapWidth, mapHeight;               
           

        Point food;
        Snake snake;

        public Map(int cellSize, int imgWidth, int imgHeight)
        {
            this.cellSize = cellSize;
            
            this.imgWidth = imgWidth; 
            this.imgHeight = imgHeight;

            // cell num
            mapWidth = imgWidth / cellSize;
            mapHeight = imgHeight / cellSize;

            food = new Point();

            MapInit();
            GenerateFood();
            GenerateCellDist();
        }

        public void AddSnake(Snake snake)
        {
            this.snake = snake;
        }

        public void MapInit()
        {
            map = new int[mapHeight, mapWidth];
            for (int y = 0; y < mapHeight; y++)
                for (int x = 0; x < mapWidth; x++)
                    map[y, x] = -1;
        }

        public void CheckEating()
        {
            // Check collision with the food
            if (snake.body[0].X == food.X &&
                snake.body[0].Y == food.Y)
            {
                // Add new element in the snake
                snake.body.Add(new Point(food.X, food.Y));

                GenerateFood();
                MapInit();
                GenerateCellDist();
            }
        }

        // Generate an initial random position for the food
        private void GenerateFood()
        {
            food.X = 20 * rnd.Next(0, mapWidth - 1);
            food.Y = 20 * rnd.Next(0, mapHeight - 1);
        }

        private void GenerateCellDist()
        {
            int totalCells = mapWidth * mapHeight - 1;
            int step = 0;
            int xm1, xp1, ym1, yp1;

            int X = (int)food.X / cellSize;
            int Y = (int)food.Y / cellSize;

            map[Y, X] = 0; // 

            while (totalCells > 0)
            {
                for (int y = 0; y < mapHeight; y++)
                    for (int x = 0; x < mapWidth; x++)
                    {
                        if (map[y, x] == step)
                        {
                            xm1 = (x - 1 < 0) ? 0 : x - 1;
                            xp1 = (x + 1 > mapWidth - 1) ? mapWidth - 1 : x + 1;
                            ym1 = (y - 1 < 0) ? 0 : y - 1;
                            yp1 = (y + 1 > mapHeight - 1) ? mapHeight - 1 : y + 1;

                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (map[ym1, x] < 0) map[ym1, x] = step + 1;
                            if (map[yp1, x] < 0) map[yp1, x] = step + 1;
                            if (map[y, xm1] < 0) map[y, xm1] = step + 1;
                            if (map[y, xp1] < 0) map[y, xp1] = step + 1;
                        }
                    }
                step++;
                totalCells -= 1; // Каждый раз уменьшаем на единицу
            }
        }

        public Point ChangeSnakeDirection(int x, int y)
        {
            List<int> p = new List<int> { };

            if (y - 1 >= 0)
                p.Add(map[y - 1, x]);
            else
                p.Add(99999);

            if (y + 1 < mapHeight)
                p.Add(map[y + 1, x]);
            else
                p.Add(99999);

            if (x - 1 >= 0)
                p.Add(map[y, x - 1]);
            else
                p.Add(99999);

            if (x + 1 < mapWidth)
                p.Add(map[y, x + 1]);
            else
                p.Add(99999);

            int indx = p.IndexOf(p.Min()); // find min index of 4 sides

            Point snakeDir = new Point();
            if (indx == 0) snakeDir = new Point(0, -cellSize);  // up
            if (indx == 1) snakeDir = new Point(0, cellSize);  // down
            if (indx == 2) snakeDir = new Point(-cellSize, 0);  // left
            if (indx == 3) snakeDir = new Point(cellSize, 0); // right

            return snakeDir;
        }

        public void Drawing(Canvas g)
        {
            // map
            int offs_y = 0;
            int offs_x = 0;

            for (int y = 0; y < mapHeight; y++)
            {
                offs_y = y * cellSize;
                for (int x = 0; x < mapWidth; x++)
                {
                    offs_x = x * cellSize;

                    Rectangle cellMap = new Rectangle()
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0.1,
                    };
                    Canvas.SetLeft(cellMap, offs_x);
                    Canvas.SetTop(cellMap, offs_y);
                    g.Children.Add(cellMap);

                    TextBlock cellText = new TextBlock()
                    {
                        Text = map[y, x].ToString(),
                        FontSize = 8
                    };
                    Canvas.SetLeft(cellText, offs_x);
                    Canvas.SetTop(cellText, offs_y);
                    g.Children.Add(cellText);
                }
            }

            // food
            Rectangle rec = new Rectangle()
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.OrangeRed,
            };
            Canvas.SetLeft(rec, food.X);
            Canvas.SetTop(rec, food.Y);
            g.Children.Add(rec);
        }

    }
}
