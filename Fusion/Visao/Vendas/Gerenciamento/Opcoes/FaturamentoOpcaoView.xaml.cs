using System;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.ControlarNfces;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.Papeis.Enums;
using FusionCore.Vendas.Servicos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SharedViews.AutorizarOperacao;

namespace Fusion.Visao.Vendas.Gerenciamento.Opcoes
{
    public partial class FaturamentoOpcaoView
    {
        public FaturamentoOpcaoView()
        {
            InitializeComponent();
        }

        private FaturamentoOpcaoContexto Contexto => DataContext as FaturamentoOpcaoContexto;

        private void CancelaFaturamentoClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var faturamento = Contexto.CarregarFaturamento();
                var payload = new FaturamentoCancelado(faturamento.Id, faturamento.Total);

                var sessao = SessaoSistema.Instancia.SessaoManager;

                var autorizarUsuario = new AutorizarUsuarioAdm(sessao);

                var autorizarCancelamento = new AutorizarOperacaoView(
                    sessao,
                    autorizarUsuario,
                    SessaoSistema.Instancia.UsuarioLogado,
                    faturamento.Id.ToString(),
                    Permissao.CANCELAR_FATURAMENTO,
                    payload,
                     () =>
                {
                    EfetuarCancelamento();
                });

                autorizarCancelamento.ExecutarAcao();
                Close();
                DialogBox.MostraInformacao("Faturamento cancelado com sucesso!");

            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EfetuarCancelamento()
        {
            AbstractCanceladorTela cancelar = new CancelarNfceTela();

            cancelar.CancelouComSucesso += delegate
            {
                Contexto.OnCompletouAcao();
                Close();
            };

            cancelar.Cancelar(Contexto.CarregarFaturamento());

        }

        private void ImprimeClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.Visualizar();
                Close();
            }
            catch (PreferenciaException pex)
            {
                DialogBox.MostraAviso(pex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EmitirCupom_Clique(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ContingenciaAtiva.EstaAtiva())
                {
                    DialogBox.MostraAviso("Contingência está ativa, espere 40 minutos é tente novamente.");
                    return;
                }

                var venda = Contexto.CarregarFaturamento();

                if (venda == null)
                {
                    DialogBox.MostraAviso("Não encontrei o faturamento");
                    return;
                }

                if (venda.Autorizada() || venda.EUmaVendaOffline())
                {
                    var preferencia = Contexto.PreferenciaFacade.GetPreferenciaDaMaquina();

                    var imprimir = new ImprimirCupomFiscalNfce(preferencia.LayoutImpressao
                        , preferencia.Impressora
                        , preferencia.ImprimeDuasVias
                        , true
                        , false);
                    imprimir.Imprime(venda);
                    return;
                }

                IAutorizadorTela autorizador = new AutorizadorNfceTela();
                autorizador.EnviaSefaz(venda, () => Contexto.OnCompletouAcao());
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                return;
            }

            Contexto.OnCompletouAcao();
            Close();
        }
    }
}
