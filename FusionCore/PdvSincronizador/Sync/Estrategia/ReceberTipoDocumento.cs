using System;
using System.Collections.Generic;
using FusionCore.FusionPdv.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionPdv;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberTipoDocumento : SincronizacaoBase
    {
        public override string Tag => @"receber-tipodocumento";
        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var tipoDocumentos = ObterTipoDocumentosAlterados(ultimaSincronizacao);
            if (tipoDocumentos.Count == 0)
                return;


            using (var transacao = SessaoPdv.BeginTransaction())
            {
                var repositorio = new RepositorioTipoDocumentoPdv(SessaoPdv);

                tipoDocumentos.ForEach(repositorio.Salvar);

                transacao.Commit();
            }

            RegistraEvento = true;
        }

        private IList<TipoDocumentoPdv> ObterTipoDocumentosAlterados(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioTipoDocumento(SessaoAdm);

            var lista = repositorio.ListaParaSincronizacao(ultimaSincronizacao);

            return lista;
        }
    }
}