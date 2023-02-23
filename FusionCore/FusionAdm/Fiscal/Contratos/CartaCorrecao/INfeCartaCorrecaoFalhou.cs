namespace FusionCore.FusionAdm.Fiscal.Contratos.CartaCorrecao
{
    public interface INfeCartaCorrecaoFalhou
    {
        string Mensagem { get; set; }

        System.Exception Exception { get; set; }
    }
}