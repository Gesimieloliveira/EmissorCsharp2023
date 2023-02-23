using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using FusionCore.Vendas.Shared;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class Destinatario : Entidade
    {
        private Destinatario()
        {
            //nhibernate
        }

        public Destinatario(FaturamentoVenda faturamento, Cliente cliente, Endereco endereco) : this()
        {
            Faturamento = faturamento;
            Cliente = cliente;
            Endereco = endereco;
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; set; }
        public FaturamentoVenda Faturamento { get;  set; }
        public Cliente Cliente { get; set; }
        public Endereco Endereco { get; set; }
        public bool IsAddEndereco => GetIsAddEndereco();

        private bool GetIsAddEndereco()
        {
            return Endereco.Logradouro.IsNotNullOrEmpty() || Endereco.Numero.IsNotNullOrEmpty() || Endereco.Bairro.IsNotNullOrEmpty() ||
                   Endereco.Cep.IsNotNullOrEmpty() || Endereco.Complemento.IsNotNullOrEmpty() || Endereco.Cidade != null;
        }
    }
}