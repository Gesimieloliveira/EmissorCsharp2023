using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class PedidoVenda : Entidade
    {
        private readonly IList<PedidoVendaProduto> _itens;
        private readonly IList<Negociacao> _negociacoes;

        public PedidoVenda()
        {
            Referencia = string.Empty;
            Observacao = string.Empty;
            MotivoCancelamento = string.Empty;

            _itens = new List<PedidoVendaProduto>();
            _negociacoes = new List<Negociacao>();
        }

        public string Uuid { get; set; } = GuuidHelper.ComputarComPrefixo("pedido-venda");
        public int Id { get; set; }
        public bool IsNovo => Id == 0;
        protected override int ReferenciaUnica => Id;
        public EstadoAtual EstadoAtual { get; private set; } = EstadoAtual.Aberto;
        public TipoPedido TipoPedido { get; private set; } = TipoPedido.PedidoVenda;
        public EmpresaDTO Empresa { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public DateTime AbertoEm { get; set; }
        public DateTime? FinalizadoEm { get; private set; }
        public DateTime? CanceladoEm { get; private set; }
        public decimal TotalProdutos { get; private set; }
        public decimal PercentualDesconto { get; private set; }
        public decimal TotalDesconto { get; private set; }
        public decimal Total { get; private set; }
        public string Referencia { get; set; }
        public PedidoDestinatario Destinatario { get; set; }
        public bool EstaFinalizado => EstadoAtual == EstadoAtual.Finalizado;
        public bool EstaAberto => EstadoAtual == EstadoAtual.Aberto;
        public bool IsOrcamento => TipoPedido == TipoPedido.Orcamento;
        public bool EstaCancelado => EstadoAtual == EstadoAtual.Cancelado;
        public bool EstaFaturado => EstadoAtual == EstadoAtual.Faturado;
        public bool IsPedidoVenda => TipoPedido == TipoPedido.PedidoVenda;
        public bool UsandoVisitante => IsUsandoVisitante();
        public string Observacao { get; set; }
        public IEnumerable<PedidoVendaProduto> ItensPedidoVenda => _itens;
        public IEnumerable<Negociacao> Negociacao => _negociacoes;
        public PedidoVendedor Vendedor { get; private set; }
        public string MotivoCancelamento { get; private set; }
        public bool NaoTemCliente => Destinatario?.Cliente == null;
        public TabelaPreco TabelaPreco { get; set; }

        private bool IsUsandoVisitante()
        {
            return Destinatario?.Visitante != null && Destinatario.Visitante.Nome != string.Empty;
        }

        public void MarcarComoPedido()
        {
            if (IsPedidoVenda)
            {
                throw new InvalidOperationException("Documento já é um Pedido de Venda!");
            }

            if (IsOrcamento)
            {
                TipoPedido = TipoPedido.PedidoVenda;
            }
        }

        public void MarcarComoOrcamento()
        {
            if (IsNovo && IsPedidoVenda)
            {
                TipoPedido = TipoPedido.Orcamento;
                return;
            }

            if (IsOrcamento)
            {
                throw  new InvalidOperationException("Documento já é um Orçamento!");
            }

            throw  new InvalidOperationException("Documento não pode ser convertido para um Orçamento!");
        }

        public void Faturar()
        {
            EstadoAtual = EstadoAtual.Faturado;
            FinalizadoEm = DateTime.Now;
        }

        public void Cancelar(string motivoCancelamento)
        {
            EstadoAtual = EstadoAtual.Cancelado;
            MotivoCancelamento = motivoCancelamento;
            CanceladoEm = DateTime.Now;
        }

        public void Abrir(EmpresaDTO empresa, UsuarioDTO usuario)
        {
            EstadoAtual = EstadoAtual.Aberto;
            AbertoEm = DateTime.Now;
            Empresa = empresa;
            Usuario = usuario;
        }

        public void AplicaDesconto(decimal percentual)
        {
            if (percentual < 0)
            {
                throw new InvalidOperationException("Preciso de um percentual de desconto positivo");
            }

            if (TotalProdutos <= 0)
            {
                throw new InvalidOperationException("Total dos produtos precisa ser positivo");
            }

            PercentualDesconto = decimal.Round(percentual, 6);

            CalcularTotais();
        }

        public void AddItem(ProdutoDTO produto
            , decimal quantidade
            , decimal precoUnitario
            , UsuarioDTO usuario)
        {
            if (EstaAberto == false)
            {
                throw new InvalidOperationException("Item só pode ser adicionado em documentos abertos");
            }

            var item = new PedidoVendaProduto(this, produto, _itens.Count + 1, usuario);


            if (TabelaPreco != null)
            {
                item.Quantificar(quantidade, precoUnitario);
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(TabelaPreco,
                        new RepositorioTabelaPreco(sessao),
                        produto,
                        new AtualizaPrecosCalculadosPorTabelaPreco(item, item));
                }
                item.Quantificar(item.Quantidade, item.PrecoUnitario);
            }

            if (TabelaPreco == null)
            {
                item.Quantificar(quantidade, precoUnitario);
            }

            _itens.Add(item);

            CalcularTotais();
        }

        public void RemoverItem(PedidoVendaProduto item)
        {
            if (EstaAberto == false)
            {
                throw new InvalidOperationException("Item só pode ser removido em documentos abertos");
            }

            _itens.Remove(item);

            RecalculaNumeroItem();

            CalcularTotais();
        }

        public void UpdateItem(int itemId, ItemValue args)
        {
            var item = _itens.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new InvalidOperationException(
                    $"Não encontrei item para o documento com esse identificador {itemId}");
            }

            item.DefinirObservacao(args.Observacao);
            item.Quantificar(args.Quantidade, args.ValorUnitario);
            item.AplicarDesconto(args.TotalDesconto);

            CalcularTotais();
        }

        private void CalcularTotais()
        {
            var totalProdutos = _itens.Sum(i => i.Total);
            var descontoSobreTotal = decimal.Round(totalProdutos * PercentualDesconto / 100, 2);
            var novoTotal = totalProdutos - descontoSobreTotal;

            if (novoTotal < 0)
            {
                throw new InvalidOperationException("Operação inválido, documento ficará com total negativo");
            }

            TotalProdutos = decimal.Round(totalProdutos, 2);
            TotalDesconto = decimal.Round(descontoSobreTotal, 2);
            Total = decimal.Round(novoTotal, 2);
        }

        public bool PossuiNegociacao()
        {
            return Negociacao.Any();
        }

        public void Finalizar(IList<Negociacao> negociacoes)
        {
            if (EstadoAtual == EstadoAtual.Finalizado)
            {
                throw new InvalidOperationException("Documento já está finalizado");
            }

            EstadoAtual = EstadoAtual.Finalizado;
            FinalizadoEm = DateTime.Now;
        }

        public void RemoverFinalizacao()
        {
            if (EstaFaturado)
            {
                throw new InvalidOperationException("Pedido de venda já foi faturado");
            }

            _negociacoes.Clear();

            EstadoAtual = EstadoAtual.Aberto;
            FinalizadoEm = DateTime.Now;
        }

        private void RecalculaNumeroItem()
        {
            var numero = (short) 0;

            foreach (var itemProduto in _itens.OrderBy(x => x.Numero))
            {
                itemProduto.AlterarNumero(++numero);
            }
        }

        public void ValidarParaFinalizacao()
        {
            if (EstaFinalizado == false)
            {
                throw new InvalidOperationException("Apenas Documentos Finalizados podem ser Faturados!");
            }

            if (IsOrcamento)
            {
                throw new InvalidOperationException("Não posso faturar um orçamento, preciso de um Pedido de Venda!");
            }

            if (ItensPedidoVenda.Any() == false)
            {
                throw new InvalidOperationException("Apenas Documentos que tem produtos adicionados podem ser Faturados!");
            }
        }

        public void DefineVendedor(Vendedor vendedor)
        {
            if (Vendedor == null)
            {
                Vendedor = new PedidoVendedor(vendedor, this);
                return;
            }

            Vendedor.AlteraVendedor(vendedor);
        }

        public bool NaoContemVendedor()
        {
            return Vendedor == null;
        }

        public void AbrirPedidoNovamente()
        {
            EstadoAtual = EstadoAtual.Aberto;
        }

        public void RecalcularComtabelaPreco(RepositorioTabelaPreco repositorioTabelaPreco)
        {
            foreach (var pedidoVendaProduto in _itens)
            {
                if (TabelaPreco == null)
                {
                    pedidoVendaProduto.Quantificar(pedidoVendaProduto.Quantidade
                        , pedidoVendaProduto.Produto.PrecoVenda);

                    continue;
                }

                AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(TabelaPreco,
                    repositorioTabelaPreco,
                    pedidoVendaProduto.Produto,
                    new AtualizaPrecosCalculadosPorTabelaPreco(pedidoVendaProduto, pedidoVendaProduto));

                pedidoVendaProduto.Quantificar(pedidoVendaProduto.Quantidade, pedidoVendaProduto.PrecoUnitario);
            }

            CalcularTotais();
        }
    }
}