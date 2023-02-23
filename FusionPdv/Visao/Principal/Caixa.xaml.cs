using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ACBrFramework;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Patterns.Observer;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.Helper.Criptografia;
using FusionPdv.Ecf;
using FusionPdv.Modelos;
using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.Ecf.EstadoEcf;
using FusionPdv.Servicos.ValidacaoInicial;
using FusionPdv.Visao.AdicionarImposto;
using FusionPdv.Visao.Cliente;
using FusionPdv.Visao.EspelhoMfd;
using FusionPdv.Visao.MapearFormasDePagamentos;
using FusionPdv.Visao.MemoriaFiscalEcf;
using FusionPdv.Visao.Pagamento;
using FusionPdv.Visao.Produto;
using FusionPdv.Visao.Validacao;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Sobre;
using Newtonsoft.Json;

namespace FusionPdv.Visao.Principal
{

    public partial class Caixa : ICoreObserver<CaixaModel>
    {
        private readonly CaixaModel _caixaModel;

        public Caixa()
        {
            InitializeComponent();
            _caixaModel = new CaixaModel(this);
            _caixaModel.Inscrever(this);
            DataContext = _caixaModel;
        }

        private void VerificarEstadosDaEcf()
        {
            try
            {
                _caixaModel.VerificaEstadoEcf();
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (ExceptionGtEcf)
            {
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => { DialogBox.MostraAviso(ex.Message); });
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;

            switch (e.SystemKey)
            {
                case Key.F10:
                    BtAbrirGaveta_OnClick(sender, e);
                break;
            }

            switch (e.Key)
            {
                case Key.F1:

                    if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
                    new ConsultarPrecoProduto().ShowDialog();
                    break;
                case Key.F2:
                    CancelamentoDeItem();
                    break;
                case Key.F3:
                    CancelamentoDeCupom();
                    break;
                case Key.F4:
                    FinalizarVenda();
                    break;
                case Key.F6:
                    VendeProdutoManual();
                    break;
                case Key.F5:
                    AdicionarCliente();
                    break;
                case Key.F12:
                    if (_caixaModel.UsandoTef) return;
                    if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
                    MiMenuFiscal.IsSubmenuOpen = true;
                    MiMenuFiscal.Focus();
                    break;
                case Key.Escape:
                    Close();
                    break;
                case Key.F8:
                    AdicionarQuantidade();
                    break;
            }
        }

        private void VendeProdutoManual()
        {
            if (_caixaModel.UsandoTef) return;

            var consultarProduto = new ConsultarProduto();
            consultarProduto.ShowDialog();

            var produto = consultarProduto.Retorno();

            if (produto == null) return;

            _caixaModel.CodigoBarra = produto.Id.ToString();
            BuscaProdutoEVendePeloCodigoBarras();
        }

        private void FinalizarVenda()
        {
            if (_caixaModel.UsandoTef) return;

            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            try
            {
                _caixaModel.ValidaQuantidadeDeItems();
                _caixaModel.ValidarFormaPagamento();
            }
            catch (ExceptionFormaPagamentoNaoExiste ex)
            {
                DialogBox.MostraAviso(ex.Message);
                new MapearFormaDePagamento().ShowDialog();
                TbCodigoBarras.Focus();
                return;
            }
            catch (QuantidadeItemZeroException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                TbCodigoBarras.Focus();
                return;
            }

            if (!_caixaModel.ExisteVendaEmAndamento())
            {
                TbCodigoBarras.Focus();
                return;
            }

            _caixaModel.CalculaTotaisVenda();
            _caixaModel.CalculaIbpt();
            _caixaModel.RemoveEventosTef();

            var model = new EfetuaPagamentoModel(_caixaModel.ObterVenda());
            model.FalhaTransmissaoDocumentoReceber += FalhaTransmissaoDocumentoReceber;
            var efetuaPagamento = new EfetuaPagamento(model);
            efetuaPagamento.ShowDialog();

            _caixaModel.AdicionaEventosTef();

            if (efetuaPagamento.PagamentoConcluido)
            {
                _caixaModel.ReiniciarCaixa();
            }

            TbCodigoBarras.Focus();
        }

        private void FalhaTransmissaoDocumentoReceber(object sender, FalhaTransmissao e)
        {
        }

        private void AdicionarCliente()
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            try
            {
                _caixaModel.ValidarAdiconaCliente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                TbCodigoBarras.Focus();
                return;
            }

            if (_caixaModel.ClienteAdicionadoPeloCabecalho)
            {
                TbCodigoBarras.Focus();
                return;
            }

            var adicionarCliente = new AdicionarCliente();
            adicionarCliente.ShowDialog();

            var clienteCupom = adicionarCliente.Retorno();

            if (clienteCupom == null)
            {
                TbCodigoBarras.Focus();
                return;
            }

            if (clienteCupom.Cliente != null)
            {
                try
                {
                    AdicionarCliente(clienteCupom);
                }
                catch (Exception ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
            }

            TbCodigoBarras.Focus();
        }

        private void TbDefinirCliente_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AdicionarCliente();
        }

