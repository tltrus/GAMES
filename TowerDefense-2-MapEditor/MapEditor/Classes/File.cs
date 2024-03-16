using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MapEditor
{
    class Files
    {
        public static void FileRead(string name, List<Point> points)
        {
            if (!File.Exists(name)) return;

            points.Clear();

            string[] rows = File.ReadAllLines(name);

            if (rows.Length <= 0) return;

            int z = 0;
            for (int i = 0; i < rows.Length; ++i)
            {
                var row = rows[z++];
                string[] xy = row.Split(new char[] { ':' });

                if (xy.Length != 2) continue;

                var x = double.Parse(xy[0]);
                var y = double.Parse(xy[1]);
                points.Add(new Point(x, y));
            }

        }

        public static void SaveToFile(string name, List<Point> points)
        {
            var len = points.Count();

            if (len <= 0) return;

            string[] rows = new string[len];

            for (int i = 0; i < len; ++i)
            {
                rows[i] = points[i].X + ":" + points[i].Y;
            }

            if (!File.Exists(name)) File.Create(name).Close();

            File.WriteAllLines(name, rows);
        }
    }
}
