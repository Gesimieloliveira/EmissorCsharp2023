namespace FusionCore.RecipienteDados
{
    public interface IRecipiente
    {
        bool ManterCache { get; }
        void RecarregaCache();
    }
}
