using System.Xml;
using DFe.Utils;
using FusionCore.DFe.WsdlCte.Homologacao.ConsultaProtocolo;
using FusionCore.DFe.WsdlCte.Homologacao.Helper;
using FusionCore.DFe.XmlCte.XmlCte.ConsultaProtocolo;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionAdm.CteEletronico.ConsultarProtocolos
{
    public class NegocioConsultaProtocolo
    {
        private readonly ICancelarCte _cte;

        public NegocioConsultaProtocolo(ICancelarCte cte)
        {
            _cte = cte;
        }


        public FusionRetornoConsultaProtocoloCTe Consultar()
        {
            var consultaProtocoloCTe = Constroi();

            ValidaSchema(consultaProtocoloCTe);

            var retornoConsulta = EnviaSefaz(consultaProtocoloCTe);

            return retornoConsulta;
        }

        private FusionRetornoConsultaProtocoloCTe EnviaSefaz(FusionConsultaProtocoloCTe consultaProtocoloCTe)
        {
            var xmlEnviar = FuncoesXml.ClasseParaXmlString(consultaProtocoloCTe);

            var url = UrlHelper.ObterUrl(_cte.Estado.ToXml(),
                _cte.TipoAmbiente.ToXml(), _cte.TipoEmissao.ToXml(_cte.Estado));

            var wsdl = new CteConsulta(url.CteConsultaProtocolo)
            {
                cteCabecMsgValue = new cteCabecMsg
                {
                    cUF = _cte.Estado.CodigoIbge.ToString(),
                    versaoDados = "3.00"
                }
            };

            wsdl.ClientCertificates.Add(CertificadoDigitalFactory.Cria(_cte.EmissorFiscal, true));

            var xmlRequest = new XmlDocument();
            xmlRequest.LoadXml(xmlEnviar);

            var retorno = wsdl.cteConsultaCT(xmlRequest);

            var objetoRetorno = FuncoesXml.XmlStringParaClasse<FusionRetornoConsultaProtocoloCTe>(retorno.OuterXml);

            var xml = new XmlDocument();
            xml.LoadXml(retorno.OuterXml);

            objetoRetorno.Xml = xml;

            return objetoRetorno;
        }

        private void ValidaSchema(FusionConsultaProtocoloCTe consultaProtocoloCTe)
        {
            var xmlConsultaProtocolo = FuncoesXml.ClasseParaXmlString(consultaProtocoloCTe);

            var validacao = new ValidarSchema();

            validacao.Validar(xmlConsultaProtocolo, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\consSitCTe_v3.00.xsd");
        }

        private FusionConsultaProtocoloCTe Constroi()
        {
            return new FusionConsultaProtocoloCTe
            {
                Chave = _cte.Chave,
                Ambiente = _cte.TipoAmbiente.ToXml(),
                Versao = "3.00"
            };
        }
    }
}