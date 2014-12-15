using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PolygonEditor.Model
{
    /// <summary>
    /// Абстрактний базовий клас для моделі з реалізацією INotifyPropertyChanged
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {

        [Key]
        public int Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
