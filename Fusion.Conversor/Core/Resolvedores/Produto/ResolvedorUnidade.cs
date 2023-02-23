using System.Text.RegularExpressions;
using Fusion.Conversor.Core.Cache;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorUnidade : ResolvedorComum<ProdutoUnidadeDTO>
    {
        private readonly Regex _regex = new Regex(@"[A-z]{1}[A-z0-9]{1,9}", RegexOptions.IgnoreCase);

        public ResolvedorUnidade(IStatelessSession session, ArrayCache<ProdutoUnidadeDTO> cache) 
            : base(session, cache)
        {
        }

        protected override ProdutoUnidadeDTO DoResolve(string input, ProdutoUnidadeDTO @default)
        {
            if (_regex.IsMatch(input) == false)
            {
                return @default;
            }

            if (Cache.TryGetCache(i => i.Sigla == input, out var unidade))
            {
                return unidade;
            }

            unidade = Session.QueryOver<ProdutoUnidadeDTO>()
                .Where(i => i.Sigla == input)
                .Take(1)
                .SingleOrDefault();

            if (unidade != null)
            {
                Cache.Add(unidade);
                return unidade;
            }

            unidade = new ProdutoUnidadeDTO
            {
                Nome = input,
                Sigla = input.SubstringWithTrim(0, 10)
            };

            Session.Insert(unidade);
            Cache.Add(unidade);

            return unidade;
        }
    }
}