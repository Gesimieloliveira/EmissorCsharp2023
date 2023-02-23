using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.Compras.NotaFiscal.Controls;
using Fusion.Visao.Produto;
using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Compras;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Federal;
using FusionLibrary.VisaoModel;

// ReSharper disable ExplicitCallerInfoArgument

namespace Fusion.Visao.Compras.NotaFiscal
{
    public sealed class CompraItemViewModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly RecipienteUnidade _recipienteUnidade = RecipienteFactory.Get<RecipienteUnidade>();
        private readonly RecipienteTributacaoCst _recipienteIcmsNFe = RecipienteFactory.Get<RecipienteTributacaoCst>();
        private readonly RecipientePis _recipientePis = RecipienteFactory.Get<RecipientePis>();
        private readonly RecipienteCofins _recipienteCofins = RecipienteFactory.Get<RecipienteCofins>();
        private readonly RecipienteIpi _recipienteIpi = RecipienteFactory.Get<RecipienteIpi>();
        private readonly NotaFiscalCompra _nota;
        private readonly CfopFacade _cfopFacade;
        private ItemCompra _item;
        private readonly bool _edicao;
        private bool _freteCompoeIcms;
        private bool _seguroCompoeIcms;
        private bool _despesasCompoeIcms;
        private bool _ipiCompoeIcms;
        private string _ultimoCfopBuscado;
        private int? _ultimoCodigoProdutoBuscado;

        public CompraItemViewModel(NotaFiscalCompra nota, ItemCompra item = null)
        {
            _nota = nota;
            _item = item;
            _edicao = item != null;
            _cfopFacade = new CfopFacade(_sessaoSistema.SessaoManager);

            FatorConversao = 1;
            Cfops = new List<CfopDTO>();
        }

        public ICommand ProdutoPickerCommand => GetSimpleCommand(ProdutoPickerAction);

        public ProdutoDTO Produto
        {
            get => GetValue<ProdutoDTO>();
            set
            {
                SetValue(value);

                SetValue(value?.Id, nameof(CodigoProduto));
                SetValue(Quantidade >= 1 ? Quantidade : 1, nameof(Quantidade));
                SetValue(value?.ProdutoUnidadeDTO, nameof(Unidade));
                SetValue(value?.PrecoCompra ?? 0.00M, nameof(ValorUnitario));
                SetValue(value?.ProdutoUnidadeDTO.Sigla, nameof(UnidadeConversao));

                SetIpiData(value);
                SetPisData(value);
                SetCofinsData(value);

                CalculaConversao();
                CalculaTotal();
            }
        }

        public CfopDTO Cfop
        {
            get => GetValue<CfopDTO>();
            set => SetValue(value);
        }

        public string CodigoCfop
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? CodigoProduto
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public IEnumerable<CfopDTO> Cfops
        {
            get => GetValue<IEnumerable<CfopDTO>>();
            set => SetValue(value);
        }

        public ObservableCollection<ProdutoUnidadeDTO> Unidades
        {
            get => GetValue<ObservableCollection<ProdutoUnidadeDTO>>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoCst> TributacoesIcms
        {
            get => GetValue<ObservableCollection<TributacaoCst>>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoPis> TributacoesPis
        {
            get => GetValue<ObservableCollection<TributacaoPis>>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoCofins> TributacoesCofins
        {
            get => GetValue<ObservableCollection<TributacaoCofins>>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoIpi> TributacoesIpi
        {
            get => GetValue<ObservableCollection<TributacaoIpi>>();
            set => SetValue(value);
        }

        public TributacaoPis Pis
        {
            get => GetValue<TributacaoPis>();
            set
            {
                SetValue(value);
                CalculaImpostoPis();
            }
        }

        public TributacaoCofins Cofins
        {
            get => GetValue<TributacaoCofins>();
            set
            {
                SetValue(value);
                CalculaImpostoCofins();
            }
        }

        public TributacaoCst Icms
        {
            get => GetValue<TributacaoCst>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public TributacaoIpi Ipi
        {
            get => GetValue<TributacaoIpi>();
            set
            {
                SetValue(value);
                CalculaImpostoIpi();
                CalculaImpostoIcms();
            }
        }

        public ProdutoUnidadeDTO Unidade
        {
            get => GetValue<ProdutoUnidadeDTO>();
            set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaConversao();
                CalculaTotal();
            }
        }

        public decimal ValorUnitario
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaTotal();
            }
        }

        public decimal ValorDescontoTotal
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaTotal();
            }
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IcmsReducaoSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public decimal PercentualFcpSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);

