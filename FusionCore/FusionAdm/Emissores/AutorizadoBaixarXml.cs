namespace FusionCore.FusionAdm.Emissores
{
    public class AutorizadoBaixarXml
    {
        public byte EmissorFiscalId { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public string Descricao { get; set; }
        public string DocumentoUnico { get; set; }
    }
}