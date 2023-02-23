using System;
using System.Windows;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ProdutoGrupo
{
    public partial class ProdutoGrupoForm
    {
        private readonly ProdutoGrupoFormModel _modeloVisao;

        public ProdutoGrupoForm(ProdutoGrupoDTO produtoGrupo)
        {
            _modeloVisao = new ProdutoGrupoFormModel(produtoGrupo);
            DataContext = _modeloVisao;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _modeloVisao.Inicializa();
            ConfigurarTela();
        }

        private void ConfigurarTela()
        {
            if (_modeloVisao.NovoRegistro)
            {
                BotaoDeletar.Visibility = Visibility.Collapsed;
                BotaoSalvar.Content = "Salvar Inclusão";
                CNome.Focus();
            };
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _modeloVisao.SalvarModel();
                DialogBox.MostraInformacao("Grupo de produto salvo com sucesso!");
                _modeloVisao.NovoRegistro = false;
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Deseja excluir este grupo?") != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _modeloVisao.DeletarModel();
                DialogBox.MostraInformacao("Grupo produto excluido com sucesso!");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

    }
}