using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DFe.Utils;
using FusionCore.DFe.RegrasNegocios;
using FusionCore.DFe.XmlCte.XmlCte.RetornoRecepcao;
using FusionCore.FusionAdm.CteEletronico.ConsultarProtocolos;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador
{
    public class SituacaoNotaSefazCte
    {
        private readonly X509Certificate2 _certificado;
        private readonly CteEmissaoHistorico _emissao;

        public SituacaoNotaSefazCte(X509Certificate2 certificado, CteEmissaoHistorico emissao)
        {
            _certificado = certificado;
            _emissao = emissao;
        }

        public XmlNode GetSituacaoPeloRecibo(string numeroRecibo, TipoAmbiente ambienteSefaz)
        {
            var retornoRecepcao = new FusionRetornoRecepcaoCTe
            {
                Ambiente = ambienteSefaz.ToXml(),
                NumeroRecibo = numeroRecibo
            };

            var xmlRetornoEnvioString = FuncoesXml.ClasseParaXmlString(retornoRecepcao);

            ValidarSchemaConsultaRecibo(xmlRetornoEnvioString);

            var xmlEnvio = new XmlDocument();
            xmlEnvio.LoadXml(xmlRetornoEnvioString);

            var retornoTratado = ConsultaRecibo(_emissao, xmlEnvio);

            var xmlRetorno = new XmlDocument();
            xmlRetorno.LoadXml(retornoTratado.Xml);

            return xmlRetorno;

        }

        private FusionResultadoEnvioLoteCTe ConsultaRecibo(CteEmissaoHistorico emissao, XmlDocument xmlRetornoEnvio)
        {
            var retornoRecepcaoSefaz = new RetornoRecepcaoCte().Executa(xmlRetornoEnvio,
                emissao.Cte.PerfilCte.EmissorFiscal.Empresa.EstadoDTO.ToXml(), _certificado, emissao.Cte.PerfilCte.EmissorFiscal.EmissorFiscalCte.Ambiente.ToXml(),
                emissao.Cte.TipoEmissao.ToXml(emissao.Cte.Estado));
            var retornoTratado = FuncoesXml.XmlStringParaClasse<FusionResultadoEnvioLoteCTe>(retornoRecepcaoSefaz.OuterXml);

            retornoTratado.Xml = retornoRecepcaoSefaz.OuterXml;

            return retornoTratado;
        }

        private static void ValidarSchemaConsultaRecibo(string xmlRetornoEnvioString)
        {
            var validar = new ValidarSchema();
            validar.Validar(xmlRetornoEnvioString, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\consReciCTe_v3.00.xsd");
        }

        public XmlNode GetSituacaoPelaChave(string chaveCte)
        {
            _emissao.Cte.SetHistorico(_emissao);
            var negocio = new NegocioConsultaProtocolo(_emissao.Cte);

            var retorno = negocio.Consultar();

            return retorno.Xml;
        }
    }
}