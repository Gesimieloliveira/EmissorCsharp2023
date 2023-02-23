using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.SelecionarNfce
{
    public partial class SelecionarUmClienteParaNfeFormulario
    {
        private readonly SelecionarUmClienteParaNfeFormularioModel _modelo;

        public SelecionarUmClienteParaNfeFormulario(SelecionarUmClienteParaNfeFormularioModel modelo)
        {
            _modelo = modelo;
            _modelo.Fechar += delegate { Close(); };
            InitializeComponent();
            DataContext = _modelo;
        }

        private void AdicionarClienteParaConversao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DialogBox.MostraConfirmacao("Selecionar este cliente para emissão da nf-e?", MessageBoxImage.Question))
                    _modelo.SelecionarCliente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
