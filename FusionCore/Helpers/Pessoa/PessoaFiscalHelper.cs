using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags.Pessoa;

namespace FusionCore.Helpers.Pessoa
{
    public static class PessoaFiscalHelper
    {
        public static string GetDocumentoUnico(this PessoaDTO pessoa)
        {
            switch (pessoa.Tipo)
            {
                case PessoaTipo.Fisica:
                    return pessoa.Cpf;
                case PessoaTipo.Juridica:
                    return pessoa.Cnpj;
            }

            return string.Empty;
        }
    }
}