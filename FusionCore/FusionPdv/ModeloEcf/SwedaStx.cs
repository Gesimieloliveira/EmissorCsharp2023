namespace FusionCore.FusionPdv.ModeloEcf
{
    public class SwedaStx : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.SwedaSTX;

        public override string ObterModeloEcf { get; set; } = "SwedaSTX";
        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
