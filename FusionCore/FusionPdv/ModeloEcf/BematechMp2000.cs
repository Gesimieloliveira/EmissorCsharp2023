namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp2000 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Bematech;

        public override string ObterModeloEcf { get; set; } = "Bematech MP 2000 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