        private void AdicionarCliente(ClienteCupom clienteCupom)
        {
            var estadoEcf = SessaoEcf.EcfFiscal.Estado();

            if (estadoEcf != EstadoEcfFiscal.Livre)
            {
                new EstadoEcf(estadoEcf).ValidarEstadoDoEcf();
                _caixaModel.AdicionarClienteNoCupom(clienteCupom);
            }
            else
            {
                _caixaModel.ClienteAdicionadoPeloCabecalho = true;


                AbrirVendaCliente(clienteCupom);
            }
        }

        private void AbrirVendaCliente(ClienteCupom clienteCupom)
        {
            try
            {
                _caixaModel.AbreVenda(clienteCupom);
            }
            catch (ExceptionSerieEcf ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ExceptionGtEcf)
            {
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
                AbrirVendaCliente(clienteCupom);
                return;
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            TbCodigoBarras.Focus();
        }

        private void TbConsultarProduto_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VendeProdutoManual();
        }

        private bool PrecoManual(ProdutoDt produto)
        {
            if (produto.SolicitaTotal != true) return false;
            if (produto.PodeFracionar != IntBinario.Sim) return false;

            _caixaModel.ValorKiloItem = (produto.PrecoVenda*_caixaModel.QuantidadeAAdicionar).ToString("N2");
            _caixaModel.ProdutoPorKilo = produto;
            _caixaModel.EditarValorItem = true;

            TbValorKiloItem.Focus();
            TbValorKiloItem.SelectAll();
            return true;
        }

        private void VendeProduto(ProdutoDt produto)
        {
            try
            {
                _caixaModel.CodigoBarra = "";
                _caixaModel.AbreVenda(produto);
            }
            catch (ExceptionSerieEcf ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ExceptionGtEcf)
            {
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
                VendeProduto(produto);
                return;
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            TbCodigoBarras.Focus();
        }

        private void TbCodigoBarras_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (e.Key != Key.Enter) return;

            BuscaProdutoEVendePeloCodigoBarras();
        }

        private void BuscaProdutoEVendePeloCodigoBarras()
        {
            ProdutoDt produto = null;

            if (!string.IsNullOrEmpty(_caixaModel.CodigoBarra) &&
                !_caixaModel.MensagemConsultarProduto.Equals("Cancelamento de item")
                && !_caixaModel.CodigoBarra.Contains("*"))
            {
                try
                {
                    if (VerificaSeEstaVazio())
                    {
                        return;
                    }

                    produto = _caixaModel.BuscarProdutoPorCodigoBarra(_caixaModel.CodigoBarra);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                    return;
                }
                catch (ArgumentException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                    return;
                }
                catch (FormatException)
                {
                    DialogBox.MostraAviso(
                        "Codigo de barras está incorreto, tentamos convertelo mas ouve uma falha.\n" +
                        "Bom tentamos converter o codigo de barras toda vez que o produto utiliza uma balança, \n" +
                        "quando o produto utiliza uma balança o código de barras somente pode conter números.");
                    return;
                }

                if (VerificaSeAchouAlgumProduto(produto)) return;

                if (PrecoManual(produto)) return;
            }

            AbreCupomOuVendeItem(produto);
        }

