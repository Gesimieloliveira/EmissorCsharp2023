using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class EmpresaRepositorio : RepositorioBase<EmpresaDt>
    {
        public EmpresaRepositorio(ISession sessao)
            : base(sessao)
        {
        }
    }
}