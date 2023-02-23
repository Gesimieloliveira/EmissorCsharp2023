using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;

namespace FusionNfce.Visao.Configuracao
{
    public partial class PreferenciasTerminalView
    {
        public PreferenciasTerminalView()
        {
            InitializeComponent();
            DataContext = new PreferenciasTerminalContexto(SessaoSistemaNfce.SessaoManager);
        }

        public PreferenciasTerminalContexto Contexto => DataContext as PreferenciasTerminalContexto;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            BindingVisibilidades();
            Contexto.CarregarImpressoras();
            Contexto.CarregarTabelas();
            Contexto.CarregarInformacoes();
        }

        private void BindingVisibilidades()
        {
            var bindingIsNfce = new Binding(nameof(Contexto.IsNfce)) {Source = Contexto};
            BindingOperations.SetBinding(LayoutImpressao, VisibilityHelper.IsVisibleProperty, bindingIsNfce);
            BindingOperations.SetBinding(DadosCartao, VisibilityHelper.IsVisibleProperty, bindingIsNfce);
        }

        private void SalvarPreferenciasClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.SalvarAlteracoes();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
            if (SessaoSistemaNfce.Preferencia != null)
            {
                return;
            }

            DialogBox.MostraAviso("Preciso que configure as preferencias inicias para este terminal");
            e.Cancel = true;
        }
    }
}