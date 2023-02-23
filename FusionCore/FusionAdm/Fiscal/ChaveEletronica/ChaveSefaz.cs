using System;
using FusionCore.Excecoes.RegraNegocio;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.ChaveEletronica
{
    public class ChaveSefaz
    {
        public static ChaveSefaz Empty => new ChaveSefaz();
        public string Chave { get; private set; }
        public int Dv => GetDv();

        private ChaveSefaz()
        {
            //nhibernate
            Chave = string.Empty;
        }

        public ChaveSefaz(string chaveComDigito) : this()
        {
            if (chaveComDigito?.Length != 44)
            {
                throw new RegraNegocioException("Chave deve conter 44 caracteres");
            }

            Chave = chaveComDigito;
        }

        private int GetDv()
        {
            if (string.IsNullOrWhiteSpace(Chave))
            {
                throw new InvalidOperationException("Chave vazia não possui DV");
            }

            return Convert.ToInt32(Chave.Substring(43, 1));
        }

        public short GetSerie()
        {
            if (string.IsNullOrWhiteSpace(Chave))
            {
                throw new InvalidOperationException("Chave vazia não possui DV");
            }

            return short.Parse(Chave.Substring(22, 3));
        }

        public override string ToString()
        {
            return Chave;
        }

        public int GetCodigoNumerico()
        {
            if (string.IsNullOrWhiteSpace(Chave))
            {
                throw new InvalidOperationException("Chave vazia não possui código numérico");
            }

            return Convert.ToInt32(Chave.Substring(35, 8));
        }

        public bool IsValida()
        {
            return string.IsNullOrWhiteSpace(Chave) || Valida(Chave);
        }

        public static bool Valida(string input)
        {
            if (input?.Length != 44)
            {
                return false;
            }

            var digito = Convert.ToInt32(input.Substring(43, 1));
            var chaveSemDigito = input.Substring(0, 43);
            var digitoCorreto = ChaveSefazHelper.GeraDigitoVerificador(chaveSemDigito);

            return digito == digitoCorreto;
        }

        public string GetNumeroFiscal()
        {
            return Chave.Substring(25, 9);
        }
    }
}