using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.Repositorio.Base;

namespace Fusion.Visao.MdfeEletronico.IncluirPagamento
{
    public class TipoComponenteDTO : EntidadeBase<int>
    {
        public int Id { get; set; }
        public TipoComponente TipoComponente { get; set; }
        public decimal ValorComponente { get; set; }
        protected override int ChaveUnica => Id;
        public string DescricaoOutros { get; set; }
        public ComponentePagamentoFrete MdfeComponente { get; set; }
        public MdfeAutorizacaoComponentePagamentoFrete MdfeComponenteAba { get; set; }
    }
}