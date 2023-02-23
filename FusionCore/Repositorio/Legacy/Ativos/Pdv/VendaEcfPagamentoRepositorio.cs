using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class VendaEcfPagamentoRepositorio : RepositorioBase<VendaEcfPagamentoDt>
    {
        public VendaEcfPagamentoRepositorio(ISession sessao) : base(sessao)
        {
        }
    }
}
