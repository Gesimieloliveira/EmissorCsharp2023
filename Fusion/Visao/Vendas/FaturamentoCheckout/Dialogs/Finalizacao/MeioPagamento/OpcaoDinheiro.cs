using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public class OpcaoDinheiro : IOpcaoPagamento
    {
        public string Descricao { get; } = "DINHEIRO";

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var pagamento = FPagamento.CriarNoDinheiro(SessaoSistema.ObterUsuarioLogado(), valor);

                callback(Resultado.Sucesso(pagamento));
            });
        }

        public bool PermiteTroco()
        {
            return true;
        }
    }
}