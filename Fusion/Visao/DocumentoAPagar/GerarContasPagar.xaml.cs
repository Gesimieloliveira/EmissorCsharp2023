using System;
using System.ComponentModel;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAPagar
{
    public partial class GerarContasPagar
    {
        private readonly GerarContasPagarModel _viewModel;
        private bool _docsForamGerados;

        public GerarContasPagar(GerarContasPagarModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_docsForamGerados || string.IsNullOrWhiteSpace(_viewModel.ConfirmacaoAoFehcarSemGerar))
            {
                return;
            }

            if (DialogBox.MostraDialogoDeConfirmacao(_viewModel.ConfirmacaoAoFehcarSemGerar))
            {
                return;
            }

            e.Cancel = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;

            if (!_viewModel.UsuarioTemPermissao)
            {
                RootContent.IsEnabled = false;
                DialogBox.MostraAviso("Usuário sem permissão para gerar documentos avulsos!");
            }
        }

        private void FornecedorPickClick(object sender, RoutedEventArgs e)
        {
            var picker = new PessoaPickerModel(new FornecedorEngine());

            picker.PickItemEvent += (o, args) =>
            {
                var fornecedor = args.GetItem<Fornecedor>();
                _viewModel.FornecedorSelecionado = fornecedor;
            };
            
            picker.GetPickerView().Show();
        }

        private void OnClickGerar(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SalvarDocumentos();
                DialogBox.MostraInformacao("Documentos foram gerados com sucesso!");
                _docsForamGerados = true;
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private async void OnClickGerenciarParcelas(object sender, RoutedEventArgs e)
        {
            if (!(_viewModel.ValorTotal is decimal d) || d <= 0)
            {
                DialogBox.MostraAviso("Preciso de valor para continuar");
                return;
            }

            var dialog = SessaoSistema.Instancia.ParcelamentoFactory.CriaDialog(_viewModel.ValorTotal.Value);

            dialog.Contexto.ComParcelas(_viewModel.ParcelasItems);
            dialog.Contexto.TipoDocumento = _viewModel.TipoDocumento;

            dialog.Contexto.ParceladoComSucesso += (o, args) =>
            {
                _viewModel.ComParcelas(args);
            };

            await this.TryFindParent<Window>().ShowChildWindowAsync(dialog);

            BotaoSalvar.Focus();
        }
    }
}
