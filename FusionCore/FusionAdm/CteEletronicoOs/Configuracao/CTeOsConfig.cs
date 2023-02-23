using System.Net;
using DFe.Configuracao;
using DFe.DocumentosEletronicos.Entidades;
using DFe.DocumentosEletronicos.Flags;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.FusionAdm.CteEletronicoOs.Configuracao
{
    public class CTeOsConfig : DFeConfig
    {
        public void Inicializar(EmissorFiscal emissorFiscal, TipoEmissao tipoEmissao)
        {
            TimeOut = 30000;
            ProtocoloDeSeguranca = emissorFiscal.ProtocoloSeguranca.ToSecurityProtocol();
            EstadoUf = EstadoUf.CodigoIbgeParaEstado(emissorFiscal.Empresa.EstadoDTO.CodigoIbge.ToString());
            TipoAmbiente = emissorFiscal.EmissorFiscalCteOs.Ambiente == Fiscal.Flags.TipoAmbiente.Homologacao
                ? TipoAmbiente.Homologacao
                : TipoAmbiente.Producao;

            VersaoServico = VersaoServico.Versao300;
            CaminhoSchemas = ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.CteOs";
            IsSalvarXml = true;
            CaminhoSalvarXml = new ManipulaPasta(ManipulaArquivo.LocalAplicacao() + @"\XmlCTeOS").CriaPastaSeNaoExistir().Diretorio;
            CnpjEmitente = emissorFiscal.Empresa.Cnpj;
            RemoverAcentos = true;
            IsEfetuarCacheCertificadoDigital = true;
            IsContingenciaSvcAtiva = tipoEmissao == TipoEmissao.Contingencia;
        }


        public override TipoAmbiente TipoAmbiente { get; set; }
        public override VersaoServico VersaoServico { get; set; }
        public override Estado EstadoUf { get; set; }
        public override SecurityProtocolType ProtocoloDeSeguranca { get; set; }
        public override string CnpjEmitente { get; set; }
    }
}