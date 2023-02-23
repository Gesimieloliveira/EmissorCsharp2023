using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace Fusion.FastReport.Facades.Infra
{
    public class RepositorioDanfeNfce : Repositorio<Nfce, int>, IObterXml
    {
        private readonly IRepositorioNfce _repositorioNfce;
        public RepositorioDanfeNfce(ISession sessao) : base(sessao)
        {
            _repositorioNfce = new RepositorioNfce(sessao);
        }


        public string UltimoXmlAssinado(int cupomId)
        {
            return _repositorioNfce.BuscarUltimoXmlAssinado(cupomId);
        }

        public string ObterXmlAutorizado(int cupomId)
        {
            return _repositorioNfce.ObterXmlAutorizado(cupomId);
        }
    }
}