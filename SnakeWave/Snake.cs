using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake_Wave
{
    class Snake
    {
        private int snakeStep;              // Snake step
        public Point direction;             // Snake movement direction
        public List<Point> body = new List<Point>() { }; // Snake list of (x, y) positions
        private Point food = new Point();   // Food
        private bool drawingAllow = true;
        private Random rnd = new Random();  // Random generator
        int cellSize;


        public Snake(int cellSize)
        {
            this.cellSize = cellSize;
            snakeStep = cellSize;
            Reset();
        }


        public void Update(int imgWidth, int imgHeight)
        {
            // Calc a new position of the head
            Point newHeadPosition = new Point(
                body[0].X + direction.X,
                body[0].Y + direction.Y
            );

            // Insert new position in the beginning of the snake list
            body.Insert(0, newHeadPosition);

            // Remove the last element
            body.RemoveAt(body.Count - 1);
        }

        public void Drawing(Canvas g)
        {
            foreach (var cell in body)
            {
                Rectangle rec = new Rectangle()
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.Green
                };
                Canvas.SetLeft(rec, cell.X);
                Canvas.SetTop(rec, cell.Y);
                g.Children.Add(rec);
            }
        }

        public void Reset()
        {
            drawingAllow = true;
            direction = new Point(cellSize, 0);
            body.Add(new Point(20, 20));
        }
    }
}
