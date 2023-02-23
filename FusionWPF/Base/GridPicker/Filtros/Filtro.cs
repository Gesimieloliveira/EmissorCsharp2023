using System.Windows.Controls;

namespace FusionWPF.Base.GridPicker.Filtros
{
    public class Filtro
    {
        public class FiltroBuilder
        {
            private UserControl _userControlFiltro;
            private object _model;

            public FiltroBuilder UsarUserControlFiltro(UserControl userControlFiltro)
            {
                _userControlFiltro = userControlFiltro;
                return this;
            }

            public FiltroBuilder UsarModel(object model)
            {
                _model = model;
                return this;
            }

            internal Filtro BuilderFiltro()
            {
                var filtro = new Filtro
                {
                    AtivarFiltro = _userControlFiltro != null && _model != null,
                    UserControlFiltro = _userControlFiltro,
                    Model = _model
                };

                return filtro;
            }
        }

        public UserControl UserControlFiltro { get; set; }
        public object Model { get; set; }
        public bool AtivarFiltro { get; set; }
    }
}