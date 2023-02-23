using System;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Empresa;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.EmissorFiscal
{
    public class NfceEmissorFiscal : Entidade
    {
        public byte Id { get; set; }
        public EmpresaNfce Empresa { get; set; }
        public string Descricao { get; set; }
        public string ArquivoCertificado { get; set; }
        public string SenhaCertificado { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public bool FlagNfe { get; set; }
        public bool FlagNfce { get; set; }
        public string SerialNumberCertificado { get; set; }
        public NfceEmissorFiscalNfce EmissorFiscalNfce { get; set; }
        public ModeloDocumento Modelo => EmissorFiscalNfce?.Modelo ?? EmissorFiscalSat.ModeloDocumento;
        public TipoAmbiente Ambiente => EmissorFiscalNfce?.Ambiente ?? EmissorFiscalSat.Ambiente;
        public NfceEmissorFiscalSat EmissorFiscalSat { get; set; }
        public NfceAutorizadoBaixarXml AutorizadoBaixarXml { get; set; }
        public bool Sincronizado { get; set; }
        public TipoCertificadoDigital TipoCertificadoDigital { get; set; }
        protected override int ReferenciaUnica => Id;
        public bool FlagSat { get; set; }
        public ProtocoloSeguranca ProtocoloSeguranca { get; set; } = ProtocoloSeguranca.Tls11;
        public int? TerminalOfflineId { get; set; }
        public bool EmUso { get; set; }
    }
}
