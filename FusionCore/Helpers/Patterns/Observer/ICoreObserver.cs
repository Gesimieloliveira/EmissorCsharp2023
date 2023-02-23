namespace FusionCore.Helpers.Patterns.Observer
{
    public interface ICoreObserver<in T>
    {
        void Notificacao(T observable);
    }
}