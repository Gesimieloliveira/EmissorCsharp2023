using System;
using System.Windows;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ProdutoUnidade
{
    public partial class ProdutoUnidadeForm
    {
        public ProdutoUnidadeForm(ProdutoUnidadeDTO produtoUnidade)
        {
            DataContext = new ProdutoUnidadeFormModel(produtoUnidade);
            InitializeComponent();
        }

        private ProdutoUnidadeFormModel ModelVisao => DataContext as ProdutoUnidadeFormModel;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ConfigurarTela();

        }
        private void ConfigurarTela()
        {
            if (ModelVisao.NovoRegistro)
            {
                BotaoDeletar.Visibility = Visibility.Collapsed;
                BotaoSalvar.Content = "Salvar Inclusão";
                CSigla.Focus();
            };
        }

        private void ContentRenderedHandler(object sender, EventArgs e)
        {
            ModelVisao.Inicializa();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelVisao.SalvarModel();
                DialogBox.MostraInformacao("Unidade de produto salva com sucesso");
                Close();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraDialogoDeConfirmacao("Quer mesmo excluir está unidade?"))
            {
                return;
            }

            try
            {
                ModelVisao.DeletarModel();
                DialogBox.MostraInformacao("Unidade de produto foi deletada");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

    }
}