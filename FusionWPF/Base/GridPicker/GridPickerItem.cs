using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker.Contrato;

namespace FusionWPF.Base.GridPicker
{
    public class GridPickerItem : ViewModel, IGridPickerItem
    {
        private string _subtitulo;
        private bool _temSubtitulo;
        public string Titulo { get; set; }
        public string Coluna1 { get; set; }
        public string Coluna2 { get; set; }
        public string Coluna3 { get; set; }
        public string Coluna4 { get; set; }
        public object ItemReal { get; set; }

        public string Subtitulo
        {
            get { return _subtitulo; }
            set
            {
                if (value == _subtitulo) return;
                _subtitulo = value;
                TemSubtitulo = !string.IsNullOrWhiteSpace(value);
                PropriedadeAlterada();
            }
        }

        public bool TemSubtitulo
        {
            get { return _temSubtitulo; }
            private set
            {
                if (value == _temSubtitulo) return;
                _temSubtitulo = value;
                PropriedadeAlterada();
            }
        }

        public T GetItemReal<T>()
        {
            return (T) ItemReal;
        }
    }
}