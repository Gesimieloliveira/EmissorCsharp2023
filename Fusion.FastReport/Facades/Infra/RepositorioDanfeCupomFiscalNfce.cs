using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using NHibernate;

namespace Fusion.FastReport.Facades.Infra
{
    public class RepositorioDanfeCupomFiscalNfce : Repositorio<CupomFiscal, int>, IObterXml
    {
        private readonly RepositorioCupomFiscal _repositorioCupomFiscal;

        public RepositorioDanfeCupomFiscalNfce(ISession sessao) : base(sessao)
        {
            _repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
        }

        public string ObterXmlAutorizado(int cupomId)
        {
            return _repositorioCupomFiscal.BaixarObterXmlAutorizado(cupomId);
        }

        public string UltimoXmlAssinado(int cupomId)
        {
            return _repositorioCupomFiscal.BaixarUltimoXmlAssinado(cupomId);
        }
    }
}