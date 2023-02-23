using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Preferencias.Repositorios
{
    public class RepositorioPreferenciaSistema : RepositorioBase
    {
        public RepositorioPreferenciaSistema(ISession sessao) : base(sessao)
        {
        }

        public PreferenciaSistema Buscar(string idMaquina, string chave)
        {
            return Sessao.QueryOver<PreferenciaSistema>()
                .Where(e => e.IdMaquina == idMaquina && e.Chave == chave)
                .SingleOrDefault();
        }

        public void Inserir(PreferenciaSistema preferencia)
        {
            Sessao.Persist(preferencia);
            Sessao.Flush();
        }

        public void Alterar(PreferenciaSistema preferencia)
        {
            Sessao.Update(preferencia);
            Sessao.Flush();
        }
    }
}
