using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Visao.PerfilCfop;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.TabelaIbpt;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public class AbaCteOsCabecalhoCteOsModel : AbaCTeOSViewModel
    {
        public event EventHandler<AbaCteOsCabecalhoCteOsModel> Proximo;
        public event EventHandler<AbaCteOsCabecalhoCteOsModel> AlocarNumeracaoFiscal;

        private TipoCte _tipoCte;
        private TipoServico _tipoServico;
        private Modal _modal;
        private decimal _valorServico;
        private decimal _valorAReceber;
        private PerfilCfopDTO _perfilCfop;
        private DateTime? _emitidaEm;
        private string _descricaoTabelaIbpt;
        private EstadoDTO _inicioEstado;
        private CidadeDTO _inicioCidade;
        private EstadoDTO _finalEstado;
        private CidadeDTO _finalCidade;
        private string _observacao;
        private string _naturezaOperacao;
        private ObservableCollection<EstadoDTO> _inicioEstados;
        private ObservableCollection<EstadoDTO> _finalEstados;
        private ObservableCollection<CidadeDTO> _inicioCidades;
        private ObservableCollection<CidadeDTO> _finalCidades;
        private decimal? _inss;
        private decimal _numeroDocumento;
        private decimal _serieDocumento;
        private TipoEmissao _tipoEmissao;
        private bool _isTransportePessoas;
        private TipoFretamento _tipoFretamento = TipoFretamento.Eventual;
        private DateTime? _viagemEm;

        public Ibpt Ibpt { get; set; }

        public TipoCte TipoCte
        {
            get => _tipoCte;
            set
            {
                if (value == _tipoCte) return;
                _tipoCte = value;
                PropriedadeAlterada();
            }
        }

        public TipoEmissao TipoEmissao
        {
            get => _tipoEmissao;
            set
            {
                _tipoEmissao = value;
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

                IsTransportePessoas = value == TipoServico.TransportePessoas;
            }
        }

        public TipoFretamento TipoFretamento
        {
            get => _tipoFretamento;
            set
            {
                if (value == _tipoFretamento) return;
                _tipoFretamento = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? ViagemEm
        {
            get => _viagemEm;
            set
            {
                if (Nullable.Equals(value, _viagemEm)) return;
                _viagemEm = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTransportePessoas
        {
            get => _isTransportePessoas;
            set
            {
                if (value == _isTransportePessoas) return;
                _isTransportePessoas = value;
                PropriedadeAlterada();

                TipoFretamento = TipoFretamento.Eventual;
                ViagemEm = null;
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

        public PerfilCfopDTO PerfilCfop
        {
            get => _perfilCfop;
            set
            {
                if (Equals(value, _perfilCfop)) return;
                _perfilCfop = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? EmitidaEm
        {
            get => _emitidaEm;
            set
            {
                if (value.Equals(_emitidaEm)) return;
                _emitidaEm = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoTabelaIbpt
        {
            get => _descricaoTabelaIbpt;
            set
            {
                if (value == _descricaoTabelaIbpt) return;
                _descricaoTabelaIbpt = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO InicioEstado
        {
            get => _inicioEstado;
            set
            {
                _inicioEstado = value;
                PropriedadeAlterada();
                InicioCidades = new ObservableCollection<CidadeDTO>(LocalidadesServico.GetInstancia(false).GetCidades(x => x.SiglaUf == value.Sigla));
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

        public EstadoDTO FinalEstado
        {
            get => _finalEstado;
            set
            {
                _finalEstado = value;
                PropriedadeAlterada();
                FinalCidades = new ObservableCollection<CidadeDTO>(LocalidadesServico.GetInstancia(false).GetCidades(x => x.SiglaUf == value.Sigla));
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

        public string Observacao
        {
            get => _observacao;
            set
            {
                if (value == _observacao) return;
                _observacao = value;
                PropriedadeAlterada();
            }
        }

        public string NaturezaOperacao
        {
            get { return _naturezaOperacao; }
            set
            {
                if (value == _naturezaOperacao) return;
                _naturezaOperacao = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> InicioEstados
        {
            get { return _inicioEstados; }
            set
            {
                if (Equals(value, _inicioEstados)) return;
                _inicioEstados = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> FinalEstados
        {
            get { return _finalEstados; }
            set
            {
                if (Equals(value, _finalEstados)) return;
                _finalEstados = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> InicioCidades
        {
            get { return _inicioCidades; }
            set
            {
                if (Equals(value, _inicioCidades)) return;
                _inicioCidades = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> FinalCidades
        {
            get { return _finalCidades; }
            set
            {
                if (Equals(value, _finalCidades)) return;
                _finalCidades = value;
                PropriedadeAlterada();
            }
        }


        public decimal SerieDocumento
        {
            get { return _serieDocumento; }
            set
            {
                _serieDocumento = value;
                PropriedadeAlterada();
            }
        }

        public decimal NumeroDocumento
        {
            get { return _numeroDocumento; }
            set
            {
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandAlocarProximoNumero => GetSimpleCommand(AlocarProximoNumeroAction);

        private void AlocarProximoNumeroAction(object obj)
        {
            if (NumeroDocumento == 0 && SerieDocumento == 0)
            {
                DialogBox.MostraInformacao("Como ainda não possui número, ele será alocado ao emitir.");
                return;
            }

            OnAlocarNumeracaoFiscal();
        }

        public void Inicializa()
        {
            var localidades = LocalidadesServico.GetInstancia();

            InicioEstados = new ObservableCollection<EstadoDTO>(localidades.GetEstados());
            FinalEstados = new ObservableCollection<EstadoDTO>(localidades.GetEstados());
        }

        public void ComInserirCte(CteOs cteOs)
        {
            TipoEmissao = cteOs.TipoEmissao;
            TipoCte = cteOs.Perfil.TipoCte;
            TipoServico = cteOs.Perfil.TipoServico;
            Modal = cteOs.Modal;
            ValorServico = cteOs.PrecoServico.Valor;
            ValorAReceber = cteOs.PrecoServico.AReceber;
            PerfilCfop = cteOs.Perfil.PerfilCfop;
            EmitidaEm = DateTime.Now;
            Observacao = cteOs.Perfil.Observacao;
            InicioEstado = cteOs.Perfil?.LocalInicialPrestacao?.EstadoUF ?? cteOs.Perfil.EmissorFiscal.Empresa.EstadoDTO;
            InicioCidade = cteOs.Perfil?.LocalInicialPrestacao?.Cidade ?? cteOs.Perfil.EmissorFiscal.Empresa.CidadeDTO;
            FinalEstado = cteOs.Perfil?.LocalFinalPrestacao?.EstadoUF ?? cteOs.Perfil.EmissorFiscal.Empresa.EstadoDTO;
            FinalCidade = cteOs.Perfil?.LocalFinalPrestacao?.Cidade ?? cteOs.Perfil.EmissorFiscal.Empresa.CidadeDTO;
            NaturezaOperacao = cteOs.Perfil.NaturezaOperacao;
            Ibpt = cteOs.Perfil.Ibpt;
            DescricaoTabelaIbpt = Ibpt?.Descricao;
            TipoFretamento = cteOs.TipoFretamento;
            ViagemEm = cteOs.ViagemEm;
        }

        public void ComAlteracaoCte(CteOs cteOs)
        {
            TipoCte = cteOs.Tipo;
            TipoServico = cteOs.Servico;
            Modal = cteOs.Modal;
            ValorServico = cteOs.PrecoServico.Valor;
            ValorAReceber = cteOs.PrecoServico.AReceber;
            PerfilCfop = cteOs.PerfilCfop;
            EmitidaEm = cteOs.EmissaoEm;
            Observacao = cteOs.Observacao;
            InicioEstado = cteOs.LocalInicialPrestacao.EstadoUF;
            InicioCidade = cteOs.LocalInicialPrestacao.Cidade;
            FinalEstado = cteOs.LocalFinalPrestacao.EstadoUF;
            FinalCidade = cteOs.LocalFinalPrestacao.Cidade;
            NaturezaOperacao = cteOs.NaturezaOperacao;
            SerieDocumento = cteOs.SerieEmissao;
            NumeroDocumento = cteOs.NumeroEmissao;
            TipoEmissao = cteOs.TipoEmissao;
            TipoFretamento = cteOs.TipoFretamento;
            ViagemEm = cteOs.ViagemEm;
            //Ibpt = cteOs.Tributacao.ValorIbpt
            //DescricaoTabelaIbpt = Ibpt?.Descricao;
        }

        public ICommand CommandBuscarIbpt => GetSimpleCommand(BuscarIbptAction);

        public ICommand CommandLimpaIbpt => GetSimpleCommand(LimpaIbptAction);

        public ICommand CommandBuscarPerfilCfop => GetSimpleCommand(BuscarPerfilCfopAction);

        private void BuscarPerfilCfopAction(object obj)
        {
            var model = new PerfilCfopPickerModel();
            model.PickItemEvent += RecebePerfilCfop;
            model.GetPickerView().ShowDialog();
        }

        private void RecebePerfilCfop(object sender, GridPickerEventArgs e)
        {
            PerfilCfop = e.GetItem<PerfilCfopDTO>();
        }


        private void LimpaIbptAction(object obj)
        {
            DescricaoTabelaIbpt = string.Empty;
            Ibpt = null;
        }

        private void BuscarIbptAction(object obj)
        {
            var model = new IbptPicker();
            model.PickItemEvent += SelecionarIpbt;

            model.GetPickerView().ShowDialog();
        }

        private void SelecionarIpbt(object sender, GridPickerEventArgs e)
        {
            Ibpt = e.GetItem<Ibpt>();

            if (Ibpt == null) throw new ArgumentException("IBPT invalido");

            DescricaoTabelaIbpt = Ibpt.Descricao;
        }

        public ICommand CommandProximoPasso => GetSimpleCommand(ProximoPassoAction);


        private void ProximoPassoAction(object obj)
        {
            try
            {
                if (InicioEstado == null) 
                    throw new InvalidOperationException("Selecionar um UF de início");

                if (InicioCidade == null)
                    throw new InvalidOperationException("Selecionar uma Cidade de início");

                if (FinalEstado == null)
                    throw new InvalidOperationException("Selecionar um UF final");

                if (FinalCidade == null)
                    throw new InvalidOperationException("Selecionar uma cidade final");

                if (PerfilCfop == null)
                    throw new InvalidOperationException("Selecionar um cfop");

                if (EmitidaEm == null)
                    throw new InvalidOperationException("Selecionar uma data de emissão");


                OnProximo();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        protected virtual void OnProximo()
        {
            Proximo?.Invoke(this, this);
        }

        protected virtual void OnAlocarNumeracaoFiscal()
        {
            AlocarNumeracaoFiscal?.Invoke(this, this);
        }
    }
}