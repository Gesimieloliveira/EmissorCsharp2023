using FusionCore.FusionAdm.Fiscal.NF;
using static System.Decimal;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.Tributacoes
{
    public class IcmsInterstadual
    {
        private int ItemId { get; set; }
        public ItemNfe Item { get; private set; }
        public decimal AliquotaInternaDestino { get; set; }
        public decimal ValorBcIcmsDestino { get; private set; }
        public decimal AliquotaInterstadual { get; set; }
        public decimal ValorIcmsDestino { get; private set; }
        public decimal AliquotaCombatePobreza { get; set; }
        public decimal ValorCombatePobreza { get; private set; }
        public decimal ValorIcmsOrigem { get; private set; }
        public decimal PercentualParaDestino { get; set; }

        private IcmsInterstadual()
        {
            //nhibernate
        }

        public IcmsInterstadual(ItemNfe item) : this()
        {
            Item = item;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((IcmsInterstadual) obj);
        }

        private bool Equals(IcmsInterstadual other)
        {
            return ItemId == other.ItemId;
        }

        public override int GetHashCode()
        {
            return ItemId;
        }

        public static bool operator ==(IcmsInterstadual left, IcmsInterstadual right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IcmsInterstadual left, IcmsInterstadual right)
        {
            return !Equals(left, right);
        }

        public bool IsNovo()
        {
            return ItemId <= 0;
        }

        public void Calcular()
        {
            decimal valorIcmDestino = 0M, valorIcmsOrigem = 0M, valorCp = 0M;

            var baseCalculoIcms = Item.ImpostoIcms.ValorBcIcms;

            if (AliquotaCombatePobreza > 0M)
            {
                valorCp = Round(baseCalculoIcms * (AliquotaCombatePobreza / 100), 2);
            }

            var diferencaAliquotas = AliquotaInternaDestino - AliquotaInterstadual;

            if (diferencaAliquotas > 0M)
            {
                var valorBaseDifal = baseCalculoIcms * (diferencaAliquotas/100);
                var percentualPartilhaOrigem = 100 - PercentualParaDestino;

                valorIcmDestino = Round(valorBaseDifal * (PercentualParaDestino / 100), 2);
                valorIcmsOrigem = Round(valorBaseDifal * (percentualPartilhaOrigem / 100), 2);
            }

            ValorBcIcmsDestino = baseCalculoIcms;
            ValorIcmsDestino = valorIcmDestino;
            ValorIcmsOrigem = valorIcmsOrigem;
            ValorCombatePobreza = valorCp;
        }
    }
}