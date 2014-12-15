using System;
using PolygonEditor.Helper;
using PolygonEditor.Model;

namespace PolygonEditor.ViewModel
{
    public class EditPolygonVievModel : BaseViewModel
    {
        #region Property

        private Polygon _polygonItem = new Polygon(); // Переданий/новий полігон для редагування
        private bool? _dialogResult;

        /// <summary>
        /// Property для закриття діалогового вікна
        /// </summary>
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Полігон для редагування
        /// </summary>
        public Polygon PolygonItem
        {
            get { return _polygonItem; }
            set
            {
                _polygonItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command

        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(SavePolygon)); }
        }

        private void SavePolygon(object obj)
        {
            try
            {
                using (var context = new DataContext("dbPolygon"))
                {
                    context.Polygons.Add(PolygonItem);
                    context.SaveChanges();
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                HelpUtilites.ShowFullException(ex);
            }
        }

        #endregion
    }

}