using System;
using FusionWPF.Base.GridPicker.Contrato;

namespace FusionWPF.Base.GridPicker
{
    public class GridPickerEventArgs : EventArgs
    {
        private readonly IGridPickerItem _item;

        public GridPickerEventArgs(IGridPickerItem item)
        {
            _item = item;
        }

        public GridPickerEventArgs(object itemReal)
        {
            _item = new GridPickerItem {ItemReal = itemReal};
        }

        public bool ItemIs(Type type)
        {
            return _item.ItemReal.GetType() == type;
        }

        public T GetItem<T>()
        {
            if (_item.ItemReal is T)
            {
                return _item.GetItemReal<T>();
            }

            throw new InvalidCastException("Item da PickerView não representa um: " + typeof(T));
        }
    }
}