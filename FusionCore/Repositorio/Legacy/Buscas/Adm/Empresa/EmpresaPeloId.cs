using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa
{
    public class EmpresaPeloId : IBuscaUnico<EmpresaDTO>
    {
        private readonly int _id;

        public EmpresaPeloId(int id)
        {
            _id = id;
        }

        public EmpresaDTO Busca(ISession sessao)
        {
            return sessao.Get<EmpresaDTO>(_id);
        }
    }
}
