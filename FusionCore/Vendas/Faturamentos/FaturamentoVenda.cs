using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.ControleCaixa;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Repositorio;
using FusionCore.Vendas.Shared;
using FusionLibrary.Helper.Criptografia;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoVenda : Entidade, IFaturuamentoImprimivel, IVendaRegistravelEmCaixa
    {
        private readonly IList<FaturamentoProduto> _produtos = new List<FaturamentoProduto>();
        private readonly IList<FPagamento> _pagamentos = new List<FPagamento>();

        private FaturamentoVenda()
        {
            Uuid = GuuidHelper.ComputarComPrefixo("faturamento-venda");
            CriadoEm = DateTime.Now;
            EstadoAtual = Estado.Aberto;
            Observacao = string.Empty;
            CancelamentoJustificativa = string.Empty;
            SituacaoFiscalNaoEnviado();
        }

        public FaturamentoVenda(EmpresaDTO empresa, UsuarioDTO usuario, TabelaPreco tabelaPrecos = null) : this()
        {
            Empresa = empresa;
            CriadoPor = usuario;
            TabelaPreco = tabelaPrecos;
        }

        protected override int ReferenciaUnica => Id;
        public int Id { get; private set; }
        public EmpresaDTO Empresa { get; private set; }
        public Destinatario Destinatario { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public UsuarioDTO CriadoPor { get; private set; }
        public string Uuid { get; private set; }
        public Estado EstadoAtual { get; private set; }
        public DateTime? FinalizadoEm { get; private set; }
        public decimal TotalProdutos { get; private set; }
        public decimal PercentualDesconto { get; private set; }
        public decimal TotalDesconto { get; private set; }
        public decimal Total { get; private set; }
        public decimal Troco { get; private set; }
        public Malote Malote { get; private set; }
        public string Observacao { get; set; }
        public SituacaoFiscal SituacaoFiscal { get; private set; }
        public FaturamentoVendedor Vendedor { get; set; }
        public TabelaPreco TabelaPreco { get; private set; }

        public IEnumerable<FPagamento> Pagamentos => _pagamentos;
        public IEnumerable<FaturamentoProduto> Produtos => _produtos;

        internal void VincularDestinatario(Cliente cliente, PessoaEndereco endereco)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            var destinatarioAnterior = Destinatario;
            var novoDestinatrio = new Destinatario(this, cliente, new Endereco(endereco));

            if (destinatarioAnterior?.Id > 0)
            {
                novoDestinatrio.Id = destinatarioAnterior.Id;
            }

            Destinatario = novoDestinatrio;
        }

        public void ThrowExceptionSeEstadoDiferenteAberto()
        {
            if (EstadoAtual != Estado.Aberto)
                throw new InvalidOperationException("Faturamento precisa estar aberto");
        }

        internal void AddItem(FaturamentoProduto item)
        {
            ThrowExceptionSeEstadoDiferenteAberto();
            _produtos.Add(item);
            CalcularTotais();
        }

        internal void FaturarProdutoDePedido(
            ProdutoDTO produto,
            UsuarioDTO usuario,
            decimal precoVenda,
            decimal quantidade,
            decimal percentualDesconto)
        {
            //TODO: Faturar item de pedido (verificar tabela de preços)

            ThrowExceptionSeEstadoDiferenteAberto();

            var numero = (short)(_produtos.Any() ? (_produtos.Max(i => i.Numero) + 1) : 1);
            var item = new FaturamentoProduto(this, usuario, produto, numero, precoVenda, quantidade);

            item.AplicarPercentualDesconto(percentualDesconto);
            item.MovimentaEstoque = false;

            _produtos.Add(item);

            CalcularTotais();
        }

        public void CalcularTotais()
        {
            TotalProdutos = _produtos.Sum(i => i.Total);
            TotalDesconto = 0;

            if (PercentualDesconto != 0)
            {
                TotalDesconto = decimal.Round(TotalProdutos * PercentualDesconto / 100, 2);
            }

            Total = decimal.Round(TotalProdutos - TotalDesconto, 2);

            RatearDescontoFixoNosItens();
        }

        internal void AplicarDesconto(decimal totalDesconto)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            if (TotalProdutos == 0)
            {
                TotalDesconto = totalDesconto;
                PercentualDesconto = 0.00M;
                return;
            }

            var novoTotal = decimal.Round(TotalProdutos - totalDesconto, 2);
            var percentual = decimal.Round((totalDesconto / TotalProdutos) * 100, 4);

            TotalDesconto = totalDesconto;
            PercentualDesconto = percentual;
            Total = novoTotal;
        }

        private void RatearDescontoFixoNosItens()
        {
            var totalDescontoAplicado = 0M;

            for (var i = 0; i < _produtos.Count; i++)
            {
                var item = _produtos[i];

                item.AplicarDescontoFixoNoTotal(PercentualDesconto);
                totalDescontoAplicado += item.TotalDescontoFixo;

                if (i + 1 == _produtos.Count)
                {
                    var sobra = decimal.Round(TotalDesconto - totalDescontoAplicado, 2);
                    item.AdicionarDescontoFixo(sobra);
                }
            }
        }

        internal void AplicarPercentualDesconto(decimal percentual)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            var valorDesconto = decimal.Round(TotalProdutos * percentual / 100, 2);
            var novoTotal = TotalProdutos - valorDesconto;

            if (novoTotal <= 0)
            {
                throw new InvalidOperationException("Desconto inválido! Total do faturamento não pode ficar negativo.");
            }

            PercentualDesconto = percentual;
            TotalDesconto = valorDesconto;
            Total = novoTotal;

            CalcularTotais();
        }

        internal void RemoverProduto(int itemId)
        {
            var item = _produtos.SingleOrDefault(i => i.Id == itemId);
            if (item is null) throw new InvalidOperationException("Item não existe no faturamento");

            RemoverProduto(item);
        }

        internal void RemoverProduto(FaturamentoProduto item)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            if (_produtos.Where(i => i != item).Sum(i => i.Total) < 0)
            {
                throw new InvalidOperationException(
                    "Não é possível remover este item o Faturamento ficará negativo!");
            }

            _produtos.Remove(item);

            CalcularTotais();
            ReordenarNumeros();
        }

        internal void Cancelar(RepositorioFaturamento repositorio, string justificativa)
        {
            if (EstadoAtual == Estado.Cancelado)
            {
                throw new InvalidOperationException("Faturamento já está cancelado");
            }

            if (repositorio.PossuiFinanceiroAberto(this))
            {
                throw new InvalidOperationException(
                    "Faturamento possui documentos a receber não estornados. Para cancelar é preciso estornar todos os documentos");
            }

            CancelamentoJustificativa = justificativa;
            EstadoAtual = Estado.Cancelado;
        }

        public string CancelamentoJustificativa { get; private set; }

        internal void Pagar(IReadOnlyList<FPagamento> pagamentos)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            if (pagamentos.Sum(i => i.Valor) != Total)
            {
                throw new InvalidOperationException("Total dos pagamentos precisa ser igual Total do faturamento");
            }

            foreach (var pg in pagamentos)
            {
                pg.AnexarVenda(this);

                if (pg.RegistraFinanceiro && Malote != null)
                {
                    throw new InvalidOperationException("No momento é possível registrar financeiro apenas para um tipo de pagamento");
                }

                if (pg.RegistraFinanceiro)
                {
                    var malote = pg.CriaMalote(
                        Empresa,
                        Destinatario.Cliente,
                        OrigemDocumento.Faturamento,
                        Guid.Parse(Uuid),
                        pg.CriadoPor
                    );

                    Malote = malote;
                }

                _pagamentos.Add(pg);
            }
        }

        internal void Finalizar(decimal troco)
        {
            ThrowExceptionSeEstadoDiferenteAberto();

            if (_pagamentos.Sum(i => i.Valor) != Total)
            {
                throw new InvalidOperationException("Total dos pagamentos diverge do Total do Faturamento");
            }

            FinalizadoEm = DateTime.Now;
            EstadoAtual = Estado.Finalizado;
            Troco = troco;
        }

        public IEnumerable<OperacaoCaixa> ObterOperacoes()
        {
            return Pagamentos.Select(pg
                => new OperacaoCaixa(pg.CriadoEm, pg.Especie, EOrigemFluxoCaixaIndividual.Faturamento, pg.Valor)
            );
        }

        public bool ContemMalote()
        {
            return Malote != null;
        }

        public void DefineVendedor(Vendedor vendedor)
        {
            if (Vendedor is null)
            {
                Vendedor = new FaturamentoVendedor(vendedor, this);
                return;
            }

            Vendedor.AlteraVendedor(vendedor);
        }

        public void SituacaoFiscalNaoEnviado()
        {
            SituacaoFiscal = SituacaoFiscal.NaoEnviado;
        }

        public void SituacaoFiscalComRejeicao()
        {
            SituacaoFiscal = SituacaoFiscal.ComRejeicao;
        }

        public void SituacaoFiscalAutorizado()
        {
            SituacaoFiscal = SituacaoFiscal.Autorizado;
        }

        public void SituacaoFiscalCancelado()
        {
            SituacaoFiscal = SituacaoFiscal.Cancelado;
        }

        public void SituacaoFiscalAutorizadoSemInternet()
        {
            SituacaoFiscal = SituacaoFiscal.AutorizadoSemInternet;
        }

        public void SituacaoFiscalAutorizadoDenegada()
        {
            SituacaoFiscal = SituacaoFiscal.AutorizadoDenegada;
        }

        public void SituacaoFiscalRejeicaoOffline()
        {
            SituacaoFiscal = SituacaoFiscal.ComRejeicaoOffline;
        }

        public bool Autorizada()
        {
            return SituacaoFiscal == SituacaoFiscal.Autorizado;
        }

        public bool EUmaVendaOffline()
        {
            return SituacaoFiscal == SituacaoFiscal.AutorizadoSemInternet;
        }

        private void ReordenarNumeros()
        {
            short numeroProduto = 1;
            foreach (var item in _produtos.OrderBy(i => i.Numero))
            {
                item.AtualizaNumero(numeroProduto++);
            }
        }

        internal void AplicarTabelaPrecos(TabelaPreco tabelaPreco)
        {
            var jaUtilizouTabela = TabelaPreco != null;

            foreach (var item in Produtos)
            {
                if (jaUtilizouTabela)
                {
                    item.RemoverTabelaPrecos();
                }

                item.AplicarTabelaPrecos(tabelaPreco);
            }

            TabelaPreco = tabelaPreco;
            CalcularTotais();
        }

        internal void RemoverTabelaPrecos()
        {
            foreach (var item in Produtos)
            {
                item.RemoverTabelaPrecos();
            }

            TabelaPreco = null;
            CalcularTotais();
        }
    }
}