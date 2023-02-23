using System;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoProduto : Entidade, IMovimentavel
    {
        private FaturamentoProduto()
        {
            CriadoEm = DateTime.Now;
        }

        public FaturamentoProduto(
            FaturamentoVenda faturamento,
            UsuarioDTO usuario,
            ProdutoDTO produto,
            short numero,
            decimal precoUnitario,
            decimal quantidade) : this()
        {
            Faturamento = faturamento;
            CriadoPor = usuario;
            Produto = produto;
            Numero = numero;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;

            PrecoCusto = produto.PrecoCusto == 0 ? produto.PrecoCompra : produto.PrecoCusto;
            PrecoVenda = produto.PrecoVenda;
            SiglaUnidade = produto.ProdutoUnidadeDTO.Sigla;

            TotalizarItem();
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; private set; }
        public FaturamentoVenda Faturamento { get; private set; }
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
        public UsuarioDTO CriadoPor { get; private set; }
        public decimal PrecoCusto { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public bool MovimentaEstoque { get; set; } = true;
        public decimal TotalDescontoFixo { get; private set; }
        public bool FoiAlterado { get; private set; }

        public static decimal CalculaTotalBruto(decimal quantidade, decimal unitario)
        {
            return decimal.Round(quantidade * unitario, 2);
        }

        public EstoqueModel CriaMovimentoInclusao()
        {
            var model = new EstoqueModel(
                Produto,
                Quantidade,
                CriadoPor,
                OrigemEventoEstoque.ItemAdicionadoFaturamento
            );

            model.MarcaComoInverso();

            return model;
        }

        public EstoqueModel CriaMovimentoRemocao()
        {
            var model = new EstoqueModel(
                Produto,
                Quantidade,
                CriadoPor,
                OrigemEventoEstoque.ItemRemovidoFaturamento
            );

            model.MarcaComoInverso();

            return model;
        }

        public void Alterar(decimal valorUnitario, decimal percentualDesconto)
        {
            FoiAlterado = true;

            PercentualDesconto = percentualDesconto;
            PrecoUnitario = valorUnitario;
            TotalizarItem();
        }

        private void TotalizarItem()
        {
            var indiceDesconto = 1 - (PercentualDesconto / 100);
            var totalBruto = CalculaTotalBruto(Quantidade, PrecoUnitario);
            var totalLiquido = decimal.Round(totalBruto * indiceDesconto, 2);

            TotalBruto = totalBruto;
            Total = totalLiquido;
            TotalDesconto = decimal.Round(TotalBruto - Total, 2);
        }

        public void AplicarPercentualDesconto(decimal percentual)
        {
            FoiAlterado = true;

            PercentualDesconto = percentual;
            TotalizarItem();
        }

        public void AplicarDescontoFixoNoTotal(decimal percentual)
        {
            FoiAlterado = true;
            TotalDescontoFixo = decimal.Round(Total * percentual / 100, 2);
        }

        public void AdicionarDescontoFixo(decimal valor)
        {
            FoiAlterado = true;
            TotalDescontoFixo += valor;
        }

        public void AtualizaNumero(short numeroProduto)
        {
            Numero = numeroProduto;
        }

        public void AplicarTabelaPrecos(TabelaPreco tabela)
        {
            FoiAlterado = true;
            PrecoUnitario = tabela.CalcularNovoPreco(Produto.Id, PrecoUnitario);
            PrecoVenda = tabela.CalcularNovoPreco(Produto.Id, PrecoVenda);
            TotalizarItem();
        }

        public void RemoverTabelaPrecos()
        {
            FoiAlterado = true;
            PrecoUnitario = Produto.PrecoVenda;
            PrecoVenda = Produto.PrecoVenda;
            TotalizarItem();
        }
    }
}