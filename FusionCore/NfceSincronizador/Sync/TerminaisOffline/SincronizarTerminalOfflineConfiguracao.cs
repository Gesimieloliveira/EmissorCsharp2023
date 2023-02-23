using System;
using System.Collections.Generic;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.NfceSincronizador.Sync.Start;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.NfceSincronizador.Sync.TerminaisOffline
{
    public class SincronizarTerminalOfflineConfiguracao : SincronizavelPadraoAdm
    {
        public SincronizarTerminalOfflineConfiguracao(string bindTerminal)
        {
            BindTerminal = bindTerminal;
        }

        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.TerminalOffline;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioTerminalOfflineServidor = new RepositorioTerminalOffline(sessaoServidor);
            var repositorioConfiguracaoTerminalNfce = new RepositorioConfiguracaoTerminalNfce(sessaoNfce);
            var configuracaoLocalNfce = repositorioConfiguracaoTerminalNfce.GetPeloId(1);
            var terminalOffline = repositorioTerminalOfflineServidor.ConfiguracaoNfce(BindTerminal);

            if (terminalOffline == null)
            {
                throw new InvalidOperationException("Terminal ainda não localizado. Vamos configura-lo!");
            }

            var configuracao = new ConfiguracaoTerminalNfce
            {
                Id = 1,
                BindTerminal = terminalOffline.BindTerminal,
                IntervaloSync = terminalOffline.IntervaloSync,
                TerminalOfflineId = terminalOffline.Id,
                ObservacaoPadrao = terminalOffline.Observacao
            };

            sessaoServidor.Flush();
            sessaoServidor.Clear();
            sessaoNfce.Flush();
            sessaoNfce.Clear();

            VerificaSeENecessarioInserirDadosASincronizar(sessaoServidor, sessaoNfce, configuracaoLocalNfce, configuracao);

            InsereOuAtualizaAConfiguracao(configuracao);
        }

        private void VerificaSeENecessarioInserirDadosASincronizar(ISession sessaoServidor, ISession sessaoNfce,
            ConfiguracaoTerminalNfce configuracaoLocalNfce, ConfiguracaoTerminalNfce configuracao)
        {
            if (configuracaoLocalNfce != null) return;

            var repositorioSincronizacao = new RepositorioSincronizacaoPendente(sessaoServidor);
            AdicionaTodosNaSync(repositorioSincronizacao, configuracao.TerminalOfflineId);

            sessaoServidor.Flush();
            sessaoServidor.Clear();
            sessaoNfce.Flush();
            sessaoNfce.Clear();

            TransacaoServidor.Commit();
            TransacaoNfce.Commit();
            CommitFeito = true;

            SessaoSistemaNfce.Configuracao = configuracao;
            new SincronizadorStart().Start();
        }

        private static void AdicionaTodosNaSync(RepositorioSincronizacaoPendente repositorio, byte idTerminal)
        {
            new AdicionarPrimeiraSincronizacao(repositorio).Executar(idTerminal);
        }

        private static void InsereOuAtualizaAConfiguracao(ConfiguracaoTerminalNfce configuracao)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorioConfiguracaoTerminalNfce = new RepositorioConfiguracaoTerminalNfce(sessao);
                repositorioConfiguracaoTerminalNfce.Salvar(configuracao);

                sessao.Flush();
                sessao.Clear();
                transacao.Commit();
            }
        }
    }
}