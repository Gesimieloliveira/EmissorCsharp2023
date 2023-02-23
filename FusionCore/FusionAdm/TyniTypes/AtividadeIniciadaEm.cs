using System;
using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionAdm.TyniTypes
{
    public class AtividadeIniciadaEm : IComponenteValorUnico<DateTime>
    {
        private DateTime _valor;

        public AtividadeIniciadaEm(DateTime ativdadeIniciadaEm)
        {
            _valor = ativdadeIniciadaEm;
        }

        public DateTime Valor => _valor;

        protected bool Equals(AtividadeIniciadaEm other)
        {
            return _valor.Equals(other._valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AtividadeIniciadaEm) obj);
        }

        public override int GetHashCode()
        {
            return _valor.GetHashCode();
        }
    }
}