using System.Data;
using FusionCore.FusionNfce.Sessao;
using NHibernate;

namespace FusionCore.NfceSincronizador
{
    public class SessaoSyncFactory
    {
        public ISession CriarSessaoLocal(IsolationLevel? isolationLevel = null)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            if (isolationLevel != null)
            {
                sessao.BeginTransaction(isolationLevel.Value);
            }

            return sessao;
        }

        public ISession CriarSessaoServidor(IsolationLevel? isolationLevel = null)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();

            if (isolationLevel != null)
            {
                sessao.BeginTransaction(isolationLevel.Value);
            }

            return sessao;
        }
    }
}