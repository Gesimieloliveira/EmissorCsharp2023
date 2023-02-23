using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public class OpcaoCartaoCredito : IOpcaoPagamento
    {
        public string Descricao { get; } = "CARTÃO CRÉDITO";

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var pagamento = FPagamento.CriarNoCartaoCredito(SessaoSistema.ObterUsuarioLogado(), valor);

                callback(Resultado.Sucesso(pagamento));
            });
        }

        public bool PermiteTroco() => false;
    }
}