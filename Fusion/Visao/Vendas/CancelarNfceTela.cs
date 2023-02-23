using System;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Cancelamento;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Facades;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas
{
    public class CancelarNfceTela : AbstractCanceladorTela
    {
        public event EventHandler CancelouComSucesso;

        public override void Cancelar(FaturamentoVenda venda)
        {
            var confirmou = DialogBox.MostraConfirmacao("Operação não pode ser revertida. Deseja continuar?");

            if (confirmou != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistema.ObterUsuarioLogado());

                var modelo = new CancelarFaturamentoViewModel(venda);

                modelo.CanceladoSucesso += delegate
                {
                    OnCancelouComSucesso();
                };

                new CancelarFaturamentoView(modelo).ShowDialog();
            }
            catch (NaoExisteCaixaAbertoException exC)
            {
                DialogBox.MostraInformacao(exC.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        protected virtual void OnCancelouComSucesso()
        {
            CancelouComSucesso?.Invoke(this, EventArgs.Empty);
        }
    }
}