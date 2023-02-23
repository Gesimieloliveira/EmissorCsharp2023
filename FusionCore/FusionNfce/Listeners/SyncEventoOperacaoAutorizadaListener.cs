using FusionCore.AutorizacaoOperacao;
using NHibernate.Event;

namespace FusionCore.FusionNfce.Listeners
{
    public class SyncEventoOperacaoAutorizadaListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            if (@event.Entity is EventoOperacaoAutorizada entity)
            {
                var insert = @event.Session
                    .CreateSQLQuery(@"insert into sync_evento_operacao_autorizada(id) values(:id);");

                insert.SetGuid("id", entity.Id);
                insert.ExecuteUpdate();
            }
        }
    }
}