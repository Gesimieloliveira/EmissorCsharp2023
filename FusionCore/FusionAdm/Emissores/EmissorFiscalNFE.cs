using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalNFE : IEntidade, IDadosServicoSefaz
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.NFe;
        public short Serie { get; set; }
        public int NumeroAtual { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public byte IbgeEstadoEmissao => EmissorFiscal.Empresa.EstadoDTO.CodigoIbge;
        public ConfiguracaoCertificado Certificado => EmissorFiscal.GetCertificadoZeus();
        public ProtocoloSeguranca ProtocoloSeguranca => EmissorFiscal.ProtocoloSeguranca;
    }
}