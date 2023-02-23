using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class FormaDePagamentoRepositorio : RepositorioBase<FormaPagamentoEcfDt>
    {
        public FormaDePagamentoRepositorio(ISession sessao) : base(sessao)
        {
        }
    }
}