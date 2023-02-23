namespace FusionCore.FusionPdv.ModeloEcf
{
    public class Epson : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Epson;

        public override string ObterModeloEcf { get; set; } = "Epson";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
