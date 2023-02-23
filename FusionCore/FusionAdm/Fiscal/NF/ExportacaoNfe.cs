using System;
using static System.String;

#pragma warning disable 649
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class ExportacaoNfe
    {
        private string _localEmbarque;
        private string _localDespacho;
        private string _ufSaidaPais;
        private int _nfeId;
        public Nfeletronica Nfe { get; private set; }

        public string UfSaidaPais
        {
            get { return _ufSaidaPais; }
            set
            {
                if (IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException(@"Você não pode fazer uma exportação sem UF de saida.");

                if (value == "EX")
                    throw new InvalidOperationException(@"Você não pode usar EX na UF de saida da exportação");

                _ufSaidaPais = value;
            }
        }

        public string LocalEmbarque
        {
            get { return _localEmbarque; }
            set
            {
                if (IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException(@"Você deve informar um local de embarque na exportação");

                _localEmbarque = value;
            }
        }

        public string LocalDespacho
        {
            get { return _localDespacho; }
            set
            {
                if (IsNullOrWhiteSpace(value))
                    throw new ArgumentException(@"Você deve informar um local de despacho na exportação");

                _localDespacho = value;
            }
        }

        private ExportacaoNfe()
        {
        }

        public ExportacaoNfe(Nfeletronica nfe, string ufSaidaPais, string localEmbarque, string localDespacho) : this()
        {
            Nfe = nfe;
            UfSaidaPais = ufSaidaPais;
            LocalEmbarque = localEmbarque;
            LocalDespacho = localDespacho;
        }

        private bool Equals(ExportacaoNfe other)
        {
            return _nfeId == other._nfeId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ExportacaoNfe) obj);
        }

        public override int GetHashCode()
        {
            return _nfeId;
        }

        public static bool operator ==(ExportacaoNfe left, ExportacaoNfe right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExportacaoNfe left, ExportacaoNfe right)
        {
            return !Equals(left, right);
        }
    }
}