using System;

namespace FusionCore.Comum
{
    public abstract class Comparavel<T>
    {
        protected Guid UniqueGid { get; set; } = Guid.NewGuid();

        protected abstract T ChaveUnica { get; }

        protected bool Equals(Comparavel<T> other)
        {
            if (ChaveUnica == null || ChaveUnica.Equals(default(T)))
            {
                return UniqueGid.Equals(other.UniqueGid);
            }

            return ChaveUnica.Equals(other.ChaveUnica);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Comparavel<T>) obj);
        }

        public override int GetHashCode()
        {
            return ChaveUnica == null || ChaveUnica.Equals(default(T))
                ? UniqueGid.GetHashCode()
                : ChaveUnica.GetHashCode();
        }

        public static bool operator ==(Comparavel<T> left, Comparavel<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Comparavel<T> left, Comparavel<T> right)
        {
            return !Equals(left, right);
        }
    }
}