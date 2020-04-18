using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodFillTest
{
    /// <summary>
    /// An Extension Method for the XNA Library Rectangle Class
    /// Will return a list of the points forming the outer walls of the rectangle
    /// </summary>
    public static class RectangleExtension
    {
        public static List<Point> Walls(this Rectangle rect)
        {
            List<Point> Walls = new List<Point>();

            for (int x = rect.Left; x <= rect.Right; x++)
            {
                Walls.Add(new Point(x, rect.Top));
                Walls.Add(new Point(x, rect.Bottom));
            }

            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                Walls.Add(new Point(rect.Left, y));
                Walls.Add(new Point(rect.Right, y));
            }
            return Walls;
        }

        public static List<Point> Area(this Rectangle rect)
        {
            List<Point> Points = new List<Point>();

            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    Points.Add(new Point(x, y));
                }
            }
            return Points;
        }
    }
}
