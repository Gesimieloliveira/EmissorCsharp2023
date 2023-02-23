namespace FusionCore.Exportacao.ItensBuscaRapida
{
    public interface ILayoutBuscaRapida
    {
        string Tag { get; }
        CasasDecimais CasasDecimais { get; set; }
        string ConverteLinha(Linha item);
    }
}