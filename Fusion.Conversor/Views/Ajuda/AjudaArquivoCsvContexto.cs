using Fusion.Conversor.Core.Map;
using FusionLibrary.VisaoModel;

namespace Fusion.Conversor.Views.Ajuda
{
    public class AjudaArquivoCsvContexto : ViewModel
    {
        private readonly IMap _map;

        public AjudaArquivoCsvContexto(IMap map)
        {
            _map = map;
        }

        public string ColunasObrigatorias
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ColunasPossiveis
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            ColunasObrigatorias = _map.ColunasObrigatorias;
            ColunasPossiveis = string.Join(", ", _map.ColunasPossiveis);
        }
    }
}