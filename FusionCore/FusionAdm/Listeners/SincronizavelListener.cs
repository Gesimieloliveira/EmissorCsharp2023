using System.Threading;
using System.Threading.Tasks;
using FusionCore.FusionAdm.Servico.Sincronizador;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using NHibernate;
using NHibernate.Event;

namespace FusionCore.FusionAdm.Listeners
{
    public class SincronizavelListener : IPostUpdateEventListener, IPostInsertEventListener
    {
        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (!IsSincronizavel(@event.Entity))
            {
                return;
            }

            PersisteRegistro((ISincronizavelAdm) @event.Entity, @event.Session);
        }


        private static bool IsSincronizavel(object entidade)
        {
            return entidade is ISincronizavelAdm sincronizavel
                   && sincronizavel.EntidadeSincronizavel != EntidadeSincronizavel.NaoSincronizar;
        }

        private static void PersisteRegistro(ISincronizavelAdm entidade, ISession session)
        {
            var servico = new SincronizacaoPendenteServico(
                session,
                entidade.EntidadeSincronizavel,
                entidade.Referencia
            );

            servico.Salvar();
        }

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            if (!IsSincronizavel(@event.Entity))
            {
                return;
            }

            PersisteRegistro((ISincronizavelAdm) @event.Entity, @event.Session);
        }
    }
}