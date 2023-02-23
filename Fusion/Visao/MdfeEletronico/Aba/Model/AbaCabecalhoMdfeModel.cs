using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.Aba.Model
{
    public class RetornoCabecalhoMDFe : EventArgs
    {
        public RetornoCabecalhoMDFe(AbaCabecalhoMdfeModel model)
        {
            Model = model;
        }
        public AbaCabecalhoMdfeModel Model { get; private set; }
    }

    public class AbaCabecalhoMdfeModel : ViewModel
    {
        private EmissorFiscalComboBox _emissorFiscal;
        private ObservableCollection<EstadoDTO> _estadosCarregamento;
        private ObservableCollection<EstadoDTO> _estadosDescarregamento;
        private MDFeTipoEmissao _tipoEmissao;
        private Modal _modal;
        private MDFeTipoEmitente _tipoEmitente;
        private EstadoDTO _estadoCarregamento;
        private EstadoDTO _estadoDescarregamento;
        private string _informacaoAdicional;
        private bool _selecionado;
        private bool _habilitaTipoEmitenteParaSelecao;
        private MDFeTipoDoTransportador _tipoDoTransportador;
        private DateTime _emitidaEm;
        private DateTime? _previsaoInicioViagemEm;
        private string _serieDocumento;
        private string _numeroDocumento;
        private bool _isEnabledAlocarNovoNumero;
        private bool _cargaFechada;
        private string _cepCarregamento;
        private string _cepDescarregamento;

        public MDFeTipoDoTransportador TipoDoTransportador
        {
            get { return _tipoDoTransportador; }
            set
            {
                if (value == _tipoDoTransportador) return;
                _tipoDoTransportador = value;
                PropriedadeAlterada();
            }
        }

        public bool Selecionado
        {
            get { return _selecionado; }
            set
            {
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public string SerieDocumento
        {
            get => _serieDocumento;
            set
            {
                if (value == _serieDocumento) return;
                _serieDocumento = value;
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

        public bool IsEnabledAlocarNovoNumero
        {
            get => _isEnabledAlocarNovoNumero;
            set
            {
                if (value == _isEnabledAlocarNovoNumero) return;
                _isEnabledAlocarNovoNumero = value;
                PropriedadeAlterada();
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

        public DateTime? PrevisaoInicioViagemEm
        {
            get => _previsaoInicioViagemEm;
            set
            {
                if (value.Equals(_previsaoInicioViagemEm)) return;
                _previsaoInicioViagemEm = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler<RetornoCabecalhoMDFe> ProximoHandler;
        public event EventHandler<AbaCabecalhoMdfeModel> EventoAlocarNumeracao;

        public ObservableCollection<EstadoDTO> EstadosCarregamento
        {
            get { return _estadosCarregamento; }
            set
            {
                _estadosCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> EstadosDescarregamento
        {
            get { return _estadosDescarregamento; }
            set
            {
                _estadosDescarregamento = value;
                PropriedadeAlterada();
            }
        }

        public EmissorFiscalComboBox EmissorFiscal
        {
            get { return _emissorFiscal; }
            set
            {
                _emissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmissorFiscalComboBox> ListaEmissorFiscal { get; set; }

        public MDFeTipoEmissao TipoEmissao
        {
            get { return _tipoEmissao; }
            set
            {
                _tipoEmissao = value;
                PropriedadeAlterada();
            }
        }

        public Modal Modal
        {
            get { return _modal; }
            set
            {
                _modal = value;
                PropriedadeAlterada();
            }
        }

        public MDFeTipoEmitente TipoEmitente
        {
            get { return _tipoEmitente; }
            set
            {
                _tipoEmitente = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoCarregamento
        {
            get { return _estadoCarregamento; }
            set
            {
                _estadoCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoDescarregamento
        {
            get { return _estadoDescarregamento; }
            set
            {
                _estadoDescarregamento = value;
                PropriedadeAlterada();
            }
        }

        public string InformacaoAdicional
        {
            get { return _informacaoAdicional; }
            set
            {
                _informacaoAdicional = value;
                PropriedadeAlterada();
            }
        }

        public bool HabilitaTipoEmitenteParaSelecao
        {
            get { return _habilitaTipoEmitenteParaSelecao; }
            set
            {
                _habilitaTipoEmitenteParaSelecao = value;
                PropriedadeAlterada();
            }
        }

        public bool CargaFechada
        {
            get => _cargaFechada;
            set
            {
                _cargaFechada = value;
                PropriedadeAlterada();
                CepCarregamento = string.Empty;
                CepDescarregamento = string.Empty;
            }
        }

        public string CepCarregamento
        {
            get => _cepCarregamento;
            set
            {
                _cepCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public string CepDescarregamento
        {
            get => _cepDescarregamento;
            set
            {
                _cepDescarregamento = value;
                PropriedadeAlterada();
            }
        }

        public AbaCabecalhoMdfeModel()
        {
            InicializaModel();
        }

        private void InicializaModel()
        {
            HabilitaTipoEmitenteParaSelecao = true;
            EstadosCarregamento = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia().GetEstados());
            EstadosDescarregamento =
                new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());
            TipoEmissao = MDFeTipoEmissao.Normal;
            Modal = Modal.Rodoviario;
            TipoEmitente = MDFeTipoEmitente.PrestadorServicoDeTransporte;
            TipoDoTransportador = MDFeTipoDoTransportador.NaoInformado;
            EmitidaEm = DateTime.Now;
            ListaEmissorFiscal = BuscarListaEmissorFiscal();
            EmissorFiscal = ListaEmissorFiscal.FirstOrNull() as EmissorFiscalComboBox;
            SerieDocumento = 0.ToString("D3");
            NumeroDocumento = 0.ToString("D9");
        }

        private ObservableCollection<EmissorFiscalComboBox> BuscarListaEmissorFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new ObservableCollection<EmissorFiscalComboBox>(new RepositorioEmissorFiscal(sessao).BuscaTodosQueSejamMdfeParaComboBox());
            }
        }

        public void Proximo()
        {
            Hidratacao();

            Validacao();

            OnProximoHandler();
        }

        private void Hidratacao()
        {
            InformacaoAdicional = InformacaoAdicional.TrimOrEmpty();
        }

        private void Validacao()
        {
            if (EmissorFiscal == null) throw new ArgumentException("Emissor fiscal é obrigatório");
            if (EmitidaEm <= DateTime.Now.AddMonths(-12)) throw new ArgumentException("Emissão Em muito antigo não e permitido");

            if (EstadoCarregamento == null) throw new ArgumentException("Estado carregamento é obrigatório");
            if (EstadoDescarregamento == null) throw new ArgumentException("Estado descarregamento é obrigatório");

            if (CargaFechada && CepCarregamento.IsNullOrEmpty()) throw new ArgumentException("Cep Carregamento é obrigatório");
            if (CargaFechada && CepDescarregamento.IsNullOrEmpty()) throw new ArgumentException("Cep Descarregamento é obrigatório");
        }

        public virtual void OnProximoHandler()
        {
            ProximoHandler?.Invoke(this, new RetornoCabecalhoMDFe(this));
        }

        public void ComMdfe(MDFeEletronico mdfe)
        {
            EmissorFiscal = new EmissorFiscalComboBox
            {
                Id = mdfe.EmissorFiscal.Id,
                Descricao = mdfe.EmissorFiscal.Descricao
            };

            TipoEmissao = mdfe.TipoEmissao;
            Modal = mdfe.Modal;
            TipoEmitente = mdfe.TipoEmitente;
            EstadoCarregamento = mdfe.EstadoCarregamento;
            EstadoDescarregamento = mdfe.EstadoDescarregamento;
            InformacaoAdicional = mdfe.Observacao;
            TipoDoTransportador = mdfe.TipoDoTransportador;
            EmitidaEm = mdfe.EmissaoEm;
            PrevisaoInicioViagemEm = mdfe.PrevisaoInicioViagemEm;
            SerieDocumento = mdfe.SerieEmissao.ToString("D3");
            NumeroDocumento = mdfe.NumeroFiscalEmissao.ToString("D9");
            IsEnabledAlocarNovoNumero = mdfe.ContemNumeroDocumento();
            CargaFechada = mdfe.CargaFechada;
            CepCarregamento = mdfe.ProdutoPredominante.CepCarregamento;
            CepDescarregamento = mdfe.ProdutoPredominante.CepDescarregamento;
        }

        public void AlocarNumeracao()
        {
            OnEventoAlocarNumeracao();
        }

        protected virtual void OnEventoAlocarNumeracao()
        {
            EventoAlocarNumeracao?.Invoke(this, this);
        }

        public void AlocouNumeracao(MDFeEletronico mdfe)
        {
            SerieDocumento = mdfe.SerieEmissao.ToString("D3");
            NumeroDocumento = mdfe.NumeroFiscalEmissao.ToString("D9");
            IsEnabledAlocarNovoNumero = mdfe.NumeroFiscalEmissao != 0;
        }
    }
}