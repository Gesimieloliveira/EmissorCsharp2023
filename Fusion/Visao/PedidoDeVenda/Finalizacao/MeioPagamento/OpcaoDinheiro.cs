using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Basico;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento
{
    public class OpcaoDinheiro : IOpcaoPagamento
    {
        public string Descricao { get; } = ETipoPagamento.Dinheiro.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var negociacao = Negociacao.CriarNoDinheiro(valor, SessaoSistema.ObterUsuarioLogado());

                callback(Resultado.Sucesso(negociacao));
            });
        }
    }
}