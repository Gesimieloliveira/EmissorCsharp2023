namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp600 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Bematech;

        public override string ObterModeloEcf { get; set; } = "Bematech MP 600 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
