using System;
using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class RepositorioResultadoCaixa : RepositorioBase
    {
        public RepositorioResultadoCaixa(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<CaixaIndividualDTO> PorPeriodo(DateTime inicio, DateTime fim)
        {
            var query = new QueryBuilderCaixaIndividualDTO(Sessao)
                .ComPeriodo(inicio, fim)
                .OrdernarPorAberturaAsc()
                .Construir();

            return query.List<CaixaIndividualDTO>();
        }

        public IEnumerable<CaixaIndividualDTO> TodosAbertos()
        {
            var query = new QueryBuilderCaixaIndividualDTO(Sessao)
                .ApenasAbertos()
                .OrdernarPorAberturaAsc()
                .Construir();

            return query.List<CaixaIndividualDTO>();
        }
    }
}