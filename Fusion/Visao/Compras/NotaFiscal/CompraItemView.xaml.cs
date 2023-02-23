using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Compras.NotaFiscal.Controls;
using FusionCore.Excecoes.RegraNegocio;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public partial class CompraItemView
    {
        private ChildWindow _childConfiguracao;

        public CompraItemView(CompraItemViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private CompraItemViewModel GetModel => DataContext as CompraItemViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Inicializar();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                AbreChildConfiguracoesRegraIcms();
            }
        }

        private async void AbreChildConfiguracoesRegraIcms()
        {
            if (_childConfiguracao?.IsOpen == true)
            {
                return;
            }

            _childConfiguracao = ChildWindowFactory.Cria(new ConfiguracaoRegraCalculoItem(), GetModel.CriaRegraModel());

            await this.ShowChildWindowAsync(_childConfiguracao);
        }

        private void Inicializar()
        {
            GetModel.Inicializar();
            TbCodigoCfop.Focus();
        }

        private void CfopLostFocusHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                GetModel.CarregaCfopPeloCodigo();
            }
            catch (Exception ex)
            {
                e.Handled = true;
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void CodigoProdutoLostFocusHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                GetModel.CarregaProdutoPeloCodigo();
            }
            catch (Exception ex)
            {
                e.Handled = true;
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void SalvarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                GetModel.SalvarAlteracoes();
                DialogBox.MostraInformacao("O item foi salvo com sucesso!");
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro("Não consegui savlar esse item :(", ex);
                return;
            }

            if (GetModel.IsEdicao())
            {
                Close();
                return;
            }

            IniciaNovoModel();
        }

        private void IniciaNovoModel()
        {
            DataContext = new CompraItemViewModel(GetModel.GetNota());
            Inicializar();
        }

        private void MouseUpConfiguraRegrasHandler(object sender, MouseButtonEventArgs e)
        {
            AbreChildConfiguracoesRegraIcms();
        }
    }
}