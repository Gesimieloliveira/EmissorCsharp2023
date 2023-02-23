using System;
using System.Threading.Tasks;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.Helpers.Basico;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento
{
    public class OpcaoDinheiro : IOpcaoPagamento
    {
        public string Descricao { get; } = ETipoPagamento.Dinheiro.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Task.Run(() =>
            {
                var d = new DinheiroNfe(SessaoSistema.ObterUsuarioLogado(), valor);

                callback(Resultado.Sucesso(d));
            });
        }
    }
}