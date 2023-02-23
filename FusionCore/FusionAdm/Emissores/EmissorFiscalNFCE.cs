using System;
using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalNFCE : IEntidade, ISincronizavelAdm, IDadosServicoSefaz
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte IbgeEstadoEmissao => EmissorFiscal.Empresa.EstadoDTO.CodigoIbge;
        public ConfiguracaoCertificado Certificado => EmissorFiscal.GetCertificadoZeus();
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.NFCe;
        public bool UsaNumeracaoDiferenteContigencia { get; set; }
        public short Serie { get; set; }
        public short SerieContingencia { get; set; }
        public int NumeroAtual { get; set; }
        public int NumeroAtualContingencia { get; set; }
        public int IdToken { get; set; }
        public string Csc { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public bool IsIntegradorCeara { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public string Referencia => EmissorFiscalId.ToString(); 
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.EmissorFiscal;
        public ProtocoloSeguranca ProtocoloSeguranca => EmissorFiscal.ProtocoloSeguranca;
    }
}