using System;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class EventoSincronizacaoRepositorio : RepositorioBase<EventoSincronizacaoDt>
    {
        public EventoSincronizacaoRepositorio(ISession sessao)
            : base(sessao)
        {
        }

        public EventoSincronizacaoDt BuscaUltimoPelaTag(String tag)
        {
            try
            {
                var query = Sessao.Query<EventoSincronizacaoDt>()
                    .Where(ev => ev.Tag == tag)
                    .OrderByDescending(ev => ev.IniciadoEm);

                return (EventoSincronizacaoDt) query.FirstOrNull();
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }
    }
}