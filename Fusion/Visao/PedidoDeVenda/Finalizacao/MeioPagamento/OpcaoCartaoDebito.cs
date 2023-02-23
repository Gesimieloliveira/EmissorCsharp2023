using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Basico;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento
{
    public class OpcaoCartaoDebito : IOpcaoPagamento
    {
        public string Descricao { get; } = ETipoPagamento.CartaoDebito.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var negociacao = Negociacao.CriarNoCartaoDebito(valor, SessaoSistema.ObterUsuarioLogado());

                callback(Resultado.Sucesso(negociacao));
            });
        }
    }
}