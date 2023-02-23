using System;
using System.Windows;
using Fusion.Helpers;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.SelecionarNfce
{
    public partial class SelecionadorNfceFormulario
    {
        private readonly SelecionadorNfceFormularioModelo _modelo;

        public SelecionadorNfceFormulario(SelecionadorNfceFormularioModelo modelo)
        {
            _modelo = modelo;
            InitializeComponent();
            DataContext = modelo;
            _modelo.Fechar += (sender, args) => { Close(); };

            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private async void SelecionadorNfceFormulario_OnContentRendered(object sender, EventArgs e)
        {
            await RunTaskWithProgress(() =>
             {
                 _modelo.Inicializar();
             });
        }

        private void AdicionarNfceParaConversao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _modelo.SelecionarNfceParaConversao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void RemoverNfceDaConversao_OnClick(object sender, RoutedEventArgs e)
        {
            _modelo.RemoverNfceSelecionadaDaConversao();
        }
    }
}
