using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;
using NHibernate.Event;

namespace FusionCore.Core.Estoque
{
    public static class ControladorEstoqueItemNfe
    {
        public static void OnPreUpdate(ItemNfe itemNfe, ISession session)
        {
            var quantidadeAnterior = session
                .QueryOver<ItemNfe>()
                .Select(i => i.Quantidade)
                .Where(i => i.Id == itemNfe.Id)
                .SingleOrDefault<decimal>();

            if (quantidadeAnterior == itemNfe.Quantidade)
            {
                return;
            }

            var model = CriarAtualizacaoDoEstoque(itemNfe, quantidadeAnterior);
            var servico = EstoqueServicoAdmFactory.Cria(session);

            if (model.Inverso)
            {
                servico.Acrescentar(model);
                return;
            }

            servico.Descontar(model);
        }

        private static EstoqueModel CriarAtualizacaoDoEstoque(ItemNfe item, decimal quantidadeAnterior)
        {
            var diferenca = decimal.Round(item.Quantidade - quantidadeAnterior, 4);
            var diferencaNegativa = diferenca < 0;
            var acrescentarNoEstoque = diferencaNegativa;

            if (item.Nfe.TipoOperacao == TipoOperacao.Entrada)
            {
                acrescentarNoEstoque = !acrescentarNoEstoque;
            }

            var model = new EstoqueModel(
                item.Produto,
                diferencaNegativa ? -diferenca : diferenca,
                SessaoEstoque.UsuarioEvento,
                OrigemEventoEstoque.ItemAlteradoNfe,
                acrescentarNoEstoque
            );

            return model;
        }
    }
}