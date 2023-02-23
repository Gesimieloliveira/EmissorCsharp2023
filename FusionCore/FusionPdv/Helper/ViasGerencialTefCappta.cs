using System.Collections.Generic;
using System.IO;
using NHibernate.Util;

namespace FusionCore.FusionPdv.Helper
{
    public class ViasGerencialTefCappta
    {
        private readonly string _caminhoArquivo;
        private string[] _primeiraVia;
        private string[] _segundaVia;
        public int QuantidadeVias { get; set; }

        public ViasGerencialTefCappta(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo;
        }

        public void CarregaArquivo()
        {
            var arquivoCompleto = File.ReadAllLines(_caminhoArquivo);
            var vias = new List<List<string>> {new List<string>(), new List<string>()};

            var posicaoVia = 0;
            var cont = 1;

            arquivoCompleto.ForEach(p =>
            {
                var chave = "029-" + cont.ToString("D3") + " = ";

                if (!p.Contains(chave)) return;

                var valor = p.Replace(chave, "");
                var sub = p.Substring(10, p.Length - 10);

                var temVia2 = valor.Equals(string.Empty);

                if (temVia2)
                {
                    if (NaoArmazenarNaImpressao(ref cont)) return;
                    posicaoVia++;
                    cont++;
                    return;
                }

                vias[posicaoVia].Add(sub);
                cont++;
            });

            _primeiraVia = new string[vias[0].Count];
            _segundaVia = new string[vias[1].Count];

            var contadorPrimeiraVia = 0;
            vias[0].ForEach(v =>
            {
                _primeiraVia[contadorPrimeiraVia] = v.Trim('"').Trim();
                contadorPrimeiraVia++;
            });

            var contadorSegundaVia = 0;
            vias[1].ForEach(v =>
            {
                _segundaVia[contadorSegundaVia] = v.Trim('"').Trim();
                contadorSegundaVia++;
            });

            QuantidadeVias = 1;
            if (vias[0].Count > 0 && vias[1].Count > 0)
            {
                QuantidadeVias = 2;
            }
        }

        private static bool NaoArmazenarNaImpressao(ref int cont)
        {
            if (cont >= 7) return false;
            cont++;
            return true;
        }

        public string[] ImprimiComprovante(int via)
        {
            return via == 1 ? _primeiraVia : _segundaVia;
        }
    }
}