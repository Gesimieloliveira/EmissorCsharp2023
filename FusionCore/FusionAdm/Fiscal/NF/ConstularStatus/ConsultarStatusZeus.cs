using DFe.Classes.Entidades;
using FusionCore.FusionAdm.Builders;
using NFe.Servicos;
using TipoEmissao = FusionCore.FusionAdm.Fiscal.Flags.TipoEmissao;
using ZeusTipoAmbiente = DFe.Classes.Flags.TipoAmbiente;

namespace FusionCore.FusionAdm.Fiscal.NF.ConstularStatus
{
    public class RetornoConsultaStatus
    {
        public int CStat { get; }
        public Estado CUf { get; }
        public ZeusTipoAmbiente TpAmb { get; }
        public string VerAplic { get; }
        public string Versao { get; }
        public string XMotivo { get; }

        public RetornoConsultaStatus(int cStat, Estado cUf, ZeusTipoAmbiente tpAmb, string verAplic, string versao,
            string xMotivo)
        {
            CStat = cStat;
            CUf = cUf;
            TpAmb = tpAmb;
            VerAplic = verAplic;
            Versao = versao;
            XMotivo = xMotivo;
        }
    }

    public class ConsultarStatusZeus
    {
        private readonly IDadosServicoSefaz _dadosServico;

        public ConsultarStatusZeus(IDadosServicoSefaz dadosServico)
        {
            _dadosServico = dadosServico;
        }

        public RetornoConsultaStatus ConsultarStatus()
        {
            var builder = new ConfiguracaoZeusBuilder(_dadosServico, TipoEmissao.Normal);
            var cfgZeus = builder.GetConfiguracao();

            var servicoNFe = new ServicosNFe(cfgZeus);
            var retornoStatus = servicoNFe.NfeStatusServico();

            var cStat = retornoStatus.Retorno.cStat;
            var cUf = retornoStatus.Retorno.cUF;
            var tpAmb = retornoStatus.Retorno.tpAmb;
            var verAplic = retornoStatus.Retorno.verAplic;
            var versao = retornoStatus.Retorno.versao;
            var xMotivo = retornoStatus.Retorno.xMotivo;

            var retorno = new RetornoConsultaStatus(cStat, cUf, tpAmb, verAplic, versao, xMotivo);

            return retorno;
        }
    }
}