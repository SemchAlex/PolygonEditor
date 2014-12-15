using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using PolygonEditor.Helper;
using PolygonEditor.Model;
using PolygonEditor.View;

namespace PolygonEditor.ViewModel
{

    public class PolygonViewModel : BaseViewModel
    {


        #region ctor

        public PolygonViewModel()
        {

        }

        /// <summary>
        /// Конструктор для створення PolygonViewModel
        /// </summary>
        /// <param name="source">Джерело даних для grid</param>
        /// <param name="defaultView"></param>
        public PolygonViewModel(PolygonsSource source, ICollectionView defaultView)
        {
            _polygons = source;
            PolygonView = defaultView;
            PolygonView.Filter = OnFilterMovie;
            RefreshGridData();
        }

        #endregion

        #region property and field

        private readonly PolygonsSource _polygons;
        private Polygon _selectedPolygon;

        public ICollectionView PolygonView { get; private set; }

        public bool IsPotintsChanged { get; set; }


        /// <summary>
        /// Вибраний полігон
        /// </summary>
        public Polygon SelectedPolygon
        {
            get { return _selectedPolygon; }
            set
            {
                if (_selectedPolygon != null && _selectedPolygon.Points.Count != 0 && this.IsPotintsChanged)
                {
                    UpdatePoints();// Оновлюємо записи в базі, якщо користувач змінив координати в редакторі
                }
                _selectedPolygon = value;
                OnPropertyChanged();
            }
        }


       

        private string _filter = string.Empty;

        /// <summary>
        /// Значення фильтра
        /// </summary>
        public string FilterString
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region command

        private DelegateCommand _addPolygonCommand;

        public DelegateCommand AddPolygonCommand
        {
            get { return _addPolygonCommand ?? (_addPolygonCommand = new DelegateCommand(AddNewPolygon)); }
        }

        private void AddNewPolygon(object obj)
        {
            var window = new PolygonEdit
            {
                DataContext = new EditPolygonVievModel()
            };
            window.ShowDialog();
            RefreshGridData();
        }

        private DelegateCommand _filterGridCommand;

        public DelegateCommand FilterGridCommand
        {
            get { return _filterGridCommand ?? (_filterGridCommand = new DelegateCommand(FilterGrid)); }
        }
        private void FilterGrid(object obj)
        {
            PolygonView.Refresh();
        }
        #endregion

        #region private method

        private void UpdatePoints()
        {
            try
            {
                using (var ctx = new DataContext("dbPolygon"))
                {
                    var dbPolygon = ctx.Polygons.Include(p => p.Points).Single(p => p.Id == _selectedPolygon.Id);
                    foreach (var point in _selectedPolygon.Points)
                    {
                        var dbPoitnt = dbPolygon.Points.SingleOrDefault(p => p.Id == point.Id);
                        if (dbPoitnt != null)
                        {
                            ctx.Entry(dbPoitnt).CurrentValues.SetValues(point);
                        }
                    }
                    ctx.SaveChanges();
                }
                IsPotintsChanged = false;
            }
            catch (Exception ex)
            {
                HelpUtilites.ShowFullException(ex);
            }
        }
        
        private bool OnFilterMovie(object obj)
        {
            var polygon = (Polygon) obj;
            return polygon.Name.Contains(FilterString);
        }

        private void RefreshGridData()
        {
            try
            {
                _polygons.Clear();
                using (var dtContext = new DataContext("dbPolygon"))
                {
                    foreach (var polygon in dtContext.Polygons.Include("Points"))
                        _polygons.Add(polygon);
                }
            }
            catch (Exception ex)
            {
                HelpUtilites.ShowFullException(ex);
            }

        }

        #endregion

    }
}
