using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class EstoqueEventoPdvRepositorio : RepositorioBase<EstoqueEventoPdvDt>
    {
        public EstoqueEventoPdvRepositorio(ISession sessao)
            : base(sessao)
        {
        }

        public List<EstoqueEventoPdvDt> BuscaParaSincronizacao()
        {
            try
            {
                var query = Sessao.Query<EstoqueEventoPdvDt>()
                    .Where(ev => ev.IdentificadorRemoto == null);

                return query.ToList();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }
    }
}