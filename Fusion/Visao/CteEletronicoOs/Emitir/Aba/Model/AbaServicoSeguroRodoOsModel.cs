using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts;
using Fusion.Visao.Veiculos;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public sealed class AbaServicoSeguroRodoOsModel : AbaCTeOSViewModel
    {
        private ObservableCollection<GridSeguro> _listaSeguro;
        private GridSeguro _seguroSelecionada;
        private Veiculo _veiculo;
        private string _veiculoNomeProprietario;
        private string _veiculoDocumentoUnicoProprietario;
        private string _taf;
        private string _numeroRegistroEstadual;
        private string _descricaoServicoPrestado;
        private decimal? _quantidadePassageirosOuVolumes = 0.0m;
        private string _tituloServico;
        private readonly CteOsEmitirFormModel _cteOsEmitirModel;
        private ObservableCollection<GridComponenteValorPrestacaoCteOs> _componentes;
        private GridComponenteValorPrestacaoCteOs _componenteSelecionado;
        private ObservableCollection<GridDocumentoReferenciadoCteOs> _documentosReferenciados;
        private GridDocumentoReferenciadoCteOs _documentoReferenciadoSelecionado;

        public AbaServicoSeguroRodoOsModel(CteOsEmitirFormModel cteOsEmitirModel)
        {
            _cteOsEmitirModel = cteOsEmitirModel;

            ListaSeguro = new ObservableCollection<GridSeguro>();
            Percursos = new ObservableCollection<CteOsPercurso>();
        }

        public ICommand CommandPassoAnterior => GetSimpleCommand(PassoAnteriorAction);
        public ICommand CommandEmitir => GetSimpleCommand(EmitirAction);
        public ICommand CommandAdicionarSeguro => GetSimpleCommand(AdicionarSeguroAction);
        public ICommand CommandAdicionarVeiculo => GetSimpleCommand(AdicionarVeiculoAction);
        public ICommand CommandLimparVeiculo => GetSimpleCommand(LimparVeiculoAction);
        public ICommand CommandAdicionarPercurso => GetSimpleCommand(AdicionarPercursoAction);
        public ICommand CommandComponenteValorPrestacao => GetSimpleCommand(ComponenteValorPrestacaoAction);
        public ICommand CommandDocumentoReferenciado => GetSimpleCommand(DocumentoReferenciado);


        public string TituloServico
        {
            get => _tituloServico;
            set
            {
                _tituloServico = value;
                PropriedadeAlterada();
            }
        }

        public Veiculo Veiculo
        {
            get => _veiculo;
            set
            {
                _veiculo = value;
                PropriedadeAlterada();

                PreencherVeiculo();
            }
        }

        public string VeiculoNomeProprietario
        {
            get => _veiculoNomeProprietario;
            set
            {
                if (value == _veiculoNomeProprietario) return;
                _veiculoNomeProprietario = value;
                PropriedadeAlterada();
            }
        }

        public string VeiculoDocumentoUnicoProprietario
        {
            get => _veiculoDocumentoUnicoProprietario;
            set
            {
                if (value == _veiculoDocumentoUnicoProprietario) return;
                _veiculoDocumentoUnicoProprietario = value;
                PropriedadeAlterada();
            }
        }

        public string Taf
        {
            get => _taf;
            set
            {
                if (value == _taf) return;
                _taf = value;
                PropriedadeAlterada();
                OnSalvarModalRodoviario();
            }
        }

        public string NumeroRegistroEstadual
        {
            get => _numeroRegistroEstadual;
            set
            {
                if (value == _numeroRegistroEstadual) return;
                _numeroRegistroEstadual = value;
                PropriedadeAlterada();
                OnSalvarModalRodoviario();
            }
        }

        public string DescricaoServicoPrestado
        {
            get => _descricaoServicoPrestado;
            set
            {
                if (value == _descricaoServicoPrestado) return;
                _descricaoServicoPrestado = value;
                PropriedadeAlterada();
                OnSalvarCTeNormal();
            }
        }

        public decimal? QuantidadePassageirosOuVolumes
        {
            get => _quantidadePassageirosOuVolumes;
            set
            {
                if (value == _quantidadePassageirosOuVolumes) return;
                _quantidadePassageirosOuVolumes = value;
                PropriedadeAlterada();
                OnSalvarCTeNormal();
            }
        }

        public ObservableCollection<GridSeguro> ListaSeguro
        {
            get => _listaSeguro;
            set
            {
                if (Equals(value, _listaSeguro)) return;
                _listaSeguro = value;
                PropriedadeAlterada();
            }
        }

        public GridSeguro SeguroSelecionada
        {
            get => _seguroSelecionada;
            set
            {
                if (Equals(value, _seguroSelecionada)) return;
                _seguroSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CteOsPercurso> Percursos
        {
            get => GetValue<ObservableCollection<CteOsPercurso>>();
            set => SetValue(value);
        }

        public CteOsPercurso PercursoSelecionado
        {
            get => GetValue<CteOsPercurso>();
            set => SetValue(value);
        }

        public ObservableCollection<GridComponenteValorPrestacaoCteOs> Componentes
        {
            get => _componentes;
            set
            {
                if (Equals(value, _componentes)) return;
                _componentes = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridDocumentoReferenciadoCteOs> DocumentosReferenciados
        {
            get => _documentosReferenciados;
            set
            {
                if (Equals(value, _documentosReferenciados)) return;
                _documentosReferenciados = value;
                PropriedadeAlterada();
            }
        }

        public GridComponenteValorPrestacaoCteOs ComponenteSelecionado
        {
            get => _componenteSelecionado;
            set
            {
                if (Equals(value, _componenteSelecionado)) return;
                _componenteSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public GridDocumentoReferenciadoCteOs DocumentoReferenciadoSelecionado
        {
            get => _documentoReferenciadoSelecionado;
            set
            {
                if (Equals(value, _documentoReferenciadoSelecionado)) return;
                _documentoReferenciadoSelecionado = value;
                PropriedadeAlterada();
            }
        }

        private void ComponenteValorPrestacaoAction(object obj)
        {
            _cteOsEmitirModel.FlyoutAddComponenteCteOsModel = new FlyoutAddComponenteCteOsModel(_cteOsEmitirModel)
            {
                IsOpen = true
            };
        }

        private void DocumentoReferenciado(object obj)
        {
            _cteOsEmitirModel.FlyoutAddDocumentoReferenciadoCteOsModel = new FlyoutAddDocumentoReferenciadoCteOsModel(_cteOsEmitirModel)
            {
                IsOpen = true
            };
        }

        public event EventHandler<AbaServicoSeguroRodoOsModel> Anterior;
        public event EventHandler<AbaServicoSeguroRodoOsModel> Emitir;
        public event EventHandler AdicioanrSeguro;
        public event EventHandler<AbaServicoSeguroRodoOsModel> SeguroDeletadoHandler;
        public event EventHandler<AbaServicoSeguroRodoOsModel> AdicionarVeiculoHandler;
        public event EventHandler<AbaServicoSeguroRodoOsModel> DeletarVeiculoHandler;
        public event EventHandler<AbaServicoSeguroRodoOsModel> SalvarModalRodoviario;
        public event EventHandler<AbaServicoSeguroRodoOsModel> SalvarCTeNormal;

        private void PassoAnteriorAction(object obj)
        {
            OnAnterior();
        }

        private void OnAnterior()
        {
            Anterior?.Invoke(this, this);
        }

        private void EmitirAction(object obj)
        {
            try
            {
                OnEmitir();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void OnEmitir()
        {
            Emitir?.Invoke(this, this);
        }

        private void AdicionarSeguroAction(object obj)
        {
            OnAdicioanrSeguro();
        }

        private void OnAdicioanrSeguro()
        {
            AdicioanrSeguro?.Invoke(this, EventArgs.Empty);
        }

        public void AdicioanrSeguroLista(GridSeguro seguro)
        {
            ListaSeguro.Add(seguro);
        }

        private void AdicionarVeiculoAction(object obj)
        {
            var pickerModel = new VeiculoPickerModel();
            pickerModel.PickItemEvent += VeiculoSelecionadoCompleted;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void VeiculoSelecionadoCompleted(object sender, GridPickerEventArgs e)
        {
            Veiculo = e.GetItem<Veiculo>();
            OnAdicionarVeiculoHandler();
        }

        public void ExcluirSeguro()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCteOs(sessao);

                repositorio.Deletar(SeguroSelecionada.CteOsSeguro);

                transacao.Commit();
            }

            ListaSeguro.Remove(SeguroSelecionada);

            OnSeguroDeletadoHandler();
        }

        private void PreencherVeiculo()
        {
            if (_veiculo?.TipoProprietario != TipoPropriedadeVeiculo.Terceiro)
            {
                return;
            }

            var proprietario = _veiculo.CarregaProprietario();

            VeiculoNomeProprietario = proprietario.Nome;
            VeiculoDocumentoUnicoProprietario = proprietario.DocumentoUnico;
        }

        private void LimparVeiculoAction(object obj)
        {
            OnDeletarVeiculoHandler();
            Veiculo = null;
            VeiculoNomeProprietario = string.Empty;
            VeiculoDocumentoUnicoProprietario = string.Empty;
        }

        private void OnSeguroDeletadoHandler()
        {
            SeguroDeletadoHandler?.Invoke(this, this);
        }

        private void OnAdicionarVeiculoHandler()
        {
            AdicionarVeiculoHandler?.Invoke(this, this);
        }

        private void OnDeletarVeiculoHandler()
        {
            DeletarVeiculoHandler?.Invoke(this, this);
        }

        public void OnSalvarModalRodoviario()
        {
            SalvarModalRodoviario?.Invoke(this, this);
        }

        public void OnSalvarCTeNormal()
        {
            SalvarCTeNormal?.Invoke(this, this);
        }

        public void SetNormalSilencioso(string descricaoServicoPrestado, decimal quantidadePassageirosVolumes)
        {
            _descricaoServicoPrestado = descricaoServicoPrestado;
            _quantidadePassageirosOuVolumes = quantidadePassageirosVolumes;

            PropriedadeAlterada(nameof(DescricaoServicoPrestado));
            PropriedadeAlterada(nameof(QuantidadePassageirosOuVolumes));
        }

        public void SetRodoviarioSilencioso(string taf, string numeroDoRegimeEstadual)
        {
            _taf = taf;
            _numeroRegistroEstadual = numeroDoRegimeEstadual;

            PropriedadeAlterada(nameof(Taf));
            PropriedadeAlterada(nameof(NumeroRegistroEstadual));
        }

        private void AdicionarPercursoAction(object obj)
        {
            _cteOsEmitirModel.FlyoutAddPercursoModel = new FlyoutAddPercursoModel(_cteOsEmitirModel)
            {
                IsOpen = true
            };
        }

        public void ExcluirPercurso()
        {
            _cteOsEmitirModel.RemovePercurso(PercursoSelecionado);
        }

        public void ExcluirComponente()
        {
            _cteOsEmitirModel.RemoveComponente(ComponenteSelecionado.Componente);
        }

        public void ExcluirDocumentoReferenciado()
        {
            _cteOsEmitirModel.RemoveDocumentoReferenciado(DocumentoReferenciadoSelecionado.DocumentoReferenciado);
        }
    }
}