        private static bool VerificaSeAchouAlgumProduto(ProdutoDt produto)
        {
            if (produto != null) return false;

            DialogBox.MostraInformacao("Produto não encontrado.");
            return true;
        }

        private bool VerificaSeEstaVazio()
        {
            _caixaModel.CodigoBarra = _caixaModel.CodigoBarra?.Trim();

            if (string.IsNullOrEmpty(_caixaModel.CodigoBarra))
            {
                DialogBox.MostraInformacao("Preciso que digite um código ou código de barras para buscar");
                _caixaModel.CodigoBarra = string.Empty;

                return true;
            }
            return false;
        }

        private void AbreCupomOuVendeItem(ProdutoDt produto)
        {
            if (_caixaModel.UsandoTef) return;
            try
            {
                _caixaModel.BuscarOuAdicionarQuantidade(produto);
                _caixaModel.MensagemConsultarProduto = "Para consultar produtos(F6)";
            }
            catch (ACBrException ex)
            {
                _caixaModel.MensagemConsultarProduto = "Para consultar produtos(F6)";
                _caixaModel.CodigoBarra = "";
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ExceptionGtEcf)
            {
                new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
                AbreCupomOuVendeItem(produto);
                return;
            }
            catch (NaoExisteCupomAbertoException ex)
            {
                _caixaModel.MensagemConsultarProduto = "Para consultar produtos(F6)";
                _caixaModel.CodigoBarra = "";
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _caixaModel.CodigoBarra = "";
                DialogBox.MostraInformacao(ex.Message);
            }

            TbCodigoBarras.Focus();
        }

        private void MiFormaPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            if (ExisteVendaEmAndamento()) return;
            new MapearFormaDePagamento().ShowDialog();
            TbCodigoBarras.Focus();
        }

        private void MiProgramarImpostoIcms_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            if (ExisteVendaEmAndamento())
            {
                TbCodigoBarras.Focus();
                return;
            }

            new Imposto().ShowDialog();

