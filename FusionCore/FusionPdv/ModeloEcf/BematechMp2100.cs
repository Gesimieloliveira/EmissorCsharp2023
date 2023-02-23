namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp2100 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Bematech;

        //public override string Instancia { get; set; } = "StarkPdv.Ecf.Implementacao.Bematech";

        public override string ObterModeloEcf { get; set; } = "Bematech MP 2100 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
