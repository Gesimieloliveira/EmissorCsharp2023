using System.Linq;
using FusionCore.FusionAdm.Pessoas;
using NHibernate.Util;
using static System.String;

namespace FusionCore.Helpers.Pessoa
{
    public static class PessoaHelper
    {
        public static string GetDocumentoUnico(this PessoaEntidade pessoa)
        {
            switch (pessoa.Tipo)
            {
                case PessoaTipo.Juridica:
                    return pessoa.Cnpj.ToString();
                case PessoaTipo.Fisica:
                    return pessoa.Cpf.ToString();
                case PessoaTipo.Extrangeiro:
                    return pessoa.DocumentoExterior;
                default:
                    return Empty;
            }
        }

        public static string GetDocumentoUnico(this PessoaExtensao pessoa)
        {
            switch (pessoa.Tipo)
            {
                case PessoaTipo.Juridica:
                    return pessoa.Cnpj.ToString();
                case PessoaTipo.Fisica:
                    return pessoa.Cpf.ToString();
                case PessoaTipo.Extrangeiro:
                    return pessoa.DocumentoExterior;
                default:
                    return Empty;
            }
        }

        public static PessoaEndereco GetEnderecoPrincipal(this PessoaEntidade pessoa)
        {
            if (pessoa.Enderecos == null || pessoa.Enderecos.Count == 0)
                return null;

            if (pessoa.Enderecos.Any(x => x.Principal == true))
                return pessoa.Enderecos.SingleOrDefault(x => x.Principal == true);

            return (PessoaEndereco)pessoa.Enderecos.FirstOrNull();
        }

        public static PessoaEndereco GetEnderecoPrincipal(this PessoaExtensao pessoa)
        {
            if (pessoa.Enderecos == null || pessoa.Enderecos.Count == 0)
                return null;

            return (PessoaEndereco) pessoa.Enderecos.FirstOrNull();
        }

        public static PessoaTelefone GetPrimeiroTelefone(this PessoaEntidade pessoa)
        {
            if (pessoa?.Telefones == null || pessoa.Telefones.Count == 0)
            {
                return null;
            }

            return (PessoaTelefone) pessoa.Telefones.FirstOrNull();
        }
    }
}