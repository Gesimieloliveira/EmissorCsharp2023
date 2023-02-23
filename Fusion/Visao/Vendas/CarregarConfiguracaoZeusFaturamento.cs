using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;

namespace Fusion.Visao.Vendas
{
    public static class CarregarConfiguracaoZeusFaturamento
    {
        public static  ConfiguracaoZeusBuilder CarregarConfiguracaoServicoZeus(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissorFiscalId = new RepositorioCupomFiscal(sessao).ObterEmissorFiscalId(venda);
                var emissorFiscal = new RepositorioEmissorFiscal(sessao).GetPeloId(emissorFiscalId);

                return new ConfiguracaoZeusBuilder(emissorFiscal.EmissorFiscalNfce, TipoEmissao.Normal);
            }
        }
    }
}