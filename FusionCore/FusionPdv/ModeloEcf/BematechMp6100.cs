namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp6100 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Bematech;

        public override string ObterModeloEcf { get; set; } = "Bematech MP 6100 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
