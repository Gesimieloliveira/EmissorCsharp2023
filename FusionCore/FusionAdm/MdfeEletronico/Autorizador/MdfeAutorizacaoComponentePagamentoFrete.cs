using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public class MdfeAutorizacaoComponentePagamentoFrete : EntidadeBase<int>
    {
        public int Id { get; set; }

        public MdfeAutorizacaoComponentePagamentoFrete()
        {
        }

        public MdfeAutorizacaoInformacaoPagamento InformacaoPagamento { get; set; }
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