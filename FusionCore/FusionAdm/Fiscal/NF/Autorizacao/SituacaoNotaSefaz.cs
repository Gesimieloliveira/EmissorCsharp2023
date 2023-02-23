using System.Security.Cryptography.X509Certificates;
using System.Xml;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.Recepcao.Retorno;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.Consulta;
using NFe.Utils.Recepcao;
using NFe.Utils.Validacao;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class SituacaoNotaSefaz
    {
        private readonly X509Certificate2 _certificado;
        private readonly ConfiguracaoServico _cfg;

        public SituacaoNotaSefaz(ConfiguracaoZeusBuilder zeusCfg, X509Certificate2 certificado)
        {
            _certificado = certificado;
            _cfg = zeusCfg.GetConfiguracao();
        }

        public XmlNode GetSituacaoPelaChave(ChaveSefaz chave)
        {
            var versaoServico = ServicoNFe.NfeConsultaProtocolo.VersaoServicoParaString(_cfg.VersaoNfeConsultaProtocolo);
            var ws = ServicoNfeFactory.CriaWsdlOutros(ServicoNFe.NfeConsultaProtocolo, _cfg, _certificado);

            var pedConsulta = new consSitNFe
            {
                versao = versaoServico,
                tpAmb = _cfg.tpAmb,
                chNFe = chave.Chave
            };

            var xmlConsulta = pedConsulta.ObterXmlString();
            Validador.Valida(ServicoNFe.NfeConsultaProtocolo, _cfg.VersaoNfeConsultaProtocolo, xmlConsulta, true, _cfg);

            var xmlEnvio = new XmlDocument();
            xmlEnvio.LoadXml(xmlConsulta);

            return ws.Execute(xmlEnvio);
        }

        public XmlNode GetSituacaoPeloRecibo(string recibo)
        {
            var versaoServico = ServicoNFe.NFeRetAutorizacao.VersaoServicoParaString(_cfg.VersaoNfeConsultaProtocolo);
            var ws = ServicoNfeFactory.CriaWsdlOutros(ServicoNFe.NFeRetAutorizacao, _cfg, _certificado);

            var pedRecibo = new consReciNFe
            {
                versao = versaoServico,
                tpAmb = _cfg.tpAmb,
                nRec = recibo
            };

            var xmlEnvio = new XmlDocument();
            xmlEnvio.LoadXml(pedRecibo.ObterXmlString());

            return ws.Execute(xmlEnvio);
        }
    }
}