                if (BaseCalculoFcpSt > 0)
                {
                    var valorFcp = decimal.Round(BaseCalculoFcpSt * value / 100, 2);
                    SetValue(valorFcp, nameof(ValorFcpSt));
                }
            }
        }

        public decimal BaseCalculoFcpSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);

                if (PercentualFcpSt > 0)
                {
                    var valorFcp = decimal.Round(value * PercentualFcpSt / 100, 2);
                    SetValue(valorFcp, nameof(ValorFcpSt));
                }
            }
        }

        public decimal ValorFcpSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IcmsMva
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public decimal IcmsBcSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IcmsAliquotaSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public decimal IcmsValorSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IcmsReducao
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public decimal IcmsBc
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IcmsAliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIcms();
            }
        }

        public decimal IcmsValor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IpiBc
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal IpiAliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoIpi();
                CalculaImpostoIcms();
            }
        }

        public decimal IpiValor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PisAliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoPis();
            }
        }

        public decimal PisBc
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PisValor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CofinsAliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostoCofins();
            }
        }

        public decimal CofinsBc
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal CofinsValor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorFrete
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorSeguro
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDespesas
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool BloqueiaAlteracaoEstoque
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public decimal FatorConversao
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaConversao();
            }
        }

        public decimal QuantidadeConversao
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string UnidadeConversao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool ImpostoManual
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        private void CalculaConversao()
        {
            var conversao = decimal.Round(Quantidade * FatorConversao, 4);
            SetValue(conversao, nameof(QuantidadeConversao));
        }

        public NotaFiscalCompra GetNota()
        {
            return _nota;
        }

        public bool IsEdicao()
        {
            return _edicao;
        }

        public void Inicializar()
        {
            InicializarListas();

            _ipiCompoeIcms = true;
            _freteCompoeIcms = true;
            _despesasCompoeIcms = true;
            _seguroCompoeIcms = true;

            if (_item != null)
            {
                _ipiCompoeIcms = _item.IpiCompoeIcms;
                _freteCompoeIcms = _item.FreteCompoeIcms;
                _despesasCompoeIcms = _item.DespesasCompoeIcms;
                _seguroCompoeIcms = _item.SeguroCompoeIcms;

                SetValue(_item.ImpostoManual, nameof(ImpostoManual));

                SetValue(_item.Cfop, nameof(Cfop));
                SetValue(_item.Cfop.Id, nameof(CodigoCfop));
                SetValue(_item.Produto, nameof(Produto));
                SetValue(_item.Produto.Id, nameof(CodigoProduto));
                SetValue(_item.Unidade, nameof(Unidade));
                SetValue(_item.Quantidade, nameof(Quantidade));
                SetValue(_item.ValorUnitario, nameof(ValorUnitario));
                SetValue(_item.ValorDescontoTotal, nameof(ValorDescontoTotal));
                SetValue(_item.ValorTotal, nameof(ValorTotal));

                SetValue(_item.ValorDespesasRateio, nameof(ValorDespesas));
                SetValue(_item.ValorFreteRateio, nameof(ValorFrete));
                SetValue(_item.ValorSeguroRateio, nameof(ValorSeguro));

                SetValue(_item.Ipi.Ipi, nameof(Ipi));
                SetValue(_item.Ipi.Aliquota, nameof(IpiAliquota));
                SetValue(_item.Ipi.BaseCalculo, nameof(IpiBc));
                SetValue(_item.Ipi.ValorIpi, nameof(IpiValor));

                SetValue(_item.Icms.Icms, nameof(Icms));
                SetValue(_item.Icms.Reducao, nameof(IcmsReducao));
                SetValue(_item.Icms.Aliquota, nameof(IcmsAliquota));
                SetValue(_item.Icms.BaseCalculo, nameof(IcmsBc));
                SetValue(_item.Icms.ValorIcms, nameof(IcmsValor));
                SetValue(_item.Icms.ReducaoSt, nameof(IcmsReducaoSt));
                SetValue(_item.Icms.Mva, nameof(IcmsMva));
                SetValue(_item.Icms.AliquotaSt, nameof(IcmsAliquotaSt));
                SetValue(_item.Icms.BaseCalculoSt, nameof(IcmsBcSt));
                SetValue(_item.Icms.ValorSt, nameof(IcmsValorSt));
                SetValue(_item.Icms.ValorFcpSt, nameof(ValorFcpSt));
                SetValue(_item.Icms.BaseCalculoFcpSt, nameof(BaseCalculoFcpSt));
                SetValue(_item.Icms.PercentualFcpSt, nameof(PercentualFcpSt));

                SetValue(_item.Pis.Pis, nameof(Pis));
                SetValue(_item.Pis.Aliquota, nameof(PisAliquota));
                SetValue(_item.Pis.BaseCalculo, nameof(PisBc));
                SetValue(_item.Pis.ValorPis, nameof(PisValor));

                SetValue(_item.Cofins.Cofins, nameof(Cofins));
                SetValue(_item.Cofins.BaseCalculo, nameof(CofinsBc));
                SetValue(_item.Cofins.Aliquota, nameof(CofinsAliquota));
                SetValue(_item.Cofins.ValorCofins, nameof(CofinsValor));

                SetValue(_item.UnidadeConversao, nameof(UnidadeConversao));
                SetValue(_item.FatorConversao, nameof(FatorConversao));
                SetValue(_item.QuantidadeConversao, nameof(QuantidadeConversao));
            }

            BloqueiaAlteracaoEstoque = _item?.Id > 0;
        }

        private void InicializarListas()
        {
            var unidades = _recipienteUnidade.GetTodos();
            var tributacaoCst = _recipienteIcmsNFe.GetTodos();
            var tributacoesIpi = _recipienteIpi.GetPorOperacao(TipoOperacao.Entrada);
            var tributacoesPis = _recipientePis.GetPorOperacao(TipoOperacao.Entrada);
            var tributacoesCofins = _recipienteCofins.GetPorOperacao(TipoOperacao.Entrada);

            Cfops = _cfopFacade.BuscarTodosDeEntrada();

            Unidades = new ObservableCollection<ProdutoUnidadeDTO>(unidades);
            TributacoesIcms = new ObservableCollection<TributacaoCst>(tributacaoCst);
            TributacoesIpi = new ObservableCollection<TributacaoIpi>(tributacoesIpi);
            TributacoesPis = new ObservableCollection<TributacaoPis>(tributacoesPis);
            TributacoesCofins = new ObservableCollection<TributacaoCofins>(tributacoesCofins);
        }

        private void ProdutoPickerAction(object obj)
        {
            var picker = new ProdutoGridPickerModel();
            picker.PickItemEvent += (s, e) => Produto = e.GetItem<ProdutoDTO>();

            var pickerView = picker.GetPickerView();
            pickerView.ShowDialog();
        }

        private void SetIpiData(ProdutoDTO produto)
        {
            SetValue(default(TributacaoIpi), nameof(Ipi));
            SetValue(0M, nameof(IpiAliquota));
            SetValue(0M, nameof(IpiBc));
            SetValue(0M, nameof(IpiValor));

            if (produto != null)
            {
                SetValue(produto.SituacaoTributariaIpi.ToEntrada(), nameof(Ipi));
            }
        }

        private void SetPisData(ProdutoDTO produto)
        {
            SetValue(default(TributacaoPis), nameof(Pis));
            SetValue(0M, nameof(PisAliquota));
            SetValue(0M, nameof(PisBc));
            SetValue(0M, nameof(PisValor));

            if (produto == null)
                return;

            SetValue(_recipientePis.Get("99"), nameof(Pis));
        }

        private void SetCofinsData(ProdutoDTO produto)
        {
            SetValue(default(TributacaoCofins), nameof(Cofins));
            SetValue(0M, nameof(CofinsAliquota));
            SetValue(0M, nameof(CofinsBc));
            SetValue(0M, nameof(CofinsValor));

            if (produto == null)
                return;

            SetValue(_recipienteCofins.Get("99"), nameof(Cofins));
        }

        private void CalculaImpostoIpi()
        {
            if (ImpostoManual)
            {
                return;
            }

            SetValue(0M, nameof(IpiBc));
            SetValue(0M, nameof(IpiValor));

            if (Ipi == null)
                return;

            var bc = ValorTotal;
            var valorIpi = decimal.Round(bc * IpiAliquota/100, 2);

            SetValue(bc, nameof(IpiBc));
            SetValue(valorIpi, nameof(IpiValor));

            CalculaImpostoIcms();
        }

        private void CalculaImpostoIcms()
        {
            if (ImpostoManual)
            {
                return;
            }

            SetValue(0M, nameof(IcmsBc));
            SetValue(0M, nameof(IcmsValor));
            SetValue(0M, nameof(IcmsValorSt));
            SetValue(0M, nameof(IcmsBcSt));

            if (Icms == null)
            {
                return;
            }

            var calcIcms = new CalculadoraIcms
            {
                ValorTributavel = ValorTotal,
                ValorIpi = (_ipiCompoeIcms ? IpiValor : 0),
                ValorFrete = (_freteCompoeIcms ? ValorFrete : 0),
                ValorOutros = (_despesasCompoeIcms ? ValorDespesas : 0),
                ValorSeguro = (_seguroCompoeIcms ? ValorSeguro : 0),
                Aliquota = IcmsAliquota,
                Reducao = IcmsReducao
            };

            var calculoIcms = calcIcms.Calcula();

            SetValue(calculoIcms.Bc, nameof(IcmsBc));
            SetValue(calculoIcms.Valor, nameof(IcmsValor));

            // ST

            var calcSt = new CalculadoraIcmsSt
            {
                ValorTributavel = ValorTotal,
                ValorIpi = (_ipiCompoeIcms ? IpiValor : 0),
                ValorFrete = (_freteCompoeIcms ? ValorFrete : 0),
                ValorOutros = (_despesasCompoeIcms ? ValorDespesas : 0),
                ValorSeguro = (_seguroCompoeIcms ? ValorSeguro : 0),
                Aliquota = IcmsAliquotaSt,
                Reducao = IcmsReducaoSt,
                Mva = IcmsMva
            };

            var calculoSt = calcSt.Calcula();

            SetValue(calculoSt.Bc, nameof(IcmsBcSt));
            SetValue(calculoSt.Valor, nameof(IcmsValorSt));
        }

        private void CalculaImpostoPis()
        {
            if (ImpostoManual)
            {
                return;
            }

            SetValue(0M, nameof(PisValor));
            SetValue(0M, nameof(PisBc));

            if (Pis == null)
            {
                return;
            }

            var bc = ValorTotal;
            var calculo = decimal.Round(bc * PisAliquota / 100, 2);

            SetValue(bc, nameof(PisBc));
            SetValue(calculo, nameof(PisValor));
        }

        private void CalculaImpostoCofins()
        {
            if (ImpostoManual)
            {
                return;
            }

            SetValue(0M, nameof(CofinsValor));
            SetValue(0M, nameof(CofinsBc));

            if (Cofins == null)
                return;

            var calc = new CalculadoraCofins
            {
                Aliquota = CofinsAliquota,
                ValorTributavel = ValorTotal
            };

            var res = calc.Calcula();

            SetValue(res.Bc, nameof(CofinsBc));
            SetValue(res.Valor, nameof(CofinsValor));
        }

        private void CalculaTotal()
        {
            var totalUnitario = Quantidade * ValorUnitario;
            var totalLiquido = totalUnitario - ValorDescontoTotal;

            SetValue(decimal.Round(totalLiquido, 2), nameof(ValorTotal));
            CalculaTodosImpostos();
        }

        private void CalculaTodosImpostos()
        {
            CalculaImpostoIpi();
            CalculaImpostoIcms();
            CalculaImpostoPis();
            CalculaImpostoCofins();
        }

        public void CarregaCfopPeloCodigo()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CodigoCfop))
                {
                    Cfop = null;
                    return;
                }

                if (CodigoCfop == Cfop?.Id || CodigoCfop == _ultimoCfopBuscado)
                {
                    return;
                }

                Cfop = Cfops.FirstOrDefault(i => i.Id == CodigoCfop);

                if (Cfop == null)
                {
                    throw new RegraNegocioException("Não encontrei CFOP para esse código");
                }

                SetValue(Cfop.Id, nameof(CodigoCfop));
            }
            finally
            {
                _ultimoCfopBuscado = CodigoCfop;
            }
        }

        public void CarregaProdutoPeloCodigo()
        {
            try
            {
                if (CodigoProduto == null)
                {
                    Produto = null;
                    return;
                }

                if (CodigoProduto == Produto?.Id || _ultimoCodigoProdutoBuscado == CodigoProduto)
                {
                    return;
                }

                using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioProduto(sessao);
                    var produto = repositorio.GetPeloId((int) CodigoProduto);

                    produto?.NaoAtivoThrowInvalidOperation();

                    Produto = produto;
                }

                if (Produto == null)
                {
                    CodigoProduto = null;
                    throw new RegraNegocioException("Não encontrei Produto para esse código");
                }
            }
            finally
            {
                _ultimoCodigoProdutoBuscado = CodigoProduto;
            }
        }

        public void SalvarAlteracoes()
        {
            ValidarFormulario();

            if (_item == null)
            {
                _item = new ItemCompra(_nota);
                _nota.Adicionar(_item);
            }

            if (_item.Id == 0)
            {
                _item.Produto = Produto;
                _item.Quantidade = Quantidade;
            }

            _item.SetCfop(Cfop);
            _item.IpiCompoeIcms = _ipiCompoeIcms;
            _item.FreteCompoeIcms = _freteCompoeIcms;
            _item.SeguroCompoeIcms = _seguroCompoeIcms;
            _item.DespesasCompoeIcms = _despesasCompoeIcms;
            _item.ImpostoManual = ImpostoManual;

            _item.Cest = Produto.Cest;
            _item.Ncm = LoadNcm(Produto);
            _item.ValorTotal = ValorTotal;
            _item.Unidade = Unidade;
            _item.ValorUnitario = ValorUnitario;
            _item.ValorDescontoTotal = ValorDescontoTotal;
            _item.ValorDespesasRateio = ValorDespesas;
            _item.ValorFreteRateio = ValorFrete;
            _item.ValorSeguroRateio = ValorSeguro;

            _item.SetConversao(new ConversaoUnidade(FatorConversao, QuantidadeConversao, UnidadeConversao));

            _item.Ipi = new IpiCompra(_item)
            {
                Ipi = Ipi,
                Aliquota = IpiAliquota,
                BaseCalculo = IpiBc,
                ValorIpi = IpiValor
            };

            _item.Pis = new PisCompra(_item)
            {
                Pis = Pis,
                Aliquota = PisAliquota,
                BaseCalculo = PisBc,
                ValorPis = PisValor
            };

            _item.Cofins = new CofinsCompra(_item)
            {
                Cofins = Cofins,
                Aliquota = CofinsAliquota,
                BaseCalculo = CofinsBc,
                ValorCofins = CofinsValor
            };

            _item.Icms = new IcmsCompra(_item)
            {
                Icms = Icms,
                Reducao = IcmsReducao,
                Aliquota = IcmsAliquota,
                BaseCalculo = IcmsBc,
                Mva = IcmsMva,
                AliquotaSt = IcmsAliquotaSt,
                BaseCalculoSt = IcmsBcSt,
                ValorIcms = IcmsValor,
                ValorSt = IcmsValorSt,
                PercentualFcpSt = PercentualFcpSt,
                ValorFcpSt = ValorFcpSt,
                BaseCalculoFcpSt = BaseCalculoFcpSt
            };

            _nota.CalculaTotais();

            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repo = new RepositorioNotaFiscalCompra(sessao);
                repo.Salvar(_nota);

                sessao.Transaction.Commit();
            }
        }

        private void ValidarFormulario()
        {
            if (Cfop == null)
                throw new RegraNegocioException("Preciso que informe um CFOP");

            if (Produto == null)
                throw new RegraNegocioException("Preciso que informe um Produto");

            if (ValorTotal <= 0)
                throw new RegraNegocioException("Preciso que o Valor Total seja maior que ZERO (0,00)");

            if (QuantidadeConversao <= 0)
                throw new RegraNegocioException("Preciso que a conversão seja mairo que 0,00");

            if (Ipi == null)
                throw new RegraNegocioException("Preciso que escolha um IPI");

            if (Icms == null)
                throw new RegraNegocioException("Preciso que escolha um ICMS");

            if (Pis == null)
                throw new RegraNegocioException("Preciso que escolha um PIS");

            if (Cofins == null)
                throw new RegraNegocioException("Preciso que escolha um COFINS");
        }

        private NcmDTO LoadNcm(ProdutoDTO produto)
        {
            using (var sessao = _sessaoSistema.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioNcm(sessao);
                return repositorio.GetPeloId(produto.Ncm);
            }
        }

        public ConfiguracaoRegraCalculoItemModel CriaRegraModel()
        {
            var model = new ConfiguracaoRegraCalculoItemModel
            {
                DespesasCompoeIcms = _despesasCompoeIcms,
                FreteCompoeIcms = _freteCompoeIcms,
                IpiCompoeIcms = _ipiCompoeIcms,
                SeguroCompoeIcms = _seguroCompoeIcms,
                ImpostoManual = ImpostoManual
            };

            model.ConfirmouAlteracoes += AplicaRegraCalculo;

            return model;
        }

        private void AplicaRegraCalculo(object sender, ConfiguracaoRegraCalculoItemModel e)
        {
            _freteCompoeIcms = e.FreteCompoeIcms;
            _despesasCompoeIcms = e.DespesasCompoeIcms;
            _seguroCompoeIcms = e.SeguroCompoeIcms;
            _ipiCompoeIcms = e.IpiCompoeIcms;
            ImpostoManual = e.ImpostoManual;

            CalculaTodosImpostos();
        }
    }
}