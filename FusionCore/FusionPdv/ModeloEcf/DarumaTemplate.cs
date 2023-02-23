namespace FusionCore.FusionPdv.ModeloEcf
{
    public class DarumaTemplate : ModeloEcfTemplate
    {
        protected string ModeloEcf = "Daruma";
        protected string VelocidadeEcf = "9600";

        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Daruma;

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
