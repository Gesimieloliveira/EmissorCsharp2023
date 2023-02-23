namespace FusionCore.RecipienteDados
{
    public abstract class Recipiente : IRecipiente
    {
        internal Recipiente()
        {
            //apenas factory instancia esse método
        }

        public abstract bool ManterCache { get; }
        public abstract void RecarregaCache();
    }
}
