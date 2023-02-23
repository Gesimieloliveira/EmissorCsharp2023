using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.PdvPagamento
{
    public class PdvPagamentoPorId : IBuscaUnico<PdvFormaPagamentoDTO>
    {
        private readonly int _id;

        public PdvPagamentoPorId(int id)
        {
            _id = id;
        }

        public PdvFormaPagamentoDTO Busca(ISession sessao)
        {
            return sessao.Get<PdvFormaPagamentoDTO>(_id);
        }
    }
}