using NHibernate;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public static class EstoqueServicoAdmFactory
    {
        public static EstoqueServicoAdm Cria(ISession sessao)
        {
            return new EstoqueServicoAdm(sessao);
        }
    }
}