using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using Fusion.Visao.PerfilCfop;
using FusionCore.FusionAdm.CteEletronico;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public sealed class AbaCabecalhoCteModel : ViewModel
    {
        private readonly LocalidadesServico _localidades = LocalidadesServico.GetInstancia();
        private bool _selecionado;
        private TipoCte _tipoCte;
        private Modal _modal;
        private TipoServico _tipoServico;
        private decimal _valorServico;
        private decimal _valorAReceber;
        private PerfilCfopDTO _perfilCfop;
        private string _informacaoAdicional;
        private string _naturezaOperacao;
        private CidadeDTO _inicioCidade;
        private ObservableCollection<CidadeDTO> _inicioCidades;
        private ObservableCollection<EstadoDTO> _inicioEstados;
        private ObservableCollection<EstadoDTO> _finalEstados;
        private CidadeDTO _finalCidade;
        private ObservableCollection<CidadeDTO> _finalCidades;
        private DateTime? _dataInicioEm;
        private DateTime? _dataFinalEm;
        private TimeSpan? _horaProgramada;
        private TimeSpan? _horaFinal;
        private bool _statusDataProgramada;
        private bool _statusDataFinal;
        private TipoPeriodoData _tipoPeriodoData;
        private string _dataProgramadaTexto;
        private string _dataProgramadaFinalTexto;
        private TipoPeriodoHora _tipoPeriodoHora;
        private bool _statusHoraProgramada;
        private bool _statusHoraFinal;
        private string _horaProgramadaTexto;
        private string _horaProgramadaFinalTexto;
        private bool _habilitado;
        private string _rntrc;
        private decimal? _valorAverbacao;
        private string _emitente;
        private DateTime _emitidaEm;
        private TipoEmissao _tipoEmissao;
        private bool _isCteComplementar;
        private string _chaveCteComplementar;
        private bool _isNormal;
        private DateTime _declaracaoEmitidaEm = DateTime.Now;
        private bool _isCteAnulacao;
        private string _chaveCteAnulacao;
        private bool _isCteSubstituicao;
        private string _chaveSubstituido;
        private string _chaveAnulacao;
        private string _chaveNfePeloTomador;
        private string _chaveCtePeloTomador;
        private string _cpfCnpj;
        private CteModeloDocumento _modeloDocumento = CteModeloDocumento.CinquentaCinco;
        private string _substitutoSerie;
        private string _substitutoSubSerie;
        private string _substitutoNumeroDocumentoFiscal;
        private decimal _substitutoValor;
        private DateTime _substitutoEmitidaEm = DateTime.Now;
        private bool _isEnabledAlocarNovoNumero;
        private short _serieDocumento;
        private long _numeroDocumento;

        public TipoEmissao TipoEmissao
        {
            get => _tipoEmissao;
            set
            {
                if (value == _tipoEmissao) return;
                _tipoEmissao = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<TipoCte> DeletaSubstituto;

        public event EventHandler AlocarNumeracaoNovaParaCTe; 

        public TipoCte TipoCte
        {
            get => _tipoCte;
            set
            {
                _tipoCte = value;
                PropriedadeAlterada();
                IsCteComplementar = TipoCte == TipoCte.ComplementoDeValores;
                IsCteAnulacao = TipoCte == TipoCte.CteDeAnulacao;
                IsNormal = TipoCte == TipoCte.Normal;
                IsCteSubstituicao = TipoCte == TipoCte.CteDeSubstituicao;

                ChaveCteComplementar = string.Empty;
                ChaveCteAnulacao = string.Empty;
                DeclaracaoEmitidaEm = DateTime.Now;

                ChaveSubstituido = string.Empty;
                ChaveAnulacao = string.Empty;
                ChaveNfePeloTomador = string.Empty;
                ChaveCtePeloTomador = string.Empty;
                CpfCnpj = string.Empty;
                ModeloDocumento = CteModeloDocumento.NF;
                SubstitutoSerie = string.Empty;
                SubstitutoSubSerie = string.Empty;
                SubstitutoNumeroDocumentoFiscal = string.Empty;
                SubstitutoValor = 0.0m;
                SubstitutoEmitidaEm = DateTime.Now;

                OnDeletaSubstituto(value);
            }
        }

        public bool IsCteSubstituicao
        {
            get => _isCteSubstituicao;
            set
            {
                if (value == _isCteSubstituicao) return;
                _isCteSubstituicao = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveCteAnulacao
        {
            get => _chaveCteAnulacao;
            set
            {
                if (value == _chaveCteAnulacao) return;
                _chaveCteAnulacao = value;
                PropriedadeAlterada();
            }
        }

        public DateTime DeclaracaoEmitidaEm
        {
            get => _declaracaoEmitidaEm;
            set
            {
                _declaracaoEmitidaEm = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCteAnulacao
        {
            get => _isCteAnulacao;
            set
            {
                if (value == _isCteAnulacao) return;
                _isCteAnulacao = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCteComplementar
        {
            get => _isCteComplementar;
            set
            {
                _isCteComplementar = value;
                PropriedadeAlterada();
                ChaveCteComplementar = string.Empty;
            }
        }

        public bool IsNormal
        {
            get => _isNormal;
            set
            {
                _isNormal = value;
                PropriedadeAlterada();
                ValorAverbacao = null;
            }
        }

        public string ChaveCteComplementar
        {
            get => _chaveCteComplementar;
            set
            {
                if (value == _chaveCteComplementar) return;
                _chaveCteComplementar = value;
                PropriedadeAlterada();
            }
        }

        public Modal Modal
        {
            get => _modal;
            set
            {
                if (value == _modal) return;
                _modal = value;
                PropriedadeAlterada();
            }
        }

        public TipoServico TipoServico
        {
            get => _tipoServico;
            set
            {
                if (value == _tipoServico) return;
                _tipoServico = value;
                PropriedadeAlterada();
            }
        }

        public string Emitente
        {
            get { return _emitente; }
            set
            {
                if (value == _emitente) return;
                _emitente = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorServico
        {
            get => _valorServico;
            set
            {
                if (value == _valorServico) return;
                _valorServico = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorAReceber
        {
            get => _valorAReceber;
            set
            {
                if (value == _valorAReceber) return;
                _valorAReceber = value;
                PropriedadeAlterada();
            }
        }

        public decimal? ValorAverbacao
        {
            get { return _valorAverbacao; }
            set
            {
                _valorAverbacao = value;
                PropriedadeAlterada();
            }
        }

        public PerfilCfopDTO PerfilCfop
        {
            get => _perfilCfop;
            set
            {
                if (Equals(value, _perfilCfop)) return;
                _perfilCfop = value;
                NaturezaOperacao = value.Descricao.SubstringWithTrim(0, 60);
                PropriedadeAlterada();
            }
        }

        public string NaturezaOperacao
        {
            get => _naturezaOperacao;
            set
            {
                if (value == _naturezaOperacao) return;
                _naturezaOperacao = value;
                PropriedadeAlterada();
            }
        }

        public string InformacaoAdicional
        {
            get => _informacaoAdicional;
            set
            {
                if (value == _informacaoAdicional) return;
                _informacaoAdicional = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataInicioEm
        {
            get => _dataInicioEm;
            set
            {
                if (value.Equals(_dataInicioEm)) return;
                _dataInicioEm = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataFinalEm
        {
            get => _dataFinalEm;
            set
            {
                if (value.Equals(_dataFinalEm)) return;
                _dataFinalEm = value;
                PropriedadeAlterada();
            }
        }

        public TimeSpan? HoraProgramada
        {
            get => _horaProgramada;
            set
            {
                if (value.Equals(_horaProgramada)) return;
                _horaProgramada = value;
                PropriedadeAlterada();
            }
        }

        public TimeSpan? HoraFinal
        {
            get => _horaFinal;
            set
            {
                if (value.Equals(_horaFinal)) return;
                _horaFinal = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO InicioEstado
        {
            get => GetValue<EstadoDTO>();
            set
            {
                SetValue(value);

                if (value == null)
                {
                    return;
                }

                var cidades = _localidades.GetCidades(c => c.SiglaUf == value.Sigla);
                InicioCidades = new ObservableCollection<CidadeDTO>(cidades);
            }
        }

        public EstadoDTO FinalEstado
        {
            get => GetValue<EstadoDTO>();
            set
            {
                SetValue(value);

                if (value == null)
                {
                    return;
                }

                var cidades = _localidades.GetCidades(c => c.SiglaUf == value.Sigla);
                FinalCidades = new ObservableCollection<CidadeDTO>(cidades);
            }
        }

        public DateTime EmitidaEm
        {
            get { return _emitidaEm; }
            set
            {
                if (value.Equals(_emitidaEm)) return;
                _emitidaEm = value;
                PropriedadeAlterada();
            }
        }

        public bool StatusDataProgramada
        {
            get => _statusDataProgramada;
            set
            {
                if (value == _statusDataProgramada) return;
                _statusDataProgramada = value;
                PropriedadeAlterada();
            }
        }

        public bool StatusDataFinal
        {
            get => _statusDataFinal;
            set
            {
                if (value == _statusDataFinal) return;
                _statusDataFinal = value;
                PropriedadeAlterada();
            }
        }

        public TipoPeriodoData TipoPeriodoData
        {
            get => _tipoPeriodoData;
            set
            {
                if (value == _tipoPeriodoData) return;
                _tipoPeriodoData = value;
                DefinirPeriodo(value);
                PropriedadeAlterada();
            }
        }

        public TipoPeriodoHora TipoPeriodoHora
        {
            get => _tipoPeriodoHora;
            set
            {
                if (value == _tipoPeriodoHora) return;
                _tipoPeriodoHora = value;
                DefinirPeriodoHora(value);
                PropriedadeAlterada();
            }
        }

        public string HoraProgramadaTexto
        {
            get => _horaProgramadaTexto;
            set
            {
                if (value == _horaProgramadaTexto) return;
                _horaProgramadaTexto = value;
                PropriedadeAlterada();
            }
        }

        public string HoraProgramadaFinalTexto
        {
            get => _horaProgramadaFinalTexto;
            set
            {
                if (value == _horaProgramadaFinalTexto) return;
                _horaProgramadaFinalTexto = value;
                PropriedadeAlterada();
            }
        }

        public bool StatusHoraProgramada
        {
            get => _statusHoraProgramada;
            set
            {
                if (value == _statusHoraProgramada) return;
                _statusHoraProgramada = value;
                PropriedadeAlterada();
            }
        }

        public string Rntrc
        {
            get => _rntrc;
            set
            {
                if (value == _rntrc) return;
                _rntrc = value;
                PropriedadeAlterada();
            }
        }

        public bool StatusHoraFinal
        {
            get => _statusHoraFinal;
            set
            {
                if (value == _statusHoraFinal) return;
                _statusHoraFinal = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> InicioEstados
        {
            get => _inicioEstados;
            set
            {
                if (Equals(value, _inicioEstados)) return;
                _inicioEstados = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> FinalEstados
        {
            get => _finalEstados;
            set
            {
                if (Equals(value, _finalEstados)) return;
                _finalEstados = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO InicioCidade
        {
            get => _inicioCidade;
            set
            {
                if (Equals(value, _inicioCidade)) return;
                _inicioCidade = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO FinalCidade
        {
            get => _finalCidade;
            set
            {
                if (Equals(value, _finalCidade)) return;
                _finalCidade = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> InicioCidades
        {
            get => _inicioCidades;
            set
            {
                if (Equals(value, _inicioCidades)) return;
                _inicioCidades = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> FinalCidades
        {
            get => _finalCidades;
            set
            {
                if (Equals(value, _finalCidades)) return;
                _finalCidades = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscarCfop => GetSimpleCommand(BuscarCfop);
        public ICommand CommandImportarXml => GetSimpleCommand(ImportarXml);

        public event EventHandler AbrirFlyoutImportarXmlEventHandler;

        public Ibpt Nbs
        {
            get => GetValue<Ibpt>();
            set
            {
                SetValue(value);
                SetValue(value != null, nameof(PossuiNbs));
            }
        }

        public bool PossuiNbs
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public bool Selecionado
        {
            get => _selecionado;
            set
            {
                if (value == _selecionado) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public bool Habilitado
        {
            get => _habilitado;
            set
            {
                if (value == _habilitado) return;
                _habilitado = value;
                PropriedadeAlterada();
            }
        }

        public string DataProgramadaTexto
        {
            get => _dataProgramadaTexto;
            set
            {
                if (value == _dataProgramadaTexto) return;
                _dataProgramadaTexto = value;
                PropriedadeAlterada();
            }
        }

        public string DataProgramadaFinalTexto
        {
            get => _dataProgramadaFinalTexto;
            set
            {
                if (value == _dataProgramadaFinalTexto) return;
                _dataProgramadaFinalTexto = value;
                PropriedadeAlterada();
            }
        }

        public bool Globalizado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public PerfilCte PerfilCte { get; set; }


        public string ChaveSubstituido
        {
            get => _chaveSubstituido;
            set
            {
                if (value == _chaveSubstituido) return;
                _chaveSubstituido = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveAnulacao
        {
            get => _chaveAnulacao;
            set
            {
                if (value == _chaveAnulacao) return;
                _chaveAnulacao = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveNfePeloTomador
        {
            get => _chaveNfePeloTomador;
            set
            {
                if (value == _chaveNfePeloTomador) return;
                _chaveNfePeloTomador = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveCtePeloTomador
        {
            get => _chaveCtePeloTomador;
            set
            {
                if (value == _chaveCtePeloTomador) return;
                _chaveCtePeloTomador = value;
                PropriedadeAlterada();
            }
        }

        public string CpfCnpj
        {
            get => _cpfCnpj;
            set
            {
                if (value == _cpfCnpj) return;
                _cpfCnpj = value;
                PropriedadeAlterada();
            }
        }

        public CteModeloDocumento ModeloDocumento
        {
            get => _modeloDocumento;
            set
            {
                if (value == _modeloDocumento) return;
                _modeloDocumento = value;
                PropriedadeAlterada();
            }
        }

        public string SubstitutoSerie
        {
            get => _substitutoSerie;
            set
            {
                if (value == _substitutoSerie) return;
                _substitutoSerie = value;
                PropriedadeAlterada();
            }
        }

        public string SubstitutoSubSerie
        {
            get => _substitutoSubSerie;
            set
            {
                if (value == _substitutoSubSerie) return;
                _substitutoSubSerie = value;
                PropriedadeAlterada();
            }
        }

        public string SubstitutoNumeroDocumentoFiscal
        {
            get => _substitutoNumeroDocumentoFiscal;
            set
            {
                if (value == _substitutoNumeroDocumentoFiscal) return;
                _substitutoNumeroDocumentoFiscal = value;
                PropriedadeAlterada();
            }
        }

        public decimal SubstitutoValor
        {
            get => _substitutoValor;
            set
            {
                if (value == _substitutoValor) return;
                _substitutoValor = value;
                PropriedadeAlterada();
            }
        }

        public DateTime SubstitutoEmitidaEm
        {
            get => _substitutoEmitidaEm;
            set
            {
                if (value.Equals(_substitutoEmitidaEm)) return;
                _substitutoEmitidaEm = value;
                PropriedadeAlterada();
            }
        }


        public AbaCabecalhoCteModel()
        {
            Inicializa();
        }

        private void ImportarXml(object obj)
        {
            OnAbrirFlyoutImportarXmlEventHandler();
        }

        public void OnAbrirFlyoutImportarXmlEventHandler()
        {
            AbrirFlyoutImportarXmlEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void Inicializa()
        {
            InicioEstados = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia().GetEstados());
            InicioCidades = new ObservableCollection<CidadeDTO>();
            FinalCidades = new ObservableCollection<CidadeDTO>();
            FinalEstados = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());
            DataFinalEm = DateTime.Now;
            DataInicioEm = DateTime.Now;
            HoraFinal = DateTime.Now.TimeOfDay;
            HoraProgramada = DateTime.Now.TimeOfDay;
            TipoPeriodoData = TipoPeriodoData.SemDataDefinida;
            TipoPeriodoHora = TipoPeriodoHora.SemHoraDefinida;
            InicioEstado = InicioEstados?[0];
            FinalEstado = FinalEstados?[0];
            Modal = Modal.Rodoviario;
        }

        private void BuscarCfop(object obj)
        {
            var model = new PerfilCfopPickerModel();
            model.PickItemEvent += RecebePerfilCfop;
            model.GetPickerView().ShowDialog();
        }

        private void RecebePerfilCfop(object sender, GridPickerEventArgs e)
        {
            PerfilCfop = e.GetItem<PerfilCfopDTO>();
        }

        public event EventHandler ProximoPasso;

        private void OnProximoPasso()
        {
            ProximoPasso?.Invoke(this, EventArgs.Empty);
        }

        public void Proximo()
        {
            NaturezaOperacao = NaturezaOperacao.TrimOrEmpty();
            InformacaoAdicional = InformacaoAdicional.TrimOrEmpty();
            ValorAReceber = Convert.ToDecimal(ValorAReceber.ToString("N2"));
            ValorServico = Convert.ToDecimal(ValorServico.ToString("N2"));
            ValorAverbacao = ValorAverbacao != null ? Convert.ToDecimal(ValorAverbacao.Value.ToString("N2")) : (decimal?) null;
            ChaveCteComplementar = ChaveCteComplementar.TrimOrEmpty();
            ChaveSubstituido = ChaveSubstituido.TrimOrEmpty();
            ChaveAnulacao = ChaveAnulacao.TrimOrEmpty();
            ChaveNfePeloTomador = ChaveNfePeloTomador.TrimOrEmpty();
            ChaveCtePeloTomador = ChaveCtePeloTomador.TrimOrEmpty();
            CpfCnpj = CpfCnpj.TrimOrEmpty();
            SubstitutoSerie = SubstitutoSerie.TrimOrEmpty();
            SubstitutoSubSerie = SubstitutoSubSerie.TrimOrEmpty();
            SubstitutoNumeroDocumentoFiscal = SubstitutoNumeroDocumentoFiscal.TrimOrEmpty();

            Validacao();

            OnProximoPasso();
        }

        private void DefinirPeriodo(TipoPeriodoData tipoPeriodoData)
        {
            switch (tipoPeriodoData)
            {
                case TipoPeriodoData.SemDataDefinida:
                    StatusDataProgramada = false;
                    StatusDataFinal = false;
                    DataProgramadaTexto = string.Empty;
                    DataProgramadaFinalTexto = string.Empty;
                    break;
                case TipoPeriodoData.NoPeriodo:
                    DataProgramadaTexto = "Data início";
                    DataProgramadaFinalTexto = "Data final";
                    StatusDataProgramada = true;
                    StatusDataFinal = true;
                    break;
                case TipoPeriodoData.AteAData:
                case TipoPeriodoData.APartirDaData:
                case TipoPeriodoData.NaData:
                    DataProgramadaTexto = "Data";
                    DataProgramadaFinalTexto = string.Empty;
                    StatusDataProgramada = true;
                    StatusDataFinal = false;
                    break;
            }
        }

        private void DefinirPeriodoHora(TipoPeriodoHora tipoPeriodoHora)
        {
            switch (tipoPeriodoHora)
            {
                case TipoPeriodoHora.SemHoraDefinida:
                    StatusHoraProgramada = false;
                    StatusHoraFinal = false;
                    HoraProgramadaTexto = string.Empty;
                    HoraProgramadaFinalTexto = string.Empty;
                    break;
                case TipoPeriodoHora.NoIntervaloDeTempo:
                    HoraProgramadaTexto = "Hora início";
                    HoraProgramadaFinalTexto = "Hora final";
                    StatusHoraProgramada = true;
                    StatusHoraFinal = true;
                    break;
                case TipoPeriodoHora.AteOHorario:
                case TipoPeriodoHora.APartirDoHorario:
                case TipoPeriodoHora.NoHorario:
                    HoraProgramadaTexto = "Hora";
                    DataProgramadaFinalTexto = string.Empty;
                    StatusHoraProgramada = true;
                    StatusHoraFinal = false;
                    break;
            }
        }

        public void Validacao()
        {
            if (EmitidaEm <= DateTime.Now.AddMonths(-12)) throw new ArgumentException("Emissão Em muito antigo não e permitido");
            if (ValorAReceber < 0) throw new ArgumentException("Valor a receber deve ser maior ou igual a 0");
            if (ValorServico < 0) throw new ArgumentException("Valor da Prestação/Serviço deve ser maior ou igual a 0");

            if (string.IsNullOrEmpty(NaturezaOperacao)) throw new ArgumentException("Preencher Natureza Operação");
            if (InicioEstado == null) throw new ArgumentException("Selecionar um estado(uf) de inicio de operação");
            if (InicioCidade == null) throw new ArgumentException("Selecionar uma cidade de inicio de operação");
            if (FinalEstado == null) throw new ArgumentException("Selecionar um estado(uf) de final de operação");
            if (FinalCidade == null) throw new ArgumentException("Selecionar uma cidade de final de operação");

            if (Rntrc.IsNullOrEmpty())
                throw new ArgumentException("RNTRC deve ser informado no cadastro de empresa");

            if (!new Regex(@"[^\d]").IsMatch(Rntrc))
            {
                if (Rntrc.Length != 8) throw new ArgumentException("Rntrc deve ter 8 digitos");
            }

            if (new Regex(@"^[a-zA-Z]+$").IsMatch(Rntrc))
            {
                if (!Rntrc.Equals("ISENTO"))
                    throw new ArgumentException("Você adicionou no rntrc " + Rntrc +
                                                " deve ser adicionado ISENTO ou o número");
            }

            if (IsCteComplementar)
                if (ChaveCteComplementar.IsNullOrEmpty())
                    throw new ArgumentException("CT-e Complementar preciso de uma chave de ct-e complementado");

            if (IsCteAnulacao)
                if (ChaveCteAnulacao.IsNullOrEmpty())
                    throw new ArgumentException("CT-e de Anulação preciso de uma chave de ct-e a ser anulado");

            if (IsCteSubstituicao)
            {
                if (ChaveSubstituido.IsNullOrEmpty())
                    throw new ArgumentException("Preciso de uma Chave de Substituto");

                if (ChaveNfePeloTomador.IsNullOrEmpty()
                && (CpfCnpj.IsNullOrEmpty() || SubstitutoSerie.IsNullOrEmpty() || SubstitutoNumeroDocumentoFiscal.IsNullOrEmpty())
                && ChaveCtePeloTomador.IsNullOrEmpty()
                && ChaveAnulacao.IsNullOrEmpty())
                {
                    throw new ArgumentException("Preciso que adicione uma: \n" +
                                                "Chave de acesso do CT-e de Anulação\n" +
                                                "OU Chave de acesso da NF-e emitida pelo Tomador\n" +
                                                "OU Chave de acesso do CT-e emitida pelo Tomador\n" +
                                                "OU Informação da NF ou CT emitido pelo Tomador");
                }
            }


            ValidaPeriodoEntrega();
        }

        private void ValidaPeriodoEntrega()
        {
            if (TipoPeriodoHora != TipoPeriodoHora.SemHoraDefinida &&
                TipoPeriodoData == TipoPeriodoData.SemDataDefinida)
            {
                throw new ArgumentException("Selecionar um período de data");
            }
        }

        public void PreencherCom(Cte cte)
        {
            PerfilCte = cte.PerfilCte;

            TipoCte = cte.TipoCte;
            Modal = cte.Modal;
            TipoServico = cte.TipoServico;
            Globalizado = cte.Globalizado;
            ValorServico = cte.ValorServico;
            ValorAReceber = cte.ValorReceber;
            ValorAverbacao = cte.ValorAverbacao;
            PerfilCfop = cte.PerfilCfop;
            NaturezaOperacao = cte.NaturezaOperacao;
            InformacaoAdicional = cte.Observacao;
            DataInicioEm = cte.DataInicio ?? DateTime.Now;
            DataFinalEm = cte.DataFinal ?? DateTime.Now;
            HoraProgramada = cte.HoraInicio ?? DateTime.Now.TimeOfDay;
            HoraFinal = cte.HoraFinal ?? DateTime.Now.TimeOfDay;
            InicioEstado = cte.EstadoInicioOperacao;
            FinalEstado = cte.EstadoFinalOperacao;
            InicioCidade = cte.CidadeInicioOperacao;
            FinalCidade = cte.CidadeFinalOperacao;
            TipoPeriodoData = cte.TipoPeriodoData;
            TipoPeriodoHora = cte.TipoPeriodoHora;
            Rntrc = cte.PerfilCte.EmissorFiscal.Empresa.Rntrc;
            Emitente = cte.PerfilCte.EmissorFiscal.Empresa.RazaoSocial;
            EmitidaEm = cte.EmissaoEm;
            Nbs = cte.FetchIbpt();
            TipoEmissao = cte.TipoEmissao;
            ChaveCteComplementar = cte.ChaveCTeComplementado;
            ChaveCteAnulacao = cte.ChaveCteAnulacao;
            DeclaracaoEmitidaEm = cte.DeclaracaoEmitidaEm;
            SerieDocumento = cte.SerieEmissao;
            NumeroDocumento = cte.NumeroFiscalEmissao;


            if (cte.IsSubstituto())
            {
                var substituicao = cte.CteSubstituicao;

                ChaveSubstituido = substituicao.ChaveSubstituido;
                ChaveAnulacao = substituicao.ChaveAnulacao;
                ChaveNfePeloTomador = substituicao.ChaveNfePeloTomador;
                ChaveCtePeloTomador = substituicao.ChaveCtePeloTomador;
                CpfCnpj = substituicao.DocumentoUnico;
                ModeloDocumento = substituicao.ModeloDocumento;
                SubstitutoSerie = substituicao.Serie;
                SubstitutoSubSerie = substituicao.Subserie;
                SubstitutoNumeroDocumentoFiscal = substituicao.NumeroDocumentoFiscal;
                SubstitutoValor = substituicao.Valor;
                SubstitutoEmitidaEm = substituicao.EmitidoEm;
            }

            IsEnabledAlocarNovoNumero = cte.JaFoiAlocadoNumeroFiscal();
        }

        public long NumeroDocumento
        {
            get => _numeroDocumento;
            set
            {
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public short SerieDocumento
        {
            get => _serieDocumento;
            set
            {
                _serieDocumento = value;
                PropriedadeAlterada();
            }
        }

        public bool IsEnabledAlocarNovoNumero
        {
            get => _isEnabledAlocarNovoNumero;
            set
            {
                _isEnabledAlocarNovoNumero = value;
                PropriedadeAlterada();
            }
        }

        public void PreencherCom(PerfilCte perfilCte)
        {
            PerfilCte = perfilCte;

            PerfilCfop = perfilCte.PerfilCfop;
            NaturezaOperacao = perfilCte.NaturezaOperacao;
            TipoCte = perfilCte.TipoCte;
            TipoServico = perfilCte.TipoServico;
            InformacaoAdicional = perfilCte.Observacao;
            Rntrc = perfilCte.EmissorFiscal.Empresa.Rntrc;
            Emitente = perfilCte.EmissorFiscal.Empresa.RazaoSocial;
            Nbs = perfilCte.FetchIbpt();
            EmitidaEm = DateTime.Now;
            TipoEmissao = TipoEmissao.Normal;
        }

        public void ImportacaoXml(RetornoImportacaoXmlCteEventArgs e)
        {
            if (FinalEstado != null && FinalCidade != null)
            {
                if (!DialogBox.MostraConfirmacao("Verifiquei que já tem um Estado e Cidade Final adicionados, deseja substituir?", MessageBoxImage.Question)) return;
            } 

            if (e.IsDestinatarioDestinatario)
            {
                CarregarCidadeFinal(e.PessoaDestinatario);
            }

            if (e.IsEmitenteDestinatario)
            {
                CarregarCidadeFinal(e.PessoaEmitente);
            }
        }

        private void CarregarCidadeFinal(PessoaEntidade pessoa)
        {
            var endereco = (PessoaEndereco) pessoa.Enderecos.FirstOrNull();

            if (endereco == null) return;

            FinalEstado = LocalidadesServico.GetInstancia(false).GetEstado(x => x.Sigla == endereco.Cidade.SiglaUf);
            FinalCidade = endereco.Cidade;
        }

        private void OnDeletaSubstituto(TipoCte e)
        {
            DeletaSubstituto?.Invoke(this, e);
        }

        public void AlocarNovaNumeracaoParaCTe()
        {
            OnAlocarNumeracaoNovaParaCTe();
        }

        private void OnAlocarNumeracaoNovaParaCTe()
        {
            AlocarNumeracaoNovaParaCTe?.Invoke(this, EventArgs.Empty);
        }

        public void AtualizaNumeracao(Cte cte)
        {
            NumeroDocumento = cte.NumeroFiscalEmissao;
            SerieDocumento = cte.SerieEmissao;
            IsEnabledAlocarNovoNumero = cte.NumeroFiscalEmissao != 0;
        }
    }
}