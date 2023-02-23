namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public interface IEnvelope
    {
        string Grupo { get; }
        string Nome { get; }
        string Conteudo { get; set; }
    }
}