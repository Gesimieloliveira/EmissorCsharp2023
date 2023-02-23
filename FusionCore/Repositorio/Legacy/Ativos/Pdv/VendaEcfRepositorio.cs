using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class VendaEcfRepositorio : RepositorioBase<VendaEcfDt>
    {
        public VendaEcfRepositorio(ISession sessao) : base(sessao)
        {
        }

        public VendaEcfDt ObterUltimaVenda()
        {

            var ultimaVendaId = Sessao.Query<VendaEcfDt>().Max(v => (int?)v.Id);

            if (ultimaVendaId == null) return null;

            var ultimaVenda = Sessao.Query<VendaEcfDt>().Max(v => v.Id);

            return (VendaEcfDt) Sessao.Query<VendaEcfDt>().Where(v => v.Id.Equals(ultimaVenda) && v.Status.Equals(VendaStatus.Aberta)).FirstOrNull();
        }

        public VendaEcfDt ObterUltimaVendaParaCancelamento()
        {
            var ultimaVenda = Sessao.Query<VendaEcfDt>().Max(v => v.Id);

            return (VendaEcfDt)Sessao.Query<VendaEcfDt>().Where(v => v.Id.Equals(ultimaVenda)).FirstOrNull();
        }

        public IList<VendaEcfDt> BuscaParaSinronizacao(DateTime ultimaSincronizacao)
        {
            try
            {
                var query = Sessao.Query<VendaEcfDt>()
                    .Where(v => v.AlteradoEm >= ultimaSincronizacao || v.IdentificadorRemoto == null);

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption(ex);
            }
        }
    }
}