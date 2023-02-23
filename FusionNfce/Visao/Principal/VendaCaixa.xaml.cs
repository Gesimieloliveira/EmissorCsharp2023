using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.FastReport.Relatorios.Sistema.Caixa;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Venda;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Papeis.Enums;
using FusionNfce.Core;
using FusionNfce.Visao.Backup;
using FusionNfce.Visao.Configuracao;
using FusionNfce.Visao.Configuracao.Impressao;
using FusionNfce.Visao.ConsultaProduto;
using FusionNfce.Visao.ConsultaVendedores;
using FusionNfce.Visao.Contigencia;
using FusionNfce.Visao.Login;
using FusionNfce.Visao.Principal.AvancaNumeracao;
using FusionNfce.Visao.Principal.CodigoAtivacaoSAT;
using FusionNfce.Visao.Principal.ConfiguracoesSAT;
using FusionNfce.Visao.Principal.ConsultaProdutoRapida;
using FusionNfce.Visao.Principal.FinalizarVenda;
using FusionNfce.Visao.Principal.Implementacoes.Caixa;
using FusionNfce.Visao.Principal.Model;
using FusionNfce.Visao.Principal.RecuperarVenda;
using FusionNfce.Visao.Principal.SolicitaInformacoes;
using FusionNfce.Visao.Principal.SolicitaTotal;
using FusionNfce.Visao.Principal.Tef;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using FusionWPF.SharedViews.AutorizarOperacao;
using FusionWPF.SharedViews.ControleCaixa;
using FusionWPF.Sobre;
using MahApps.Metro.SimpleChildWindow;
using NHibernate.Util;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace FusionNfce.Visao.Principal
{
    public partial class VendaCaixa
    {
        private readonly VendaModel _vendaModel;
        private ChildWindow _childAtual;
        private bool _existeDialogFecharAberta;

        public VendaCaixa()
        {
            _vendaModel = new VendaModel();
            _vendaModel.ConcluiuSincronizacaoNfceOffline += EnviouNfceOfflineSucesso;
            _vendaModel.SelecionaConteudoTextBoxKg += SelecionaConteudoTextBoxKg;
            _vendaModel.FinalizacaoRapidaHandler += FinalizacaoRapidaCalling;
            _vendaModel.PropertyChanged += OnPropertyChanged;
            _vendaModel.AtualizarListaItens += AtualizarListaItens;


            InitializeComponent();

            DataContext = _vendaModel;
        }

        private void VendaCaixa_OnLoaded(object sender, RoutedEventArgs e)
        {
            AplicarRestricaoModulos();
        }

        private async void VendaCaixa_OnContentRendered(object sender, EventArgs e)
        {
            await RunTaskWithProgress(() =>
            {
                _vendaModel.CarregarTabelasPrecos();
                _vendaModel.CarregarPreferencias();
                _vendaModel.DefinirTabelaPadrao();
                Thread.Sleep(1200);
            });

            if (SessaoSistemaNfce.Preferencia == null)
            {
                AcaoConfigurarPreferencias();
            }

            AdicionarVendedor();

            TbCodigoBarras.Focus();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_vendaModel.TabelaPrecoSelecionada))
            {
                OnChangedTabelaPrecos();
            }
        }

        private void OnChangedTabelaPrecos()
        {
            if (CbTabelaPrecos.IsKeyboardFocusWithin) TbCodigoBarras.Focus();
        }

        private void AdicionarVendedor()
        {
            _vendaModel.IsAdicionarVendedor = true;
        }

        private void AcaoConfigurarPreferencias()
        {
            var view = new PreferenciasTerminalView();

            view.ShowDialog();

            AdicionarVendedor();
        }

        private void AplicarRestricaoModulos()
        {
            if (SessaoSistemaNfce.AcessoConcedido.PossuiFusionGestor == false)
            {
                MenuItemAbrirCaixa.Visibility = Visibility.Collapsed;
                MenuItemGerenciarCaixa.Visibility = Visibility.Collapsed;
                MenuItemLancamentoCaixa.Visibility = Visibility.Collapsed;
                MenuItemImprimirCaixasFechados.Visibility = Visibility.Collapsed;
            }
        }

        private void FinalizacaoRapidaCalling(object sender, EventArgs e)
        {
            var peer =
                new ButtonAutomationPeer(ButtonFinalizacaoRapida);

            var invokeProv =
                peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

            invokeProv?.Invoke();
        }

        private async void SelecionaConteudoTextBoxKg(object sender, EventArgs e)
        {
            if (SessaoSistemaNfce.Preferencia?.SolicitaInformacaoItem == true)
            {
                SolicitarInformacoesDoItem();
                return;
            }

            var contexto = new SolicitaTotalContexto(_vendaModel.ProdutoManual, _vendaModel.Quantidade);

            _childAtual = ChildWindowFactory.Cria(new SolicitaTotalForm(contexto));

            contexto.FinalizadoSucesso += SolicitaTotalFinalizadoHandler;
            _childAtual.Closing += SolicitaTotalClosedHandler;

            await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void FinalizarSolicitaInformacoes(object sender, ProdutoComQuantidadePreco e)
        {
            try
            {
                var item = new ItemEspera(_vendaModel.ProdutoManual, string.Empty);

                var quantidade = e.QuantidadeCalculada();
                _vendaModel.Quantidade = quantidade;
                _vendaModel.ProdutoManual.ThrowPodeFracionar(quantidade);
                _vendaModel.AdicionaProdutoNaListaDeEspera(item);

                _childAtual.Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void SolicitaTotalFinalizadoHandler(object sender, SolicitaTotalContexto e)
        {
            var item = new ItemEspera(_vendaModel.ProdutoManual, string.Empty);

            _vendaModel.Quantidade = e.QuantidadeCalculada;
            _vendaModel.AdicionaProdutoNaListaDeEspera(item);

            _childAtual.Close();

            TbCodigoBarras.Focus();
        }

        private void SolicitaTotalClosedHandler(object sender, CancelEventArgs e)
        {
            _vendaModel.HabilitarCodigoBarras = true;
            _vendaModel.ProdutoManual = null;
            _vendaModel.Quantidade = 1.00M;

            _childAtual = null;

            TbCodigoBarras.Focus();
        }

        private void Venda_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    RemoverItem_OnClick(sender, e);
                    break;
                case Key.F3:
                    CancelarVendaEmAndamento_OnClick(sender, e);
                    break;
                case Key.F4:
                    FinalizarVenda();
                    break;
                case Key.F9:
                    ConsultaPrecoProduto_OnClick(sender, e);
                    break;
                case Key.F5:
                    RecuperarVenda_OnClick(sender, e);
                    break;
                case Key.F7:
                    AlteraItem_Click(sender, e);
                    break;
                case Key.F8:
                    AdicionaQuantidade_OnClick(sender, e);
                    break;
                case Key.F6:
                    ConsultarProduto_Click(sender, e);
                    break;
                case Key.F11:
                    ConsultarStatusSefaz_OnClick(sender, e);
                    break;
                case Key.F12:
                    MaisOpcoes_OnClick(sender, e);
                    break;
            }
        }

        private void ConsultarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotaPendente()) return;
            if (VerificaSePodeEditarVenda()) return;
            if (NaoConfirmouUsoPadraoTabelaPrecos()) return;

            var view = new ConsultaProdutosView(_vendaModel.ObterUltimaBuscaEfetuada(), _vendaModel.CarregaTabelaPrecoPorId(_vendaModel.TabelaPrecoSelecionada));

            view.Contexto.FoiSelecionado += (o, dto) =>
            {
                view.Close();

                if (SessaoSistemaNfce.Preferencia?.SolicitaInformacaoItem == true)
                {
                    _vendaModel.ProdutoManual = _vendaModel.BuscarProdutoPorId(dto.Id);

                    SolicitarInformacoesDoItem();
                    return;
                }

                _vendaModel.IniciaVenda(dto);
            };

            view.Contexto.UltimaBuscaEfetuadaSalvar += (o, efetuada) =>
            {
                _vendaModel.SalvarUltimaBuscaEfetuada(efetuada);
            };

            view.ShowDialog();
        }

        private void SolicitarInformacoesDoItem()
        {
            var contexto = new SolicitaInforamcoesItemContexto(_vendaModel.ProdutoManual, _vendaModel.Quantidade);
            var view = new SolicitaInforamcoesItemView(contexto);

            contexto.Finalizar += FinalizarSolicitaInformacoes;
            view.Closing += SolicitaTotalClosedHandler;

            _childAtual = view;

            this.ShowChildWindowAsync(view);
        }


        private void CodigoBarras_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                case Key.OemBackTab:
                    e.Handled = true;
                    break;

                case Key.Enter:
                    IniciaVenda();
                    break;
            }
        }

        private void IniciaVenda()
        {
            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);

                if (IsNotaPendente())
                {
                    TbCodigoBarras.Text = string.Empty;
                    return;
                }

                if (VerificaSePodeEditarVenda())
                {
                    TbCodigoBarras.Text = string.Empty;
                    return;
                }

                if (NaoConfirmouUsoPadraoTabelaPrecos())
                {
                    TbCodigoBarras.Text = string.Empty;
                    return;
                }

                _vendaModel.IniciaVenda();
                TbCodigoBarras.Focus();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private bool NaoConfirmouUsoPadraoTabelaPrecos()
        {
            if (SessaoSistemaNfce.Preferencia.ConfirmacaoTabelaPadraoAntesVenda == false) return false;
            if (_vendaModel.TabelaPrecoSelecionada == null) return false;
            if (_vendaModel.Nfce != null) return false;

            var msgConfirmacao = $"Continuar operação utilizando a tabela {_vendaModel.TabelaPrecoSelecionada.Descricao}";

            return !DialogBox.MostraDialogoDeConfirmacao(msgConfirmacao);
        }

        private void RemoverItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNotaPendente()) return;
            if (VerificaSePodeEditarVenda()) return;
            if (_vendaModel.Nfce == null) return;

            _vendaModel.RemoverItem();
        }

        private void AdicionaQuantidade_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNotaPendente()) return;
            if (VerificaSePodeEditarVenda()) return;
            _vendaModel.AdicionaQuantidade();
        }

        private void AlteraItem_Click(object sender, RoutedEventArgs e)
        {
            if (_vendaModel.Nfce == null) return;
            if (VerificaSePodeEditarVenda()) return;

            _vendaModel.AlterarItem();
        }

        private void CancelarVendaEmAndamento_OnClick(object sender, RoutedEventArgs e)
        {
            var venda = _vendaModel.Nfce;

            if (venda == null)
            {
                return;
            }

            var payload = new NfceCancelada(venda.Id, venda.TotalNfce);
            var autorizarUsuario = new AutorizarUsuarioNfce(SessaoSistemaNfce.SessaoManager);
            var autorizarCancelamento = new AutorizarOperacaoView(SessaoSistemaNfce.SessaoManager, autorizarUsuario, SessaoSistemaNfce.Usuario, venda.Id.ToString(), Permissao.CANCELAR_NFCE, payload, () =>
            {
                CancelaVendaEmAndamento();
            });

            autorizarCancelamento.ExecutarAcao();

        }

        public async void CancelaVendaEmAndamento()
        {
            if (_vendaModel.Nfce == null) return;

            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);
                SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CANCELAR_VENDA_ANDAMENTO);

                if (VerificaSePodeEditarVenda()) return;

                if (!DialogBox.MostraConfirmacao("Deseja realmente cancelar a venda que esta em andamento?",
                    MessageBoxImage.Question)) return;

                await _vendaModel.CancelarVendaEmAndamento();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }

        }

        private void RecuperarVenda_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_RECUPERAR_VENDA);
                VerificaVendaEmAndamento();

                if (IsNotaPendente()) return;
                if (VerificaSePodeEditarVenda()) return;

                var model = new RecuperarVendaFormModel();
                model.RetornaNfce += _vendaModel.RecuperarVenda;

                new RecuperarVendaForm(model).ShowDialog();

                _vendaModel.AtualizarStatusVendaPendente();
                TbCodigoBarras.Focus();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void VerificaVendaEmAndamento()
        {
            _vendaModel.VerificaVendaEmAndamento();
        }

        private void ConsultaPrecoProduto_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNotaPendente()) return;
            if (VerificaSePodeEditarVenda()) return;
            new ConsultaProdutoRapidaForm(new ConsultaProdutoRapidaFormModel(_vendaModel.CarregaTabelaPrecoPorId(_vendaModel.TabelaPrecoSelecionada))).ShowDialog();
        }

        private bool IsNotaPendente()
        {
            if (_vendaModel.StatusCaixa == StatusCaixa.AlterarItem) return false;

            if (_vendaModel.Nfce == null) return false;

            var isNotaPendente = _vendaModel.Nfce.Status == Status.PendenteOffline;

            if (isNotaPendente)
            {
                DialogBox.MostraInformacao("Esta nota está como pendente, houve alguma rejeição com a nota");
            }

            return isNotaPendente;
        }

        private bool VerificaSePodeEditarVenda()
        {
            if (!_vendaModel.IsContemHistoricoNaoFinalizado) return false;

            DialogBox.MostraInformacao("Tente finalizar a venda antes de editar a mesma. (F4 para finalizar)");

            return true;
        }

        private void FinalizarVenda_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            FinalizarVenda();
        }

        private void FinalizarVenda()
        {
            if (_vendaModel.Nfce == null) return;

            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);

                _vendaModel.FinalizarVenda();
                var nfce = _vendaModel.AtualizaObjetoNfce();

                var model = new FinalizarVendaFormModel(nfce);
                model.FinalizouPagamento += _vendaModel.FinalizouPagamento;

                new FinalizarVendaForm(model).ShowDialog();

                if (_vendaModel.Nfce?.Id == nfce.Id)
                {
                    _vendaModel.LimpaDados();
                    _vendaModel.Nfce = nfce;
                    _vendaModel.RecuperaVenda();
                }

                _vendaModel.AtualizarStatusVendaPendente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void VendaCaixa_OnClosing(object sender, CancelEventArgs e)
        {
            if (_existeDialogFecharAberta)
            {
                e.Cancel = true;
                return;
            }

            _existeDialogFecharAberta = true;
            if (DialogBox.MostraConfirmacao("Deseja realmente fechar?", MessageBoxImage.Question))
            {
                _existeDialogFecharAberta = false;
                e.Cancel = false;
                Application.Current.Shutdown();
                return;
            }

            _existeDialogFecharAberta = false;
            e.Cancel = true;
        }

        private void MaisOpcoes_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                VerificaVendaEmAndamento();

                if (IsNotaPendente()) return;
                if (VerificaSePodeEditarVenda()) return;

                var model = new MaisOpcoesFormModel(Dispatcher);

                model.RetornoConversaoPedidoVenda += delegate (object o, RetornoConversaoPedidoVenda venda)
                {
                    _vendaModel.RecuperarVenda(this, new NfceEvent(venda.GetNfce()));
                };

                new MaisOpcoesForm(model).ShowDialog();

                _vendaModel.AtualizarStatusVendaPendente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private async void ConsultarStatusSefaz_OnClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => _vendaModel.ConsultaStatus());
        }

        private void Contigencia_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CONTINGENCIA_OFFLINE_NFCE);

            var model = new ContigenciaFormModel();
            var dialog = new ContigenciaForm(model);
            dialog.ShowDialog();

            _vendaModel.IsContingencia = SessaoSistemaNfce.EstaEmContingencia();
        }

        private void Avisos_OnClick(object sender, RoutedEventArgs e)
        {
            _vendaModel.AbrirTelaAvisos();
        }

        private void Finalizar_OnClick(object sender, RoutedEventArgs e)
        {
            FinalizarVenda();
        }

        private async void SincronizarNfceOffline_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            await Task.Run(() => _vendaModel.SincronziarNfceOffline());
            _vendaModel.AtualizarStatusVendaPendente();
        }

        private void EnviouNfceOfflineSucesso(object sender, EventArgs e)
        {
            Dispatcher.Invoke(_vendaModel.AtualizarStatusVendaPendente);
            Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
        }

        private void FinalizacaoRapida_Click(object sender, RoutedEventArgs e)
        {
            _vendaModel.FinalizacaoRapida();
        }

        private void ExtrairLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vendaModel.ExtrairLogs();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void BloquearSAT_Click(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_BLOQUEAR_SAT);

            try
            {
                _vendaModel.BloquearSat();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void DesbloquearSAT_Click(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_DESBLOQUEAR_SAT);

            try
            {
                _vendaModel.DesbloquearSat();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void AtualizarSAT_Click(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_ATUALIZAR_SAT);

            try
            {
                _vendaModel.AtualizarSat();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void ConsultarStatusOperacionalSAT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vendaModel.ConsultarStatusSAT();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void TrocarCodigoDeAtivacao_Click(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_TROCAR_CODIGO_ATIVACAO_SAT);
            try
            {
                new TrocaCodigoAtivacaoSATForm().ShowDialog();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private void ConfiguracaosSAT_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CONFIGURACAO_SAT);

            new ConfiguracoesSATForm().ShowDialog();
        }

        private void Backup_OnClick(object sender, RoutedEventArgs e)
        {
            new BackupForm().ShowDialog();
        }

        private void Logout_OnClick(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Quer mesmo fazer o logout?") != MessageBoxResult.Yes)
                return;

            new LoginForm().Show();
            Closing -= VendaCaixa_OnClosing;
            Close();
        }

        private void CertificadoDigital_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_TROCAR_CERTIFICADO_DIGITAL);
            TrocarCertificadoDigital.Trocar();
        }

        private void Sobre_OnClick(object sender, RoutedEventArgs e)
        {
            new SobreForm().ShowDialog();
        }

        private void ConexaoDiretaImpressora_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CONEXAO_DIRETA_IMPRESSORA);
            new ConfiguracaoImpressaoDiretaForm().ShowDialog();
            _vendaModel.AtualizarAbrirGaveta();
        }

        private void AbrirGaveta_Click(object sender, RoutedEventArgs e)
        {
            if (SessaoSistemaNfce.ImpressaoDireta.Desativa)
            {
                DialogBox.MostraInformacao("Ativar impressao direta");
                return;
            }

            _vendaModel.AbrirGaveta();
        }

        private void VendaCaixa_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.G &&
                SessaoSistemaNfce.ImpressaoDireta.Ativa)
            {
                e.Handled = true;
                AbrirGaveta_Click(sender, e);
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.R)
            {
                e.Handled = true;
                FinalizacaoRapida_Click(sender, e);
            }
        }

        private void AvancarNumeracao_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_AVANCAR_NUMERACAO_NFCE);

            if (_vendaModel.Nfce == null || _vendaModel.Nfce.NumeroDocumento == 0)
            {
                DialogBox.MostraInformacao(
                    "Não posso avançar a númeração fiscal, pois não achei nem uma tentativa de emissão no momento");
                return;
            }

            if (!DialogBox.MostraConfirmacao("Deseja realmente avançar a númeração fiscal da nfc-e?",
                MessageBoxImage.Question))
                return;

            new AvancaNumeracaoFiscal(new AvancaNumeracaoFiscalModel(_vendaModel.Nfce)).ShowDialog();
        }

        private async void TefAdministrativo_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessaoSistemaNfce.ConfigTef.NaoEstaAtivo)
            {
                DialogBox.MostraInformacao("Ative o TEF em Configuração TEF");
                return;
            }

            await _vendaModel.Tef_Adm();
        }

        private void TefConfigura_OnClick(object sender, RoutedEventArgs e)
        {
            SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.PDV_CONFIGURA_TEF_NFCE);
            new ConfiguraTefForm().ShowDialog();
            _vendaModel.IsTefAtivo = SessaoSistemaNfce.ConfigTef.IsAtivo;
        }

        private void AbrirCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var contexto =
                    new AberturaDeCaixaContexto(SessaoSistemaNfce.CaixaProvider, SessaoSistemaNfce.SessaoManager);
                var view = new AberturaDeCaixaView(contexto);

                view.ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void FecharCaixaAbertoClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                CaixaIndividual caixa = null;

                using (var sessao = SessaoSistemaNfce.SessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioCaixaIndividual(sessao);
                    caixa = repositorio.BuscarCaixaAberto(SessaoSistemaNfce.Usuario, ELocalEventoCaixa.Terminal);
                }

                if (caixa == null)
                {
                    throw new InvalidOperationException("Usuário não possui caixa aberto!");
                }

                var contexto = new ResumoCaixaIndividualContexto(
                    SessaoSistemaNfce.SessaoManager,
                    SessaoSistemaNfce.CaixaProvider,
                    caixa.Id
                );

                var view = new ResumoCaixaIndividualView(contexto);

                view.ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void GerenciarLancamentosDeCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            var control =
                new GridLancamentosCaixaControl(SessaoSistemaNfce.SessaoManager, SessaoSistemaNfce.CaixaProvider);

            var view = FusionWindowFactory.Criar(
                "Gerenciar lançamentos",
                control,
                new FusionWindowFactory.WSize(980, 550)
            );

            view.ShowDialog();
        }

        private void ImprimirCaixasFechadosClickHandler(object sender, RoutedEventArgs e)
        {
            using (var r = new RListagemDeCaixasFechados(SessaoSistemaNfce.SessaoManager))
            {
                r.Visualizar();
            }
        }

        private void ClickConfigurarPreferenciasTerminal(object sender, RoutedEventArgs e)
        {
            AcaoConfigurarPreferencias();
        }

        // todo adicionar vendedor
        private void ClickAdicionarVendedor(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_vendaModel.NaoTemVendedorCadastrado())
                    throw new InvalidOperationException("Não tem vendedor cadastrado no sistema, cadastre um para poder adicionar");

                var model = new ConsultaVendedorFormModel();
                model.FoiSelecionado += delegate (object o, VendedorNfce vendedor)
                {
                    _vendaModel.AdicionarVendedor(vendedor);
                };
                new ConsultaVendedorForm(model).ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {

            if (_vendaModel.Nfce == null) return;
            if (VerificaSePodeEditarVenda()) return;


            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);

                if (IsNotaPendente())
                {
                    TbCodigoBarras.Text = string.Empty;
                    return;
                }

                if (VerificaSePodeEditarVenda())
                {
                    TbCodigoBarras.Text = string.Empty;
                    return;
                }

                _vendaModel.BloquearCaixaCorrigirVendasPendente();

                _vendaModel.FecharSistemaSeAcessoInvalido();

                var listBoxItem = sender as ListBoxItem;
                var nfceItem = listBoxItem.DataContext as NfceItem;

                new ComandoCaixaAlterarItem().ExecutaAcao(_vendaModel,
                    nfceItem.NumeroItem.ToString());
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                TbCodigoBarras.Focus();
            }
        }

        private void OnGotFocusTabelaPrecos(object sender, RoutedEventArgs e)
        {
            if (_vendaModel.Nfce == null) return;
            if (!_vendaModel.Itens.Any()) return;

            const string confirmacao = "Ao alterar a tabela de preços o preço dos itens serão reajustados. Deseja continuar?";
            if (!DialogBox.MostraDialogoDeConfirmacao(confirmacao))
            {
                e.Handled = true;
                TbCodigoBarras.Focus();
            }
        }

        private void AtualizarListaItens(object sender, EventArgs e)
        {
            VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(ListaDeItens)?.ScrollToTop();
        }
    }
}