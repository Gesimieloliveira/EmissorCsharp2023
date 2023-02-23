using System;
using System.Text;
using OpenAC.Net.Core.Extensions;

namespace FusionCore.Exportacao.ItensBuscaRapida
{
    public class LayoutBuscaRapidaGertec : ILayoutBuscaRapida
    {
        public string Tag => "Gertec";
        public CasasDecimais CasasDecimais { get; set; }

        public string ConverteLinha(Linha item)
        {
            var sb = new StringBuilder();

            sb.Append($"{item.CodigoBarras}|{item.Nome}|{ConverteCasasDecimais(item.Preco)}");

            return sb.ToString();
        }

        private decimal ConverteCasasDecimais(decimal itemPreco)
        {
            switch (CasasDecimais)
            {
                case CasasDecimais.Duas:
                    return itemPreco.RoundABNT(2);
                case CasasDecimais.Tres:
                    return itemPreco.RoundABNT(3);
                case CasasDecimais.Quatro:
                    return itemPreco;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}