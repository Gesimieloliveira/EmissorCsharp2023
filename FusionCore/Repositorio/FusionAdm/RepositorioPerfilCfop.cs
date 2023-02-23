using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPerfilCfop : Repositorio<PerfilCfopDTO, int>
    {
        public RepositorioPerfilCfop(ISession sessao) : base(sessao)
        {
        }

        public PerfilCfopDTO PeloCodigo(string codigo)
        {
            var codigoSearch = codigo;

            if (codigo?.Length == 4)
            {
                codigoSearch = $"{codigoSearch}01";
            }

            var cfop = (PerfilCfopDTO) Sessao.QueryOver<PerfilCfopDTO>()
                .Where(p => p.Codigo == codigoSearch)
                .List<PerfilCfopDTO>()
                .FirstOrNull();

            return cfop;
        }

        public void SalvaAlteracoes(PerfilCfopDTO perfil)
        {
            if (perfil.Id == 0)
            {
                Sessao.Persist(perfil);
                Sessao.Flush();
                return;
            }

            Sessao.Update(perfil);
            Sessao.Flush();
        }

        public void Deleta(PerfilCfopDTO perfil)
        {
            Sessao.Delete(perfil);
            Sessao.Flush();
        }

        public IList<PerfilCfopDTO> PorOperacao(TipoOperacao tipo)
        {
            CfopDTO cfop = null;
            PerfilCfopDTO perfil = null;

            var query = Sessao.QueryOver(() => perfil)
                .JoinAlias(() => perfil.Cfop, () => cfop, JoinType.InnerJoin)
                .Where(() => cfop.TipoOperacao == tipo);

            return query.List();
        }
    }
}