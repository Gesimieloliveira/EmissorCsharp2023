using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.Produto;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public sealed class AbaItensNfeModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private decimal _calculoBcFcpSt;
        private decimal _calculoFcpSt;
        private ObservableCollection<TabelaPrecoDto> _tabelasPrecosLista;
        private TabelaPrecoDto _tabelaPrecoSelecionada;

        public AbaItensNfeModel(ISessaoManager sessaoManager)
        {
            PossuiFinanceiro = SessaoSistema.Instancia.AcessoConcedido.PossuiFusionGestor;
            _sessaoManager = sessaoManager;
            CarregarTabelasPrecos();
        }

        public bool Selecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public Nfeletronica Nfe
        {
            get => GetValue<Nfeletronica>();
            set => SetValue(value);
        }

        public ObservableCollection<ItemNfe> Itens => Nfe?.Itens == null
            ? new ObservableCollection<ItemNfe>()
            : new ObservableCollection<ItemNfe>(Nfe.Itens.OrderByDescending(x => x.Id));

        public ItemNfe ItemSelecionado
        {
            get => GetValue<ItemNfe>();
            set => SetValue(value);
        }

        public decimal QuantidadeItens
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal QuantidadeProdutos
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoTotalDescontoProdutos
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoTotalProdutosComDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoBcIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoBcFcpSt
        {
            get => _calculoBcFcpSt;
            set
            {
                if (value == _calculoBcFcpSt) return;
                _calculoBcFcpSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal CalculoIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoBcSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoFcpSt
        {
            get => _calculoFcpSt;
            set
            {
                if (value == _calculoFcpSt) return;
                _calculoFcpSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal CalculoCofins
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoPis
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoIpi
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoIpiDevolucao
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoValorBrutoProdutos
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CalculoTotalFiscal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDescontoFixo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorFreteFixo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorSeguroFixo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDespesasFixa
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool PossuiFinanceiro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<TabelaPrecoDto> TabelasPrecosLista
        {
            get => _tabelasPrecosLista;
            set
            {
                _tabelasPrecosLista = value;
                PropriedadeAlterada();
            }
        }

        public TabelaPrecoDto TabelaPrecoSelecionada
        {
            get => _tabelaPrecoSelecionada;
            set
            {
                _tabelaPrecoSelecionada = value;

                AtualizarPrecosComTabelaPreco();
                PropriedadeAlterada();
            }
        }

        public event EventHandler PassoAnteriorCalled;
        public event EventHandler EmiteNfeCalled;
        public event EventHandler ReferenciarNfeCalled;
        public event EventHandler ReferenciarCfCalled;
        public event EventHandler<Nfeletronica> AlterarTotaisFixoCalled;

        public void Preparar(Nfeletronica nfe)
        {
            Nfe = nfe;
            AtualizaDadosView();
        }

        public void OnPassoAnteriorCalled()
        {
            PassoAnteriorCalled?.Invoke(this, EventArgs.Empty);
        }

        public void OnEmiteNfeCalled()
        {
            EmiteNfeCalled?.Invoke(this, EventArgs.Empty);
        }

        public void OnReferenciarNfeCalled()
        {
            ReferenciarNfeCalled?.Invoke(this, EventArgs.Empty);
        }

        public void OnReferenciarCfCalled()
        {
            ReferenciarCfCalled?.Invoke(this, EventArgs.Empty);
        }

        public void RemoverItemSelelecioando()
        {
            var confirm = DialogBox.MostraConfirmacao("Remover o item não tem volta, posso continuar?");

            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfe(sessao);

                try
                {
                    if (Nfe.TipoOperacao == TipoOperacao.Entrada)
                    {
                        ChecadorEstoqueNegativoNfe
                            .ThrowExceptionSeRemoverItemNotaEntradaNegativarEstoque(
                                ItemSelecionado.Produto.Id,
                                ItemSelecionado.Quantidade
                            );
                    }

                    Nfe.RemoveItem(ItemSelecionado);
                    Nfe.CalcularItens();

                    repositorio.SalvarAlteracoes(Nfe);
                    transacao.Commit();
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
                catch (Exception ex)
                {
                    DialogBox.MostraErro("Não consegui remover o item: " + ex.Message, ex);
                }
            }

            AtualizaDadosView();
        }

        public void PreVisualizarDanfe()
        {
            try
            {
                DanfeNfeHelper.GeraPreVisualizacaoDanfe(Nfe, _sessaoManager);
            }
            catch (Exception e)
            {
                DialogBox.MostraAviso("DANFE", "Não consegui processar o danfe: " + e.Message);
            }

            AtualizaDadosView();
        }

        public void AtualizaDadosView()
        {
            QuantidadeItens = Nfe.Itens.Count();
            QuantidadeProdutos = Nfe.Itens.Sum(i => i.Quantidade);
            CalculoTotalDescontoProdutos = Nfe.Itens.Sum(i => i.TotalDescontoItem);

            CalculoTotalProdutosComDesconto = Nfe.Itens.Sum(i => i.TotalItem);

            CalculoBcFcpSt = Nfe.TotalBcFcpSt;
            CalculoFcpSt = Nfe.TotalFcpSt;
            CalculoBcIcms = Nfe.TotalBcIcms;
            CalculoIcms = Nfe.TotalIcms;
            CalculoBcSt = Nfe.TotalBcSt;
            CalculoSt = Nfe.TotalSt;
            CalculoCofins = Nfe.TotalCofins;
            CalculoPis = Nfe.TotalPis;
            CalculoIpi = Nfe.CalcularTotalIpi();
            CalculoIpiDevolucao = Nfe.CalcularTotalIpiDevolucao();
            CalculoValorBrutoProdutos = Nfe.TotalItens;
            CalculoTotalFiscal = Nfe.TotalFinal;
            ValorDescontoFixo = Nfe.ValorDescontoFixo;
            ValorFreteFixo = Nfe.ValorFreteFixo;
            ValorSeguroFixo = Nfe.ValorSeguroFixo;
            ValorDespesasFixa = Nfe.ValorDespesasFixa;

            _tabelaPrecoSelecionada = Nfe.TabelaPreco != null
                ? new TabelaPrecoDto
                {
                    Descricao = Nfe.TabelaPreco.Descricao,
                    Id = Nfe.TabelaPreco.Id
                }
                : null;

            PropriedadeAlterada(nameof(TabelaPrecoSelecionada));

            Nfe.CalcularCreditoItens();

            PropriedadeAlterada(nameof(Itens));
        }

        public void OnAlterarTotaisFixoCalled()
        {
            AlterarTotaisFixoCalled?.Invoke(this, Nfe);
        }

        public void AlterarProdutoItemSelecionado()
        {
            var produtoModel = new ProdutoFormModel(ItemSelecionado.Produto);
            produtoModel.RegistroSalvo += ProdutoSalvoHandler;

            new ProdutoForm(produtoModel).ShowDialog();
        }

        private void ProdutoSalvoHandler(object sender, ProdutoDTO produto)
        {
            foreach (var nfeIten in Nfe.Itens)
            {
                if (nfeIten.Produto.Id != produto.Id) continue;
                nfeIten.Produto.Ativo = produto.Ativo;
                break;
            }

            AtualizaDadosView();
        }

        private void AtualizarPrecosComTabelaPreco()
        {
            var tabelaPreco = CarregaTabelaPrecoPorId(TabelaPrecoSelecionada);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                Nfe.TabelaPreco = tabelaPreco;
                Nfe.RecalcularComtabelaPreco(new RepositorioTabelaPreco(sessao));

                var repositorio = new RepositorioNfe(sessao);

                Nfe.RemoveItem(ItemSelecionado);
                Nfe.CalcularItens();

                repositorio.SalvarAlteracoes(Nfe);
                transacao.Commit();
            }

            AtualizaDadosView();
        }

        private TabelaPreco CarregaTabelaPrecoPorId(TabelaPrecoDto tabela)
        {
            if (tabela == null) return null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPreco(sessao);

                return repositorioTabelaPreco.GetPeloId(tabela.Id);
            }
        }

        private void CarregarTabelasPrecos()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTabelaPreco(sessao);
                var tabelas = repositorio.BuscarTodasTabelasDto();

                TabelasPrecosLista = new ObservableCollection<TabelaPrecoDto>(tabelas);
            }
        }
    }
}