using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class AbaMdfeCarregamento
    {
        private AbaMdfeCarregamentoModel _viewModel;
        private MdfeEmitirForm _window;

        public AbaMdfeCarregamento()
        {
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as AbaMdfeCarregamentoModel;
            _window = this.TryFindParent<MdfeEmitirForm>();
        }

        private void ClickAdicionarLacreHandler(object sender, RoutedEventArgs e)
        {
            TabLacre.IsSelected = true;
            _viewModel.AbrirFlyoutAddLacre();
        }

        private void ClickAdicionarPercursoHandler(object sender, RoutedEventArgs e)
        {
            TabPercurso.IsSelected = true;
            _viewModel.AbrirFlyoutAddPercurso();
        }

        private void OnClickBotaoAnterior(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Anterior();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.Proximo();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void ClickAdicionarMunicipioCarregamentoHandler(object sender, RoutedEventArgs e)
        {
            TabCarregamento.IsSelected = true;
            _viewModel.AbrirFlyoutMunicipioCarregamento();
        }

        private void ClickAdicionarSeguroHandler(object sender, RoutedEventArgs e)
        {
            TabSeguro.IsSelected = true;
            _viewModel.AbrirFlyoutAddSeguro();
        }

        private void ClickAdicionarDescarregamentoHandler(object sender, RoutedEventArgs e)
        {
            TabDescarregamento.IsSelected = true;
            var view = new MdfeAddDocumentoView(_window, _viewModel);

            view.Contexto.Sucesso += (o, args) =>
            {
                _viewModel.AdicionarDescarregamento(args);
            };

            _window.ShowChildWindowAsync(view);
        }
    }
}