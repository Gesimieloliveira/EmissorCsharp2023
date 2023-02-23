using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionCore.Excecoes.RegraNegocio;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddVeiculoTracao
    {
        private FlyoutAddVeiculoTracaoModel ViewModel => DataContext as FlyoutAddVeiculoTracaoModel;

        public FlyoutAddVeiculoTracao()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaVeiculo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel == null)
                {
                    return;
                }

                ViewModel.SalvarVeiculoTracao();
                ViewModel.LimpaCampos();
                ViewModel.IsOpen = false;
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
