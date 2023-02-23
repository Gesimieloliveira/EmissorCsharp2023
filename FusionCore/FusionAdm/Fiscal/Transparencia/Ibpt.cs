 // ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.FusionAdm.Fiscal.Transparencia
{
    public class Ibpt : ISincronizavelAdm
    {
        public string Codigo { get; private set; }
        public TipoIbpt Tipo { get; private set; }
        public string ExcecaoFiscal { get; private set; }
        public string Descricao { get; set; }
        public decimal Nacional { get; set; }
        public decimal Importado { get; set; }
        public decimal Estadual { get; set; }
        public string ChaveIbpt { get; set; }

        private Ibpt()
        {
            //nhibernate
        }

        public Ibpt(string codigo, TipoIbpt tipo, string excecaoFiscal)
        {
            Codigo = codigo;
            Tipo = tipo;
            ExcecaoFiscal = excecaoFiscal;
        }

        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }

        public decimal ImpostoFederalAproximado(IBaseCalculoIbpt valor)
        {
            return decimal.Round(valor.ValorIbpt*Nacional/100, 2);
        }

        public decimal ImpostoEstadualAproximado(IBaseCalculoIbpt valor)
        {
            return decimal.Round(valor.ValorIbpt*Estadual/100, 2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ibpt tributo)) return false;

            return tributo.Codigo == Codigo && tributo.Tipo == Tipo && tributo.ExcecaoFiscal == ExcecaoFiscal;
        }

        public override int GetHashCode()
        {
            return string.Concat(Codigo, "|", Tipo, "|", ExcecaoFiscal).GetHashCode();
        }

        public string Referencia => Codigo;
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.Ibpt;
    }
}