using FusionCore.FusionAdm.CteEletronico.Flags;
using static System.Decimal;

namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public class CteInfoQuantidadeCarga
    {
        private decimal _quantidade;
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }
        public string TipoUnidadeMedidaDescricao { get; set; }

        public decimal Quantidade
        {
            get { return _quantidade; }
            set { _quantidade = Round(value, 4); }
        }
    }
}