using FusionCore.Excecoes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Pessoa;

namespace Fusion.Visao.Validacoes.Pessoa
{
    public static class ValidacaoDocumentoUnico
    {
        public static void Executar(PessoaEntidade pessoaEntidade)
        {
            if (pessoaEntidade.NaoPossuiDocumentoUnico())
                CriaExcecao.OperacaoInvalida("Deve ter CPF ou CNPJ");

            switch (pessoaEntidade.Tipo)
            {
                case PessoaTipo.Fisica when pessoaEntidade.GetDocumentoUnico().Length != 11:
                    CriaExcecao.OperacaoInvalida("Cpf deve ter 11 dígitos");
                    break;
                case PessoaTipo.Juridica when pessoaEntidade.GetDocumentoUnico().Length != 14:
                    CriaExcecao.OperacaoInvalida("Cpf deve ter 14 dígitos");
                    break;
            }
        }
    }
}