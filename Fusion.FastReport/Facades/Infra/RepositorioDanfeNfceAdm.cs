using FusionCore.FusionAdm.Nfce;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace Fusion.FastReport.Facades.Infra
{
    public class RepositorioDanfeNfceAdm : Repositorio<NfceAdm, int>, IObterXml
    {
        private readonly IRepositorioNfceAdm _repositorioNfce;

        public RepositorioDanfeNfceAdm(ISession sessao) : base(sessao)
        {
            _repositorioNfce = new RepositorioNfceAdm(sessao);
        }

        public string ObterXmlAutorizado(int cupomId)
        {
            return _repositorioNfce.BaixarXmlAutorizado(cupomId);
        }

        public string UltimoXmlAssinado(int cupomId)
        {
            return _repositorioNfce.UltimoXmlAssinado(cupomId);
        }
    }
}