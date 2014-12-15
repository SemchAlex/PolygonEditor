using System;
using System.ComponentModel.DataAnnotations;

namespace PolygonEditor.Model
{
    public class Point : BaseModel
    {
        private const int MaxX = 500;
        private const int MaxY = 500;
        private static readonly Random RandSeryes = new Random();

        private int _x, _y;
        private Polygon _polygon;

        [Required]
        public int X
        {
            get { return _x; }
            set { _x = value > MaxX ? MaxX : value; }
        }

        [Required]
        public int Y
        {
            get { return _y; }
            set { _y = value > MaxY ? MaxY : value; }
        }

        public Polygon Polygon
        {
            get { return _polygon; }
            set
            {
                _polygon = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Повертає нову рандомну точку моделі
        /// </summary>
        /// <returns></returns>
        public static Point RandomPoint()
        {
            return new Point(RandSeryes.Next(0, MaxX), RandSeryes.Next(0, MaxY));
        }

        public Point()
        {

        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
