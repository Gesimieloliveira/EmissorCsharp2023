using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Tributacoes.Regras
{
    public class RepositorioRegraTributacao : Repositorio<RegraTributacaoSaida, short>
    {
        public RepositorioRegraTributacao(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<RegraTributacaoSaidaSlim> ListaRegrasAtivas()
        {
            var queryOver = RegraTributacaoSaidaSlim.CriaQueryOver(Sessao)
                .Where(i => i.Ativo == true)
                .OrderBy(i => i.Descricao).Asc;

            return queryOver.List<RegraTributacaoSaidaSlim>();
        }

        public IEnumerable<RegraTributacaoSaidaSlim> ListaRegras()
        {
            var query = RegraTributacaoSaidaSlim.CriaQueryOver(Sessao)
                .OrderBy(i => i.Descricao).Asc;

            return query.List<RegraTributacaoSaidaSlim>();
        }

        public void Persiste(RegraTributacaoSaida regra)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (regra.Id == 0)
            {
                Sessao.Persist(regra);
                Sessao.Flush();
                return;
            }

            Sessao.Update(regra);
            Sessao.Flush();
        }
    }
}