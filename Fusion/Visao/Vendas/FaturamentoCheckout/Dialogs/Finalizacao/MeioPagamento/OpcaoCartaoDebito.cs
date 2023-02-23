using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public class OpcaoCartaoDebito : IOpcaoPagamento
    {
        public string Descricao { get; } = "CARTÃO DÉDITO";

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var pagamento = FPagamento.CriarNoCartaoDebito(SessaoSistema.ObterUsuarioLogado(), valor);

                callback(Resultado.Sucesso(pagamento));
            });
        }

        public bool PermiteTroco() => false;
    }
}