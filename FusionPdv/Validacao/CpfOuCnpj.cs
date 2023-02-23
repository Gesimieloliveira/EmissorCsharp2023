using System.Text.RegularExpressions;

namespace FusionPdv.Validacao
{
    public class CpfOuCnpj
    {
        private readonly string _cpfOuCnpj;

        public CpfOuCnpj(string cpfOuCnpj)
        {
             cpfOuCnpj = new Regex(@"[^\d]").Replace(cpfOuCnpj, "");
            _cpfOuCnpj = cpfOuCnpj;
        }


        public void Executar()
        {
            if (_cpfOuCnpj.Length == 11)
            {
                new ValidacaoCpf(_cpfOuCnpj).Validar();
            }
            else
            {
                new ValidacaoCnpj(_cpfOuCnpj).Validar();
            }
        }
    }
}
