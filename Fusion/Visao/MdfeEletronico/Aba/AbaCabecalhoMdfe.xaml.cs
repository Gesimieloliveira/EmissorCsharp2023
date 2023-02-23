using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class AbaCabecalhoMdfe
    {
        private AbaCabecalhoMdfeModel _viewModel;

        public AbaCabecalhoMdfe()
        {
            InitializeComponent();
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
        }

        private void AbaCabecalhoMdfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaCabecalhoMdfeModel;
            if (model == null) return;
            _viewModel = model;
        }

        private void ClickAlocarProximoNmeroHandler(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente avançar númeração?", MessageBoxImage.Question)) return;

            _viewModel.AlocarNumeracao();
        }
    }
}
