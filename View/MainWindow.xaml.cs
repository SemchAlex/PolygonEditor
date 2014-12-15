using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PolygonEditor.Helper;
using PolygonEditor.Model;
using PolygonEditor.ViewModel;

namespace PolygonEditor.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PolygonViewModel _polygonViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _polygonViewModel = new PolygonViewModel((PolygonsSource)this.Resources["Items"], CollectionViewSource.GetDefaultView(MainDataGrid.ItemsSource));
            this.DataContext = _polygonViewModel;
        }
    }
}
