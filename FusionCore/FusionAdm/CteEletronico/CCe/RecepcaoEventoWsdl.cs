using System.Xml;
using FusionCore.DFe.WsdlCte.Homologacao.Evento;
using FusionCore.DFe.WsdlCte.Homologacao.Helper;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.Fiscal.Fabricas;

namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public class RecepcaoEventoWsdl
    {
        public XmlNode Executar(ICartaCorrecaoCte cte, XmlDocument xmlEnvio)
        {
            var url = UrlHelper.ObterUrl(cte.EmissorFiscal.Empresa.EstadoDTO.ToXml(), cte.TipoAmbiente.ToXml(), cte.TipoEmissao.ToXml(cte.EmissorFiscal.Empresa.EstadoDTO));

            var recepcaoEventoWsdl = new CteRecepcaoEvento(url.CteRecepcaoEvento)
            {
                cteCabecMsgValue = new cteCabecMsg
                {
                    cUF = cte.EmissorFiscal.Empresa.EstadoDTO.CodigoIbge.ToString(),
                    versaoDados = "3.00"
                }
            };

            recepcaoEventoWsdl.ClientCertificates.Add(CertificadoDigitalFactory.Cria(cte.EmissorFiscal, true));

            var xmlRespostaWsdl = recepcaoEventoWsdl.cteRecepcaoEvento(xmlEnvio);

            return xmlRespostaWsdl;
        } 
    }
}