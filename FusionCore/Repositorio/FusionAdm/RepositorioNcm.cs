using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNcm : Repositorio<NcmDTO, string>
    {
        public RepositorioNcm(ISession sessao) : base(sessao)
        {
        }

        public void Salva(NcmDTO ncm)
        {
            Sessao.Merge(ncm);
            Sessao.Flush();
        }

        public IList<string> TodosCodigosNcm()
        {
            var query = Sessao.QueryOver<NcmDTO>()
                .Select(n => n.Id);

            return query.List<string>();
        }

        public void Persiste(NcmDTO ncm)
        {
            Sessao.Persist(ncm);
            Sessao.Flush();
        }

        public bool JaExisteNcm(string codigoNcm)
        {
            var query = Sessao.QueryOver<NcmDTO>()
                .Where(n => n.Id == codigoNcm);
            return (query.RowCount() > 0);

        }
    }
}