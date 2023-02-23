namespace FusionCore.FusionPdv.ModeloEcf
{
    public class ModeloEcfTemplate
    {
        public virtual ModeloEmissor ModeloAcbrEcf { get; set; }
        public virtual string ObterModeloEcf { get; set; }
        public virtual string VelocidadeModeloEcf { get; set; }
        public virtual string Instancia { get; set; } = "FusionPdv.Ecf.Implementacao.Acbr";
    }
}