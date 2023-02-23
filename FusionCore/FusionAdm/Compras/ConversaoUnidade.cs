namespace FusionCore.FusionAdm.Compras
{
    public class ConversaoUnidade
    {
        public ConversaoUnidade(decimal fator, decimal quantidade, string unidade)
        {
            Fator = fator;
            Quantidade = quantidade;
            Unidade = unidade;
        }

        public decimal Fator { get; }
        public decimal Quantidade { get; }
        public string Unidade { get; }
    }
}