namespace FusionCore.Repositorio.Contratos
{
    public interface ISuporteSalvar<in T>
    {
        void Salvar(T entidade);
    }
}