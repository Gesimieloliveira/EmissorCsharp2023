namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class ProdutoPredominante
    {
        public TipoCarga TipoCarga { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public string Ncm { get; set; } = string.Empty;
        public string CepCarregamento { get; set; } = string.Empty;
        public string CepDescarregamento { get; set; } = string.Empty;
    }
}