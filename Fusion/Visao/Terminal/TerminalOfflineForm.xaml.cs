using System;
using System.Windows;
using System.Windows.Controls;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.TerminalOffline;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Terminal
{
    public partial class TerminalOfflineForm
    {
        private readonly TerminalOfflineFormModel _contexto;

        public TerminalOfflineForm(TerminalOffline terminalOffline)
        {
            _contexto = new TerminalOfflineFormModel(terminalOffline);
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_contexto == null)
            {
                return;
            }

            _contexto.Inicializar();
            DataContext = _contexto;
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.SalvarModel();
                DialogBox.MostraInformacao("Registro salvo com sucesso");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void DesvincularBindingTerminal_OnClick(object sender, RoutedEventArgs e)
        {
            const string aviso =
                "Processo desvincula o terminal, certifique que não existe nenhuma sincronização pendente. Continuar?";

            if (!DialogBox.MostraConfirmacao(aviso, MessageBoxImage.Information))
            {
                return;
            }

            _contexto.DesvinculacaoBindTerminal();
            DialogBox.MostraInformacao("Desvinculação efetuada com sucesso.");
        }

        private void RemoverEmissor_OnClick(object sender, RoutedEventArgs e)
        {
            var aviso = "Continuar com a retirada do Emissor da lista de Emissores Aptos?";

            if (!DialogBox.MostraDialogoDeConfirmacao(aviso))
            {
                return;
            }

            var emissorSelecionado = GetEmissorFiscalDaLista(e.OriginalSource);

            _contexto.RemoverEmissorLista(emissorSelecionado);
        }

        

        private static EmissorFiscal GetEmissorFiscalDaLista(object sender)
        {
            var button = sender as Button;
            var emissorSelecionadoLista = button?.DataContext as EmissorFiscal;
            return emissorSelecionadoLista;
        }
    }
}