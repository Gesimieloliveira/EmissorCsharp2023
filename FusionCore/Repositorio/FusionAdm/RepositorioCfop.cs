using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.FusionAdm
{
    [SuppressMessage("ReSharper", "RedundantBoolCompare")]
    public class RepositorioCfop : Repositorio<CfopDTO, string>
    {
        public RepositorioCfop(ISession sessao) : base(sessao)
        {
        }

        public IList<CfopDTO> BuscarCfopParaNfce()
        {
            var query = Sessao.QueryOver<CfopDTO>()
                .Where(c => c.ElegivelNfce == true);

            return query.List();
        }

        public IList<CfopDTO> BuscaRapida(string input, TipoOperacao? operacao = null, OrigemOperacao? origem = null)
        {
            var query = Sessao.QueryOver<CfopDTO>();

            if (!string.IsNullOrEmpty(input))
            {
                query.And(c =>
                    c.Descricao.IsLike(input, MatchMode.Anywhere) ||
                    c.Id.IsLike(input, MatchMode.Anywhere)
                );
            }

            if (operacao != null)
            {
                query.And(i => i.TipoOperacao == operacao);
            }

            if (origem != null)
            {
                return query.List().Where(i => i.OrigemOperacao == origem).ToList();
            }

            return query.List();
        }

        public void Alterar(CfopDTO cfop)
        {
            Sessao.Update(cfop);
            Sessao.Flush();
        }

        public IEnumerable<CfopDTO> BuscarApenasOsDeEntrada()
        {
            var query = Sessao.QueryOver<CfopDTO>()
                .Where(i => i.TipoOperacao == TipoOperacao.Entrada);

            return query.List();
        }
    }
}