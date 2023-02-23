using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;
using NFe.Classes.Servicos.Consulta;
using NFe.Utils.Consulta;

namespace FusionCore.FusionNfce.Servicos
{
    public class ServicoEmissaoHistoricoNfce
    {
        public NfceEmissaoHistorico FinalizaHistorico(NfceEmissaoHistorico historico, retConsSitNFe resultadoSefaz)
        {
            var historicoFinalizado = historico.ToBuilder()
                .ComXmlDeRetorno(new XmlRetorno(resultadoSefaz.ObterXmlString()))
                .ComCodigoDeAutorizacao(new CodigoAutorizacao(short.Parse(resultadoSefaz.cStat.ToString())))
                .ComMotivo(new Motivo(resultadoSefaz.xMotivo ?? string.Empty))
                .Finalizar();

            SalvarAlteracao(historicoFinalizado);

            return historicoFinalizado;
        }

        private void SalvarAlteracao(NfceEmissaoHistorico.Builder historico)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);

                repositorioNfce.SalvarHistorico(historico);

                transacao.Commit();
            }
        }

        public NfceEmissaoHistorico FinalizaHistoricoSemResultado(NfceEmissaoHistorico historico, string motivo)
        {
            var historicoFinalizado = historico.ToBuilder()
                .ComMotivo(new Motivo(motivo))
                .Finalizar();

            SalvarAlteracao(historicoFinalizado);

            return historicoFinalizado;
        }
    }
}