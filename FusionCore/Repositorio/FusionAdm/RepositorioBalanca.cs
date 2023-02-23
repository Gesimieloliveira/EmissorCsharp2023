using FusionCore.FusionAdm.Configuracoes;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioBalanca : Repositorio<Balanca, byte>, IRepositorioBalanca
    {
        public RepositorioBalanca(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(Balanca balanca)
        {
            Sessao.SaveOrUpdate(balanca);
            Sessao.Flush();
        }

        public Balanca BuscarUnicaBalanca()
        {
            return GetPeloId(1) ?? new Balanca();
        }
    }
}