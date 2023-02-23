using FusionPdv.Ecf;

namespace FusionPdv.ManipulaValor
{
    public class TruncaArredonda
    {
        private readonly decimal _valor;
        private readonly int _precisao;
        private static bool _arredonda;
        private static bool _obterArredondaEcf;

        public TruncaArredonda(decimal valor, int precisao)
        {
            ObterArredondaEcf();
            _valor = valor;
            _precisao = precisao;
        }

        public decimal ExecutaComQuantidadeDecimal()
        {
            return !_arredonda ? TruncarValor.Trunca(_valor, _precisao) : ArredondarValor.Arredonda(_valor, _precisao);
        }

        public TruncaArredonda(decimal valor)
        {
            ObterArredondaEcf();
            _valor = valor;
        }

        public decimal Executa()
        {
            return !_arredonda ? TruncarValor.Trunca(_valor) : ArredondarValor.Arredonda(_valor);
        }

        private static void ObterArredondaEcf()
        {
            if (_obterArredondaEcf) return;
            _arredonda = SessaoEcf.EcfFiscal.Arredonda();
            _obterArredondaEcf = true;
        }
    }
}
