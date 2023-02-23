using System;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Nfce
{
    public class NfceEmissaoAdm
    {
        public int NfceId { get; set; }
        public NfceAdm Nfce { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public Versao Versao { get; set; }
        public short Serie { get; set; }
        public int NumeroDocumento { get; set; }
        public int CodigoNumerico { get; set; }
        public string Chave { get; set; }
        public string TagId { get; set; }
        public string VersaoAplicativo { get; set; }
        public TipoEmissao TipoEmissao { get; set; }
        public TipoAmbiente TipoAmbiente { get; set; }
        public short CodigoAutorizacao { get; set; }
        public string Protocolo { get; set; }
        public string DigestValue { get; set; }
        public ProcessoEmissao ProcessoEmissao { get; set; }
        public string VersaoAplicativoAutorizacao { get; set; }
        public DateTime? RecebidoEm { get; set; }
        public bool Autorizado { get; set; }
        public string XmlAutorizado { get; set; }
        public string JustificativaContingencia { get; set; }
        public DateTime? EntrouEmContingenciaEm { get; set; }
    }
}