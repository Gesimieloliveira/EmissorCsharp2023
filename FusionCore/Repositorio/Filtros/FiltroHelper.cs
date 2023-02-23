using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Type;

namespace FusionCore.Repositorio.Filtros
{
    public static class FiltroHelper
    {
        public static ICriterion Eq(Expression<Func<object>> property, object valor)
        {
            var projection = Projections.Property(property);
            var restricion = Restrictions.Eq(projection, valor);

            return restricion;
        }

        public static ICriterion DataGe(Expression<Func<object>> property, DateTime valor)
        {
            var projection = Projections.Cast(new DateType(), Projections.Property(property));
            var rescrition = Restrictions.Ge(projection, valor);

            return rescrition;
        }

        public static ICriterion DataLe(Expression<Func<object>> property, DateTime valor)
        {
            var projection = Projections.Cast(new DateType(), Projections.Property(property));
            var restricion = Restrictions.Le(projection, valor);

            return restricion;
        }

        public static ICriterion DataLt(Expression<Func<object>> property, DateTime valor)
        {
            var projection = Projections.Cast(new DateType(), Projections.Property(property));
            var restricion = Restrictions.Lt(projection, valor);

            return restricion;
        }

        public static ICriterion DataEq(Expression<Func<object>> property, DateTime valor)
        {
            var projection = Projections.Cast(new DateType(), Projections.Property(property));
            var restricion = Restrictions.Eq(projection, valor);

            return restricion;
        }
    }
}