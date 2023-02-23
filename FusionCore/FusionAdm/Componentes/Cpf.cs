using System;
using FusionCore.Helpers.DocumentoUnico;
using FusionLibrary.Validacao.Regras;

namespace FusionCore.FusionAdm.Componentes
{
    public class Cpf : IComponenteValorUnico<string>
    {
        private readonly DocumentoUnicoHelper _helper = new DocumentoUnicoHelper();
        private readonly string _valor;

        private Cpf()
        {
            _valor = string.Empty;
        }

        public Cpf(string cpf)
        {
            _valor = _helper.PreparaString(cpf);
        }

        public static Cpf Vazio => new Cpf();
        public string Valor => _valor;

        public void ThrowExcetionSeInvalido()
        {
            var regra = new CpfRegra();

            if (!string.IsNullOrEmpty(_valor) && !regra.AplicaRegra(_valor))
            {
                throw new InvalidOperationException($"O CPF {_valor} não é um número válido!");
            }
        }

        public override string ToString()
        {
            return Valor;
        }

        private bool Equals(Cpf other)
        {
            return string.Equals(Valor, other.Valor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Cpf) obj);
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Cpf left, Cpf right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Cpf left, Cpf right)
        {
            return !Equals(left, right);
        }
    }
}