using System;
using FusionCore.Comum;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.Repositorio.Base
{
    public abstract class Entidade : Comparavel<int>, IEntidade, ICloneable
    {
        protected abstract int ReferenciaUnica { get; }
        protected override int ChaveUnica => ReferenciaUnica;

        public virtual object Clone()
        {
            var clone = (Entidade) MemberwiseClone();

            clone.UniqueGid = UniqueGid;

            return clone;
        }
    }
}