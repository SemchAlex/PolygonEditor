using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Media;

namespace PolygonEditor.Model
{


    public class Polygon : BaseModel
    {
        public Polygon()
        {
            this.ColorBrush = Brushes.Black; //Default value
        }

        private string _name = "New polygon";

        [MaxLength(50)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _color;

        [Required]
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public Brush ColorBrush
        {
            get { return _color != null ? (Brush) new BrushConverter().ConvertFromString(_color) : null; }
            set
            {
                _color = value.ToString();
                OnPropertyChanged();
            }
        }

        private IList<Point> _points = new List<Point>();

        /// <summary>
        /// Колекція обєктів Model.Point 
        /// </summary>
        public IList<Point> Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// PointCollection, який будується з Points
        /// </summary>
        [NotMapped]
        public PointCollection WinPoints
        {
            get
            {
                return new PointCollection(_points.Select(a => new System.Windows.Point(a.X, a.Y)).ToList()); 
            }
            set
            {
                if (_points != null && _points.Count == value.Count)
                {
                    for (int i = 0; i < _points.Count; i++)
                    {
                        _points[i].X = Convert.ToInt32(value[i].X);
                        _points[i].Y = Convert.ToInt32(value[i].Y);

                    }
                }
            }
        }

        /// <summary>
        /// Повертає/задає кількість вершин в полігоні
        /// При створенні нового запису заповнює полігон рандомними вершинами
        /// </summary>
        [NotMapped]
        public int PointCount
        {
            get { return _points.Count; }
            set
            {
                if (_points.Count >= (int) value) return;
                for (var i = 0; i < value; i++)
                    _points.Add(Point.RandomPoint());
                OnPropertyChanged();
            }
        }
    }
}
