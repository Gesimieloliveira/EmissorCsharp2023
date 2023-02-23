using System;
using System.Threading.Tasks;
using System.Windows;
using Fusion.Base.Notificacoes;
using Fusion.Factories;
using Fusion.Sessao;
using Fusion.Visao.Compras.NotaFiscal;
using Fusion.Visao.DocumentoAPagar;
using FusionCore.FusionAdm.Compras;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Compras.Importacao
{
    public partial class ImportacaoCompraView
    {
        private ImportacaoCompraViewModel _contexto;

        public ImportacaoCompraView(string xml = null)
        {
            InitializeComponent();
            InicializaContexto(xml);
        }

        private void InicializaContexto(string xml = null)
        {
            _contexto = new ImportacaoCompraViewModel();
            _contexto.ImportacaoCompleta += ImportacaoCompletaHandler;
            _contexto.AnalisarDocumentoXmlImportadoPelaManifestacao(xml);

            DataContext = _contexto;
        }

        private async void ImportacaoCompletaHandler(object sender, NotaFiscalCompra e)
        {
            if (e.XMLPossuiCobrancas() && SessaoSistema.Instancia.AcessoConcedido.PossuiFusionGestor)
            {
                await SolicitarImportacaoDeCobrancaAsync(e);
            }

            var confrimacao = DialogBox.MostraConfirmacao("Nota importada e salva com sucesso. Deseja visualiza-la?");
            if (confrimacao != MessageBoxResult.Yes)
            {
                Close();
                return;
            }

            Close();

            var view = new NotaFiscalCompraView(e);
            Dispatcher.Invoke(() => view.ShowDialog());
        }

        private async Task SolicitarImportacaoDeCobrancaAsync(NotaFiscalCompra nf)
        {
            try
            {
                var model = GerarContasPagarModelFactory.Criar(new Notificador(), nf);

                model.ConfirmacaoAoFehcarSemGerar 
                    = "Deseja cancelar a geração de documentos a pagar?" +
                      "\nObs: Você poderá gerar mais tarde na opção da nota.";

                model.AntesComitarDelegate = (sessao, malote) =>
                {
                    nf.Malote = malote;
                    var repositorio = new RepositorioNotaFiscalCompra(sessao);
                    repositorio.Salvar(nf);
                };

                var dialog = new GerarContasPagar(model);
                await this.ShowChildWindowAsync(dialog);
            }
            catch (Exception e)
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    var erro = e.Message;
                    DialogBox.MostraAviso($"Nota importada com sucesso. Porém houve um erro ao processar as parcelas, use a opção Gerar Documentos na nota lançada! Detalhe do erro: {erro}");
                }));
            }
        }

        private void CancelarAnalise(object sender, RoutedEventArgs e)
        {
            InicializaContexto();
        }

        private void BuscarXmlClickHandler(object sender, RoutedEventArgs e)
        {
            var fileDialog = FileDialogFactory.CriaDialogXml();

            if (fileDialog.ShowDialog() == true && fileDialog.CheckFileExists)
            {
                _contexto.DocumentoImportar = fileDialog.FileName;
            }
        }
    }
}
