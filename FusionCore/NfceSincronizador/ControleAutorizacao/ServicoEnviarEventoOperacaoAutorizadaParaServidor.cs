using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using FusionCore.AutorizacaoOperacao;
using NHibernate;

namespace FusionCore.NfceSincronizador.ControleAutorizacao
{
    public class ServicoEnviarEventoOperacaoAutorizadaParaServidor
    {
        private readonly SessaoSyncFactory _sessaoSync;

        public ServicoEnviarEventoOperacaoAutorizadaParaServidor(SessaoSyncFactory sessaoSync)
        {
            _sessaoSync = sessaoSync;
        }

        public void EnviarEventos()
        {
            var eventosPendentes = ObterEventosPendentes();

            foreach (var eventoId in eventosPendentes)
            {
                using (var sessaoNfce = _sessaoSync.CriarSessaoLocal(IsolationLevel.ReadCommitted))
                using (var sessaoServidor = _sessaoSync.CriarSessaoServidor(IsolationLevel.ReadCommitted))
                {
                    var evento = sessaoNfce.Get<EventoOperacaoAutorizada>(eventoId);

                    sessaoServidor.Persist(evento);
                    sessaoServidor.Flush();

                    DeletarEventoDaTabelaSincronizacao(eventoId, sessaoNfce);

                    sessaoServidor.Transaction.Commit();
                    sessaoNfce.Transaction.Commit();
                }
            }
        }


        private IEnumerable<Guid> ObterEventosPendentes()
        {
            using (var sessaoNfce = _sessaoSync.CriarSessaoLocal(IsolationLevel.ReadCommitted))
            {
                var query = sessaoNfce.CreateSQLQuery("select id from sync_evento_operacao_autorizada");
                var ids = query.List<Guid>();

                return ids;
            }
        }

        private void DeletarEventoDaTabelaSincronizacao(Guid eventoId, ISession sessaoNfce)
        {
            var query = sessaoNfce.CreateSQLQuery("delete from sync_evento_operacao_autorizada where id = :id");

            query.SetGuid("id", eventoId);
            query.ExecuteUpdate();
        }
    }
}