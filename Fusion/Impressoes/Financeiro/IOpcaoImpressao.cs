namespace Fusion.Impressoes.Financeiro
{
    public interface IOpcaoImpressao
    {
        bool PreVisualizar { get; set; }
        string Impresora { get; set; }

        void FazerImpressao(int maloteId);
    }
}