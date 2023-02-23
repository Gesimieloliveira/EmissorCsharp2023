using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;

namespace Fusion.Visao.ProdutoLocalizacoes
{
    public partial class ProdutoLocalizacaoForm
    {
        private readonly ProdutoLocalizacaoFormModel _model;

        public ProdutoLocalizacaoForm(ProdutoLocalizacaoFormModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            VisibilityHelper.SetIsCollapsed(BotaoDeletar, _model.EhUmRegistroNovo());
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Salvar();
                DialogBox.MostraInformacao("Alterações foram salvas com sucesso :)");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Deseja prosseguir com a exclusão?") != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _model.Deletar();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}