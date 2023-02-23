using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles.Events
{
    public struct ProdutoAlteradoEvent
    {
        public ProdutoAlteradoEvent(ProdutoDTO produto, ItemContexto contexto)
        {
            Produto = produto;
            Contexto = contexto;
        }

        public ItemContexto Contexto { get; }
        public ProdutoDTO Produto { get; }
    }
}