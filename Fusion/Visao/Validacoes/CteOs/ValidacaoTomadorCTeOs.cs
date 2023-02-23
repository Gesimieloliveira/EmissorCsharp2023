using Fusion.Visao.Validacoes.Pessoa;
using FusionCore.Excecoes;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using NHibernate.Util;

namespace Fusion.Visao.Validacoes.CteOs
{
    public static class ValidacaoTomadorCTeOs
    {
        public static void Executar(PessoaEntidade pessoaEntidade)
        {
            ValidacaoDocumentoUnico.Executar(pessoaEntidade);

            ValidaInscricaoEstadual(pessoaEntidade);

            ValidaNome(pessoaEntidade);

            ValidaNomeFantasia(pessoaEntidade);

            ValidaTelefoneSeTiver(pessoaEntidade);

            if (pessoaEntidade.Enderecos.IsNullOrEmpty())
                CriaExcecao.OperacaoInvalida("Deve ter um Endereço");

            var endereco = (PessoaEndereco) pessoaEntidade.Enderecos.First();

            if (ExtValidaListasEColecoes.IsNullOrEmpty(endereco.Logradouro))
                CriaExcecao.OperacaoInvalida("Deve ter Logradouro");

            if (endereco.Logradouro.Length < 2)
                CriaExcecao.OperacaoInvalida("Deve ter Logradouro com tamanho mínimo de 2 dígitos");

            if (endereco.Numero.IsNullOrEmpty())
                CriaExcecao.OperacaoInvalida("Número obrigatório");

            if (endereco.Bairro.IsNullOrEmpty())
                CriaExcecao.OperacaoInvalida("Bairro obrigatório");

            if (endereco.Bairro.Length < 2)
                CriaExcecao.OperacaoInvalida("Bairro deve ter tamanho mínimo de 2");

            ValidaCepSeTiver(endereco);
        }

        private static void ValidaCepSeTiver(PessoaEndereco endereco)
        {
            if (!endereco.Cep.IsNullOrEmpty()) return;

            if (endereco.Cep.Length != 8)
                CriaExcecao.OperacaoInvalida("Cep deve ter 8 dígitos");
        }

        private static void ValidaTelefoneSeTiver(PessoaEntidade pessoaEntidade)
        {
            if (pessoaEntidade.Telefones.IsNullOrEmpty()) return;

            var telefone = (PessoaTelefone) pessoaEntidade.Telefones.First();

            if (telefone.Numero.Length < 6 || telefone.Numero.Length > 14)
            {
                CriaExcecao.OperacaoInvalida("Telefone tamanho mínimo 6 maximo 14");
            }
        }

        private static void ValidaNomeFantasia(PessoaEntidade pessoaEntidade)
        {
            if (pessoaEntidade.NomeFantasia.IsNullOrEmpty()) return;

            if (pessoaEntidade.NomeFantasia.Length < 2)
                CriaExcecao.OperacaoInvalida("Deve ter Nome Fantasia com tamanho mínimo de 2 dígitos");
        }

        private static void ValidaNome(PessoaEntidade pessoaEntidade)
        {
            if (ExtValidaListasEColecoes.IsNullOrEmpty(pessoaEntidade.Nome))
                CriaExcecao.OperacaoInvalida("Deve ter Nome");

            if (pessoaEntidade.Nome.Length < 2)
                CriaExcecao.OperacaoInvalida("Deve ter Nome com tamanho mínimo de 2 dígitos");
        }

        private static void ValidaInscricaoEstadual(PessoaEntidade pessoaEntidade)
        {
            var inscricaoEstadual = pessoaEntidade.InscricaoEstadual;

            if (inscricaoEstadual.IsNotNullOrEmpty())
                if (inscricaoEstadual.Length > 14)
                    CriaExcecao.OperacaoInvalida("Deve ter Inscrição Estadual");
        }
    }
}