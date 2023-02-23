using System;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class PedidoVendaProduto : Entidade, IMovimentavel, IAtualizaPrecoVenda, IAtualizaValorUnitario
    {
        private PedidoVendaProduto()
        {
            CriadoEm = DateTime.Now;
            Observacao = string.Empty;
            SavePendente = true;
        }

        public PedidoVendaProduto(PedidoVenda pedidoVenda, ProdutoDTO produto, int numero, UsuarioDTO usuario)
            : this()
        {
            PedidoVenda = pedidoVenda;
            Numero = short.Parse(numero.ToString());
            Usuario = usuario;
            Produto = produto;
            PrecoCusto = produto.PrecoCusto;
            PrecoVenda = produto.PrecoVenda;
            PrecoUnitario = produto.PrecoVenda;
            SiglaUnidade = produto.ProdutoUnidadeDTO.Sigla;
        }

        public int Id { get; set; }
        protected override int ReferenciaUnica => Id;
        public PedidoVenda PedidoVenda { get; private set; }
        public short Numero { get; private set; }
        public ProdutoDTO Produto { get; private set; }
        public decimal Quantidade { get; private set; }
        public string SiglaUnidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal TotalBruto { get; private set; }
        public decimal PercentualDesconto { get; private set; }
        public decimal TotalDesconto { get; private set; }
        public decimal Total { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public UsuarioDTO Usuario { get; private set; }
        public string Observacao { get; private set; }
        public decimal PrecoCusto { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public bool SavePendente { get; private set; }

        public bool MovimentaEstoque => !PedidoVenda.IsOrcamento;

        public void Quantificar(decimal quantidade, decimal precoUnitario)
        {
            SavePendente = true;

            TotalDesconto = 0M;
            PercentualDesconto = 0M;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
            TotalBruto = decimal.Round(quantidade * precoUnitario, 2);
            Total = TotalBruto;
        }

        public void AplicarDesconto(decimal totalDesconto)
        {
            SavePendente = true;
            PercentualDesconto = 0M;
            TotalDesconto = totalDesconto;

            if (TotalDesconto > 0)
            {
                PercentualDesconto = decimal.Round((TotalDesconto * 100 / TotalBruto), 6);
                Total = decimal.Round(TotalBruto - TotalDesconto, 2);
            }
        }

        public void DefinirObservacao(string observacao)
        {
            Observacao = observacao;
            SavePendente = true;
        }

        public EstoqueModel CriaMovimentoInclusao()
        {
            var model = new EstoqueModel(Produto, Quantidade, Usuario, OrigemEventoEstoque.ItemAdicionadoPedidoVenda)
            {
                QuantidadeReservaEstoque = Quantidade
            };

            model.MarcaComoInverso();

            return model;
        }

        public EstoqueModel CriaMovimentoRemocao()
        {
            var model = new EstoqueModel(
                Produto,
                Quantidade,
                Usuario,
                OrigemEventoEstoque.ItemRemovidoPedidoVenda
            ) {
                QuantidadeReservaEstoque = Quantidade
            };

            model.MarcaComoInverso();

            return model;
        }

        public void AlterarNumero(short numero)
        {
            Numero = numero;
            SavePendente = true;
        }

        public void MarcaComoSalvo()
        {
            SavePendente = false;
        }

        public void AtualizarPrecoVenda(decimal novoPrecoVenda)
        {
            PrecoVenda = novoPrecoVenda;
        }

        public void AtualizarValorUnitario(decimal novoValorUnitario)
        {
            PrecoUnitario = novoValorUnitario;
        }
    }
}