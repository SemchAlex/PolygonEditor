using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace PolygonEditor.AppControl
{
    /// <summary>
    /// Текст-бокс, в який можна ввести тільки числові значення
    /// </summary>
    public class NumericTextBox:TextBox
    {
       
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            Int32 temp;
            e.Handled = !Int32.TryParse(e.Text,out temp);//Робимо неможливим внесення нечислових значень в TextBox
            base.OnPreviewTextInput(e);
        }
    }
}
