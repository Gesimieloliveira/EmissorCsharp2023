using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.EmissaoSefaz
{
    public partial class EmissaoSefazView
    {
        private EmissaoSefazViewModel GetModel => (EmissaoSefazViewModel) DataContext;

        public EmissaoSefazView(EmissaoSefazViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            GetModel.CloseRequest += CloseRequestHandler;
            GetModel.Inicializar();
        }

        private void CloseRequestHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void ClickEmitirHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                GetModel.IniciarEmissao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            GetModel.CloseHandler();
        }
    }
}