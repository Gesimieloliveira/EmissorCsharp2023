using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAdicionaSeguradora
    {
        public FlyoutAdicionaSeguradora()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaSeguradora(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddSeguroModel;

                model?.SalvarSeguro();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void DeletarAverbacao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Deseja realmente deletar?", MessageBoxImage.Question)) return;

                var model = DataContext as FlyoutAddSeguroModel;

                model?.DeletarAverbacao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void IncluirAverbacao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddSeguroModel;

                model?.IncluirAverbacao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
