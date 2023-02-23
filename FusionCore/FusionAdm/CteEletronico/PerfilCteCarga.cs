using FusionCore.FusionAdm.CteEletronico.Flags;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionAdm.CteEletronico
{
    public class PerfilCteCarga
    {
        private PerfilCteCarga()
        {
            //nhibernate
        }

        public PerfilCteCarga(bool ativo, UnidadeMedida unidade, string tipoMedida, decimal quantidade) : this()
        {
            Ativo = ativo;
            Unidade = unidade;
            TipoMedida = tipoMedida ?? string.Empty;
            Quantidade = quantidade;
        }

        public bool Ativo { get; set; }
        public UnidadeMedida Unidade { get; set; }
        public string TipoMedida { get; set; }
        public decimal Quantidade { get; set; }
    }
}