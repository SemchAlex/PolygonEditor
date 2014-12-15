using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolygonEditor.AppControl
{
    /// <summary>
    /// Логика взаимодействия для EditablePolygon.xaml
    /// </summary>
    public partial class EditablePolygon : UserControl
    {
        private Brush _colorPolygon = Brushes.Black;//Дефаултне значення вибраного кольору
        private Polygon _selectedPolygon = new Polygon(); //Полігон, який відображається
        private Path _selectedCornerEllipse; //Вибрана вершина полігону
        public Polygon SelectedPolygon
        {
            get { return _selectedPolygon; }
            set { _selectedPolygon = value; }
        }
        
        #region binding property

        /// <summary>
        /// Показує, чи змінювалися координати точкок в полігоні
        /// </summary>
        public bool IsPoitntsChanged
        {
            get { return (bool) GetValue(PointsChangedProperty); }
            set { SetValue(PointsChangedProperty, value); }
        }

        public static readonly DependencyProperty PointsChangedProperty =
            DependencyProperty.Register("IsPoitntsChanged", typeof (bool), typeof (EditablePolygon),
                new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Колір контуру полігону
        /// </summary>
        public Brush ColorValue
        {
            get { return (Brush)GetValue(ColorValueProperty); }
            set { SetValue(ColorValueProperty, value); }
        }
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register("ColorValue", typeof (Brush), typeof (EditablePolygon),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    _onColorPropertChanged));

        private static void _onColorPropertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as EditablePolygon;
            if (source != null && e.NewValue != null)
            {
                source._colorPolygon = (Brush) e.NewValue;
                source.DrawPolygon();
            }
        }
        /// <summary>
        /// Колекція координат для побудови полігону
        /// </summary>
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof (PointCollection), typeof (EditablePolygon),
                new FrameworkPropertyMetadata(default(PointCollection),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    _onPointsPropertChanged));

        private static void _onPointsPropertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as EditablePolygon;
            if (source != null)
            {
                source._selectedPolygon.Points = (PointCollection) e.NewValue;
                source.DrawPolygon();
            }
        }


        #endregion
        
        
        public EditablePolygon()
        {
            InitializeComponent();
            this.DataContext = this;

        }



        #region Events

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedCornerEllipse == null)
                return;

            var point = e.GetPosition(cnv);

            var geometry = _selectedCornerEllipse.Data as EllipseGeometry;
            if (geometry == null)
                return;

            geometry.Center = point;
            var index = (int) _selectedCornerEllipse.Tag;
            this.Points[index] = point;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Points = _selectedPolygon.Points;
            _selectedCornerEllipse = null;
            this.IsPoitntsChanged = true;
        }

        private void CornerEllipseMouseUp(object sender, MouseButtonEventArgs e)
        {
            var path = sender as Path;
            if (path == null) return;

            _selectedCornerEllipse = null;
        }

        private void CornerEllipseMouseDown(object sender, MouseButtonEventArgs e)
        {
            var path = sender as Path;
            if (path == null) return;

            _selectedCornerEllipse = path;
        }


        private static void CornerEllipseMouseLeave(object sender, MouseEventArgs e)
        {
            var path = sender as Path;
            if (path == null)
                return;
            var geometry = path.Data as EllipseGeometry;
            if (geometry == null)
                return;

            geometry.RadiusX = 4;
            geometry.RadiusY = geometry.RadiusX;
        }

        private static void CornerEllipseMouseEnter(object sender, MouseEventArgs e)
        {
            var path = sender as Path;
            if (path == null)
                return;
            var geometry = path.Data as EllipseGeometry;
            if (geometry == null)
                return;

            geometry.RadiusX = 6;
            geometry.RadiusY = geometry.RadiusX;
        }

        #endregion

        #region Private Methods

        private void DrawPolygon()
        {
            _selectedPolygon.Stroke = _colorPolygon;
            _selectedPolygon.StrokeThickness = 1;
            _selectedPolygon.Fill = Brushes.Transparent;
            _selectedPolygon.Points = this.Points;

            cnv.Children.Clear();
            cnv.Children.Add(_selectedPolygon);
            int tmp = 0;
            if (this.Points != null)
                foreach (var point in Points)
                {
                    cnv.Children.Add(GetCornerEllipse(point, tmp));
                    tmp++;
                }

        }

        private Path GetCornerEllipse(Point point, int index)
        {
            var geometry = new EllipseGeometry {Center = point, RadiusX = 4};
            geometry.RadiusY = geometry.RadiusX;
            var path = new Path {Data = geometry, Fill = Brushes.Gray};
            path.MouseDown += CornerEllipseMouseDown;
            path.MouseUp += CornerEllipseMouseUp;
            path.MouseEnter += CornerEllipseMouseEnter;
            path.MouseLeave += CornerEllipseMouseLeave;
            path.Tag = index;
            return path;
        }

        #endregion
    }
}
