namespace FusionCore.FusionPdv.ModeloEcf
{
    public class ElginTemplate : ModeloEcfTemplate
    {
        protected string ModeloEcf = "Elgin";
        protected string VelocidadeEcf = "9600";

        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.FiscNET;

        public override string ObterModeloEcf
        {
            get { return ModeloEcf; }
            set { ModeloEcf = value; }
        }

        public override string VelocidadeModeloEcf
        {
            get { return VelocidadeEcf; }
            set { VelocidadeEcf = value; }
        }
    }
}
