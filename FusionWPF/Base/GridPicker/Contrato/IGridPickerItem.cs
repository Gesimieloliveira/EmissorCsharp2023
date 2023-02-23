using System.ComponentModel;

namespace FusionWPF.Base.GridPicker.Contrato
{
    public interface IGridPickerItem : INotifyPropertyChanged
    {
        string Titulo { get; set; }
        string Subtitulo { get; set; }
        bool TemSubtitulo { get; }
        string Coluna1 { get; set; }
        string Coluna2 { get; set; }
        string Coluna3 { get; set; }
        string Coluna4 { get; set; }
        object ItemReal { get; set; }

        T GetItemReal<T>();
    }
}