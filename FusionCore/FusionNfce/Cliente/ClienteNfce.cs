using System.Collections.Generic;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Cliente
{
    public class ClienteNfce : Entidade
    {
        public ClienteNfce() { }

        public ClienteNfce(FusionAdm.Pessoas.Cliente cliente)
        {
            CopiaInformacoes(cliente);
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string DocumentoUnico { get; set; }
        public string InscricaoEstadual { get; set; }
        public bool AplicaLimiteCredito { get; set; }
        public decimal LimiteCredito { get; set; }

        public bool Ativo { get; set; }

        public string Cpf { get; set; }
        public string Cnpj { get; set; }

        public IList<ClienteEnderecoNfce> Enderecos { get; set; }
        public IList<ClienteEmailNfce> Emails { get; set; }

        protected override int ReferenciaUnica => Id;

        private void CopiaInformacoes(FusionAdm.Pessoas.Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            DocumentoUnico = cliente.Documento;
            InscricaoEstadual = cliente.InscricaoEstadual;
            AplicaLimiteCredito = cliente.AplicaLimiteCredito;
            LimiteCredito = cliente.LimiteCredito;
            Ativo = cliente.Pessoa.Ativo;

            Enderecos = new List<ClienteEnderecoNfce>();
            Emails = new List<ClienteEmailNfce>();

            foreach (var clienteEndereco in cliente.Enderecos)
            {
                Enderecos.Add(new ClienteEnderecoNfce(clienteEndereco, this));
            }

            foreach (var pessoaEmail in cliente.Pessoa.Emails)
            {
                Emails.Add(new ClienteEmailNfce(pessoaEmail, this));
            }
        }
    }
}