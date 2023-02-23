using System.Collections.Generic;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Cliente;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Fiscal.Converter
{
    public class ConverterClienteAdmParaClienteNFCe
    {
        private readonly PessoaEntidade _pessoaEntidade;

        public ConverterClienteAdmParaClienteNFCe(PessoaEntidade pessoaEntidade)
        {
            _pessoaEntidade = pessoaEntidade;
        }

        public ClienteNfce Executar()
        {
            var documentoUnico = string.Empty;
            var nome = _pessoaEntidade.Nome;

            if (!_pessoaEntidade.Cnpj.IsEmpty())
            {
                documentoUnico = _pessoaEntidade.Cnpj.ToString();
            }

            if (!_pessoaEntidade.Cpf.IsEmpty())
            {
                documentoUnico = _pessoaEntidade.Cpf.ToString();
            }

            var cliente = new ClienteNfce
            {
                Id = _pessoaEntidade.Id,
                DocumentoUnico = documentoUnico,
                Nome = nome,
                InscricaoEstadual = _pessoaEntidade.InscricaoEstadual,
                Ativo = _pessoaEntidade.Ativo
            };

            ConverterEmails(_pessoaEntidade, cliente);

            ConverterEnderecos(_pessoaEntidade, cliente);

            return cliente;
        }

        private void ConverterEnderecos(PessoaEntidade pessoaEntidade, ClienteNfce cliente)
        {
            IList<ClienteEnderecoNfce> enderecos = new List<ClienteEnderecoNfce>();

            pessoaEntidade.Enderecos.ForEach(e =>
            {
                enderecos.Add(new ClienteEnderecoNfce
                {
                    Id = e.Id,
                    Cliente = cliente,
                    Bairro = e.Bairro,
                    Cep = e.Cep,
                    Cidade = new CidadeNfce { Id = e.Cidade.Id },
                    Complemento = e.Complemento,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero
                });
            });

            cliente.Enderecos = enderecos;
        }

        private void ConverterEmails(PessoaEntidade pessoaEntidade, ClienteNfce cliente)
        {
            IList<ClienteEmailNfce> emails = new List<ClienteEmailNfce>();

            pessoaEntidade.Emails.ForEach(e =>
            {
                emails.Add(new ClienteEmailNfce
                {
                    Id = e.Id,
                    Cliente = cliente,
                    Email = e.Email.Valor
                });
            });

            cliente.Emails = emails;
        }
    }
}