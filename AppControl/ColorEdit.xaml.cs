using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolygonEditor.AppControl
{

    /// <summary>
    /// Логика взаимодействия для ColorEdit.xaml
    /// </summary>
    public partial class ColorEdit : UserControl, INotifyPropertyChanged
    {
        #region Default collection 

        private static readonly ICollection<ComboBoxItem> CollectionColorDefault = new Collection<ComboBoxItem>()
        {
            new ComboBoxItem() {Background = Brushes.Black, Content = "", Foreground = Brushes.White},
            new ComboBoxItem() {Background = Brushes.White, Content = "", Foreground = Brushes.Black},
            new ComboBoxItem() {Background = Brushes.Blue, Content = "", Foreground = Brushes.White},
            new ComboBoxItem() {Background = Brushes.Red, Content = "", Foreground = Brushes.Black},
            new ComboBoxItem() {Background = Brushes.Lime, Content = "", Foreground = Brushes.Black},
            new ComboBoxItem() {Background = Brushes.Gray, Content = "", Foreground = Brushes.White},
            new ComboBoxItem() {Background = Brushes.Yellow, Content = "", Foreground = Brushes.Black}
        };

        #endregion

        #region Property

        private ICollection<ComboBoxItem> _colorItemsCollection;

        /// <summary>
        /// Задає/повертає колекцію обєктів для випадаючого списку
        /// </summary>
        public ICollection<ComboBoxItem> ColorItemsCollection
        {
            get { return _colorItemsCollection; }
            set
            {
                _colorItemsCollection = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region SelectedItem ComboBoxItem

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (ComboBoxItem), typeof (ColorEdit),
                new PropertyMetadata(default(ComboBoxItem), _onSelectedItemPropertyChanged));

        public ComboBoxItem SelectedItem
        {
            get { return (ComboBoxItem) GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        private static void _onSelectedItemPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ColorEdit) obj;
            var value = e.NewValue as ComboBoxItem;
            if (value != null)
                owner.ColorValue = value.Background;
        }

        #endregion

        #region  ColorValue Brush

        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register("ColorValue", typeof (Brush), typeof (ColorEdit),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    _onColorValuePropertyChanged));
        /// <summary>
        /// Повертає/задає значення вибраного кольору у списку
        /// </summary>
        public Brush ColorValue
        {
            get { return (Brush) GetValue(ColorValueProperty); }
            set { SetValue(ColorValueProperty, value); }
        }

        private static void _onColorValuePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var source = obj as ColorEdit;
            if (source != null)
                source.SelectedItem = source.ColorItemsCollection.FirstOrDefault(a => a.Background == e.NewValue);
        }

        #endregion

        #region Ctor

        public ColorEdit()
        {
            InitializeComponent();
            this.DataContext = this;
            ColorItemsCollection = CollectionColorDefault;
        }

        #endregion

        #region INotifyProperty 

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}