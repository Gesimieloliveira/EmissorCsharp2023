using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Facades;
using Fusion.Sessao;
using Fusion.Visao.PedidoDeVenda.Finalizacao;
using Fusion.Visao.PedidoDeVenda.Preferencias;
using Fusion.Visao.PedidoDeVenda.Servicos;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Pessoa.Picker.OpcoesBusca;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TrocarEmpresa;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Preferencias;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.PedidoDeVenda
{
    public partial class PedidoVendaForm
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly PedidoVendaFormModel _contexto;
        private ChildWindow _dialogAtual;
        private bool _carregandoPedido;

        public PedidoVendaForm()
        {
            _contexto = new PedidoVendaFormModel(_sessaoManager);
            _contexto.Fechar += Fechar;
            _contexto.AtualizaTabelaPrecoListagem += AtualizaTabelaPrecoListagemAcao;

            InitializeComponent();
            _contexto.PropertyChanged += PropertyChangedHandler;
        }

        private void AtualizaTabelaPrecoListagemAcao(object sender, ITabelaPreco tabelaPreco)
        {
            ProdutoComboPicker.AtualizarTabelaPreco(tabelaPreco);
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.PropertyName == nameof(PedidoVendaFormModel.PermiteEdicao))
                {
                    BlocoBotoes.Visibility = _contexto.PermiteEdicao ? Visibility.Visible : Visibility.Collapsed;
                }
            });
        }

        private void RootLoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.IniciarNovo();

            DataContext = _contexto;
            ProdutoComboPicker.FocusTbCodigo();
        }

        private void ContentRendereHandler(object sender, EventArgs e)
        {
            var preferenciaFacade = new PedidoVendaPreferenciaFacade();

            if (preferenciaFacade.PossuiPreferenciaParaMaquina() == false)
            {
                preferenciaFacade.SalvarPadrao();
                new PedidoVendaPreferenciaForm().ShowDialog();
            }

            if (_contexto.PedidoEmAndamento || _carregandoPedido)
            {
                return;
            }

            AcaoEscolherTipoPedido();
        }

        private void AcaoEscolherTipoPedido()
        {
            var contexto = new TipoPedidoContexto();

            _dialogAtual = new TipoPedidoControl(contexto);

            contexto.EscolhaCompleta += (o, tipo) =>
            {
                _contexto.IniciarNovo();
                _contexto.MarcarComo(tipo);
            };

            _dialogAtual.Closing += (sender, args) =>
            {
                ChildWindowClosingHandler(sender, args);

                if (_contexto.PedidoEmAndamento || contexto.TipoEscolhido != null)
                {
                    return;
                }

                Close();
            };

            this.ShowChildWindowAsync(_dialogAtual);
        }

        private void WindowKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (_dialogAtual?.IsOpen == true || _contexto.PedidoVenda.EstaCancelado)
            {
                return;
            }

            if (e.Key == Key.F12)
            {
                e.Handled = true;
                AcaoTrocarTipoDocumento();
                return;
            }

            if (e.Key == Key.F11)
            {
                e.Handled = true;
                AcaoAdicionarDestinatario();
                return;
            }

            if (e.SystemKey == Key.F10)
            {
                e.Handled = true;
                AcaoVisitante();
                return;
            }

            if (e.Key == Key.F9)
            {
                e.Handled = true;
                AcaoAdicionarVendedor();
                return;
            }

            if (e.Key == Key.F6)
            {
                e.Handled = true;
                AcaoAplicaDesconto();
                return;
            }

            if (e.Key == Key.F4)
            {
                e.Handled = true;
                Finaizar_OnClick(sender, e);
                return;
            }

            if (e.Key == Key.F3)
            {
                e.Handled = true;
                AcaoCancelar();
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.R)
            {
                e.Handled = true;
                AcaoEditarReferencia();
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.N)
            {
                e.Handled = true;
                AcaoEscolherTipoPedido();
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                e.Handled = true;
                AcaoListarPedidoAberto();
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                e.Handled = true;
                AcaoEditarObservacao();
            }
        }

        private void AcaoAplicaDesconto()
        {
            if (_contexto.PedidoEstaAberto == false)
            {
                AcaoPedidoNaoEstaAberto();
                return;
            }

            if (_contexto.PedidoEmAndamento == false)
            {
                DialogBox.MostraAviso("Preciso de um pedido para dar o desconto!");
                return;
            }

            if (!_contexto.PossuiItens())
            {
                DialogBox.MostraAviso("Pedido não possui itens, preciso de itens para o desconto!");
                return;
            }

            var descontoCtx = new DescontoNoTotalPedidoVendaModel();
            var control = new DescontoNoTotalPedidoVenda(descontoCtx);

            descontoCtx.Update(_contexto.TotalProdutos, _contexto.PercentualDesconto);
            descontoCtx.DescontoAplicado += DescontoAplicadoHandler;

            AbrirChildWindow(control, titulo: "Aplicar desconto documento");
        }

        private void DescontoAplicadoHandler(object sender, DescontoArgs args)
        {
            _contexto.PercentualDesconto = args.Percentual;
            _contexto.AplicarDescontoNoTotal();

            _dialogAtual.Close();
        }

        private void AcaoPedidoNaoEstaAberto()
        {
            DialogBox.MostraAviso("Documento não está aberto, remova a finalização para reabri-lo.");
        }

        private bool QuantidadeNaoValida(decimal quantidade)
        {
            if (quantidade > 0)
            {
                return false;
            }

            return true;
        }

        private void ClickAlterarEmpresaHandler(object sender, MouseButtonEventArgs e)
        {
            if (_contexto.PedidoEstaAberto == false)
            {
                AcaoPedidoNaoEstaAberto();
                return;
            }

            //TODO: Corrigir utilizacao do controle -> foi usado no faturamento tb
            var childModel = new TrocarEmpresaViewModel(_sessaoManager);
            var childView = new TrocarEmpresaView(childModel);

            childModel.SelecionouEmpresa += (o, empresa) =>
            {
                _contexto.TrocaEmpresaPara(empresa);
                DialogBox.MostraInformacao("Empresa alterada com sucesso");
            };

            AbrirChildWindow(childView);
        }

        private void AbrirChildWindow(ChildWindow child)
        {
            _dialogAtual = child;
            _dialogAtual.Closing += ChildWindowClosingHandler;

            this.ShowChildWindowAsync(_dialogAtual);
        }

        private void AbrirChildWindow(UserControl content, string titulo = null)
        {
            _dialogAtual = ChildWindowFactory.Cria(content, titulo: titulo);
            _dialogAtual.Closing += ChildWindowClosingHandler;

            this.ShowChildWindowAsync(_dialogAtual);
        }

        private void ChildWindowClosingHandler(object sender, CancelEventArgs e)
        {
            _contexto.NotificarAlteracaoEstado();
            _dialogAtual = null;

            ProdutoComboPicker.FocusTbCodigo();
        }

        private void AcaoAdicionarDestinatario()
        {
            var picker = new PessoaPickerModel(new ClienteEngine())
            {
                Titulo = "Escolha um cliente para o pedido"
            };

            picker.UsarBusca<BuscaPessoaPickerPadrao>();


            picker.PickItemEvent += (o, args) =>
            {
                try
                {
                    _contexto.ComDestinatario(args.GetItem<Cliente>());
                }
                catch (InvalidOperationException e)
                {
                    DialogBox.MostraInformacao(e.Message);
                }
            };

            picker.GetPickerView().ShowDialog();
        }

        private void AdicionarCliente_OnClick(object sender, RoutedEventArgs e)
        {
            AcaoAdicionarDestinatario();
        }

        private void ExcluirPedidoVendaProduto(object sender, RoutedEventArgs e)
        {
            const string aviso = "Continuar com a exclusão deste item?";

            if (DialogBox.MostraConfirmacao(aviso) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _contexto.DeletarItemSelecionado();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EditarPedidoVendaProduto(object sender, RoutedEventArgs e)
        {
            var contexto = new EditarItemViewModel();
            var content = new EditarItemView(contexto);

            contexto.UpdateModel(_contexto.ItemSelecionado);
            contexto.IsPedidoVenda = _contexto.IsPedidoVenda;

            contexto.CompletoSucesso += (o, args) =>
            {
                _contexto.UpdateItemSelecionado(args);
                _dialogAtual.Close();
            };

            AbrirChildWindow(content, "Alterar item do documento");
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void AcaoTrocarTipoDocumento()
        {
            try
            {
                if (_contexto.IsOrcamento)
                {
                    _contexto.MarcarComoPedidoVenda();
                    return;
                }

                if (_contexto.IsPedidoVenda)
                {
                    _contexto.MarcarComoOrcamento();
                }
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ListarPedidoAberto_OnClick(object sender, RoutedEventArgs e)
        {
            AcaoListarPedidoAberto();
        }

        private void AcaoListarPedidoAberto()
        {
            var listagemContexto = new ListaPedidoVendaControlModel();
            var content = new ListaPedidoVendaControl(listagemContexto);

            listagemContexto.FoiSelecionado += (o, dto) =>
            {
                _dialogAtual.Close();
                CarregarPedidoAsync(dto);
            };

            AbrirChildWindow(content, "Listagem de documentos para seleção");
        }

        private void IniciarNovoPedidoVenda_OnClick(object sender, RoutedEventArgs e)
        {
            AcaoEscolherTipoPedido();
        }

        private void ReferenciaClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEditarReferencia();
        }

        private void VisitanteClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoVisitante();
        }

        private void AcaoVisitante()
        {
            var contexto = new ClienteVisitanteContexto();
            var content = new ClienteVisitanteControl(contexto);

            if (_contexto.IsTemDestinatario)
            {
                var visitante = _contexto.GetVisitante();
                contexto.Update(visitante);
            }

            contexto.FinalizadoSucesso += (o, args) =>
            {
                _contexto.ComVisitante(args);
                _dialogAtual.Close(true);
            };

            AbrirChildWindow(content, "Adicionar visitante");
        }

        private void CancelarClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoCancelar();
        }

        private void AcaoCancelar()
        {
            if (_contexto.PedidoEmAndamento == false)
            {
                DialogBox.MostraAviso("Preciso de um documento iniciado para cancelar!");
                return;
            }

            var formModel = new FormCancelamentoPedidoModel();
            var janela = new FormCancelamentoPedido(formModel);

            var resultado = janela.ShowDialog();

            if (resultado == false)
            {
                return;
            }

            try
            {
                _contexto.CancelarDocumento(formModel.MotivoCancelamento);

                DialogBox.MostraInformacao("Documento foi cancelado com sucesso");
                AcaoEscolherTipoPedido();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClickObservacaoHandler(object sender, RoutedEventArgs e)
        {
            AcaoEditarObservacao();
        }

        public async void CarregarPedidoAsync(PedidoVendaDTO dto)
        {
            _carregandoPedido = true;

            await RunTaskWithProgress(() =>
            {
                try
                {
                    _contexto.CarregarPedidoSelecionado(dto);
                    Dispatcher.Invoke(() => _dialogAtual?.Close());
                }
                catch (InvalidOperationException e)
                {
                    Dispatcher.Invoke(() => { DialogBox.MostraAviso(e.Message); });
                }
                catch (Exception e)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraErro(e.Message, e);
                        Close();
                    });
                }
                finally
                {
                    _carregandoPedido = false;
                }
            });
        }

        private void LabelAplicarDescontoClickHandler(object sender, MouseButtonEventArgs e)
        {
            AcaoAplicaDesconto();
        }

        private void LabelDescontoClickHandler(object sender, MouseButtonEventArgs e)
        {
            AcaoAplicaDesconto();
        }

        private void AcaoEditarReferencia()
        {
            if (TbReferencia.IsEnabled)
            {
                _contexto.SalvarReferencia();

                TbReferencia.IsEnabled = false;
                ProdutoComboPicker.FocusTbCodigo();
                return;
            }

            TbReferencia.IsEnabled = true;

            TbReferencia.Focus();
            TbReferencia.SelectAll();
        }

        private void AcaoEditarObservacao()
        {
            var model = new ObservacaoPedidoVendaControlModel();
            var observacaoContrl = new ObservacaoPedidoVendaControl(model);

            model.Observacao = _contexto.ObterObservacao();

            model.SolicitaFechamento += (o, args) =>
            {
                _dialogAtual.Close();
                Focus();
            };

            model.ConfirmouAlteracoes += (o, args) =>
            {
                try
                {
                    _contexto.AtualizarObservacao(model.Observacao);
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                }
            };

            AbrirChildWindow(observacaoContrl, "Adicionais Documento");
        }

        private void ClickConfiguracoesHandler(object sender, RoutedEventArgs e)
        {
            new PedidoVendaPreferenciaForm().ShowDialog();
        }

        private void VendedorClickHandler(object sender, RoutedEventArgs eventArgs)
        {
            AcaoAdicionarVendedor();
        }

        private void AcaoAdicionarVendedor()
        {
            var picker = new PessoaPickerModel(new VendedorEngine())
            {
                Titulo = "Escolha um vendedor para o pedido"
            };

            picker.UsarBusca<BuscaPessoaPickerVendedor>();

            picker.InicializarComPesquisa(string.Empty);

            picker.PickItemEvent += (o, args) =>
            {
                try
                {
                    _contexto.ComVendedor(args.GetItem<Vendedor>());
                }
                catch (InvalidOperationException e)
                {
                    DialogBox.MostraInformacao(e.Message);
                }
            };

            picker.GetPickerView().ShowDialog();
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            EditarPedidoVendaProduto(sender, e);
        }

        private void Finaizar_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_contexto.NomeVendedor) && VendedorObrigatorioFacade.VendedorEhObrigatorio())
            {
                DialogBox.MostraAviso(VendedorObrigatorioFacade.MensagemVendedorObrigatorio);
                return;
            }

            if (_contexto.PedidoEmAndamento == false)
            {
                DialogBox.MostraAviso("Preciso de um pedido para Finalizar!");
                return;
            }

            if (!_contexto.PossuiItens())
            {
                DialogBox.MostraAviso("Preciso que adicione itens/produtos no pedido!");
                return;
            }

            if (_contexto.PossuiProdutoInativo())
            {
                DialogBox.MostraInformacao(
                    "Preciso que verifique os produtos, existem produtos inativos.\nProdutos inativos estão de vermelho na lista.");
                return;
            }

            var model = new FinalizacaoFormModel(_contexto.PedidoVenda);
            var dialog = new FinalizacaoForm(model);

            model.FinalizacaoConcluida += ConcluiuPagamentoHandler;
            model.FinalizacaoRemovida += (s, p) => _contexto.NotificarAlteracaoEstado();

            AbrirChildWindow(dialog);
        }

        private bool VerificarObrigatoriedadeVendedor()
        {
            var sessao = _sessaoSistema.Preferencias;
            var preferencia = sessao.Obter<bool>("vendas.vendedor.obrigarUso", false);

            return preferencia;

        }

        private void ConcluiuPagamentoHandler(object sender, PedidoVenda pedido)
        {
            try
            {
                var preferencia = new PedidoVendaPreferenciaFacade().GetPreferenciaDaMaquina();

                var impressor = new ImpressorPedidoVenda(_sessaoManager);

                if (preferencia.ImprimeAposFinalizar)
                {
                    impressor.Imprimir(pedido.Id, preferencia.Impressora, preferencia.ObterQuantidadeVias());
                }

                if (preferencia.VisualizarAposFinalizar)
                {
                    impressor.Visualizar(pedido.Id);
                }

                _contexto.IniciarNovo();

                AcaoEscolherTipoPedido();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AdicionarItemClickHandler(object sender, RoutedEventArgs e)
        {
            if (_contexto.PedidoEstaAberto == false)
            {
                AcaoPedidoNaoEstaAberto();
                return;
            }

            try
            {
                if (QuantidadeNaoValida(_contexto.Quantidade))
                {
                    DialogBox.MostraInformacao("Quantidade não pode ser menor ou igual a 0,00");
                    return;
                }

                if (_contexto.ProdutoCombo == null)
                {
                    throw new InvalidOperationException("Preciso que informe um produto");
                }

                _contexto.AdicionarProduto();

                ProdutoComboPicker.LimparCampos();
                ProdutoComboPicker.FocusTbCodigo();
            }
            catch (EstoqueException exception)
            {
                DialogBox.MostraAviso(exception.Message);
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraAviso(exception.Message);
            }
            finally
            {
                ProdutoComboPicker.FocusTbCodigo();
            }
        }
    }
}