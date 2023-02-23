using System;
using FusionCore.Helpers.DocumentoUnico;
using FusionLibrary.Validacao.Regras;

// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionAdm.Componentes
{
    public class Cnpj : IComponenteValorUnico<string>
    {
        private readonly DocumentoUnicoHelper _helper = new DocumentoUnicoHelper();
        private readonly string _valor;

        private Cnpj()
        {
            _valor = string.Empty;
        }

        public Cnpj(string cnpj)
        {
            _valor = _helper.PreparaString(cnpj);
        }

        public static Cnpj Vazio => new Cnpj();
        public string Valor => _valor;

        public void ThrowExcetpionSeInvalido()
        {
            var regra = new CnpjRegra();

            if (!string.IsNullOrEmpty(_valor) && !regra.AplicaRegra(_valor))
            {
                throw new InvalidOperationException($"O CNPJ {_valor} não é válido!");
            }
        }

        public override string ToString()
        {
            return Valor;
        }

        private bool Equals(Cnpj other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Cnpj) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Cnpj left, Cnpj right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Cnpj left, Cnpj right)
        {
            return !Equals(left, right);
        }
    }
}