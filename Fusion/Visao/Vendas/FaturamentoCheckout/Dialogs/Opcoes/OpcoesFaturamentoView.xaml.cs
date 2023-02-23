using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Facades;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Opcoes
{
    public partial class OpcoesFaturamentoView
    {
        private readonly PreferenciasFaturamentoFacade _preferenciaFacade;
        private readonly ImpressorFaturamento _impressor;
        private readonly FaturamentoVenda _faturamento;

        public OpcoesFaturamentoView(PreferenciasFaturamentoFacade preferenciaFacade, ImpressorFaturamento impressor, FaturamentoVenda faturamento)
        {
            _impressor = impressor;
            _faturamento = faturamento;
            _preferenciaFacade = preferenciaFacade;
            InitializeComponent();

            AtalhoBinder.Iniciar(this)
                .BindBotao(Key.F2, BtnImprimir)
                .BindBotao(Key.F3, BtnEmitirCupom);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            VerificaContingencia();
        }

        private void OnImprimirClick(object sender, RoutedEventArgs e)
        {
            BtnImprimir.IsEnabled = false;

            try
            {
                var preferencias = _preferenciaFacade.GetPreferenciaDaMaquina();
                switch (preferencias.PreVisualizar)
                {
                    case false:
                        _impressor.Imprime(_faturamento, preferencias);
                        break;
                    case true:
                        _impressor.Viazualiza(_faturamento, preferencias);
                        break;
                }

                _impressor.VisualizaPromissoriaCarne(_faturamento, preferencias);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                BtnImprimir.IsEnabled = true;
            }
        }

        private void OnEmitirCupomClick(object sender, RoutedEventArgs e)
        {
            BtnEmitirCupom.IsEnabled = false;

            try
            {
                var preferencia = _preferenciaFacade.GetPreferenciaDaMaquina();

                IAutorizadorTela autorizador = new AutorizadorNfceTela(
                    preferencia.LayoutImpressao,
                    preferencia.Impressora,
                    preferencia.ImprimeDuasVias,
                    preferencia.VisualizarCupom,
                    preferencia.ImprimirCupom);

                autorizador.EnviaSefaz(_faturamento);
            }
            finally
            {
                BtnEmitirCupom.IsEnabled = true;
            }

            VerificaContingencia();
        }

        private void VerificaContingencia()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                ITodasContingenciasNfce todasContingencias = new TodasContingencias(sessao);

                TextBoxContingenciaAtivada.Visibility = todasContingencias.ExisteContingenciaEmAberto()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
    }
}