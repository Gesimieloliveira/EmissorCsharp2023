using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCidade : Repositorio<CidadeDTO, int>
    {
        private readonly CidadeDTO _tbCidade = null;

        public RepositorioCidade(ISession sessao) : base(sessao)
        {
        }

        public IList<CidadeDTO> BuscaPelaSiglaEstado(string sigla)
        {
            var query = Sessao.QueryOver<CidadeDTO>().Where(c => c.SiglaUf == sigla);
            return query.List();
        }

        public CidadeDTO BuscaPeloIbge(int ibge)
        {
            var query = Sessao.QueryOver<CidadeDTO>().Where(c => c.CodigoIbge == ibge);
            var cidade = query.SingleOrDefault<CidadeDTO>();

            return cidade;
        }

        public void Salvar(CidadeDTO cidade)
        {
            Sessao.SaveOrUpdate(cidade);
        }

        public IList<CidadeDTO> Busca(string input)
        {
            var sessao = Sessao.QueryOver(() => _tbCidade);

            if (string.IsNullOrWhiteSpace(input))
            {
                return sessao.List();
            }

            var nome = Restrictions.Like(Projections.Property(() => _tbCidade.Nome), input, MatchMode.Anywhere);
            sessao.Where(nome);

            return sessao.List<CidadeDTO>();
        }
    }
}