using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop
{
    public class PerfilCfopPeloId : IBuscaUnico<PerfilCfopDTO>
    {
        private readonly string _codigo;

        public PerfilCfopPeloId(string codigo)
        {
            _codigo = codigo;
        }

        public PerfilCfopDTO Busca(ISession sessao)
        {
            var perfilCfop = sessao.Query<PerfilCfopDTO>().Where(p => p.Codigo.Equals(_codigo)).FirstOrNull();

            return (PerfilCfopDTO) perfilCfop;
        }
    }
}
