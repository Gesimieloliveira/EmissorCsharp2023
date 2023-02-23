using System;
using System.Windows;
using Fusion.Conversor.Core.BancoDados;
using Fusion.Conversor.Views.CvClientes;
using Fusion.Conversor.Views.CvProdutos;
using Fusion.Conversor.Views.SGBD;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Configuracao;

namespace Fusion.Conversor
{
    public partial class MainWindow
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly ConfiguradorConexao _configuradorConexao = new ConfiguradorConexao();
        private ProdutoConversaoControl _converterProdutoContent;
        private PessoaConversaoControl _converterPessoaContent;

        public MainWindow()
        {
            InitializeComponent();
            Instancia = this;
        }

        public static MainWindow Instancia { get; private set; }

        public void ClearContent()
        {
            Dispatcher.Invoke(() => { PageContent.Child = null; });
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_configuradorConexao.ArquivoExiste())
            {
                return;
            }

            AbreJanelaConexaoBancoDados();
        }

        private void AbreJanelaConexaoBancoDados()
        {
            var contexto = new ConfigurarConexaoViewModel(_configuradorConexao);
            var view = new ConfigurarConexaoView(contexto);

            view.ShowDialog();

            if (_configuradorConexao.ArquivoExiste())
            {
                return;
            }

            DialogBox.MostraAviso("Arquivo de conexão não foi criado");
            Close();
        }

        private void ConverterProdutosClickHandler(object sender, RoutedEventArgs e)
        {
            PageContent.Child = GetConverterProdutosContent();
        }

        private UIElement GetConverterProdutosContent()
        {
            if (_converterProdutoContent != null)
            {
                return _converterProdutoContent;
            }

            var contexto = new ProdutoConversaoContexto(_sessaoManager);
            _converterProdutoContent = new ProdutoConversaoControl(contexto);

            return _converterProdutoContent;
        }

        private void ConverterPessoasClickHandler(object sender, RoutedEventArgs e)
        {
            PageContent.Child = GetConverterPessoaContent();
        }

        private UIElement GetConverterPessoaContent()
        {
            if (_converterPessoaContent != null)
            {
                return _converterPessoaContent;
            }

            var contexto = new PessoaConversaoContexto(_sessaoManager);
            _converterPessoaContent = new PessoaConversaoControl(contexto);

            return _converterPessoaContent;
        }

        private void ConexaoFusionClickHandler(object sender, RoutedEventArgs e)
        {
            AbreJanelaConexaoBancoDados();
        }

        private void GerenciarSGBDClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new GerenciarSGBDConexto();
            var view = new GerenciarSGBDView(contexto);

            view.ShowDialog();
        }

        private void UpdateBaseDadosClickHandler(object sender, RoutedEventArgs e)
        {
            var conexaoFacade = new ConexaoFacade();
            var cfg = conexaoFacade.GetDadosConexao();

            var msgConfirmacao = $"Deseja atualizar o banco de dados {cfg.BancoDados}?";

            if (DialogBox.MostraConfirmacao(msgConfirmacao) != MessageBoxResult.Yes)
            {
                return;
            }

            AcaoAtualizarBancoDados();
        }

        private async void AcaoAtualizarBancoDados()
        {
            var atualizador = new AtualizadorBancoDados();

            if (atualizador.PrecisaAtualizar() == false)
            {
                DialogBox.MostraInformacao("Banco de dados não precisa ser atualizado!");
                return;
            }

            await RunTaskWithProgress(() =>
            {
                try
                {
                    atualizador.Atualizar();
                    DialogBox.MostraInformacao("Banco de dados foi atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => { DialogBox.MostraErro(ex.Message, ex); });
                }
            });
        }
    }
}