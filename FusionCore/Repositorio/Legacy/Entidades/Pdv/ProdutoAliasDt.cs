using System;
using System.Linq;
using System.Text.RegularExpressions;
using FusionCore.Repositorio.Base;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class ProdutoAliasDt : Entidade
    {
        private readonly string _mensagemErroAliasCodigoBarraInvalido;

        public ProdutoAliasDt()
        {
        }

        public ProdutoAliasDt(string alias, bool isCodigoBarra = true, string mensagemErroAliasCodigoBarraInvalido = "Código de barras somente pode ser de 8 ou 13 digitos")
        {
            _mensagemErroAliasCodigoBarraInvalido = mensagemErroAliasCodigoBarraInvalido;
            Alias = alias;
            IsCodigoBarras = isCodigoBarra;

            if (isCodigoBarra)
                Valida();
        }

        public int Id { get; set; }
        public ProdutoDt Produto { get; set; }
        public bool IsCodigoBarras { get; set; }
        public string Alias { get; set; }

        public void Valida()
        {
            if (!IsCodigoBarras) return;
            switch (Alias.Length)
            {
                case 8:
                    ValidaCodigoBarrasEan8();
                    break;
                case 13:
                    ValidaCodigoBarrasEan13();
                    break;
                default:
                    throw new ArgumentException(_mensagemErroAliasCodigoBarraInvalido);
            }
        }

        private void ValidaCodigoBarrasEan8()
        {
            var temLetras = new Regex(@"[^\d]").IsMatch(Alias);

            if (temLetras) throw new ArgumentException("Código de barras deve ter apenas números");

            var codigoBarra = Alias.Select(c => int.Parse(c.ToString())).ToArray();
            var somaPares = codigoBarra[1] + codigoBarra[3] + codigoBarra[5];
            var somaImpares = codigoBarra[0] + codigoBarra[2] + codigoBarra[4] + codigoBarra[6];
            var soma = (somaPares + somaImpares * 3);

            var resultado = (10 - (soma % 10) == 10 ? 0 : 10 - (soma % 10)) == codigoBarra[7];

            if (!resultado) throw new ArgumentException("Código de barras inválido!");
        }

        private void ValidaCodigoBarrasEan13()
        {
            var somenteNumeros = new Regex(@"[^\d]").IsMatch(Alias);

            if (somenteNumeros) throw new ArgumentException("Código de barras deve ter apenas números");

            var codigoBarra = Alias.Select(c => int.Parse(c.ToString())).ToArray();
            var somaPares = codigoBarra[1] + codigoBarra[3] + codigoBarra[5] + codigoBarra[7] + codigoBarra[9] + codigoBarra[11];
            var somaImpares = codigoBarra[0] + codigoBarra[2] + codigoBarra[4] + codigoBarra[6] + codigoBarra[8] + codigoBarra[10];
            var soma = (somaImpares + somaPares * 3);

            var resultado = (10 - (soma % 10) == 10 ? 0 : 10 - (soma % 10)) == codigoBarra[12];

            if (!resultado) throw new ArgumentException("Código de barras inválido!");
        }


        protected override int ReferenciaUnica => Id;
    }
}