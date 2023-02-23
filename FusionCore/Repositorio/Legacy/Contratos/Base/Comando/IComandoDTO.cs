using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Contratos.Base.Comando
{
    public interface IComandoDTO<T> where T : IEntidade
    {
        T ExecutaComando(ISession sessao);
    }
}