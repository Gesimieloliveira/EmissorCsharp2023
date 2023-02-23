using System;
using NHibernate.Criterion;
using static NHibernate.Criterion.Projections;
using static NHibernate.Criterion.Restrictions;

namespace FusionCore.Repositorio.NfeEletronica
{
    public class NfeGridFiltroBuilder
    {
        public DateTime? EmitidasApartir { get; set; }
        public int? NumeroIgual { get; set; }
        public int? IdentityIgual { get; set; }
        public string NomeEmitenteContenha { get; set; }
        public string NomeDestinatarioContenha { get; set; }

        public Conjunction Build()
        {
            var and = Conjunction();

            if (EmitidasApartir != null)
            {
                and.Add(Ge(Property(() => QueryMap.TbNfe.EmitidaEm), EmitidasApartir));
            }

            if (NumeroIgual != null)
            {
                and.Add(Eq(Property(() => QueryMap.TbNfe.NumeroEmissao), NumeroIgual));
            }

            if (IdentityIgual != null)
            {
                and.Add(Eq(Property(() => QueryMap.TbNfe.Id), IdentityIgual));
            }

            if (!string.IsNullOrEmpty(NomeEmitenteContenha))
            {
                and.Add(Like(Property(() => QueryMap.TbEmpresa.RazaoSocial), NomeEmitenteContenha, MatchMode.Anywhere));
            }

            if (!string.IsNullOrEmpty(NomeDestinatarioContenha))
            {
                and.Add(Like(Property(() => QueryMap.TbDestinatario.Nome), NomeDestinatarioContenha, MatchMode.Anywhere));
            }

            return and;
        }
    }
}