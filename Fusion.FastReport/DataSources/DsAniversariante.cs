using System;
using System.Text.RegularExpressions;

namespace Fusion.FastReport.DataSources
{
    public struct DsAniversariante
    {
        private int _cacheIdade;
        private string _cacheTelefone;

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int AnoNascimento => DataNascimento.Year;

        public int Idade
        {
            get
            {
                if (_cacheIdade > 0)
                {
                    return _cacheIdade;
                }

                var now = DateTime.Now;
                var idadeAno = now.Year - DataNascimento.Year;

                if (now.Month < DataNascimento.Month)
                {
                    return _cacheIdade = idadeAno - 1;
                }

                if (now.Month == DataNascimento.Month && now.Day < DataNascimento.Day)
                {
                    return _cacheIdade = idadeAno - 1;
                }

                return _cacheIdade = idadeAno;
            }
        }

        public string PrimeiroTelefone { get; set; }

        public string TelefoneFormatdao
        {
            get
            {
                if (_cacheTelefone != null)
                {
                    return _cacheTelefone;
                }

                switch (PrimeiroTelefone?.Length)
                {
                    case 10:
                        _cacheTelefone = Regex.Replace(PrimeiroTelefone, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
                        break;
                    case 11:
                        _cacheTelefone = Regex.Replace(PrimeiroTelefone, @"(\d{2})(\d{1})(\d{4})(\d{4})", "($1) $2 $3-$4");
                        break;
                }

                return _cacheTelefone;
            }
        }
    }
}