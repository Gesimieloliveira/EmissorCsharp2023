using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Base
{
    public abstract class SincronizavelPadraoAdm : ISincronizavelPadrao
    {
        protected string BindTerminal { get; set; }
        protected ISession OpenSessionNfce => GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
        protected ISession OpenSessionServidor => GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();

        protected SincronizavelPadraoAdm()
        {
            SessaoServidor = OpenSessionServidor;
            SessionNfce = OpenSessionNfce;

            SincronizacaoPendentesADeletar = new List<SincronizacaoPendente>();
        }

        private ISession SessaoServidor { get; }
        private ISession SessionNfce { get; }
        public ITransaction TransacaoServidor;
        public ITransaction TransacaoNfce;
        protected readonly IList<SincronizacaoPendente> SincronizacaoPendentesADeletar;
        private int HoraDeFlush { get; set; }
        public bool CommitFeito { get; set; }
        private ConfiguracaoTerminalNfce _configuracaoTerminalNfce;
         
        protected abstract EntidadeSincronizavel EntidadeSincronizavel { get; }

        public void RealizarSincronizacao()
        {
            TransacaoServidor = SessaoServidor.BeginTransaction();
            TransacaoNfce = SessionNfce.BeginTransaction();

            using (SessaoServidor)
            using (SessionNfce)
            using (TransacaoServidor)
            using (TransacaoNfce)
            {   
                var repositorioTerminalOfflineConfiguracao = new RepositorioConfiguracaoTerminalNfce(SessionNfce);
                var repositorioSincronizacaoPendente = new RepositorioSincronizacaoPendente(SessaoServidor);

                var todasSincronizacaoPendentes = TentaAcharConfiguracaoNoServidorSeAverBinding(repositorioTerminalOfflineConfiguracao, repositorioSincronizacaoPendente);

                Sincroniza(todasSincronizacaoPendentes, SessaoServidor, SessionNfce);

                SincronizacaoPendentesADeletar.ForEach(sp =>
                {
                    repositorioSincronizacaoPendente.Deletar(sp);
                });

                if (CommitFeito)
                    return;

                SessaoServidor.Flush();
                SessaoServidor.Clear();
                SessionNfce.Flush();
                SessionNfce.Clear();

                TransacaoServidor.Commit();
                TransacaoNfce.Commit();
            }
        }

        private IList<SincronizacaoPendente> TentaAcharConfiguracaoNoServidorSeAverBinding(
            RepositorioConfiguracaoTerminalNfce repositorioTerminalOfflineConfiguracao,
            IRepositorioSincronizacaoPendente repositorioSincronizacaoPendente)
        {
            if (!string.IsNullOrEmpty(BindTerminal)) return null;
            
                _configuracaoTerminalNfce = repositorioTerminalOfflineConfiguracao.GetPeloId(1);

                SessionNfce.Clear();

                if (_configuracaoTerminalNfce == null)
                {
                    _configuracaoTerminalNfce = SessaoSistemaNfce.Configuracao;
                }

            if (_configuracaoTerminalNfce == null)
            {
                throw new InvalidOperationException("Este terminal não foi configurado. Preciso que configure!");
            }
            

            var todasSincronizacaoPendentes = repositorioSincronizacaoPendente.BuscaTodosParaSincronizacao(
                EntidadeSincronizavel, _configuracaoTerminalNfce.TerminalOfflineId);
            return todasSincronizacaoPendentes;
        }

        protected abstract void Sincroniza(IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor, ISession sessaoNfce);

        protected void ExecutarFlush()
        {
            HoraDeFlush++;

            if (HoraDeFlush%20 != 0) return;

            SessaoServidor.Flush();
            SessaoServidor.Clear();
            SessionNfce.Flush();
            SessionNfce.Clear();
        }
    }
}