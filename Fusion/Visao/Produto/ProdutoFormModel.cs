using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.Ncm;
using Fusion.Visao.Produto.Historico;
using Fusion.Visao.Produto.Models;
using Fusion.Visao.ProdutoLocalizacoes;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoGrupo;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ProdutoUnidade;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Federal;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate;
using NHibernate.Util;
using static System.Decimal;

// ReSharper disable ExplicitCallerInfoArgument

namespace Fusion.Visao.Produto
{
    internal class ProdutoFormModel : ViewModel
    {
        private readonly int _produtoId;
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private bool _naoDesativarCampoCodigoBarras;
        private ProdutoDTO _produto;
        private Balanca _balanca;
        private FlyoutCodigoBarraModel _flyoutCodigoBarraModel;
        private ProdutoAlias _produtoAliasSelecionado;
        private readonly RecipienteCofins _recipienteCofins = RecipienteFactory.Get<RecipienteCofins>();
        private readonly RecipientePis _recipientePis = RecipienteFactory.Get<RecipientePis>();

        private readonly RecipienteEnquadramentoIpi _recipienteEnquadramentoIpi =
            RecipienteFactory.Get<RecipienteEnquadramentoIpi>();

        private bool _isIndicadorEscalaRelevante;
        private string _cnpjFabricante;
        private ObservableCollection<EquadramentoIpi> _enquadramentosIpi;
        private EquadramentoIpi _enquadramentoIpi;
        private bool _isControlaIndicadorEscala;
        private decimal _percentualGlpPetroleo;
        private decimal _percentualGasNacional;
        private decimal _percentualGasImportador;
        private decimal _valorDePartida;
        private string _codigoDfe;
        private bool _isGerenciarProdutoUnidade;
        private bool _isGerenciarProdutoLocalizacao;
        private bool _isGerenciarProdutoGrupo;
        private bool _isGerenciarProdutoRegra;
        private bool _isProdutoInserirOuAlterar;
        private decimal _estoqueMinimo;
        private decimal _estoqueMaximo;
        private ProdutoEstoqueDTO _produtoEstoque;
        private decimal _quantidadeUnidadeTributavel;
        private bool _usarObservacaoNoItemFiscal;

        public ProdutoFormModel()
        {
            IpiModel = new ProdutoIpiModel();
            IcmsModel = new ProdutoIcmsModel(_sessaoManager);
            NaoDesativarCampoCodigoBarras = true;
            ListaDeAlias = new ObservableCollection<ProdutoAlias>();
            TabelaPrecos = new ObservableCollection<ProdutoTabelaPrecoViewModel>();

            PropriedadeAlterada(nameof(NovoRegistro));
        }

        public ProdutoFormModel(int produtoId) : this()
        {
            _produtoId = produtoId;
            PropriedadeAlterada(nameof(NovoRegistro));
        }

        public ProdutoFormModel(ProdutoDTO produto = null) : this()
        {
            _produto = produto ?? new ProdutoDTO();
            _produtoId = produto?.Id ?? 0;

            PropriedadeAlterada(nameof(NovoRegistro));
        }

        public ObservableCollection<ProdutoGrupoDTO> ListaDeGrupos { get; } =
            new ObservableCollection<ProdutoGrupoDTO>();

        public ObservableCollection<ProdutoUnidadeDTO> ListaDeUnidades { get; } =
            new ObservableCollection<ProdutoUnidadeDTO>();

        public ObservableCollection<ProdutoUnidadeDTO> ListaDeUnidadeTributavel { get; } =
            new ObservableCollection<ProdutoUnidadeDTO>();

        public ObservableCollection<RegraTribuacaoInfo> RegrasPorEstado { get; } =
            new ObservableCollection<RegraTribuacaoInfo>();


        public ICommand CommandBuscaLocalizacao => GetSimpleCommand(BuscaLocalizacao);
        public ICommand CommandBuscaNcm => GetSimpleCommand(BuscaNcmAction);

        public ICommand CommandClearLocalizacao => GetSimpleCommand(o =>
        {
            Localizacao = null;
            LocalizacaoNome = string.Empty;
        });

        public FlyoutRegraTributacaoModel RegraTributacaoModel
        {
            get => GetValue<FlyoutRegraTributacaoModel>();
            set => SetValue(value);
        }

        public string LocalizacaoNome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Observacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ProdutoLocalizacao Localizacao
        {
            get => GetValue<ProdutoLocalizacao>();
            set => SetValue(value);
        }

        public string Referencia
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TributacaoPis TributacaoPis
        {
            get => GetValue<TributacaoPis>();
            set => SetValue(value);
        }

        public TributacaoCofins TributacaoCofins
        {
            get => GetValue<TributacaoCofins>();
            set => SetValue(value);
        }

