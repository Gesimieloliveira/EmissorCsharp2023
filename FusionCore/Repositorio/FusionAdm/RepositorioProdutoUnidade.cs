using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioProdutoUnidade : Repositorio<ProdutoUnidadeDTO, int>
    {
        public RepositorioProdutoUnidade(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<ProdutoUnidadeDTO> GetUnidadesPelaSigla(string sigla)
        {
            return Sessao.QueryOver<ProdutoUnidadeDTO>().Where(u => u.Sigla == sigla).List();
        }

        public ProdutoUnidadeDTO PrimeiraUnidadePelaSigla(string sigla)
        {
            var query = Sessao.QueryOver<ProdutoUnidadeDTO>()
                .Where(u => u.Sigla == sigla)
                .Take(1);

            return query.SingleOrDefault<ProdutoUnidadeDTO>();
        }

        public void Salva(ProdutoUnidadeDTO unidade)
        {
            if (unidade.Id == 0)
            {
                Sessao.Persist(unidade);
            }
            else
            {
                Sessao.Update(unidade);
            }

            Sessao.Flush();
        }

        public void Deleta(ProdutoUnidadeDTO unidade)
        {
            Sessao.Delete(unidade);
            Sessao.Flush();
        }

        public bool JaExisteSigla(string sigla)
        {
            var query = Sessao.QueryOver<ProdutoUnidadeDTO>()
                .Where(v => v.Sigla == sigla);
            return query.RowCount() > 0;
        }

        public bool JaExisteNome(string nome, int id)
        {
            var query = Sessao.QueryOver<ProdutoUnidadeDTO>()
                .Where(v => v.Nome == nome && v.Id != id);
            return query.RowCount() > 0;
        }
    }
}