            TbCodigoBarras.Focus();
        }

        private async void MiLeituraX_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            var messageBoxResult = MessageBox.Show("Deseja efetuar Leitura X", "Leitura X",
                MessageBoxButton.YesNo);

            if (messageBoxResult != MessageBoxResult.Yes)
            {
                TbCodigoBarras.Focus();
                return;
            }

            ProgressBarAgil4.ShowProgressBar();

            if (ExisteVendaEmAndamento()) return;

            try
            {
                await Task.Run(() => LeituraX());
                DialogBox.MostraInformacao("Leitura X efetuada.");
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                ProgressBarAgil4.CloseProgressBar();
                TbCodigoBarras.Focus();
            }
        }

        private void LeituraX()
        {
            SessaoEcf.EcfFiscal.LeituraX();
        }

        private static bool ExisteVendaEmAndamento()
        {
            var retorno = SessaoEcf.EcfFiscal.Estado() != EstadoEcfFiscal.Livre;

            if (retorno)
            {
                MessageBox.Show("Existe uma venda em andamento.");
            }

            return retorno;
        }

        private async void MiReducaoZ_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.ReducaoZEmAndamento) return;

            _caixaModel.AtivarReducaoZ();

            if (_caixaModel.UsandoTef)
            {
                DesativarReducaoZ();
                return;
            }
            if (!_caixaModel.ListaEsperaProdutoConcluida())
            {
                DesativarReducaoZ();
                return;
            }
            if (ValidaDisponibilidadePapel())
            {
                DesativarReducaoZ();
                return;
            }

            var messageBoxResult =
                DialogBox.MostraConfirmacao("A Redução Z pode Bloquear o seu ECF até a 12:00pm.\nContinua assim mesmo ?");

            if (messageBoxResult != MessageBoxResult.Yes)
            {
                DesativarReducaoZ();
                TbCodigoBarras.Focus();
                return;
            }

            var messageBoxCerteza = DialogBox.MostraConfirmacao("Você confirma a emissão da Redução Z?");

            if (messageBoxCerteza != MessageBoxResult.Yes)
            {
                DesativarReducaoZ();
                TbCodigoBarras.Focus();
                return;
            }

            ProgressBarAgil4.ShowProgressBar();
            if (ExisteVendaEmAndamento())
            {
                DesativarReducaoZ();
                return;
            }

            try
            {
                await Task.Run(() => SessaoEcf.EcfFiscal.ReducaoZ());
                DialogBox.MostraInformacao("Redução Z tirada com sucesso.");
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
            finally
            {
                DesativarReducaoZ();
                ProgressBarAgil4.CloseProgressBar();
                TbCodigoBarras.Focus();
            }
        }

        private void DesativarReducaoZ()
        {
            _caixaModel.DesativarReducaoZ();
        }

        private bool ValidaDisponibilidadePapel()
        {
            try
            {
                if (new VerificarPapelEcf().Executar())
                {
                    TbCodigoBarras.Focus();
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
                TbCodigoBarras.Focus();
                return true;
            }
            TbCodigoBarras.Focus();
            return false;
        }

        private void BtCancelarCupom_OnClick(object sender, RoutedEventArgs e)
        {
            CancelamentoDeCupom();
        }

        private async void CancelamentoDeCupom()
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            var messageBoxResult = DialogBox.MostraConfirmacao("Deseja cancelar o cupom");

            if (messageBoxResult != MessageBoxResult.Yes)
            {
                TbCodigoBarras.Focus();
                return;
            }

            ProgressBarAgil4.ShowProgressBar();

            try
            {
                await Task.Run(() => _caixaModel.CancelarCupom());
                DialogBox.MostraAviso("Cupom cancelado com sucesso.");
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (CancelarVendaException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                ProgressBarAgil4.CloseProgressBar();
                TbCodigoBarras.Focus();
            }

        }

        private void BtCancelamentoItem_OnClick(object sender, RoutedEventArgs e)
        {
            CancelamentoDeItem();
        }

        private void CancelamentoDeItem()
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            _caixaModel.MensagemConsultarProduto =
                _caixaModel.MensagemConsultarProduto.Equals("Para consultar produtos(F6)")
                    ? "Cancelamento de item"
                    : "Para consultar produtos(F6)";

            TbCodigoBarras.Focus();
        }

        private void BtnConsultarPrecos_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            new ConsultarPrecoProduto().ShowDialog();

            TbCodigoBarras.Focus();
        }

        private void Caixa_OnClosing(object sender, CancelEventArgs e)
        {
            var messageBoxResult = DialogBox.MostraConfirmacao("Deseja fechar o sistema?");

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    SessaoEcf.EcfFiscal.Close();
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                }
                return;
            }

            e.Cancel = true;
        }

        private void MiHorarioDeVerao_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;

            if(SessaoEcf.EcfFiscal.Estado() != EstadoEcfFiscal.Bloqueada)
                if (ExisteVendaEmAndamento()) return;

            var messageBoxResult = DialogBox.MostraConfirmacao("Ajustar horário de verão");

            if (messageBoxResult != MessageBoxResult.Yes)
            {
                TbCodigoBarras.Focus();
                return;
            }

            if (SessaoEcf.EcfFiscal.HorarioVerao)
            {
                var bol = DialogBox.MostraConfirmacao("Você está ajustando a ECF para sair do horário de verão");

                if (bol != MessageBoxResult.Yes)
                {
                    TbCodigoBarras.Focus();
                    return;
                }
            }
            else
            {
                var bol = DialogBox.MostraConfirmacao("Você está ajustando a ECF para entrar no horário de verão");

                if (bol != MessageBoxResult.Yes)
                {
                    TbCodigoBarras.Focus();
                    return;
                }
            }

            try
            {
                SessaoEcf.EcfFiscal.MudaHorarioVerao();
                DialogBox.MostraInformacao(
                    "Horário ajustado. O sistema sera fechado, porfavor re-abrir o mesmo e ajustar horário do computador.");
                Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            TbCodigoBarras.Focus();
        }

        private void OnClickFinalizarCupom(object sender, RoutedEventArgs e)
        {
            FinalizarVenda();
        }

        private void Caixa_OnContentRendered(object sender, EventArgs e)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    new VerificarPapelEcf(true).Executar();
                    _caixaModel.VerificacaoInicial();

                    PreparaSessaoDoSistema();
                }
                catch (ImpressoraSemPapelException ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message
                                              + "\nO sistema será finalizado, porfavor colocar papel.");
                        Dispatcher.CurrentDispatcher.Invoke(
                            () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    });
                    return;
                }
                catch (ExceptionSerieEcf)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso("Faltando dados no arquivo Auxiliar. (SerieEcf)");
                        Dispatcher.CurrentDispatcher.Invoke(
                            () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    });

                    return;
                }
                catch (ExceptionCarregarFormaPagamento ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message);
                        new MapearFormaDePagamento().ShowDialog();
                        Caixa_OnContentRendered(sender, e);
                    });

                    return;
                }
                catch (ExceptionGtEcf)
                {
                    new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
                    Caixa_OnContentRendered(sender, e);
                    return;
                }
                catch (ExceptionDataInvalidaEcf ex)
                {
                    try
                    {
                        Dispatcher.Invoke(() => { SincronizaHorarioDoSistemaComAecf(ex); });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    Caixa_OnContentRendered(sender, e);
                    return;
                }
                catch (ExceptionExisteAliquota ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message);
                        new Imposto().ShowDialog();
                        Caixa_OnContentRendered(sender, e);
                    });
                    return;
                }
                catch (JsonReaderException)
                {
                    Dispatcher.Invoke(() =>
                    {
                       DialogBox.MostraInformacao("Ocorreu um erro ao tentar verificar o arquivo auxiliar, \n" +
                                                  "vamos fechar a aplicação, tente abrir novamente, se o erro \n" +
                                                  "persistir, ligar para o suporte.");
                         
                    });
                    Dispatcher.CurrentDispatcher.Invoke(
                        () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                }
                catch (ArquivoAuxiliarInvalidoException)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraInformacao("Ocorreu um erro ao tentar verificar o arquivo auxiliar, \n" +
                                                  "vamos fechar a aplicação, tente abrir novamente, se o erro \n" +
                                                  "persistir, ligar para o suporte.");

                    });
                    Dispatcher.CurrentDispatcher.Invoke(
                        () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    return;
                }
                catch (Exception ex)
                {
                    DialogBox.MostraErro(ex.Message, ex);
                    Dispatcher.CurrentDispatcher.Invoke(
                        () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    return;
                }

                try
                {
                    VerificarEstadosDaEcf();
                }
                catch (ACBrException ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message);
                        Dispatcher.CurrentDispatcher.Invoke(
                            () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    });
                    return;
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraAviso(ex.Message);
                        Dispatcher.CurrentDispatcher.Invoke(
                            () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    });
                    return;
                }

                Dispatcher.Invoke(() =>
                {
                    GdProgressBar.Visibility = Visibility.Collapsed;
                    SpHeader.Visibility = Visibility.Visible;
                    GdCaixa.Visibility = Visibility.Visible;
                    FocusInicialCodigoBarras();
                });
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private static void PreparaSessaoDoSistema()
        {
            SessaoSistema.FormasPagamentoEcf = new EcfPegarTiposPagamentos().TipoPagamento();
            SessaoSistema.AliquotasDoEcf = new EcfPegarAliquotas().Aliquotas();

            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                var listaInterno = new FormaDePagamentoRepositorio(sessao).BuscaTodos();
                SessaoSistema.FormasPagamentoInterno = listaInterno;
            }
        }

        private static void SincronizaHorarioDoSistemaComAecf(ExceptionDataInvalidaEcf ex = null)
        {
            string mensagemErro;

            if (ex != null)
            {
                mensagemErro = ex.Message;
            }
            else
            {
                mensagemErro =
                    "A data da ecf não confere com a do computador.\nPorfavor ajustar o horário do computador." +
                    "\nHora na ecf: " + SessaoEcf.EcfFiscal.DataEHora().ToString(CultureInfo.CurrentCulture);
            }

            try
            {
                using (new UacHelper("SeSystemtimePrivilege"))
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    try
                    {
                        new MudarHoraSistema(SessaoEcf.EcfFiscal.DataEHora());
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    Thread.Sleep(2000);
                    if (new EcfVerificaDataEhora().DiferencaDeHoraDoEcfAceita())
                    {
                        DialogBox.MostraInformacao("Hora do computador ajustada para a mesma da Ecf.");
                    }
                    else
                    {
                        DialogBox.MostraAviso(mensagemErro + "\n" +
                                              "Ou você pode abrir o sistema em modo Administrador para que possamos mudar o horário para você");
                        Dispatcher.CurrentDispatcher.Invoke(
                            () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                    }
                }
            }
            catch (Exception)
            {
                Dispatcher.CurrentDispatcher.Invoke(
                    () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
            }
        }

        private void FocusInicialCodigoBarras()
        {
            var timer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TbCodigoBarras.Focus();
            ((DispatcherTimer) sender).Stop();
        }

        private void MiMemoriaFiscal_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;
            if (ExisteVendaEmAndamento()) return;
            new LeituraMemoriaFiscal().ShowDialog();
        }

        private void MiArredondamentoTruncamento_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ExisteVendaEmAndamento()) return;
            MessageBox.Show(SessaoEcf.EcfFiscal.Arredonda()
                ? "Está impressora está arredondando o valor"
                : "Está impressora está truncando o valor.");
        }

        private void BtQuantidade_OnClick(object sender, RoutedEventArgs e)
        {
            AdicionarQuantidade();
        }

        private void AdicionarQuantidade()
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            TbCodigoBarras.Text = "";
            TbCodigoBarras.Text = "*";
            TbCodigoBarras.SelectionStart = TbCodigoBarras.Text.Length;
            TbCodigoBarras.SelectionLength = 0;
            TbCodigoBarras.Focus();
        }

        private void TbValorKiloItem_OnKeyDown(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (_caixaModel.ProdutoPorKilo == null) return;

            switch (_caixaModel.ValorKiloItem.Trim())
            {
                case "0,00":
                case "0":
                    _caixaModel.QuantidadeAAdicionar = 1;
                    return;
                case "":
                    return;
            }

            const int kG = 1000;

            var valorVenda = _caixaModel.ProdutoPorKilo.PrecoVenda;

            var valor = _caixaModel.ValorKiloItem;
            try
            {
                var kgEvalorVenda = kG/valorVenda;
                var qtdKg = kgEvalorVenda*decimal.Parse(valor);
                qtdKg = qtdKg/kG;
                _caixaModel.QuantidadeAAdicionar = decimal.Parse(qtdKg.ToString("0.000"));
            }
            catch (Exception)
            {
                MessageBox.Show("Valor digitado não é valido.");
                _caixaModel.ValorKiloItem = 0.ToString("N2");
                TbValorKiloItem.SelectAll();
            }
        }

        private void TbValorKiloItem_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            VendeProduto(_caixaModel.ProdutoPorKilo);
            _caixaModel.EditarValorItem = false;
            _caixaModel.ValorKiloItem = 0.ToString("N2");
        }

        public void Notificacao(CaixaModel observable)
        {
            if (observable.TemErro == false) return;

            if (observable.UltimaException.GetType() == typeof (ACBrException))
            {
                DialogBox.MostraAviso(observable.UltimaException.Message);
            }
            else if (observable.UltimaException.GetType() == typeof (InvalidOperationException))
            {
                DialogBox.MostraAviso(observable.UltimaException.Message);
            }
            else if (observable.UltimaException.GetType() == typeof (Exception))
            {
                DialogBox.MostraAviso(observable.UltimaException.Message);
            }
        }

        private void MiAdministracaoTef_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;
            if (ExisteVendaEmAndamento()) return;

            try
            {
                _caixaModel.AdministracaoTef();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void MiEspelhoMfd_OnClick(object sender, RoutedEventArgs e)
        {
            if (_caixaModel.UsandoTef) return;
            if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
            if (ValidaDisponibilidadePapel()) return;
            if (ExisteVendaEmAndamento()) return;
            new EspelhoMfdForm().ShowDialog();
        }

        private void BtAbrirGaveta_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_caixaModel.UsandoTef) return;
                if (!_caixaModel.ListaEsperaProdutoConcluida()) return;
                _caixaModel.AbrirGaveta();
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Sobre_OnClick(object sender, RoutedEventArgs e)
        {
            new SobreForm().ShowDialog();
        }
    }
}