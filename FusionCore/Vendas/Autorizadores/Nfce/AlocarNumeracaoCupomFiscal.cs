using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Faturamentos;
using NHibernate;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class AlocarNumeracaoCupomFiscal
    {
        private readonly FaturamentoVenda _venda;

        public AlocarNumeracaoCupomFiscal(FaturamentoVenda venda)
        {
            _venda = venda;
        }

        public void Aloca(ISession sessao)
        {
            var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);

            var facadeCupomFiscal = new FacadeCupomFiscal(sessao);
            var cupomFiscal = facadeCupomFiscal.ObterCupomFiscal(_venda);

            var emissorFiscal = repositorioEmissorFiscal.GetPeloId(cupomFiscal.EmissorFiscalId);

            cupomFiscal.AlocaDadosFiscais(emissorFiscal.EmissorFiscalNfce);

            facadeCupomFiscal.SalvarOuAlterar(cupomFiscal);
            repositorioEmissorFiscal.SalvarEmissorFiscalNfce(emissorFiscal.EmissorFiscalNfce);
        }
    }
}