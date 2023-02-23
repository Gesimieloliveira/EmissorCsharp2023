using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Basico;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento
{
    public class OpcaoCartaoCredito : IOpcaoPagamento
    {
        public string Descricao { get; } = ETipoPagamento.CartaoCredito.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var negociacao = Negociacao.CriarNoCartaoCredito(valor, SessaoSistema.ObterUsuarioLogado());

                callback(Resultado.Sucesso(negociacao));
            });
        }
    }
}