using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.TerminalOffline;
using FusionCore.Helpers.Wmi;
using FusionCore.NfceSincronizador;
using FusionCore.NfceSincronizador.Sync.EmissoresFiscais;
using FusionCore.NfceSincronizador.Sync.Start;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using NHibernate;
using NHibernate.Util;

namespace FusionNfce.Visao.ConfiguracaoTerminal
{
    public class ConfiguracaoTerminalFormModel : ModelValidation
    {
        private ObservableCollection<TerminalOfflineNfce> _listaTerminalDisponivel;

        [Required(ErrorMessage = @"Porfavor selecionar um terminal offline")]
        public TerminalOfflineNfce TerminalOffline
        {
            get { return GetValue(() => TerminalOffline); }
            set { SetValue(value); }
        }

        public ObservableCollection<TerminalOfflineNfce> ListaTerminalDisponivel
        {
            get { return _listaTerminalDisponivel; }
            set
            {
                if (Equals(value, _listaTerminalDisponivel)) return;
                _listaTerminalDisponivel = value;
                PropriedadeAlterada();
            }
        }

        public ConfiguracaoTerminalFormModel()
        {
            _listaTerminalDisponivel = new ObservableCollection<TerminalOfflineNfce>();
        }

        public void AtualizarDadosTela()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioTerminalOffline(sessao);
                var terminais = repositorio.TodosTerminaisSincronizaveis();

                ListaTerminalDisponivel = new ObservableCollection<TerminalOfflineNfce>();

                terminais.ForEach(t =>
                {
                    var terminalOfflineNfce = new TerminalOfflineNfce
                    {
                        Id = t.Id,
                        Descricao = t.Descricao,
                        Ativo = t.Ativo,
                        BindTerminal = t.BindTerminal,
                        IntervaloSync = t.IntervaloSync
                    };

                    foreach (var fiscal in t.EmissorFiscalLista)
                    {
                        terminalOfflineNfce.ListaEmissorNfce.Add(fiscal.ToNfce());
                    }

                    ListaTerminalDisponivel.Add(terminalOfflineNfce);
                });
            }
        }

        public void Salvar()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var sessaoServer = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
            var diskdrive = WmiHelper.GetDiskDriveZero();

            var transacao = sessao.BeginTransaction();
            var transacaoServer = sessaoServer.BeginTransaction();
            var bindTerminal = Md5Helper.Computar("agil4@" + diskdrive.SerialNumber);

            using (sessaoServer)
            using (sessao)
            using (transacao)
            using (transacaoServer)
            {
                var repositorio = new RepositorioConfiguracaoTerminalNfce(sessao);

                var configuracao = new ConfiguracaoTerminalNfce
                {
                    Id = 1,
                    IntervaloSync = TerminalOffline.IntervaloSync,
                    BindTerminal = bindTerminal,
                    TerminalOfflineId = TerminalOffline.Id,
                    ObservacaoPadrao = GetObservacaoPadrao(sessaoServer, TerminalOffline.Id)
                };

                configuracao.EmissorFiscalLista = TerminalOffline.ListaEmissorNfce;

                SessaoSistemaNfce.Configuracao = configuracao;

                AdicionaTodosNaSync(configuracao.TerminalOfflineId);

                new SincronizadorStart().Start();
                
                repositorio.Salvar(configuracao);

                SalvarTerminalNoServidor(sessaoServer, configuracao);

                transacaoServer.Commit();
                transacao.Commit();

                sessaoServer.Flush();
                sessaoServer.Clear();
                sessao.Flush();
                sessao.Clear();
            }
        }

        private string GetObservacaoPadrao(ISession servidor, byte terminalId)
        {
            var repositorio = new RepositorioTerminalOffline(servidor);
            var observacao = repositorio.GetObservacaoPadrao(terminalId);

            return observacao;
        }

        private static void AdicionaTodosNaSync(byte idTerminal)
        {
            var sessaoServer = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
            var repositorio = new RepositorioSincronizacaoPendente(sessaoServer);

            using (repositorio)
            {
                new AdicionarPrimeiraSincronizacao(repositorio).Executar(idTerminal);
            }
        }

        private void SalvarTerminalNoServidor(ISession sessaoServer, ConfiguracaoTerminalNfce configuracao)
        {
            var repositorioTerminalOfflineServer = new RepositorioTerminalOffline(sessaoServer);
            var terminalOffline = repositorioTerminalOfflineServer.GetPeloId(TerminalOffline.Id);

            terminalOffline.BindTerminal = configuracao.BindTerminal;
            terminalOffline.Impressora = string.Empty;

            repositorioTerminalOfflineServer.Salvar(terminalOffline);
        }

        public void CarregaConfiguracao()
        {
            CarregarConfiguracaoNfce();
        }

        private void CarregarConfiguracaoNfce()
        {
            var configuracaoTermialNfce = BuscaConfiguracao();

            if (configuracaoTermialNfce == null) return;

            TerminalOffline = new TerminalOfflineNfce
            {
                Id = configuracaoTermialNfce.TerminalOfflineId
            };

            SessaoSistemaNfce.Configuracao = configuracaoTermialNfce;
        }

        public ConfiguracaoTerminalNfce BuscaConfiguracao()
        {
            ConfiguracaoTerminalNfce configuracaoTerminalNfce;
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            using (sessao)
            {
                var repositorio = new RepositorioConfiguracaoTerminalNfce(sessao);


                configuracaoTerminalNfce = repositorio.GetPeloId(1);
            }
            return configuracaoTerminalNfce;
        }

        public void DeletaEmissorNfceEEmissorSat()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);

                repositorio.DeletarEmissorNfceEEmissorSat();

                transacao.Commit();
            }
        }
    }
}