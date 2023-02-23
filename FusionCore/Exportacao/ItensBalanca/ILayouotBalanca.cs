namespace FusionCore.Exportacao.ItensBalanca
{
    public interface ILayouotBalanca
    {
        string Tag { get; }
        string ConverteLinha(ModeloItem item);
    }
}