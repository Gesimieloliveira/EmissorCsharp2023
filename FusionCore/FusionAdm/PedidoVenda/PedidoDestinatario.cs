using System;
using System.Linq;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class PedidoDestinatario : Entidade
    {
        private PedidoDestinatario()
        {
            Cep = string.Empty;
            Logradouro = string.Empty;
            Bairro = string.Empty;
            Complemento = string.Empty;
            Numero = string.Empty;
        }

        public PedidoDestinatario(Cliente cliente, PedidoVenda pedidoVenda) : this()
        {
            DefinirPedido(pedidoVenda);
            DefinirCliente(cliente);
        }

        public PedidoDestinatario(Visitante visitante, PedidoVenda pedidoVenda) : this()
        {
            Visitante = visitante;
            DefinirPedido(pedidoVenda);
        }

        protected override int ReferenciaUnica => PedidoVendaId;
        public int PedidoVendaId { get; private set; }
        public PedidoVenda PedidoVenda { get; private set; }
        public Cliente Cliente { get; private set; }
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Complemento { get; private set; }
        public CidadeDTO Cidade { get; private set; }
        public Visitante Visitante { get; private set; }

        public string GetNome => Cliente?.Nome ?? Visitante.Nome;
        public string GetDocumento => Cliente?.Documento ?? string.Empty;
        public string GetEndereco => CriaStringEndereco();

        private void DefinirCliente(Cliente cliente)
        {
            Cliente = cliente;
            Visitante = Visitante.Empty;
            DefinirEndereco(cliente);
        }

        private string CriaStringEndereco()
        {
            return string.IsNullOrEmpty(Logradouro) 
                ? string.Empty
                : $"{Logradouro}, {Numero}, {Bairro} / {Cep} / {Cidade}";
        }

        private void DefinirPedido(PedidoVenda pedidoVenda)
        {
            PedidoVendaId = pedidoVenda.Destinatario?.PedidoVendaId ?? 0;
            PedidoVenda = pedidoVenda;
        }

        private void DefinirEndereco(Cliente entidade)
        {
            var endereco = entidade.Enderecos.FirstOrDefault();

            if (endereco == null)
            {
                throw new InvalidOperationException("Destinatário deve ter um endereço");
            }

            Cep = endereco.Cep;
            Logradouro = endereco.Logradouro;
            Numero = endereco.Numero;
            Bairro = endereco.Bairro;
            Complemento = endereco.Complemento;
            Cidade = endereco.Cidade;
        }
    }
}