using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Individual;
using NHibernate.Event;

namespace FusionCore.FusionNfce.Listeners
{
    public class SyncCaixaInidividualListener : IPostUpdateEventListener, IPostInsertEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            ChecarAlteracaoEmCaixa(@event);
        }

        private void ChecarAlteracaoEmCaixa(IPostDatabaseOperationEventArgs @event)
        {
            if (@event.Entity is CaixaIndividual caixa)
            {
                InserirCaixaParaSincronizacao(caixa, @event);
            }

            if (@event.Entity is Fluxo fluxo)
            {
                InserirCaixaParaSincronizacao(fluxo.Caixa, @event);
            }
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            ChecarAlteracaoEmCaixa(@event);
        }

        private void InserirCaixaParaSincronizacao(CaixaIndividual caixa, IDatabaseEventArgs @event)
        {
            var insert = @event.Session
                .CreateSQLQuery(@"insert into sync_caixa_individual(caixaIndividual_id) select :id 
                    where not exists(
                        select b.caixaIndividual_id from sync_caixa_individual b where b.caixaIndividual_id = :id
                    );"
                );

            insert.SetGuid("id", caixa.Id);
            insert.ExecuteUpdate();
        }
    }
}