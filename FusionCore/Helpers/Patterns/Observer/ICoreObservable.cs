namespace FusionCore.Helpers.Patterns.Observer
{
    public interface ICoreObservable<in T>
    {
        void Inscrever(T observer);
        void NotificarObservadores();
    }
}