using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using Fusion.Visao.DocumentoAPagar.Lancamentos;
using Fusion.Visao.Menu;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAPagar
{
    public class GridDocumentoPagarModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        private string _filtroFornecedor;
        private ObservableCollection<DocumentoPagarDTO> _documentosAPagar;
        private decimal _valorTotal;
        private Situacao? _situacao;
        private string _numeroDocumento;
        private DateTime? _dataVencimentoInicial;
        private DateTime? _dataVencimentoFinal;
        private bool _isAbrirFiltro;
        private DocumentoPagarDTO _selecionado;
        private Fornecedor _fornecedorSelecionado;
        private decimal _valorTotalAQuitado;
        private decimal _valorTotalAPagar;
        private EmpresaComboBoxDTO _empresaSelecionada;
        private ObservableCollection<EmpresaComboBoxDTO> _empresas;
        private bool _isGerenciarRecibo;
        private bool _isGerarAvulso;
        private bool _isPermissaoListar;
        private readonly Notificador _notificador;

        public GridDocumentoPagarModel(Notificador notificador)
        {
            _notificador = notificador;
        }

        public event EventHandler ImprimirRecibo;
        public event EventHandler<DocumentoPagar> ImprimirReciboComDocumento;

        public ICommand FiltroCommand => GetSimpleCommand(FiltroAction);
        public ICommand LimparFiltrosCommand => GetSimpleCommand(LimparFiltrosAction);
        public ICommand AplicarPesquisaCommand => GetSimpleCommand(AplicarPesquisaAction);
        public ICommand BuscarFornecedorCommand => GetSimpleCommand(BuscarFornecedorAction);
        public ICommand FecharPesquisaCommand => GetSimpleCommand(FecharPesquisaAction);
        public ICommand NovoCommand => GetSimpleCommand(NovoAction);
        public ICommand LimparFornecedorCommand => GetSimpleCommand(LimparFornecedorAction);
        public ICommand ImprimirReciboCommand => GetSimpleCommand(ImprimirReciboAction);

        private void ImprimirReciboAction(object obj)
        {
            OnImprimirRecibo();
        }


        private void LimparFiltrosAction(object obj)
        {
            FiltroFornecedor = string.Empty;
            Situacao = null;
            DataVencimentoInicial = null;
            DataVencimentoFinal = null;
            _fornecedorSelecionado = null;
            EmpresaSelecionada = null;

            AplicarPesquisaAction(null);
        }

        private void LimparFornecedorAction(object obj)
        {
            _fornecedorSelecionado = null;
            FiltroFornecedor = string.Empty;
        }

        private void NovoAction(object obj)
        {
            IsAbrirFiltro = false;

            var novo = new DocumentoPagar
            {
                Malote = Malote.Cria(OrigemDocumento.Manual, string.Empty, _sessaoSistema.UsuarioLogado, 0 /*todo entrada*/)
            };

            var model = new DocumentoPagarFormModel(novo, _notificador);
            var documentoForm = new DocumentoPagarForm(model);

            MenuPrincipal.Instancia.ShowChildWindowAsync(documentoForm);
        }

        private void FecharPesquisaAction(object obj)
        {
            IsAbrirFiltro = false;
        }

        private void BuscarFornecedorAction(object obj)
        {
            IsAbrirFiltro = false;
            var picker = new PessoaPickerModel(new FornecedorEngine());

            picker.InicializarComPesquisa(FiltroFornecedor);

            picker.PickItemEvent += FornecedorSelecionado;

            var pickerView = picker.GetPickerView();
            
            pickerView.Closed += AbrirIsFiltro;

            pickerView.ShowDialog();
        }

        private void AbrirIsFiltro(object sender, EventArgs e)
        {
            IsAbrirFiltro = true;
        }

        private void FornecedorSelecionado(object sender, GridPickerEventArgs e)
        {
            try
            {
                var fornecedor = e.GetItem<Fornecedor>();

                _fornecedorSelecionado = fornecedor;
                FiltroFornecedor = _fornecedorSelecionado.Nome;
            }
            catch (InvalidCastException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }


        public DocumentoPagarDTO Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        private void FiltroAction(object obj)
        {
            IsAbrirFiltro = true;
        }

        public bool IsAbrirFiltro
        {
            get => _isAbrirFiltro; set
            {
                if (value == _isAbrirFiltro) return;
                _isAbrirFiltro = value;
                PropriedadeAlterada();
            }
        }

        public string FiltroFornecedor
        {
            get => _filtroFornecedor;
            set
            {
                if (value == _filtroFornecedor) return;
                _filtroFornecedor = value;
                PropriedadeAlterada();
            }
        }

        public Situacao? Situacao
        {
            get => _situacao;
            set
            {
                if (value == _situacao) return;
                _situacao = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDocumento
        {
            get => _numeroDocumento;
            set
            {
                if (value == _numeroDocumento) return;
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataVencimentoInicial
        {
            get => _dataVencimentoInicial; set
            {
                if (value.Equals(_dataVencimentoInicial)) return;
                _dataVencimentoInicial = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataVencimentoFinal
        {
            get => _dataVencimentoFinal;
            set
            {
                if (value.Equals(_dataVencimentoFinal)) return;
                _dataVencimentoFinal = value;
                PropriedadeAlterada();
            }
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => _empresaSelecionada;
            set
            {
                if (Equals(value, _empresaSelecionada)) return;
                _empresaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmpresaComboBoxDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }


        public ObservableCollection<DocumentoPagarDTO> DocumentosAPagar
        {
            get => _documentosAPagar;
            set
            {
                if (Equals(value, _documentosAPagar)) return;
                _documentosAPagar = value;
                PropriedadeAlterada();

                ValorTotal = _documentosAPagar.Sum(d => d.ValorAjustado);
                ValorTotalAQuitado = _documentosAPagar.Sum(d => d.ValorQuitado);
                ValorTotalAPagar = ValorTotal - ValorTotalAQuitado;
            }
        }

        public decimal ValorTotalAPagar
        {
            get => _valorTotalAPagar;
            set
            {
                if (value == _valorTotalAPagar) return;
                _valorTotalAPagar = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotalAQuitado
        {
            get => _valorTotalAQuitado;
            set
            {
                if (value == _valorTotalAQuitado) return;
                _valorTotalAQuitado = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorTotal
        {
            get => _valorTotal;
            set
            {
                if (value == _valorTotal) return;
                _valorTotal = value;
                PropriedadeAlterada();
            }
        }


        public void InicializaLoadded()
        {
            BuscaLoadded();

            var usuarioLogado = _sessaoSistema.UsuarioLogado;

            IsGerenciarRecibo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_GERAR_RECIBO);
            IsGerarAvulso = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO);
            IsPermissaoListar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_DOCUMENTO_APAGAR_LISTAR);
        }

        public bool IsPermissaoListar
        {
            get => _isPermissaoListar;
            set
            {
                if (value == _isPermissaoListar) return;
                _isPermissaoListar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerarAvulso
        {
            get => _isGerarAvulso;
            set
            {
                if (value == _isGerarAvulso) return;
                _isGerarAvulso = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarRecibo
        {
            get => _isGerenciarRecibo;
            set
            {
                if (value == _isGerenciarRecibo) return;
                _isGerenciarRecibo = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaLoadded()
        {
            Empresas = BuscarEmpresas();
            Situacao = FusionCore.FusionAdm.Financeiro.Flags.Situacao.Aberto;

            BuscaFiltrada(new FiltroDocumentoReceberFaturamento
            {
                Situacao = Situacao,
                NumeroDocumento = string.Empty,
                DataVencimentoFinal = null,
                DataVencimentoInicial = null,
                PessoaId = 0,
                Empresa = EmpresaSelecionada
            });
        }

        private ObservableCollection<EmpresaComboBoxDTO> BuscarEmpresas()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmpresa(sessao);

                return new ObservableCollection<EmpresaComboBoxDTO>(repositorio.BuscarEmpresaComboBoxDtos());
            }
        }

        private void BuscaFiltrada(FiltroDocumentoReceberFaturamento filtroDocumento)
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();

            using (sessao)
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);

                var lista = repositorio.BuscaParaFaturamento(filtroDocumento);

                DocumentosAPagar = new ObservableCollection<DocumentoPagarDTO>(lista);
            }
        }

        public void AplicarPesquisaAction(object obj = null)
        {

            try
            {
                if (FiltroFornecedor.IsNotNullOrEmpty())
                {
                    if (_fornecedorSelecionado == null) throw new ArgumentException("Se tiver filtro no fornecedor, adicionar o fornecedor");
                }

                if (DataVencimentoFinal != null && DataVencimentoInicial == null)
                {
                    throw new ArgumentException("Adicionar Data Vencimento Inicial");
                }


                BuscaFiltrada(new FiltroDocumentoReceberFaturamento
                {
                    Situacao = Situacao,
                    NumeroDocumento = NumeroDocumento,
                    DataVencimentoFinal = DataVencimentoFinal,
                    DataVencimentoInicial = DataVencimentoInicial,
                    PessoaId = _fornecedorSelecionado?.Id ?? 0,
                    Empresa = EmpresaSelecionada
                });
                IsAbrirFiltro = false;
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        public void AbrirJanelaOpcoes()
        {
            var documento = BuscarDocumento();

            try
            {
                DocumentoCancelado(documento);

                var model = new OpcoesDocumentoModel(documento, _notificador);
                var opcoes = new OpcoesDocumento(model);

                model.ImprimirDocumento += ImprimirReciboPeloOpcoes;

                opcoes.ShowDialog();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void ImprimirReciboPeloOpcoes(object sender, DocumentoPagar e)
        {
            OnImprimirReciboComDocumento(e);
        }

        private void DocumentoCancelado(DocumentoPagar documento)
        {
            if (documento.EstaCancelado())
            {
                throw new ArgumentException("Este documento foi cancelado");
            }
        }

        private DocumentoPagar BuscarDocumento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioDocumentoPagar(sessao);

                return repositorio.GetPeloId(Selecionado.Id);
            }
        }

        public void EfetuarLancamentos()
        {
            var documento = BuscarDocumento();

            try
            {
                DocumentoCancelado(documento);

                var model = new DocumentoAPagarLancamentoModel(documento, _notificador);
                var documentoReceberLancamento = new DocumentoAPagarLancamento(model);

                documentoReceberLancamento.ShowDialog();

                AplicarPesquisaAction();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        protected virtual void OnImprimirRecibo()
        {
            ImprimirRecibo?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnImprimirReciboComDocumento(DocumentoPagar e)
        {
            ImprimirReciboComDocumento?.Invoke(this, e);
        }
    }
}