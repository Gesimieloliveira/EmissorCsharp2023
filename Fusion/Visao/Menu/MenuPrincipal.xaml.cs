using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Threading;
using Fusion.Base.Notificacoes;
using Fusion.Controladores.Menu;
using Fusion.Sessao;
using Fusion.Visao.Base.Grid;
using Fusion.Visao.CentroCusto;
using Fusion.Visao.CentroDeLucro;
using Fusion.Visao.Cfop;
using Fusion.Visao.Configuracao;
using Fusion.Visao.Configuracao.Model;
using Fusion.Visao.CteEletronico.Grid;
using Fusion.Visao.CteEletronico.Inutilizacao;
using Fusion.Visao.CteEletronico.Perfil;
using Fusion.Visao.CteEletronicoOs.Grid;
using Fusion.Visao.CteEletronicoOs.Perfil.Grid;
using Fusion.Visao.Dashboard;
using Fusion.Visao.DocumentoAPagar;
using Fusion.Visao.DocumentoAReceber;
using Fusion.Visao.Ecf;
using Fusion.Visao.EmissorFiscalEletronico;
using Fusion.Visao.Empresa;
using Fusion.Visao.Fiscal.Estadual;
using Fusion.Visao.GerenciadorManifestacoesDestinatarios;
using Fusion.Visao.ImportarIbpt;
using Fusion.Visao.Lancamentos;
using Fusion.Visao.Licenciamento;
using Fusion.Visao.Login;
using Fusion.Visao.MovimentacaoEstoque;
using Fusion.Visao.Ncm;
using Fusion.Visao.NfeInutilizacaoNumeracao;
using Fusion.Visao.NotaFiscalEletronica.Contingencia;
using Fusion.Visao.NotaFiscalEletronica.Exportacao;
using Fusion.Visao.NotaFiscalEletronica.Principal;
using Fusion.Visao.NotaFiscalEletronica.Status;
using Fusion.Visao.PedidoDeVenda;
using Fusion.Visao.PedidoDeVenda.Preferencias;
using Fusion.Visao.PerfilCfop;
using Fusion.Visao.PerfilNfe;
using Fusion.Visao.ProdutoGrupo;
using Fusion.Visao.ProdutoLocalizacoes;
using Fusion.Visao.ProdutoUnidade;
using Fusion.Visao.Relatorios;
using Fusion.Visao.Sintegra;
using Fusion.Visao.TabelasPrecos;
using Fusion.Visao.Tef;
using Fusion.Visao.Terminal;
using Fusion.Visao.TipoDocumentoFinanceiro;
using Fusion.Visao.Usuario;
using Fusion.Visao.Veiculos;
using Fusion.Visao.Vendas.FaturamentoCheckout;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Helpers.Ambiente;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SharedViews.ControleCaixa;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Menu
{
    public partial class MenuPrincipal
    {
        private MenuViewModel _contexto;
        private ComprasController _comprasController;
        private CaixaControlador _caixaControlador;
        private EstoqueController _estoqueController;
        private PessoaController _pessoaController;
        private FaturamentoController _faturamentoController;
        private RegraTributacaoSaidaController _regraTributacaoSaidaController;
        private PedidoVendaController _pedidoVendaController;
        private MdfeControladorTela _mdfeControladorTela;
        private SessaoSistema _sessaoSistema;
        private bool _fecharCasoNaoTenhaEmpresa;

        private MenuPrincipal()
        {
            InitializeComponent();
        }

        private static MenuPrincipal _instancia;
        public static MenuPrincipal Instancia => _instancia ?? (_instancia = new MenuPrincipal());

        public MenuPrincipal Inicializar(MenuViewModel contexto)
        {
            _sessaoSistema = SessaoSistema.Instancia;
            _faturamentoController = new FaturamentoController(TabControl);
            _comprasController = new ComprasController(TabControl);
            _estoqueController = new EstoqueController(TabControl);
            _pessoaController = new PessoaController(TabControl);
            _regraTributacaoSaidaController = new RegraTributacaoSaidaController(TabControl);
            _pedidoVendaController = new PedidoVendaController(TabControl);
            _caixaControlador = new CaixaControlador(TabControl, _sessaoSistema.SessaoManager, _sessaoSistema.CaixaProvider);
            _mdfeControladorTela = new MdfeControladorTela(TabControl);

            _contexto = contexto;
            _contexto.Deslogou += DeslogouHandler;

            return this;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DockPanelMenu.Visibility = Visibility.Hidden;

            try
            {
                _contexto.Inicializa();

                DataContext = _contexto;
                AbreAbaPrincipal();
            }
            catch (Exception)
            {
                DockPanelMenu.Visibility = Visibility;
                throw;
            }
        }

        private void Menu_OnContentRendered(object sender, EventArgs e)
        {
            try
            {
                ChecarEmpresa();
                EsconderItemsSemFilhos();
                AplicarRestricaoModulos();
            }
            finally
            {
                DockPanelMenu.Visibility = Visibility.Visible;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _instancia = null;
        }

        private void AbreAbaPrincipal()
        {
            if (SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissao(Permissao.DASHBOARD) == false) return;

            TabInicial.Content = new DashboardSimples(new DashboardSimplesModel());
        }

        private void DeslogouHandler(object sender, EventArgs e)
        {
            new LoginAdm().Show();
            Close();
        }

        private void EsconderItemsSemFilhos()
        {
            try
            {
                MenuTop.Visibility = Visibility.Collapsed;

                foreach (var rsmiItem in MenuTop.Items)
                {
                    if (rsmiItem is RibbonApplicationMenuItem r)
                    {
                        if (r.Visibility == Visibility.Visible)
                        {
                            MenuTop.Visibility = Visibility.Visible;
                        }
                    }
                }

                foreach (var ribbonMenuItem in RibbonMenu.Items)
                {
                    if (ribbonMenuItem is RibbonTab ribbonTab)
                    {
                        ribbonTab.Visibility = Visibility.Collapsed;

                        foreach (var rcadastrosItem in ribbonTab.Items)
                        {
                            if (rcadastrosItem is RibbonGroup ribbonGroup)
                            {
                                ribbonGroup.Visibility = Visibility.Collapsed;

                                foreach (var ribbonGroupItem in ribbonGroup.Items)
                                {
                                    if (ribbonGroupItem is RibbonButton ribbonButton)
                                    {
                                        if (ribbonButton.Visibility == Visibility.Visible)
                                        {
                                            ribbonGroup.Visibility = Visibility.Visible;
                                            ribbonTab.Visibility = Visibility.Visible;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                DockPanelMenu.Visibility = Visibility.Visible;
            }
        }

        private void AplicarRestricaoModulos()
        {
            TabNfe.Visibility = _contexto.PossuiFusionStarter ? TabNfe.Visibility : Visibility.Collapsed;
            TabMovimentacoes.Visibility = _contexto.PossuiFusionStarter ? TabMovimentacoes.Visibility : Visibility.Collapsed;
            TabTransportes.Visibility = _contexto.PossuiTransporte ? TabTransportes.Visibility : Visibility.Collapsed;
            TabFinanceiro.Visibility = _contexto.PossuiFusionGestor ? TabFinanceiro.Visibility : Visibility.Collapsed;
            TabPedidoVenda.Visibility = _contexto.PossuiFusionGestor ? TabPedidoVenda.Visibility : Visibility.Collapsed;

            MenuItemTerminal.Visibility = _contexto.PossuiFusionStarter ? MenuItemTerminal.Visibility : Visibility.Collapsed;
            MenuItemEcf.Visibility = _contexto.PossuiFusionStarter ? MenuItemEcf.Visibility : Visibility.Collapsed;

            GroupCte.Visibility = _contexto.PossuiFusionCte ? GroupCte.Visibility : Visibility.Collapsed;
            GroupCteos.Visibility = _contexto.PossuiFusionCteOs ? GroupCteos.Visibility : Visibility.Collapsed;
            GroupMdfe.Visibility = _contexto.PossuiFusionMdfe ? GroupMdfe.Visibility : Visibility.Collapsed;
        }

        private void ChecarEmpresa()
        {
            var setup = _contexto.ConsultarSituacaoEmpresa();

            if (setup.EmpresaCadastrada)
            {
                return;
            }

            if (_fecharCasoNaoTenhaEmpresa)
            {
                Application.Current.Shutdown();
                return;
            }

            _fecharCasoNaoTenhaEmpresa = true;

            SolicitarCadastroDaEmpresa();
            ChecarEmpresa();
        }

        private static void SolicitarCadastroDaEmpresa()
        {
            DialogBox.MostraAviso("Nenhuma empresa cadastrada. Necessário cadastrar");

            var model = new EmpresaFormModel(new EmpresaDTO());
            var formEmpresa = new EmpresaForm(model);

            formEmpresa.ShowDialog();
        }

        private void AbrirJanelaEmAba(string titulo, UserControl janela)
        {
            var novaTab = new MetroTabItem { Header = titulo, Content = janela, CloseButtonEnabled = true };

            foreach (var tabItem in TabControl.Items.Cast<MetroTabItem>().Where(tabItem => tabItem.Header == novaTab.Header))
            {
                tabItem.Focus();
                return;
            }

            TabControl.Items.Add(novaTab);
            novaTab.Focus();
        }

        private void ClickListagemProdutos(object sender, RoutedEventArgs e)
        {
            _estoqueController.ListarProdutos();
        }

        private void OnClickGrupo(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new ProdutoGrupoGridModel());
            AbrirJanelaEmAba("Produto Grupo", grid);
        }

        private void OnClickUnidade(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new ProdutoUnidadeGridModel());
            AbrirJanelaEmAba("Produto Unidade", grid);
        }

        private void OnClickPessoa(object sender, RoutedEventArgs e)
        {
            _pessoaController.Listagem();
        }

        private void OnClickUsuario(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Usuario", new GerenciarUsuariosControl());
        }

        private void OnClickNcm(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new NcmGridModel());
            AbrirJanelaEmAba("Ncm", grid);
        }

        private void OnClickImportarIbpt(object sender, RoutedEventArgs e)
        {
            var importar = new JanelaImportarIbpt();
            importar.ShowDialog();
        }

        private void OnClickSairSistema(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickEmpresa(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new EmpresaGridModel());
            AbrirJanelaEmAba("Empresa", grid);
        }

        private void OnClickEcf(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new EcfGridModel());
            AbrirJanelaEmAba("Ecf", grid);
        }

        private void OnClickMovimentacaoEstoque(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new MovimentoEstoqueGridModel());
            AbrirJanelaEmAba("Movimentos em Estoque", grid);
        }

        private void OnClickEmissorFiscalEletronico(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new EmissorFiscalGridModel());
            AbrirJanelaEmAba("Emissor Fiscal", grid);
        }

        private void OnClickPerfilCfop(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new PerfilCfopGridModel());
            AbrirJanelaEmAba("CFOP", grid);
        }

        private void OnClickPerfilNfe(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new PerfilNfeGridModel());
            AbrirJanelaEmAba("Perfil NF-e", grid);
        }

        private void OnClickEmitirNfe(object sender, RoutedEventArgs e)
        {
            var contexto = new NfeletronicaGridModel();
            var grid = new NfeEletronicaGridControl(contexto, _pedidoVendaController);

            AbrirJanelaEmAba("Gerenciar NF-e", grid);
        }

        private void OnClickInutilizarNumeracao(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new NfeInutilizacaoNumeracaoGridModel());
            AbrirJanelaEmAba("Inutilização de Número", grid);
        }

        private void OnClickRelatorios(object sender, RoutedEventArgs e)
        {
            var contexto = new ListagemRelatoriosContexto(_sessaoSistema.SessaoManager);
            var control = new ListagemRelatoriosControl(contexto);

            AbrirJanelaEmAba("Relatórios", control);
        }

        private void OnClickExportacaoDFeXml(object sender, RoutedEventArgs e)
        {
            var view = new ExportacaoXmlView();
            view.ShowDialog();
        }

        private void OnClickExportacaoDFeXmlImportacao(object sender, RoutedEventArgs e)
        {
            var view = new ExportacaoXmlDistribuicaoView();
            view.ShowDialog();
        }

        private void OnClickEmissorFiscal(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new TerminalOfflineGridModel());
            AbrirJanelaEmAba("Terminal off-line", grid);
        }

        private void OnClickProdutoLocalizacao(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new ProdutoLocalizacaoGridModel());
            AbrirJanelaEmAba("Produto Localização", grid);
        }

        private void OnClickCentroDeCusto(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Centro de Custo", new CentroDeCustoGrid());
        }

        private void OnClickCentroDeLucro(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Centro de Lucro", new CentroLucroGrid());
        }

        private void OnClickTipoDocumento(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new TipoDocumentoFinanceiroGridModel());
            AbrirJanelaEmAba("Tipo Documento", grid);
        }

        private void OnClickDocumentoPagar(object sender, RoutedEventArgs e)
        {
            var notificador = new Notificador();
            var grid = new GridDocumentoPagar(notificador);

            grid.ImprimirRecibo += ImprimirRecibo;
            grid.ImprimirComDocumento += ImprimirReciboComDocumento;

            AbrirJanelaEmAba("Documento Pagar", grid);
        }

        private async void ImprimirReciboComDocumento(object sender, DocumentoPagar e)
        {
            var dialog = new ReciboForm();

            dialog.Model.Preencher(e);

            await this.ShowChildWindowAsync(dialog, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void ImprimirRecibo(object sender, EventArgs e)
        {
            OnClickRecibo(null, null);
        }

        private void OnClickPainelLicencas(object sender, RoutedEventArgs e)
        {
            var view = new PainelLicencas();
            view.ShowDialog();
        }

        private void PerfilCte_OnClick(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new PerfilCteGridModel());
            AbrirJanelaEmAba("Perfil CT-e", grid);
        }

        private void Emitir_OnClick(object sender, RoutedEventArgs e)
        {
            var grid = new GridCTe();
            AbrirJanelaEmAba("Gerenciar CT-e", grid);
        }

        private void ClickContigenciaNfeHandler(object sender, RoutedEventArgs e)
        {
            new HistoricoContingenciaView().ShowDialog();
        }

        private void Inutilizacao_OnClick(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new CteInutilizacaoGridModel());
            AbrirJanelaEmAba("Inutilizar Númeração", grid);
        }

        private void EmitirMDFe_OnClick(object sender, RoutedEventArgs e)
        {
            _mdfeControladorTela.GridMdfe();
        }

        private void ConsultaNaoEncerrado_OnClick(object sender, RoutedEventArgs e)
        {
            _mdfeControladorTela.MdfeNaoEncerrados();
        }

        private void ClickCfop(object sender, RoutedEventArgs e)
        {
            var view = new GridPadrao(new CfopGridModel());
            AbrirJanelaEmAba("CFOP Base", view);
        }

        private void ClickNovaEntradaHandler(object sender, RoutedEventArgs e)
        {
            _comprasController.NovaEntrada();
        }

        private void ClickImportarCompraHandler(object sender, RoutedEventArgs e)
        {
            _comprasController.ImportarCompra();
        }

        private void ClickListarEntradasHandler(object sender, RoutedEventArgs e)
        {
            _comprasController.ListarEntradas();
        }

        private async void OnClickRecibo(object sender, RoutedEventArgs e)
        {
            await this.ShowChildWindowAsync(new ReciboForm(), ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void ConfiguracoesClickHandler(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Configurações", new ConfiguracaoUserControl(new ConfiguracaoModel()));
        }

        private void OnClickCadastrarTefPos(object sender, RoutedEventArgs e)
        {
            new GridPosForm(new GridPosFormModel()).ShowDialog();
        }

        private void CadastrarVeiculo_OnClick(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new VeiculoGridModel());
            AbrirJanelaEmAba("Veículos", grid);
        }

        private void GerenciarCteOS_OnClick(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Gerenciar CT-e Os", new GridCteOs(new GridCteOsModel()));
        }

        private void PerfilCteOs_OnClick(object sender, RoutedEventArgs e)
        {
            var grid = new GridPadrao(new GridPerfilCteOsModel());
            AbrirJanelaEmAba("Perfil CT-e OS", grid);
        }

        private void ClickFaturamentoVenda(object sender, RoutedEventArgs e)
        {
            try
            {
                IValidacaoAntesAbrirFaturamento validacaoAntesAbrirFaturamento = new ValidacaoNfce();
                validacaoAntesAbrirFaturamento.Valiar();

                _faturamentoController.AbrirFaturamento();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                DialogBox.MostraAviso(invalidOperationException.Message);

                _faturamentoController.AbrirFaturamento();
            }
        }

        private void ClickListagemFaturamento(object sender, RoutedEventArgs e)
        {
            _faturamentoController.AbirListagemFaturamentos();
        }

        private void NovaRegraTributacaoClickHandler(object sender, RoutedEventArgs e)
        {
            _regraTributacaoSaidaController.NovaRegra();
        }

        private void GerenciarRegraTributacaoClickHandler(object sender, RoutedEventArgs e)
        {
            _regraTributacaoSaidaController.GerenciarRegras();
        }

        private void ClickPedidoVendaOrcamento(object sender, RoutedEventArgs e)
        {
            _pedidoVendaController.AbrirFormulario();
        }

        private void ClickListarPedidoVenda(object sender, RoutedEventArgs e)
        {
            _pedidoVendaController.MenuListagemPedidoVenda();
        }

        private void OnClickSintegra(object sender, RoutedEventArgs e)
        {
            var model = new SintegraFormModel(_sessaoSistema.SessaoManager);
            var view = new SintegraForm(model);

            view.ShowDialog();
        }

        private void OnClickLancarEnergiaEletrica(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Nota Fiscal Outros", new GridPadrao(new GridLancamentoOutroModel()));
        }

        private void OnClickLancarCteOuCteOs(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("CT-e ENTRADAS", new GridPadrao(new GridCteEntrada()));
        }

        private void ClickStatusSefazNfeHandler(object sender, RoutedEventArgs e)
        {
            var model = new ConsultaStatusSefazFormModel(_sessaoSistema.SessaoManager);

            new ConsultaStatusSefazForm(model).ShowDialog();
        }

        private void ListarLancamentosCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            _caixaControlador.GradeLancamentoAvulsos();
        }

        private void GerenciarCaixasClickHandler(object sender, RoutedEventArgs e)
        {
            _caixaControlador.GerenciarCaixaAberto();
        }

        private void OnClickManifestadorNfe(object sender, RoutedEventArgs e)
        {
            var model = new GridGerenciadorManifestadorModel();
            AbrirJanelaEmAba("Gerenciar MD-e's", new GridGerenciadorManifestador(model));
        }

        private void AbrirCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new AberturaDeCaixaContexto(_sessaoSistema.CaixaProvider, _sessaoSistema.SessaoManager);
            var view = new AberturaDeCaixaView(contexto);

            view.ShowDialog();
        }

        private void CaixaAbertoClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                CaixaIndividual caixa;

                using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioCaixaIndividual(sessao);
                    caixa = repositorio.BuscarCaixaAberto(SessaoSistema.Instancia.UsuarioLogado, ELocalEventoCaixa.Gestao);
                }

                if (caixa == null)
                {
                    throw new InvalidOperationException("Não encontrei caixa aberto!");
                }

                var contexto = new ResumoCaixaIndividualContexto(
                    _sessaoSistema.SessaoManager,
                    _sessaoSistema.CaixaProvider,
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

        private void ClickNovaListagemDocumentosReceber(object sender, RoutedEventArgs e)
        {
            var contexto = new GridGerenciarAhReceberContexto();
            var control = new GridGerenciarAhReceberView(contexto);

            AbrirJanelaEmAba("Documentos Receber", control);
        }

        private void ClickPreferenciasPedidoVenda(object sender, RoutedEventArgs e)
        {
            new PedidoVendaPreferenciaForm().ShowDialog();
        }

        private void NoCliqueAliquotaInternaPorEstado(object sender, RoutedEventArgs e)
        {
            new AliquotaInternaForm().ShowDialog();
        }

        private void OnClickCadastrarTabelaPreco(object sender, RoutedEventArgs e)
        {
            AbrirJanelaEmAba("Tabela de preços", new TabelaPrecosListagem(new TabelaPrecosListagemModel()));
        }

        private void CliqueGerenciarNFCe(object sender, RoutedEventArgs e)
        {
            _faturamentoController.AbrirListagemNfce();
        }
    }
}