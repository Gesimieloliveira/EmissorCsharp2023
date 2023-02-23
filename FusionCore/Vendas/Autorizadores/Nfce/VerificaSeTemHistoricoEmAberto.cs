using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public static class VerificaSeTemHistoricoEmAberto
    {
        public static bool Verifica(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                if (repositorioCupomFiscal.ExisteHistoricoEmAberto(venda)) return true;
            }

            return false;
        }

        public static bool ComRejeicao(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                if (repositorioCupomFiscal.ExisteHistoricoEmAbertoComRejeicao(venda)) return true;
            }

            return false;
        }
    }
}