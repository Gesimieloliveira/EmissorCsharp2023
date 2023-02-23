using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using static System.String;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.Vendas.Shared
{
    public class Endereco
    {
        private Endereco()
        {
            //nhibernate
            Complemento = Empty;
        }

        public Endereco(string cep, string logradouro, string numero, string bairro, CidadeDTO cidade) : this()
        {
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
        }

        public Endereco(PessoaEndereco endereco)
        {
            Cep = endereco.Cep;
            Logradouro = endereco.Logradouro;
            Numero = endereco.Numero;
            Bairro = endereco.Bairro;
            Cidade = endereco.Cidade;
            Complemento = endereco.Complemento ?? Empty;
        }

        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public CidadeDTO Cidade { get; set; }

        public override string ToString()
        {
            return $"{Logradouro}, {Numero}, {Bairro} / {Cep} / {Cidade}";
        }
    }
}