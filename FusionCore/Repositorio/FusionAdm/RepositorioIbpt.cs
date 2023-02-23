using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioIbpt : IRepositorioIbpt
    {
        private readonly ISession _sessao;

        public RepositorioIbpt(ISession sessao)
        {
            _sessao = sessao;
        }

        public IList<Ibpt> BuscaTodos()
        {
            var query = _sessao.QueryOver<Ibpt>();
            return query.List<Ibpt>();
        }

        public void DeletaTodos()
        {
            _sessao.Delete("from Ibpt t");
            _sessao.Flush();
        }

        public void Persiste(Ibpt ibpt)
        {
            _sessao.Persist(ibpt);
            _sessao.Flush();
        }

        public Ibpt GetPeloNcm(string codigo)
        {
            var query = _sessao.QueryOver<Ibpt>()
                .Where(i => i.Codigo == codigo && i.Tipo == TipoIbpt.Ncm);

            var resultados = query.List();

            if (resultados == null|| resultados.Count == 0)
                return null;

            if (resultados.Count == 1)
                return resultados[0];

            var rand = new Random().Next(0, resultados.Count - 1);
            return resultados[rand];
        }

        public Ibpt GetPeloNbs(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return null;
            }

            var query = _sessao.QueryOver<Ibpt>()
                .Where(i => i.Codigo == codigo && i.Tipo == TipoIbpt.Nbs);

            return query.SingleOrDefault<Ibpt>();
        }

        public string GetChaveIbpt()
        {
            var query = _sessao.QueryOver<Ibpt>()
                .Select(i => i.ChaveIbpt)
                .Take(1);

            var chave = query.SingleOrDefault<string>();
            return chave;
        }

        public IList<Ibpt> GetTodosPeloNcm(string ncm)
        {
            var query = _sessao.QueryOver<Ibpt>()
                .Where(i => i.Codigo == ncm);

            var resultados = query.List();

            return resultados;
        }

        public IList<Ibpt> BuscarTodosPeloNbs()
        {
            var query = _sessao.QueryOver<Ibpt>()
                .Where(i => i.Tipo == TipoIbpt.Nbs);

            var resultados = query.List<Ibpt>();

            return resultados;
        }
    }
}