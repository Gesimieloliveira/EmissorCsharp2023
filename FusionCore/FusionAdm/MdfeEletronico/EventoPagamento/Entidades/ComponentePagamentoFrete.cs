using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    public class ComponentePagamentoFrete : EntidadeBase<int>
    {
        public int Id { get; set; }

        private ComponentePagamentoFrete()
        {
        }

        public ComponentePagamentoFrete(TipoComponente tipoComponente, string descricao, decimal valor)
        {
            TipoComponente = tipoComponente;
            Descricao = descricao;
            Valor = valor;
        }

        public InformacaoPagamento InformacaoPagamento { get; set; }
        public TipoComponente TipoComponente { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        protected override int ChaveUnica => Id;

        public string ObterDescricao()
        {
            return Descricao.Length != 0 ? Descricao : null;
        }
    }
}