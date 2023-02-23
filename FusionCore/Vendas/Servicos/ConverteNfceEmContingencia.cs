using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using NFe.Utils.NFe;

namespace FusionCore.Vendas.Servicos
{
    public static class ConverteNfceEmContingencia
    {
        public static void Converter(FaturamentoVenda venda)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupom = new RepositorioCupomFiscal(sessao);
                var cupomHistorico = repositorioCupom.BuscarHistoricoEmAberto(venda);

                var xmlEnvio = new ConstruirXmlNfceDeUmaVenda(venda, cupomHistorico.CupomFiscal).Construir()
                    .ObterXmlString().RemoverAcentos();

                cupomHistorico.AtualizarXmlEnvio(xmlEnvio);

                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                repositorioCupomFiscal.SalvarOuAlterarHistorico(cupomHistorico);

                transacao.Commit();
            }
        }
    }
}