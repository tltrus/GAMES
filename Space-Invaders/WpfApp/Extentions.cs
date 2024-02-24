using System.Windows;

namespace WpfApp
{
    public static class Extentions
    {
        public static Vector2D ToVector2D(this Point p)
        {
            return new Vector2D(p.X, p.Y);
        }

        public static Point ToPoint(this Vector2D v)
        {
            return new Point(v.X, v.Y);
        }

        /// <summary>
        /// Случайный выбор числа в диапазоне (мин, макс)
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minNumber">Минимальная граница</param>
        /// <param name="maxNumber">Максимальная граница</param>
        /// <returns></returns>
        public static double NextDoubleRange(this System.Random random, double minNumber, double maxNumber)
        {
            return random.NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}
