using System;

namespace WpfApp
{
    internal class Vector2D
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Vector2D(double x = 0.0, double y = 0.0)
        {
            X = x;
            Y = y;
        }

        //
        // Summary:
        //     Копирование вектора
        //
        // Returns:
        //     Новый вектор
        public Vector2D CopyToVector()
        {
            return new Vector2D(X, Y);
        }

        //
        // Summary:
        //     Копирование вектора в массив
        //
        // Returns:
        //     Новый массив [x, y]
        public double[] CopyToArray()
        {
            return new double[2] { X, Y };
        }

        //
        // Summary:
        //     Присвоение скалярных значений вектору
        //
        // Parameters:
        //   x:
        //
        //   y:
        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        //
        // Summary:
        //     Текстовое представление вектора
        //
        // Returns:
        //     Строка вида "[x, y]"
        public override string ToString()
        {
            return $"Vector2D Object: [{X}, {Y}]";
        }

        //
        // Summary:
        //     Вывод значений вектора на консоль вида "[x, y]"
        public void ToConsole()
        {
            Console.WriteLine(ToString());
        }

        //
        // Summary:
        //     Сложение вектора со скалярами
        //
        // Parameters:
        //   vector:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Add(double x, double y)
        {
            X += x;
            Y += y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Сложение вектора со скаляром
        //
        // Parameters:
        //   value:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Add(double value)
        {
            X += value;
            Y += value;
            return CopyToVector();
        }

        //
        // Summary:
        //     Сложение двух векторов
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Add(Vector2D v)
        {
            X += v.X;
            Y += v.Y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Сложение двух векторов
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Add(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        //
        // Summary:
        //     Вычитание из вектора скаляров
        //
        // Parameters:
        //   x:
        //
        //   y:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Sub(double x, double y)
        {
            X -= x;
            Y -= y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Вычитание из вектора другого вектора
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Sub(Vector2D v)
        {
            X -= v.X;
            Y -= v.Y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Вычитание двух векторов
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Sub(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   n:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Div(double n)
        {
            X /= n;
            Y /= n;
            return CopyToVector();
        }

        //
        // Summary:
        //     Деление вектора на другой вектор
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Div(Vector2D v)
        {
            X /= v.X;
            Y /= v.Y;
            return CopyToVector();
        }

        //
        // Summary:
        //     Divide (деление) двух векторов
        //
        // Parameters:
        //   val:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Div(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X / v2.X, v1.Y / v2.Y);
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   v:
        //     Вектор
        //
        //   n:
        //     Скаляр
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Div(Vector2D v, double n)
        {
            return new Vector2D(v.X / n, v.Y / n);
        }

        //
        // Summary:
        //     Multiply (умножение) вектора на число
        //
        // Parameters:
        //   n:
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Mult(double n)
        {
            X *= n;
            Y *= n;
            return CopyToVector();
        }

        //
        // Summary:
        //     Multiply (умножение) вектора на число
        //
        // Parameters:
        //   v:
        //
        //   n:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Mult(Vector2D v, double n)
        {
            return new Vector2D(v.X * n, v.Y * n);
        }

        public static Vector2D Mult(double[,] matrix2D, Vector2D v)
        {
            Vector2D vector2D = new Vector2D();
            vector2D.X = matrix2D[0, 0] * v.X + matrix2D[0, 1] * v.Y;
            vector2D.Y = matrix2D[1, 0] * v.X + matrix2D[1, 1] * v.Y;
            return vector2D;
        }

        //
        // Summary:
        //     Скалярное (Dot) умножение векторов
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Скаляр
        public double Dot(Vector2D v)
        {
            return X * v.X + Y * v.Y;
        }

        //
        // Summary:
        //     Скалярное (Dot) умножение векторов
        //
        // Parameters:
        //   v1:
        //     Вектор 1
        //
        //   v2:
        //     Вектор 2
        //
        // Returns:
        //     Скаляр двоичное число
        public static double Dot(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        //
        // Summary:
        //     Векторное произведение векторов. Для 2D результат всегда скаляр, т.к. z = 0
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Скаляр
        public double Cross(Vector2D v)
        {
            return X * v.Y - Y * v.X;
        }

        public static double Cross(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   x:
        //
        //   y:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        //
        // Returns:
        //     Возвращает новый вектор
        public Vector2D Lerp(double x, double y, double amt)
        {
            return new Vector2D(X + (x - X) * amt, Y + (y - Y) * amt);
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   v:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        //
        // Returns:
        //     Возвращает новый вектор
        public Vector2D Lerp(Vector2D v, double amt)
        {
            return new Vector2D(X + (v.X - X) * amt, Y + (v.Y - Y) * amt);
        }

        //
        // Summary:
        //     Интерполяция вектора к другому вектору
        //
        // Parameters:
        //   v1:
        //
        //   v2:
        //
        //   amt:
        //     Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new
        //     vector. 0.5 is halfway in between
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Lerp(Vector2D v1, Vector2D v2, double amt)
        {
            Vector2D vector2D = new Vector2D();
            vector2D = v1.CopyToVector();
            return vector2D.Lerp(v2, amt);
        }

        //
        // Summary:
        //     Возвращает угол вектора (only 2D vectors)
        //
        // Returns:
        //     Значение угла в радианах
        public double HeadingRad()
        {
            return Math.Atan2(Y, X);
        }

        //
        // Summary:
        //     Возвращает угол вектора (only 2D vectors)
        //
        // Returns:
        //     Значение угла в градусах [от 0 до 359]
        public double HeadingDeg()
        {
            double num = HeadingRad();
            return (num >= 0.0) ? (num * 180.0 / Math.PI) : ((Math.PI * 2.0 + num) * 360.0 / (Math.PI * 2.0));
        }

        //
        // Summary:
        //     Вычисление угла между двумя векторами
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Возвращает угол в радианах
        public double angleBetween(Vector2D v)
        {
            double val = Dot(v) / (Mag() * v.Mag());
            return Math.Acos(Math.Min(1.0, Math.Max(-1.0, val)));
        }

        //
        // Summary:
        //     Поворот вектора на заданный угол (only 2D vectors)
        //
        // Parameters:
        //   a:
        //     Угол в радианах
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Rotate(double a)
        {
            double num = HeadingRad() + a;
            double num2 = Mag();
            X = Math.Cos(num) * num2;
            Y = Math.Sin(num) * num2;
            return CopyToVector();
        }

        //
        // Summary:
        //     Получение квадрата длины вектора
        //
        // Returns:
        //     Скаляр
        public double MagSq()
        {
            return X * X + Y * Y;
        }

        //
        // Summary:
        //     Вычисление длины вектора
        //
        // Returns:
        //     Скаляр
        public double Mag()
        {
            return Math.Sqrt(MagSq());
        }

        //
        // Summary:
        //     Задание длины вектора
        //
        // Parameters:
        //   n:
        //     вещественная длина
        public void SetMag(double n)
        {
            Vector2D vector2D = Normalize();
            vector2D.Mult(n);
            X = vector2D.X;
            Y = vector2D.Y;
        }

        //
        // Summary:
        //     Задание длины вектора
        //
        // Parameters:
        //   n:
        //     целочисленная длина
        public void SetMag(int n)
        {
            Vector2D vector2D = Normalize();
            vector2D.Mult(n);
            X = vector2D.X;
            Y = vector2D.Y;
        }

        //
        // Summary:
        //     Нормализация вектора
        //
        // Returns:
        //     Возвращает новый вектор и изменяет текущий
        public Vector2D Normalize()
        {
            double num = Mag();
            if (num == 0.0)
            {
                return new Vector2D();
            }

            Mult(1.0 / num);
            return CopyToVector();
        }

        public static Vector2D Normalize(Vector2D v)
        {
            Vector2D vector2D = v.CopyToVector();
            vector2D.Normalize();
            return vector2D;
        }

        //
        // Summary:
        //     Ограничение (Limit) длины вектора до max значения
        //
        // Parameters:
        //   max:
        //     Требуемая максимальная длина вектора
        //
        // Returns:
        //     Вектор с лимитированной длиной
        public Vector2D Limit(double max)
        {
            double num = MagSq();
            if (num > max * max)
            {
                Div(Math.Sqrt(num)).Mult(max);
            }

            return CopyToVector();
        }

        //
        // Summary:
        //     Создание вектора по углу
        //
        // Parameters:
        //   angle:
        //     Угол в радианах
        //
        //   len:
        //     Длина вектора. По умолчанию длина = 1
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D FromAngle(double angle, double len = 1.0)
        {
            return new Vector2D(len * Math.Cos(angle), len * Math.Sin(angle));
        }

        //
        // Summary:
        //     Создание единичного 2D вектора по случайному углу 2PI
        //
        // Parameters:
        //   rnd:
        //
        // Returns:
        //     Возвращает новый вектор
        public static Vector2D Random2D(Random rnd)
        {
            return FromAngle(rnd.NextDouble() * Math.PI * 2.0);
        }

        //
        // Summary:
        //     Создание единичного 2D вектора по углу в диапазоне
        //
        // Parameters:
        //   rnd:
        //
        //   start:
        //     Угол От
        //
        //   end:
        //     Угод До
        public static Vector2D Random2D(Random rnd, double start = 0.0, double end = Math.PI)
        {
            return FromAngle(rnd.NextDoubleRange(start, end));
        }

        //
        // Summary:
        //     Вычисление расстояния между векторами
        //
        // Parameters:
        //   v:
        //
        // Returns:
        //     Cкаляр
        public double Dist(Vector2D v)
        {
            return Sub(this, v).Mag();
        }

        //
        // Summary:
        //     Вычисление расстояния между двумя векторами
        //
        // Parameters:
        //   v1:
        //     Вектор 1
        //
        //   v2:
        //     Вектор 2
        //
        // Returns:
        //     Cкаляр
        public static double Dist(Vector2D v1, Vector2D v2)
        {
            return v1.Dist(v2);
        }

        //
        // Summary:
        //     Вычисление перпендикулярного вектора к вектору a
        //
        // Parameters:
        //   v:
        //     Вектор, к которому строится нормаль
        //
        // Returns:
        //     Новый перпендикулярный вектор
        public static Vector2D NormalVector(Vector2D a)
        {
            double num = 0.0;
            double num2 = 0.0;
            if (a.X != 0.0)
            {
                num2 = 1.0;
                num = (0.0 - a.Y) * num2 / a.X;
            }
            else if (a.Y != 0.0)
            {
                num = 1.0;
                num2 = (0.0 - a.X) * num / a.Y;
            }

            return new Vector2D(num, num2);
        }

        //
        // Summary:
        //     Adds two vectors together
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый суммированный вектор
        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return Add(left, right);
        }

        //
        // Summary:
        //     Изменение вектора на негативный
        //
        // Parameters:
        //   v:
        //     Вектор
        //
        // Returns:
        //     Новый отрицательный вектор
        public static Vector2D operator -(Vector2D v)
        {
            return v.Mult(-1.0);
        }

        //
        // Summary:
        //     Разность векторов
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return Sub(left, right);
        }

        //
        // Summary:
        //     Умножение скаляра на вектор
        //
        // Parameters:
        //   left:
        //     Скаляр
        //
        //   right:
        //     Вектор
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(float left, Vector2D right)
        {
            return Mult(right, left);
        }

        //
        // Summary:
        //     Умножение пар элементов обоих векторов
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X * right.X, left.Y * right.Y);
        }

        //
        // Summary:
        //     Умножение вектора на скаляр
        //
        // Parameters:
        //   left:
        //     Вектор
        //
        //   right:
        //     Скаляр
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator *(Vector2D left, float right)
        {
            return Mult(left, right);
        }

        public static Vector2D operator *(Vector2D left, double right)
        {
            return Mult(left, right);
        }

        //
        // Summary:
        //     Деление первого вектора на второй вектор
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator /(Vector2D left, Vector2D right)
        {
            return Div(left, right);
        }

        //
        // Summary:
        //     Деление вектора на скаляр
        //
        // Parameters:
        //   value1:
        //     Вектор
        //
        //   value2:
        //     Скаляр
        //
        // Returns:
        //     Новый вектор
        public static Vector2D operator /(Vector2D value1, float value2)
        {
            return Div(value1, value2);
        }

        //
        // Summary:
        //     Returns a value that indicates whether each pair of elements in two specified
        //     vectors is equal.
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     true if left and right are equal; otherwise, false.
        public static bool operator ==(Vector2D left, Vector2D right)
        {
            if (left.X == right.X && left.Y == right.Y)
            {
                return true;
            }

            return false;
        }

        //
        // Summary:
        //     Returns a value that indicates whether two specified vectors are not equal.
        //
        // Parameters:
        //   left:
        //     Вектор 1
        //
        //   right:
        //     Вектор 2
        //
        // Returns:
        //     true if left and right are not equal; otherwise, false.
        public static bool operator !=(Vector2D left, Vector2D right)
        {
            if (left.X != right.X || left.Y != right.Y)
            {
                return true;
            }

            return false;
        }
    }
}
