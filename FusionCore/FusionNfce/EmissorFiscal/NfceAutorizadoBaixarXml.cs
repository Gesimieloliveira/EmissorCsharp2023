using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.EmissorFiscal
{
    public class NfceAutorizadoBaixarXml : Entidade
    {
        public NfceAutorizadoBaixarXml() { }

        public NfceAutorizadoBaixarXml(AutorizadoBaixarXml autorizadorBaixarXml, NfceEmissorFiscal nfceEmissorFiscal) : base()
        {
            Descricao = autorizadorBaixarXml.Descricao;
            DocumentoUnico = autorizadorBaixarXml.DocumentoUnico;
            EmissorFiscal = nfceEmissorFiscal;
            EmissorFiscalId = nfceEmissorFiscal.Id;
        }

        public byte EmissorFiscalId { get; set; }
        public NfceEmissorFiscal EmissorFiscal { get; set; }
        public string Descricao { get; set; }
        public string DocumentoUnico { get; set; }

        protected override int ReferenciaUnica => EmissorFiscalId;
    }
}