using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;
using NHibernate;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class FacadeCupomFiscal
    {
        private readonly RepositorioCupomFiscal _repositorioCupomFiscal;

        public FacadeCupomFiscal(ISession sessao)
        {
            _repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
        }

        public CupomFiscal ObterCupomFiscal(FaturamentoVenda venda)
        {
            return _repositorioCupomFiscal.ObterCupomFiscal(venda);
        }

        public void SalvarOuAlterar(CupomFiscal cupomFiscal)
        {
            _repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
        }
    }
}