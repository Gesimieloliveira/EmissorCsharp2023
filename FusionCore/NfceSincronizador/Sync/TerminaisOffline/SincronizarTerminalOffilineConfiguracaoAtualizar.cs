using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.TerminalOffline;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.TerminaisOffline
{
    public class SincronizarTerminalOffilineConfiguracaoAtualizar : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.TerminalOffline;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor,
            ISession sessaoNfce)
        {
            var repositorioTerminalOfflineServidor = new RepositorioTerminalOffline(sessaoServidor);
            var repositorioConfiguracaoTerminalNfce = new RepositorioConfiguracaoTerminalNfce(sessaoNfce);

            var configuracaoLocalNfce = repositorioConfiguracaoTerminalNfce.GetPeloId(1);

            var terminalOffline = repositorioTerminalOfflineServidor.GetPeloId(configuracaoLocalNfce.TerminalOfflineId);

            if (!string.IsNullOrEmpty(configuracaoLocalNfce.BindTerminal) &&
                string.IsNullOrEmpty(terminalOffline.BindTerminal))
            {
                configuracaoLocalNfce.BindTerminal = string.Empty;
                repositorioConfiguracaoTerminalNfce.Deletar(configuracaoLocalNfce);
                pendentes.ForEach(SincronizacaoPendentesADeletar.Add);

                sessaoServidor.Flush();
                sessaoServidor.Clear();
                sessaoNfce.Flush();
                sessaoNfce.Clear();
                return;
            }

            if (configuracaoLocalNfce == null)
            {
                throw new InvalidOperationException(
                    "Porfavor vamos configurar seu terminal, abra o sistema para configurar ele.");
            }


            SalvarTerminalNoServidor(terminalOffline, configuracaoLocalNfce, repositorioTerminalOfflineServidor);


            var configuracao = MontaObjetoConfiguracaoDeNfce(terminalOffline, configuracaoLocalNfce, sessaoNfce);

            pendentes.ForEach(SincronizacaoPendentesADeletar.Add);

            sessaoServidor.Flush();
            sessaoServidor.Clear();
            sessaoNfce.Flush();
            sessaoNfce.Clear();

            InsereOuAtualizaAConfiguracao(configuracao);
        }

        // ReSharper disable once RedundantAssignment
        private static ConfiguracaoTerminalNfce MontaObjetoConfiguracaoDeNfce(TerminalOffline terminalOffline,
            ConfiguracaoTerminalNfce configuracaoLocalNfce,
            ISession sessaoNfce){

            var configuracao = new ConfiguracaoTerminalNfce
            {
                Id = 1,
                BindTerminal = terminalOffline.BindTerminal,
                IntervaloSync = terminalOffline.IntervaloSync,
                TerminalOfflineId = terminalOffline.Id,
                TipoEmissao = configuracaoLocalNfce.TipoEmissao,
                JustificativaUltimaContigencia = configuracaoLocalNfce.JustificativaUltimaContigencia ?? string.Empty,
                DataUltimaContingencia = configuracaoLocalNfce.DataUltimaContingencia,
                ObservacaoPadrao = terminalOffline.Observacao
            };

            var repositorioTerminal = new RepositorioConfiguracaoTerminalNfce(sessaoNfce);
            var listaEmissorNfce = repositorioTerminal.BuscarTodosEmissorDoTerminal(terminalOffline.Id);

            foreach (var nfceEmissorFiscal in listaEmissorNfce)
            {
                nfceEmissorFiscal.TerminalOfflineId = null;
            }

            foreach (var fiscal in terminalOffline.EmissorFiscalLista)
            {
                var nfceEmissor = listaEmissorNfce.First(x => x.Id == fiscal.Id);
                nfceEmissor.TerminalOfflineId = terminalOffline.Id;
            }

            foreach (var nfceEmissorFiscal in listaEmissorNfce)
            {
                new RepositorioEmissorFiscalNfce(sessaoNfce).SalvarENaoSincronizar(nfceEmissorFiscal);
            }

            configuracao.EmissorFiscalLista = listaEmissorNfce;

            return configuracao;
        }

        private static void SalvarTerminalNoServidor(
            TerminalOffline terminalOffline,
            ConfiguracaoTerminalNfce configuracaoLocalNfce,
            RepositorioTerminalOffline repositorioTerminalOfflineServidor
        ){
            terminalOffline.BindTerminal = configuracaoLocalNfce.BindTerminal;
            terminalOffline.Impressora = string.Empty;

            repositorioTerminalOfflineServidor.Salvar(terminalOffline);
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