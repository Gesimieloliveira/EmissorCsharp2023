using System;
using NHibernate.Criterion;
using NHibernate.Type;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroPeriodo
    {
        public FiltroPeriodo(DateTime inicio, DateTime? fim = null)
        {
            Inicio = inicio;
            Fim = fim ?? DateTime.Now;
        }

        public static FiltroPeriodo Todos => new FiltroPeriodo(new DateTime(1900, 1, 1), DateTime.MaxValue);

        public DateTime Inicio { get; }
        public DateTime Fim { get; }

        public override string ToString()
        {
            return $"{Inicio:dd/MM/yyyy} à {Fim:dd/MM/yyyy}";
        }

        public AbstractCriterion Restriction(PropertyProjection property)
        {
            return Restrictions.Between(
                Projections.Cast(new DateType(), property), 
                Inicio.Date, 
                Fim.Date
            );
        }
    }
}