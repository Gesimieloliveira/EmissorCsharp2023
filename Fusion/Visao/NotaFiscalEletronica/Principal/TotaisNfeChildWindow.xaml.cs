using System;
using System.Windows;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class TotaisNfeChildWindow
    {
        public TotaisNfeChildWindow(Nfeletronica nfe)
        {
            Contexto = new TotaisNfeChildWindowModel(nfe);
            InitializeComponent();
        }

        public TotaisNfeChildWindowModel Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            TextBoxDesconto.Focus();
        }

        private void SalvarAlteracoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.SalvarAlteracoes();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}