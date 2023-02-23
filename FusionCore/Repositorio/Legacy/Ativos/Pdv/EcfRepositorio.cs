using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class EcfRepositorio : RepositorioBase<EcfDt>
    {
        public EcfRepositorio(ISession sessao) : base(sessao)
        {
        }

        public IList<EcfDt> BuscarEcfEmUso()
        {
            IList<EcfDt> ecfsEcfDts = Sessao.Query<EcfDt>().Where(ecf => ecf.EmUso == 1).ToList();

            return ecfsEcfDts;
        }

        public EcfDt BuscarEfAtivo(string serie)
        {
            return (EcfDt) Sessao.Query<EcfDt>().Where(ecf => ecf.Ativo == 1 && ecf.Serie == serie).FirstOrNull();
        }

        public IList<EcfDt> BuscarEcfsAtivos()
        {
            IList<EcfDt> ecfsEcfDts = Sessao.Query<EcfDt>().Where(ecf => ecf.Ativo == 1).ToList();

            return ecfsEcfDts;

        } 

        public EcfDt BuscarEcfPorSerie(string serie)
        {
            var resultadoEcf = (EcfDt) Sessao.Query<EcfDt>().Where(ecf => ecf.Serie.Equals(serie)).FirstOrNull();

            return resultadoEcf;
        }
    }
}