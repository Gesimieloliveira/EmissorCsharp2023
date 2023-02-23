using NHibernate;

namespace FusionCore.FusionPdv.Servico.Estoque
{
    public class EstoqueServicoPdvFactory
    {
        public static EstoqueServicoPdv CriarEstoqueServico(ISession sessao)
        {
            return new EstoqueServicoPdv(sessao) { TipoControle = TipoControle.NaoControlar };
        }
    }
}