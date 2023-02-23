using System;
using System.Windows;
using Fusion.Visao.Empresa.ConsultaNaSefaz;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Empresa
{
    public partial class EmpresaForm
    {
        private readonly EmpresaFormModel _viewModel;

        public EmpresaForm(EmpresaFormModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.FecharTela += FecharTela;
        }

        private void FecharTela(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadedHandler(object sender, EventArgs eventArgs)
        {
            DataContext = _viewModel;

            try
            {
                _viewModel.PreencherViewModel();

                if (_viewModel.IsNovo)
                {
                    AbreJanelaBuscaSefazSeNovaEmpresa();
                }
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private async void AbreJanelaBuscaSefazSeNovaEmpresa()
        {
            var buscaEmpresaModel = new BuscarEmpresaNaSefazModel();
            buscaEmpresaModel.EmpresaEncontrada += _viewModel.EmpresaReceitaHandler;

            var buscarEmpresa = new BuscarEmpresaNaSefazForm(buscaEmpresaModel);

            await this.ShowChildWindowAsync(buscarEmpresa, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void SalvarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.SalvarModel();
        }
    }
}