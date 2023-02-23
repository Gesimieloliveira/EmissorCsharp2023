using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionNfce.Cfop;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.VisaoModel;

// ReSharper disable ExplicitCallerInfoArgument

namespace FusionNfce.Visao.Principal.AlteraItem
{
    public class NfceItemEvent : EventArgs
    {
        public NfceItem Item { get; }
        public decimal QuantidadeOriginal { get; }

        public NfceItemEvent(NfceItem item, decimal quantidadeOriginal)
        {
            Item = item;
            QuantidadeOriginal = quantidadeOriginal;
        }
    }

    public sealed class AlteraItemFormModel : ViewModel
    {
        private readonly NfceItem _item;

        public bool IsTemFormaDePagamento
        {
            get => _isTemFormaDePagamento;
            set
            {
                if (value == _isTemFormaDePagamento) return;
                _isTemFormaDePagamento = value;
                PropriedadeAlterada();
            }
        }

        private decimal _quantidadeOriginal;
        private readonly SessaoManagerNfce _sessaoManager;
        private bool _isRejeicaoOffline;
        private bool _isEditarDesconto;
        private bool _isNaoPodeDesconto;
        private bool _isTemFormaDePagamento;
        private ObservableCollection<TributacaoCstNfce> _tributacoesCst;
        private TributacaoCstNfce _tributacaoCstSelecionado;


        public ObservableCollection<TributacaoCstNfce> TributacoesCst
        {
            get => _tributacoesCst;
            set
            {
                if (Equals(value, _tributacoesCst)) return;
                _tributacoesCst = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CfopNfce> ListaDeCfop
        {
            get => GetValue<ObservableCollection<CfopNfce>>();
            set => SetValue(value);
        }

        public CfopNfce Cfop
        {
            get => GetValue<CfopNfce>();
            set => SetValue(value);
        }

        public PrecoItem PrecoItem
        {
            get => GetValue<PrecoItem>();
            set => SetValue(value);
        }

        public string NomeProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsRejeicaoOffline
        {
            get => _isRejeicaoOffline;
            set
            {
                if (value == _isRejeicaoOffline) return;
                _isRejeicaoOffline = value;
                PropriedadeAlterada();
            }
        }

        public AlteraItemFormModel(NfceItem item, bool isTemFormaDePagamento)
        {
            _item = item;
            IsTemFormaDePagamento = isTemFormaDePagamento;
            _sessaoManager = new SessaoManagerNfce();
        }

        public bool IsEditarDesconto
        {
            get => _isEditarDesconto;
            set
            {
                _isEditarDesconto = value;
                PropriedadeAlterada();
                IsNaoPodeDesconto = !value;
            }
        }

        public bool IsNaoPodeDesconto
        {
            get => _isNaoPodeDesconto;
            set
            {
                _isNaoPodeDesconto = value;
                PropriedadeAlterada();
            }
        }

        public string GtinProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ObservacaoProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TributacaoCstNfce TributacaoCstSelecionado
        {
            get => GetValue<TributacaoCstNfce>();
            set => SetValue(value);
        }

        public string LabelCst { get; set; } =
            SessaoSistemaNfce.Empresa().RegimeTributario == RegimeTributario.SimplesNacional ? "CSOSN" : "CST";

        public event EventHandler<NfceItemEvent> RetornaItemAtualizado;

        public void Inicializar()
        {
            CarregarCfops();
            AtualizaModel();
            IsRejeicaoOffline = _item.Nfce.Status != Status.PendenteOffline;

            if (IsTemFormaDePagamento)
            {
                IsRejeicaoOffline = false;
            }

            IsEditarDesconto = _item.Nfce.TotalAcrescimo == 0;
            using (var repositorioTributacaoNfce = new RepositorioTributacaoCstNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                TributacoesCst =
                    new ObservableCollection<TributacaoCstNfce>(repositorioTributacaoNfce.ObterTributacaoPorRegimeTributario(_item.Nfce.RegimeTributario));
            }
        }

        private void AtualizaModel()
        {
            SetValue(_item.Nome, nameof(NomeProduto));
            SetValue(_item.Cfop, nameof(Cfop));
            SetValue(_item.ImpostoIcms.CST, nameof(TributacaoCstSelecionado));
            SetValue(_item.Gtin, nameof(GtinProduto));
            SetValue(_item.Observacao, nameof(ObservacaoProduto));

            PrecoItem = _item.FactoryPrecoItem();
            _quantidadeOriginal = _item.Quantidade;
        }

        private void CarregarCfops()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCfopNfce(sessao);
                var cfops = repositorio.BuscaTodos();

                ListaDeCfop = new ObservableCollection<CfopNfce>(cfops);
            }
        }

        public void AplicarAlteracoes()
        {
            ChecarGtin();
            ChecarTributacoes();
            ChecarValores();
            ChecarEstoque();

            _item.Cfop = Cfop;
            _item.ImpostoIcms.CST = TributacaoCstSelecionado;
            _item.DefinirPreco(PrecoItem);
            _item.Gtin = GtinProduto;
            _item.Observacao = ObservacaoProduto;

            OnRetornaItemAtualizado(_item);
        }

        private void ChecarGtin()
        {
            if (string.IsNullOrWhiteSpace(GtinProduto))
                throw new InvalidOperationException("Código de barras não pode ser vazio");

            if (GtinProduto == "SEM GTIN" || Gs1GtinHelper.EhUmGtinValido(GtinProduto))
                return;

            throw new InvalidOperationException("Código de Barras é inválido");
        }

        private void ChecarEstoque()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                BoqueioEstoqueHelper.ChecaEstoqueNegativoNfce(_item.Produto, PrecoItem.Quantidade, sessao);
            }
        }

        private void ChecarValores()
        {
            PrecoItem.ThrowExceptionSeInvalido();

            if (!_item.Produto.UnidadeMedida.PodeFracionar && PrecoItem.Quantidade % 1 != 0)
            {
                throw new InvalidOperationException("Unidade de medida do produto não permite fracionar");
            }
        }

        private void ChecarTributacoes()
        {
            if (Cfop == null)
            {
                throw new InvalidOperationException("Preciso que informe um CFOP para o item.");
            }
        }

        private void OnRetornaItemAtualizado(NfceItem item)
        {
            RetornaItemAtualizado?.Invoke(this, new NfceItemEvent(item, _quantidadeOriginal));
        }
    }
}