using System.Collections.Generic;
using FusionCore.FusionAdm.Produtos;
using FusionCore.Repositorio.Contratos;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioProdutoLocalizacao : Repositorio<ProdutoLocalizacao, short>, IRepositorioProdutoLocalizacao
    {
        public RepositorioProdutoLocalizacao(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ProdutoLocalizacao produtoLocalizacao)
        {
            Sessao.SaveOrUpdate(produtoLocalizacao);
        }

        public void Deletar(ProdutoLocalizacao produtoLocalizacao)
        {
            Sessao.Delete(produtoLocalizacao);
        }

        public IList<ProdutoLocalizacao> BuscaRapida(string texto)
        {
            var queryOver =
                Sessao.QueryOver<ProdutoLocalizacao>()
                    .Where(Restrictions.On<ProdutoLocalizacao>(p => p.Nome).IsLike("%" + texto + "%")
                    || Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property<ProdutoLocalizacao>(p => p.Id)), texto));

            var lista = queryOver.List<ProdutoLocalizacao>();

            return lista;
        }

        public bool JaExisteEsseNomeCadastrado(string nome, short id)
        {
            var queryOver = Sessao.QueryOver<ProdutoLocalizacao>()
                .And(p => p.Id != id)
                .And(p => p.Nome == nome.Trim());

            return queryOver.RowCount() > 0;
        }
    }
}