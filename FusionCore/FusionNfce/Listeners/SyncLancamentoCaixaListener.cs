using FusionCore.ControleCaixa;
using NHibernate.Event;

namespace FusionCore.FusionNfce.Listeners
{
    public class SyncLancamentoCaixaListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            if (!(@event.Entity is LancamentoAvulsoCaixa entity))
            {
                return;
            }

            var insert = @event.Session
                .CreateSQLQuery("insert into sync_caixa_lancamento(caixaLancamento_id) values(:id)");

            insert.SetGuid("id", entity.Id);
            insert.ExecuteUpdate();
        }
    }
}