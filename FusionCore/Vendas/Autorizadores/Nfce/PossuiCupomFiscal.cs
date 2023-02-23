using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class PossuiCupomFiscal : IPossuiCupomFiscal
    {
        public void ExisteCupomFiscal(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var possuiCupomFiscal = new RepositorioCupomFiscal(sessao).ExisteCupomParaEssaVenda(venda);

                if (possuiCupomFiscal)
                    throw new InvalidOperationException(
                        "Existe tentativa de fazer Cupom Fiscal para essa venda: não pode alterar a empresa");
            }
        }
    }
}