namespace FusionCore.FusionAdm.CteEletronico.Emissao
{
    public struct ImpostoAproximadoIbpt
    {
        public ImpostoAproximadoIbpt(decimal estadual, decimal federal)
        {
            Estadual = estadual;
            Federal = federal;
        }

        public decimal Estadual { get; }
        public decimal Federal { get; }
        public decimal Total => Estadual + Federal;
    }
}