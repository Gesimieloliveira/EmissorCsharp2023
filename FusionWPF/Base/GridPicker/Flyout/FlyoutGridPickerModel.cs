using System.Windows.Input;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Base.GridPicker.Flyout
{
    public class FlyoutGridPickerModel : ViewModel
    {
        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public object Filtro
        {
            get { return GetValue<object>(); }
            set { SetValue(value); }
        }

        public ICommand CommandAplicarFiltro
        {
            get { return GetValue<ICommand>(); }
            set { SetValue(value); }
        }
    }
}