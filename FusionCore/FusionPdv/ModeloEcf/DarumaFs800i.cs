namespace FusionCore.FusionPdv.ModeloEcf
{
    public class DarumaFs800I : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.EscECF;

        public override string ObterModeloEcf { get; set; } = "Daruma FS-800i*";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
