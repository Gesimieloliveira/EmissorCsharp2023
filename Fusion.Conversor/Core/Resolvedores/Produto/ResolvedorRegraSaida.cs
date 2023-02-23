using System;
using Fusion.Conversor.Core.Cache;
using FusionCore.Tributacoes.Regras;
using NHibernate;

namespace Fusion.Conversor.Core.Resolvedores.Produto
{
    public class ResolvedorRegraSaida : ResolvedorComum<RegraTributacaoSaida>
    {
        public ResolvedorRegraSaida(IStatelessSession session, ArrayCache<RegraTributacaoSaida> cache) 
            : base(session, cache)
        {
        }

        protected override RegraTributacaoSaida DoResolve(string input, RegraTributacaoSaida @default)
        {
            try
            {
                var cstRegra = int.Parse(input);
                input = cstRegra.ToString("D2");
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Código CST ({input}) não compatível com opções disponíveis, ex: 00, 10, 60..");
            }

            if (input.Length != 2)
            {
                throw new InvalidOperationException($"Código CST ({input}) não compatível com opções disponíveis, ex: 00, 10, 60..");
            }

            if (Cache.TryGetCache(i => i.Cst.Codigo == input, out var cst))
            {
                return cst;
            }

            cst = Session.QueryOver<RegraTributacaoSaida>()
                .Where(i => i.Cst.Codigo == input)
                .SingleOrDefault();

            if (cst == null)
            {
                return @default;
            }

            Cache.Add(cst);

            return cst;
        }
    }
}