        public decimal AliquotaPis
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliquotaCofins
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal EstoqueInicial
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool NovoRegistro => _produto?.IsNovo ?? true;

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal PrecoCompra
        {
            get => GetValue<decimal>();
            set
            {
                if (PrecoCusto <= 0 || PrecoCusto == PrecoCompra)
                {
                    PrecoCusto = value;
                }

                SetValue(value);
                AtualizarPrecoTabelaPreco();
            }
        }

        public decimal PrecoCusto
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularPrecoVendaComMargemLucro();
                AtualizarPrecoTabelaPreco();
            }
        }

        public decimal PrecoVenda
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularMargemLucroComBasePrecoVenda();
                AtualizarPrecoTabelaPreco();
            }
        }

        public decimal MargemLucro
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalcularPrecoVendaComMargemLucro();
                AtualizarPrecoTabelaPreco();
            }
        }

        public bool Ativo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public decimal AliquotaIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public ProdutoUnidadeDTO ProdutoUnidade
        {
            get => GetValue<ProdutoUnidadeDTO>();
            set => SetValue(value);
        }

        public ProdutoUnidadeDTO ProdutoUnidadeTributavel
        {
            get => GetValue<ProdutoUnidadeDTO>();
            set => SetValue(value);
        }

        public ProdutoGrupoDTO ProdutoGrupo
        {
            get => GetValue<ProdutoGrupoDTO>();
            set => SetValue(value);
        }

        public string CodigoNcm
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string CodigoCest
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public OrigemMercadoria OrigemMercadoria
        {
            get => GetValue<OrigemMercadoria>();
            set => SetValue(value);
        }

        public ProdutoIcmsModel IcmsModel
        {
            get => GetValue<ProdutoIcmsModel>();
            set => SetValue(value);
        }

        public int CodigoBalanca
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool NaoDesativarCampoCodigoBarras
        {
            get => _naoDesativarCampoCodigoBarras;
            set
            {
                if (value == _naoDesativarCampoCodigoBarras) return;
                _naoDesativarCampoCodigoBarras = value;
                PropriedadeAlterada();
            }
        }

        public RegraTribuacaoInfo RegraEstadoSelecionada
        {
            get => GetValue<RegraTribuacaoInfo>();
            set => SetValue(value);
        }

        public ProdutoIpiModel IpiModel
        {
            get => GetValue<ProdutoIpiModel>();
            set => SetValue(value);
        }

        public ObservableCollection<ProdutoAlias> ListaDeAlias
        {
            get => GetValue<ObservableCollection<ProdutoAlias>>();
            set => SetValue(value);
        }

        public ObservableCollection<ProdutoTabelaPrecoViewModel> TabelaPrecos
        {
            get => GetValue<ObservableCollection<ProdutoTabelaPrecoViewModel>>();
            set => SetValue(value);
        }

        public ProdutoTabelaPrecoViewModel TabelaPrecoSelecionada
        {
            get => GetValue<ProdutoTabelaPrecoViewModel>();
            set => SetValue(value);
        }

        public FlyoutCodigoBarraModel FlyoutCodigoBarraModel
        {
            get => _flyoutCodigoBarraModel;
            set
            {
                if (Equals(value, _flyoutCodigoBarraModel)) return;
                _flyoutCodigoBarraModel = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoAlias ProdutoAliasSelecionado
        {
            get => _produtoAliasSelecionado;
            set
            {
                if (Equals(value, _produtoAliasSelecionado)) return;
                _produtoAliasSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<TributacaoCofins> TributacoesCofins
        {
            get => GetValue<ObservableCollection<TributacaoCofins>>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoPis> TributacoesPis
        {
            get => GetValue<ObservableCollection<TributacaoPis>>();
            set => SetValue(value);
        }

        public string CodigoAnp
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public HistoricoCompraModel HistorioCompraModel
        {
            get => GetValue<HistoricoCompraModel>();
            set => SetValue(value);
        }

        public bool IsIndicadorEscalaRelevante
        {
            get => _isIndicadorEscalaRelevante;
            set
            {
                if (value == _isIndicadorEscalaRelevante) return;

                _isIndicadorEscalaRelevante = value;
                if (value) CnpjFabricante = string.Empty;

                PropriedadeAlterada();
            }
        }

        public string CnpjFabricante
        {
            get => _cnpjFabricante;
            set
            {
                if (value == _cnpjFabricante) return;
                _cnpjFabricante = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EquadramentoIpi> EnquadramentosIpi
        {
            get => _enquadramentosIpi;
            set
            {
                if (Equals(value, _enquadramentosIpi)) return;
                _enquadramentosIpi = value;
                PropriedadeAlterada();
            }
        }

        public EquadramentoIpi EnquadramentoIpi
        {
            get => _enquadramentoIpi;
            set
            {
                if (Equals(value, _enquadramentoIpi)) return;
                _enquadramentoIpi = value;
                PropriedadeAlterada();
            }
        }

        public bool IsControlaIndicadorEscala
        {
            get => _isControlaIndicadorEscala;
            set
            {
                if (value == _isControlaIndicadorEscala) return;
                _isControlaIndicadorEscala = value;

                if (value == false)
                    IsIndicadorEscalaRelevante = true;

                PropriedadeAlterada();
            }
        }

        public decimal PercentualGlpPetroleo
        {
            get => _percentualGlpPetroleo;
            set
            {
                if (value == _percentualGlpPetroleo) return;
                _percentualGlpPetroleo = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualGasNacional
        {
            get => _percentualGasNacional;
            set
            {
                if (value == _percentualGasNacional) return;
                _percentualGasNacional = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualGasImportador
        {
            get => _percentualGasImportador;
            set
            {
                if (value == _percentualGasImportador) return;
                _percentualGasImportador = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorDePartida
        {
            get => _valorDePartida;
            set
            {
                if (value == _valorDePartida) return;
                _valorDePartida = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoDfe
        {
            get => _codigoDfe;
            set
            {
                if (value == _codigoDfe) return;
                _codigoDfe = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<ProdutoDTO> RegistroSalvo;
        internal event EventHandler CloseRequest;

        private void CalcularMargemLucroComBasePrecoVenda()
        {
            try
            {
                var margemLucro = PrecoVenda * 100 / PrecoCusto - 100;
                SetValue(Round(margemLucro, 6), nameof(MargemLucro));
            }
            catch (DivideByZeroException)
            {
                MargemLucro = 0;
            }
        }

        private void CalcularPrecoVendaComMargemLucro()
        {
            if (MargemLucro == 0)
            {
                return;
            }

            var fator = 1 + (MargemLucro / 100);
            var valor = PrecoCusto * fator;
            SetValue(valor, nameof(PrecoVenda));
        }

        public void AtualizarPrecoTabelaPreco()
        {
            foreach (var tabela in TabelaPrecos)
            {
                tabela.CalcularPreco(_produtoId, PrecoVenda);
            }
        }

        private void BuscaLocalizacao(object obj)
        {
            var pickerModel = new ProdutoLocalizacaoGridPickerModel();
            pickerModel.PickItemEvent += ProdutoLocalizacaoSelecionado;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void ProdutoLocalizacaoSelecionado(object sender, GridPickerEventArgs e)
        {
            Localizacao = e.GetItem<ProdutoLocalizacao>();
            LocalizacaoNome = Localizacao?.Nome;
        }

        public void Inicializar()
        {
            TributacoesCofins =
                new ObservableCollection<TributacaoCofins>(_recipienteCofins.GetPorOperacao(TipoOperacao.Saida));
            TributacoesPis = new ObservableCollection<TributacaoPis>(_recipientePis.GetPorOperacao(TipoOperacao.Saida));
            EnquadramentosIpi = new ObservableCollection<EquadramentoIpi>(_recipienteEnquadramentoIpi.GetTodos());

            IcmsModel.Inicializa();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                InicializarProduto(sessao);
                CarregarListaUnidade(sessao);
                CarregarListaUnidadeTributavel(sessao);
                CarregarListaGrupo(sessao);
                PreencherViewModel(sessao);
                LoadTabelaPreco(sessao);
            }

            if (EnquadramentoIpi == null)
            {
                EnquadramentoIpi = EnquadramentosIpi.First(e => e.Id == "999");
            }

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsGerenciarProdutoUnidade =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_UNIDADE);
            IsGerenciarProdutoLocalizacao =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_LOCALIZACAO);
            IsGerenciarProdutoGrupo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_GRUPO);
            IsGerenciarProdutoRegra = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_REGRA_SAIDA);
        }

        public bool IsGerenciarProdutoRegra
        {
            get => _isGerenciarProdutoRegra;
            set
            {
                if (value == _isGerenciarProdutoRegra) return;
                _isGerenciarProdutoRegra = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoGrupo
        {
            get => _isGerenciarProdutoGrupo;
            set
            {
                if (value == _isGerenciarProdutoGrupo) return;
                _isGerenciarProdutoGrupo = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoLocalizacao
        {
            get => _isGerenciarProdutoLocalizacao;
            set
            {
                if (value == _isGerenciarProdutoLocalizacao) return;
                _isGerenciarProdutoLocalizacao = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoUnidade
        {
            get => _isGerenciarProdutoUnidade;
            set
            {
                if (value == _isGerenciarProdutoUnidade) return;
                _isGerenciarProdutoUnidade = value;
                PropriedadeAlterada();
            }
        }

        private void InicializarProduto(ISession sessao)
        {
            if (_produto == null)
            {
                _produto = new ProdutoDTO();
            }

            var repositorio = new RepositorioProduto(sessao);

            if (_produtoId != 0)
            {
                _produto = repositorio.GetPeloId(_produtoId);
                _produtoEstoque = repositorio.GetEstoquePeloId(_produtoId);
                PropriedadeAlterada(nameof(NovoRegistro));
                return;
            }

            if (_produto.Id != 0)
            {
                repositorio.Refresh(_produto);
                PropriedadeAlterada(nameof(NovoRegistro));
            }
        }

        private void CarregarListaUnidade(ISession sessao)
        {
            var repostiorio = new RepositorioComun<ProdutoUnidadeDTO>(sessao);
            var unidades = repostiorio.Busca(new TodasUnidades());

            ListaDeUnidades.Clear();
            unidades?.ForEach(ListaDeUnidades.Add);
        }

        private void CarregarListaUnidadeTributavel(ISession sessao)
        {
            var repostiorio = new RepositorioComun<ProdutoUnidadeDTO>(sessao);
            var unidades = repostiorio.Busca(new TodasUnidades());

            ListaDeUnidadeTributavel.Clear();
            unidades?.ForEach(ListaDeUnidadeTributavel.Add);
        }

        private void CarregarListaGrupo(ISession sessao)
        {
            var repositorio = new RepositorioComun<ProdutoGrupoDTO>(sessao);
            var grupos = repositorio.Busca(new TodosGrupos());

            ListaDeGrupos.Clear();
            grupos?.ForEach(ListaDeGrupos.Add);
        }

        private void PreencherViewModel(ISession sessao)
        {
            if (_produto == null)
            {
                _produto = new ProdutoDTO();
                PropriedadeAlterada(nameof(NovoRegistro));
            }

            Nome = _produto.Nome?.TrimOrEmpty();
            ProdutoGrupo = _produto.ProdutoGrupoDTO;
            ProdutoUnidade = _produto.ProdutoUnidadeDTO;
            ProdutoUnidadeTributavel = _produto.ProdutoUnidadeTributavel;
            Localizacao = _produto.Localizacao;
            Ativo = _produto.Id == 0 || _produto.Ativo;
            CodigoNcm = _produto.Ncm;
            AliquotaIcms = _produto.AliquotaIcms;
            TributacaoPis = _produto.Pis;
            AliquotaPis = _produto.AliquotaPis;
            TributacaoCofins = _produto.Cofins;
            AliquotaCofins = _produto.AliquotaCofins;
            OrigemMercadoria = _produto.OrigemMercadoria;
            Referencia = _produto.ReferenciaInterna;
            Observacao = _produto.Observacao;
            LocalizacaoNome = _produto.Localizacao?.Nome;
            CodigoBalanca = _produto.CodigoBalanca;
            CodigoCest = _produto.Cest;
            CodigoAnp = _produto.CodigoAnp;
            IsIndicadorEscalaRelevante = _produto.IsIndicadorEscalaRelevante;
            CnpjFabricante = _produto.CnpjFabricante.TrimOrEmpty();
            EnquadramentoIpi = _produto.EnquadramentoIpi;
            IsControlaIndicadorEscala = _produto.IsControlaIndicadorEscala;
            PercentualGlpPetroleo = _produto.PercentualGlpPetroleo;
            PercentualGasNacional = _produto.PercentualGasNacional;
            PercentualGasImportador = _produto.PercentualGasImportador;
            ValorDePartida = _produto.ValorDePartida;
            CodigoDfe = _produto.CodigoDfe;
            QuantidadeUnidadeTributavel = _produto.QuantidadeUnidadeTributavel;
            UsarObservacaoNoItemFiscal = _produto.UsarObservacaoNoItemFiscal;

            if (_produtoEstoque != null)
            {
                EstoqueMinimo = _produtoEstoque.EstoqueMinimo;
                EstoqueMaximo = _produtoEstoque.EstoqueMaximo;
            }

            IcmsModel.PreencherComProduto(_produto);
            IpiModel.PreencherComProduto(_produto);

            if (TributacaoCofins == null)
            {
                TributacaoCofins = TributacoesCofins.FirstOrDefault(c => c.Id == "49");
            }

            if (TributacaoPis == null)
            {
                TributacaoPis = TributacoesPis.FirstOrDefault(p => p.Id == "49");
            }

            SetValue(_produto.MargemLucro, nameof(MargemLucro));
            SetValue(_produto.PrecoCompra, nameof(PrecoCompra));
            SetValue(_produto.PrecoCusto, nameof(PrecoCusto));
            SetValue(_produto.PrecoVenda, nameof(PrecoVenda));

            ListaDeAlias = new ObservableCollection<ProdutoAlias>(_produto.ProdutosAlias);
            ListarRegrasInterstaduais(_produto.RegrasInterstaduais);
            AtualizaConfiguracoesBalanca(sessao);

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsProdutoInserirOuAlterar =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PRODUTO_INSERIR_ALTERAR);
        }

        public bool IsProdutoInserirOuAlterar
        {
            get => _isProdutoInserirOuAlterar;
            set
            {
                if (value == _isProdutoInserirOuAlterar) return;
                _isProdutoInserirOuAlterar = value;
                PropriedadeAlterada();
            }
        }

        public decimal EstoqueMinimo
        {
            get => _estoqueMinimo;
            set
            {
                if (value == _estoqueMinimo) return;
                _estoqueMinimo = value;
                PropriedadeAlterada();
            }
        }

        public decimal EstoqueMaximo
        {
            get => _estoqueMaximo;
            set
            {
                if (value == _estoqueMaximo) return;
                _estoqueMaximo = value;
                PropriedadeAlterada();
            }
        }

        public decimal QuantidadeUnidadeTributavel
        {
            get => _quantidadeUnidadeTributavel;
            set
            {
                _quantidadeUnidadeTributavel = value;
                PropriedadeAlterada();
            }
        }

        public bool UsarObservacaoNoItemFiscal
        {
            get => _usarObservacaoNoItemFiscal;
            set
            {
                if (value == _usarObservacaoNoItemFiscal) return;
                _usarObservacaoNoItemFiscal = value;
                PropriedadeAlterada();
            }
        }

        private void ListarRegrasInterstaduais(IEnumerable<ProdutoRegraTributacao> regrasInterstaduais)
        {
            RegrasPorEstado.Clear();

            regrasInterstaduais.ForEach(r =>
            {
                var info = new RegraTribuacaoInfo(r);
                RegrasPorEstado.Add(info);
            });
        }

        public void AtualizarListaUnidade()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                CarregarListaUnidade(sessao);
            }
        }

        public void AtualizarListaUnidadeTributavel()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                CarregarListaUnidadeTributavel(sessao);
            }
        }

        public void AtualizarListaGrupo()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                CarregarListaGrupo(sessao);
            }
        }

        private void AtualizaConfiguracoesBalanca(ISession sessao)
        {
            _balanca = new RepositorioBalanca(sessao).BuscarUnicaBalanca();
        }

        public void SalvarModel()
        {
            ThrowExeptionSeDadosInvalido();
            ThrowExceptionSeNcmInvalido();
            ThrowExceptionCodigoAnpInvalido();
            ThrowExceptionSeExisteAliasNaoPermitido();
            UpdateObjetoProduto();

            var isNovo = NovoRegistro;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioProduto(sessao);


                if (isNovo)
                {
                    repositorio.Salvar(_produto);
                    CriarEstoqueInicial(sessao);

                    RegrasPorEstado.ForEach(r =>
                    {
                        var regra = r.CriaRegra(_produto);
                        r.RegraVinculada = regra;

                        repositorio.Persistir(regra);
                    });
                }
                else
                {
                    var estoque = repositorio.GetEstoquePeloId(_produtoId);
                    estoque.EstoqueMinimo = EstoqueMinimo.Arredonda(4);
                    estoque.EstoqueMaximo = EstoqueMaximo.Arredonda(4);
                    estoque.ProdutoDTO = _produto;
                    repositorio.Salvar(estoque);
                    repositorio.Salvar(_produto);
                }

                transacao.Commit();
                PropriedadeAlterada(nameof(NovoRegistro));
            }

            DialogBox.MostraInformacao("Tudo Ok! Salvei o produto para você!");

            RegistroSalvo?.Invoke(this, _produto);
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        private void ThrowExceptionCodigoAnpInvalido()
        {
            var codigoAnp = CodigoAnp.TrimOrEmpty();

            if (codigoAnp.IsNullOrEmpty()) return;

            if (codigoAnp.Length != 9)
            {
                throw new InvalidOperationException("Código ANP deve ter 9 digítos");
            }

            var existeProdutoCodigoAnp = BuscaProdutoCodigoAnp(codigoAnp);

            if (existeProdutoCodigoAnp == false)
            {
                throw new InvalidOperationException("Preciso que informe um código anp válido");
            }
        }

        private static bool BuscaProdutoCodigoAnp(string id)
        {
            using (var repositorioProduto = new RepositorioProduto(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                return repositorioProduto.ExisteProdutoCodigoAnp(id);
            }
        }

        private void ThrowExeptionSeDadosInvalido()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new InvalidOperationException("Preciso que informe um nome");

            if (Nome.Length > 100)
                throw new InvalidOperationException("Nome do produto não pode ter mais que 100 caracteres");

            if (ProdutoGrupo == null)
                throw new InvalidOperationException("Preciso que informe um grupo");

            if (ProdutoUnidade == null)
                throw new InvalidOperationException("Preciso que informe uma unidade");

            if (PrecoVenda < 0)
                throw new InvalidOperationException("Não posso salvar o preço de venda negativo");

            if (PrecoVenda == 0)
                throw new InvalidOperationException("Não posso salvar o preço de venda zerado");

            if (PrecoCompra < 0)
                throw new InvalidOperationException("Não posso salvar o preço de compra negativo");

            if (MargemLucro < 0)
                throw new InvalidOperationException("Não posso salvar a margem de lucro negativa");

            if (EstoqueMinimo < 0)
                throw new InvalidOperationException("Não posso salvar o estoque mínimo negativo");

            if (EstoqueMaximo < 0)
                throw new InvalidOperationException("Não posso salvar o estoque máximo negativo");

            if (string.IsNullOrEmpty(CodigoNcm))
                throw new InvalidOperationException("Preciso que informe um NCM");

            if (IcmsModel.RegraSelecionada == null)
                throw new InvalidOperationException("Preciso que selecione uma regra de tributação");

            if (TributacaoPis == null)
                throw new InvalidOperationException("Preciso que informe um PIS");

            if (TributacaoCofins == null)
                throw new InvalidOperationException("Preciso que informe um COFINS");

            if (TributacaoCofins.Id != TributacaoPis.Id)
                throw new InvalidOperationException("Preciso que o PIS e COFINS sejam iguais");

            if (IpiModel.TributacaoIpi == null)
                throw new InvalidOperationException("Preciso que informe um IPI");

            if (IpiModel.Aliquota < 0 || IpiModel.Aliquota > 100)
                throw new InvalidOperationException("Preciso que a alíquota IPI esteja entre 0% e 100%");

            if (CodigoBalanca.ToString().Length > _balanca.TamanhoCodigo)
                throw new InvalidOperationException(
                    "Quantidade máxima que aceito de digitos no código de balança é " + _balanca.TamanhoCodigo);

            if (EnquadramentoIpi == null)
                throw new InvalidOperationException("Preciso que informa um Enquadramento IPI");

            if (IsIndicadorEscalaRelevante == false && CnpjFabricante.IsNullOrEmpty())
                throw new InvalidOperationException(
                    "Quando indicador escala relevante for não, deve informar o cnpj do fabricante");

            if (QuantidadeUnidadeTributavel <= 0 && ProdutoUnidadeTributavel != null)
                throw new InvalidOperationException("Quantidade tributável deve ser maior que 0");

            if (CnpjFabricante.IsNullOrEmpty()) return;

            if (new CnpjRegra().NaoValido(CnpjFabricante))
            {
                throw new InvalidOperationException("CNPJ do fabricante é invalido: " + CnpjFabricante);
            }
        }

        private void ThrowExceptionSeNcmInvalido()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var ncmRepositorio = new RepositorioComun<NcmDTO>(sessao);
                var ncmCompativel = ncmRepositorio.Busca(new NcmPeloCodigo(CodigoNcm));

                if (ncmCompativel == null)
                    throw new InvalidOperationException(
                        "Não consegui utilizar esse NCM, utilize o seletor para conseguir um válido");

                var tamanhoNcm = ncmCompativel.Id.Length;

                if (tamanhoNcm != 8)
                    throw new InvalidOperationException("Selecione um Ncm com 8 digítos");

                CodigoNcm = ncmCompativel.Id;
                CodigoCest = ncmCompativel.Cest.TrimOrEmpty();
            }
        }

        private void UpdateObjetoProduto()
        {
            _produto.Nome = Nome.TrimOrEmpty();
            _produto.PrecoCompra = Round(PrecoCompra, 4);
            _produto.PrecoCusto = Round(PrecoCusto, 4);
            _produto.PrecoVenda = Round(PrecoVenda, 4);
            _produto.Ativo = NovoRegistro || Ativo;
            _produto.ProdutoGrupoDTO = ProdutoGrupo;
            _produto.ProdutoUnidadeDTO = ProdutoUnidade;
            _produto.ProdutoUnidadeTributavel = ProdutoUnidadeTributavel;
            _produto.Localizacao = Localizacao;
            _produto.AlteradoEm = DateTime.Now;
            _produto.Ncm = CodigoNcm;
            _produto.RegraTributacaoSaida = IcmsModel.RegraSelecionada.GetRegra(_sessaoManager);
            _produto.PercentualMva = IcmsModel.Mva;
            _produto.AliquotaIcms = IcmsModel.Aliquota;
            _produto.ReducaoIcms = IcmsModel.Reducao;
            _produto.Cofins = TributacaoCofins;
            _produto.AliquotaCofins = Round(AliquotaCofins, 2);
            _produto.SituacaoTributariaIpi = IpiModel.TributacaoIpi;
            _produto.AliquotaIpi = IpiModel.Aliquota;
            _produto.Pis = TributacaoPis;
            _produto.AliquotaPis = Round(AliquotaPis, 2);
            _produto.OrigemMercadoria = OrigemMercadoria;
            _produto.ReferenciaInterna = Referencia.TrimOrEmpty();
            _produto.Observacao = Observacao ?? string.Empty;
            _produto.MargemLucro = MargemLucro;
            _produto.CodigoBalanca = CodigoBalanca;
            _produto.Cest = CodigoCest ?? string.Empty;
            _produto.CodigoAnp = CodigoAnp ?? string.Empty;
            _produto.IsControlaIndicadorEscala = IsControlaIndicadorEscala;
            _produto.IsIndicadorEscalaRelevante = IsIndicadorEscalaRelevante;
            _produto.CnpjFabricante = CnpjFabricante.TrimOrEmpty();
            _produto.EnquadramentoIpi = EnquadramentoIpi;
            _produto.ProdutosAlias = ListaDeAlias;
            _produto.PercentualGlpPetroleo = PercentualGlpPetroleo.Arredonda(4);
            _produto.PercentualGasNacional = PercentualGasNacional.Arredonda(4);
            _produto.PercentualGasImportador = PercentualGasImportador.Arredonda(4);
            _produto.ValorDePartida = ValorDePartida.Arredonda(2);
            _produto.CodigoDfe = CodigoDfe.TrimOrEmpty();
            _produto.QuantidadeUnidadeTributavel = QuantidadeUnidadeTributavel.Arredonda(4);
            _produto.UsarObservacaoNoItemFiscal = UsarObservacaoNoItemFiscal;
        }

        private void CriarEstoqueInicial(ISession sessao)
        {
            var estoqueServico = EstoqueServicoAdmFactory.Cria(sessao);

            var estoqueModel = new EstoqueModel
            {
                Produto = _produto,
                Quantidade = EstoqueInicial.Arredonda(4),
                OrigemEvento = OrigemEventoEstoque.SaldoInicial,
                Usuario = _sessaoSistema.UsuarioLogado,
                EstoqueMinimo = EstoqueMinimo.Arredonda(4),
                EstoqueMaximo = EstoqueMaximo.Arredonda(4)
            };

            estoqueServico.CriarEstoque(estoqueModel);
        }

        private void BuscaNcmAction(object obj)
        {
            var pickerModel = new NcmGridPickerModel();
            pickerModel.PickItemEvent += PickNcmHandler;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void PickNcmHandler(object sender, GridPickerEventArgs e)
        {
            var ncm = e.GetItem<NcmDTO>();

            if (ncm != null)
            {
                CodigoNcm = ncm.Id;
                CodigoCest = ncm.Cest;
            }
        }

        public void NovaRegraTributacao()
        {
            RegraTributacaoModel = new FlyoutRegraTributacaoModel { IsOpen = true };
            RegraTributacaoModel.RegraAdicionada += RegraTributacaoAdicionadaHandler;
        }

        private void RegraTributacaoAdicionadaHandler(object sender, RegraTribuacaoInfo e)
        {
            if (NovoRegistro)
            {
                var compativel = RegrasPorEstado.Where(r => r.Uf == e.Uf).FirstOrNull();

                RegrasPorEstado.Remove((RegraTribuacaoInfo)compativel);
                RegrasPorEstado.Add(e);

                RegraTributacaoModel.IsOpen = false;
                return;
            }

            var regra = e.CriaRegra(_produto);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);

                if (repositorio.RegraJaExiste(regra))
                {
                    DialogBox.MostraInformacao("Regra para UF já está cadastrada");
                    return;
                }

                repositorio.Persistir(regra);
                ListarRegrasInterstaduais(repositorio.BuscaRegrasPorProduto(_produto));
            }

            RegraTributacaoModel.IsOpen = false;
        }

        public void EditarRegaEstadoSelecionada()
        {
            if (IsProdutoInserirOuAlterar == false)
            {
                return;
            }

            RegraTributacaoModel = new FlyoutRegraTributacaoModel(RegraEstadoSelecionada) { IsOpen = true };
            RegraTributacaoModel.RegraAlterada += RegraTributacaoAlteradaHandler;
            RegraTributacaoModel.RegraDeletada += RegraTributacaoDeletadaHandler;
        }

        private void RegraTributacaoAlteradaHandler(object sender, RegraTribuacaoInfo e)
        {
            if (NovoRegistro)
            {
                RegraTributacaoModel.IsOpen = false;
                return;
            }

            e.RegraVinculada.Aliquota = e.Aliquota;
            e.RegraVinculada.AliquotaFcp = e.AliquotaFcp;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                repositorio.Alterar(e.RegraVinculada);

                ListarRegrasInterstaduais(repositorio.BuscaRegrasPorProduto(_produto));
            }

            RegraTributacaoModel.IsOpen = false;
        }

        private void RegraTributacaoDeletadaHandler(object sender, RegraTribuacaoInfo e)
        {
            if (NovoRegistro)
            {
                RegrasPorEstado.Remove(e);
                RegraTributacaoModel.IsOpen = false;
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                repositorio.Deletar(e.RegraVinculada);

                ListarRegrasInterstaduais(repositorio.BuscaRegrasPorProduto(_produto));
            }

            RegraTributacaoModel.IsOpen = false;
        }

        public void AbrirFlyoutNovoAlias()
        {
            FlyoutCodigoBarraModel = new FlyoutCodigoBarraModel { IsOpen = true };

            FlyoutCodigoBarraModel.Confirmou += (s, alias) =>
            {
                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioProduto(sessao);

                    ThrowExceptionSeAliasNaoPermitido(alias, repositorio);

                    if (_produto.Id > 0)
                    {
                        alias.Produto = _produto;
                        repositorio.SalvarAlias(alias);
                    }

                    ListaDeAlias.Add(alias);
                    FlyoutCodigoBarraModel = null;
                }
            };
        }

        private void ThrowExceptionSeAliasNaoPermitido(ProdutoAlias alias, RepositorioProduto repositorio)
        {
            var jaExisteNaListaDeAlias = ListaDeAlias.Any(i => i.Alias == alias.Alias && i.Id != alias.Id);

            if (repositorio.JaExisteEsseAlias(alias) || jaExisteNaListaDeAlias)
            {
                throw new InvalidOperationException($"Código {alias.Alias} já existe");
            }

            if (alias.IsCodigoBarras && ListaDeAlias.Any(i => i.IsCodigoBarras && i.Id != alias.Id))
            {
                throw new InvalidOperationException("É permitido apenas um código do tipo barras por produto");
            }
        }

        private void ThrowExceptionSeExisteAliasNaoPermitido()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioProduto(sessao);

                foreach (var alias in ListaDeAlias)
                {
                    ThrowExceptionSeAliasNaoPermitido(alias, repositorio);
                }
            }
        }

        public void DeletarAlias()
        {
            if (_produto.Id == 0)
            {
                ListaDeAlias.Remove(ProdutoAliasSelecionado);
                ProdutoAliasSelecionado = null;
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioProduto(sessao);
                repositorio.Salvar(ProdutoAliasSelecionado.Produto);
                repositorio.Deletar(ProdutoAliasSelecionado);
                transacao.Commit();
            }

            ListaDeAlias.Remove(ProdutoAliasSelecionado);
            ProdutoAliasSelecionado = null;
        }

        public void LoadNcm()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var ncmRepositorio = new RepositorioComun<NcmDTO>(sessao);
                var ncmCompativel = ncmRepositorio.Busca(new NcmPeloCodigo(CodigoNcm));

                if (ncmCompativel == null)
                {
                    return;
                }

                if (CodigoNcm != ncmCompativel.Id)
                {
                    CodigoNcm = ncmCompativel.Id;
                    CodigoCest = ncmCompativel.Cest.TrimOrEmpty();
                }
            }
        }

        public void LoadTabelaPreco(ISession sessao)
        {
            var tabelaRepositorio = new RepositorioTabelaPreco(sessao);
            var tabelas = tabelaRepositorio.BuscarPorIdProduto(_produto.Id);

            TabelaPrecos.Clear();

            foreach (var tabela in tabelas)
            {
                var itemViewModel = new ProdutoTabelaPrecoViewModel
                {
                    TabelaPrecoId = tabela.Id,
                    TabelaPrecoDescricao = tabela.Descricao,
                    TipoAjuste = tabela.TipoAjustePreco,
                    PercentualAjuste = tabela.PercentualAjuste,
                    PercentualDiferenciado = tabela.PercentualAjusteDiferenciado,
                    ProdutoId = tabela.ProdutoId
                };

                itemViewModel.CalcularPreco(_produto.Id, PrecoVenda);

                TabelaPrecos.Add(itemViewModel);
            }
        }

        public ProdutoDTO ClonaProduto()
        {
            var produto = (ProdutoDTO)_produto.Clone();

            produto.Id = 0;
            produto.ProdutosAlias = new List<ProdutoAlias>();

            return produto;
        }

        public void CarregarHistoricoCompras()
        {
            HistorioCompraModel = new HistoricoCompraModel();
            HistorioCompraModel.Carregar(_produto);
        }

        public void SalvarCodigoEditado(ProdutoAlias alias)
        {
            if (_produto.Id > 0)
            {
                using (var sessao = _sessaoManager.CriaSessao())
                {
                    var repositorio = new RepositorioProduto(sessao);
                    repositorio.SalvarAlias(alias);
                }
            }

            ListaDeAlias = new ObservableCollection<ProdutoAlias>(ListaDeAlias);
        }

        public TabelaPreco BuscarTabelaSelecionada()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var tabelaId = TabelaPrecoSelecionada.TabelaPrecoId;
                var tabelaRepositorio = new RepositorioTabelaPreco(sessao);
                var tabela = tabelaRepositorio.GetPeloId(tabelaId);

                return tabela;
            }
        }
    }
}