using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FusionCore.Exportacao.ItensBalanca
{
    public class LayouotBalancaLinhaMgv6 : ILayouotBalanca
    {
        private const char Space = ' ';
        private static readonly Dictionary<short, string> Spaces = new Dictionary<short, string>();
        private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo {NumberDecimalSeparator = "."};

        public string Tag { get; } = "MGV6";

        public string ConverteLinha(ModeloItem item)
        {
            var sb = new StringBuilder();

            sb.Append("01");
            sb.Append("0");
            sb.Append(item.Codigo.ToString("000000"));
            sb.Append(PrecoInline(item));
            sb.Append("000");
            sb.Append(DescricaoInline(item.Descricao));
            sb.Append("000000");
            sb.Append("0000");
            sb.Append("000000"); // código da informaçào nutricional
            sb.Append("1");
            sb.Append("0");
            sb.Append("0000"); // codigo do fornecedor
            sb.Append("000000000000"); // lote
            sb.Append("00000000000"); // codigo ean13 especial
            sb.Append("1"); // versão do preço
            sb.Append("0000");
            sb.Append("0000");
            sb.Append("0000");
            sb.Append("0000");
            sb.Append("0000");
            sb.Append("0000"); // codigo conversao
            sb.Append("000000000000"); // ean-13 fornecedor
            sb.Append("000000");
            sb.Append("00");
            sb.Append(CriaEspacos(35));
            sb.Append(CriaEspacos(35));
            sb.Append("000000");
            sb.Append("000000");
            sb.Append("000000");
            sb.Append("000000");
            sb.Append("0"); //SF
            sb.Append("000000");
            sb.Append("0");
            sb.Append("00");

            return sb.ToString();
        }

        private string CriaEspacos(short tamanho)
        {
            if (Spaces.ContainsKey(tamanho))
            {
                return Spaces[tamanho];
            }

            var value = "".PadLeft(tamanho, Space);
            Spaces.Add(tamanho, value);

            return value;
        }

        private string DescricaoInline(string descricao)
        {
            return descricao.Length <= 50 ? descricao.PadRight(50, Space) : descricao.Substring(0, 50);
        }

        private string PrecoInline(ModeloItem item)
        {
            var precoStr = item.Preco.ToString("0.00", NumberFormat).Replace(".", "");
            var precoLine = precoStr.PadLeft(6, '0');

            if (precoLine.Length > 6)
            {
                throw new InvalidOperationException($"Preço {precoLine} superior ao suportado pelo layout no item {item.Descricao}");
            }

            return precoLine;
        }
    }
}