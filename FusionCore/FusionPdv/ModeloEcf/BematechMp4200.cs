namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp4200 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.EscECF;

        public override string ObterModeloEcf { get; set; } = "Bematech MP 4200 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
