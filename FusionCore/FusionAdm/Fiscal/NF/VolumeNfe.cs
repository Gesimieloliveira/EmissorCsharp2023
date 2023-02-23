using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Contratos;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class VolumeNfe : IVolume
    {
        public int Id { get; set; }
        public Nfeletronica Nfe { get; set; }
        public int Quantidade { get; set; }
        public decimal PesoBruto { get; set; }
        public decimal PesoLiquido { get; set; }
        public string Especie { get; set; }
        public string Numeracao { get; set; }
        public string Marca { get; set; }
        public IList<ILacre> Lacres { get; set; }

        private VolumeNfe()
        {
            Especie = string.Empty;
            Numeracao = string.Empty;
            Marca = string.Empty;
        }

        public VolumeNfe(Nfeletronica nfe) : this()
        {
            Nfe = nfe;
        }

        private bool Equals(VolumeNfe other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VolumeNfe) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(VolumeNfe left, VolumeNfe right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VolumeNfe left, VolumeNfe right)
        {
            return !Equals(left, right);
        }
    }
}