using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionNfce.Cidade;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Cliente
{
    public class ClienteEnderecoNfce : Entidade
    {
        public ClienteEnderecoNfce() { }
        public ClienteEnderecoNfce(PessoaEndereco endereco, ClienteNfce cliente)
        {
            CopiaInformacoes(endereco, cliente);
        }

        public int Id { get; set; }
        public ClienteNfce Cliente { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public CidadeNfce Cidade { get; set; }

        protected override int ReferenciaUnica => Id;

        private void CopiaInformacoes(PessoaEndereco endereco, ClienteNfce cliente)
        {
            Id = endereco.Id;
            Cliente = cliente;
            Logradouro = endereco.Logradouro;
            Numero = endereco.Numero;
            Bairro = endereco.Bairro;
            Cep = endereco.Cep;
            Complemento = endereco.Complemento;
            Cidade = new CidadeNfce(endereco.Cidade);
        }
    }
}