using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DFe.Ext;
using Fusion.Sessao;
using Fusion.Visao.PerfilCfop;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Validacoes.CteOs;
using Fusion.Visao.Veiculos;
using FusionCore.Excecoes;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronicoOs;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.TabelaIbpt;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags;

namespace Fusion.Visao.CteEletronicoOs.Perfil
{
    public class PerfilCteOsFormModel : ViewModel
    {
        public PerfilCteOsFormModel(PerfilCteOs perfilCteOs)
        {
            PerfilCteOs = perfilCteOs;

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsGerenciarEmissorFiscal = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_EMISSOR_FISCAL_ELETRONICO);
        }

        private PerfilCteOs PerfilCteOs { get; set; }

        private string _descricao;
        private EmissorFiscalComboBox _emissorSelecionado;
        private string _codigoPerfilCfop;
        private string _descricaoPerfilCfop;
        private string _naturezaOperacao;
        private TipoCte _tipoCte;
        private TipoServico.TipoServico _tipoServico;
        private string _observacao;
        private string _veiculoPlaca;
        private string _veiculoDescricao;
        private ResponsavelSeguro _responsavelSeguro;
        private string _nomeSeguradora;
        private string _numeroApolice;
        private string _tomadorNome;
        private string _documentoUnicoTomador;
        private string _ruaTomador;
        private ObservableCollection<EstadoDTO> _inicioEstados;
        private EstadoDTO _inicioEstado;
        private ObservableCollection<CidadeDTO> _inicioCidades;
        private CidadeDTO _inicioCidade;
        private ObservableCollection<EstadoDTO> _finalEstados;
        private EstadoDTO _finalEstado;
        private ObservableCollection<CidadeDTO> _finalCidades;
        private CidadeDTO _finalCidade;
        private PerfilCfopDTO _perfilCfop;
        private Veiculo _veiculo;
        private PessoaEntidade _tomador;
        private string _taf;
        private string _numeroRegistroEstadual;
        private string _descricaoTabelaIbpt;
        private string _descricaoServico;
        private decimal _quantidadePassageirosVolumes;
        private bool _isGerenciarEmissorFiscal;

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                if (value == _descricao) return;
                _descricao = value;
                PropriedadeAlterada();
            }
        }


        public EmissorFiscalComboBox EmissorSelecionado
        {
            get { return _emissorSelecionado; }
            set
            {
                if (Equals(value, _emissorSelecionado)) return;
                _emissorSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EmissorFiscalComboBox> ListaEmissorFiscal
        {
            get => GetValue<ObservableCollection<EmissorFiscalComboBox>>();
            set => SetValue(value);
        }

        public string CodigoPerfilCfop
        {
            get { return _codigoPerfilCfop; }
            set
            {
                if (value == _codigoPerfilCfop) return;
                _codigoPerfilCfop = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoPerfilCfop
        {
            get { return _descricaoPerfilCfop; }
            set
            {
                if (value == _descricaoPerfilCfop) return;
                _descricaoPerfilCfop = value;
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

        public TipoCte TipoCte
        {
            get { return _tipoCte; }
            set
            {
                if (value == _tipoCte) return;
                _tipoCte = value;
                PropriedadeAlterada();
            }
        }

        public TipoServico.TipoServico TipoServico
        {
            get { return _tipoServico; }
            set
            {
                if (value == _tipoServico) return;
                _tipoServico = value;
                PropriedadeAlterada();
            }
        }

        public string Observacao
        {
            get { return _observacao; }
            set
            {
                if (value == _observacao) return;
                _observacao = value;
                PropriedadeAlterada();
            }
        }

        public string VeiculoPlaca
        {
            get { return _veiculoPlaca; }
            set
            {
                if (value == _veiculoPlaca) return;
                _veiculoPlaca = value;
                PropriedadeAlterada();
            }
        }

        public string VeiculoDescricao
        {
            get { return _veiculoDescricao; }
            set
            {
                if (value == _veiculoDescricao) return;
                _veiculoDescricao = value;
                PropriedadeAlterada();
            }
        }

        public ResponsavelSeguro ResponsavelSeguro
        {
            get { return _responsavelSeguro; }
            set
            {
                if (value == _responsavelSeguro) return;
                _responsavelSeguro = value;
                PropriedadeAlterada();

                if (_responsavelSeguro != ResponsavelSeguro.Nenhum) return;
                NomeSeguradora = string.Empty;
                NumeroApolice = string.Empty;
            }
        }

        public string NomeSeguradora
        {
            get { return _nomeSeguradora; }
            set
            {
                if (value == _nomeSeguradora) return;
                _nomeSeguradora = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroApolice
        {
            get { return _numeroApolice; }
            set
            {
                if (value == _numeroApolice) return;
                _numeroApolice = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorNome
        {
            get { return _tomadorNome; }
            set
            {
                if (value == _tomadorNome) return;
                _tomadorNome = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoTomador
        {
            get { return _documentoUnicoTomador; }
            set
            {
                if (value == _documentoUnicoTomador) return;
                _documentoUnicoTomador = value;
                PropriedadeAlterada();
            }
        }

        public string RuaTomador
        {
            get { return _ruaTomador; }
            set
            {
                if (value == _ruaTomador) return;
                _ruaTomador = value;
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

        public EstadoDTO InicioEstado
        {
            get { return _inicioEstado; }
            set
            {
                _inicioEstado = value;
                PropriedadeAlterada();

                InicioCidades = BuscarCidadePor(value);
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

        public CidadeDTO InicioCidade
        {
            get { return _inicioCidade; }
            set
            {
                if (Equals(value, _inicioCidade)) return;
                _inicioCidade = value;
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

        public EstadoDTO FinalEstado
        {
            get { return _finalEstado; }
            set
            {
                _finalEstado = value;
                PropriedadeAlterada();

                FinalCidades = BuscarCidadePor(value);
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

        public CidadeDTO FinalCidade
        {
            get { return _finalCidade; }
            set
            {
                if (Equals(value, _finalCidade)) return;
                _finalCidade = value;
                PropriedadeAlterada();
            }
        }

        public PerfilCfopDTO PerfilCfop
        {
            get { return _perfilCfop; }
            set
            {
                _perfilCfop = value;

                if (_perfilCfop == null) return;

                DescricaoPerfilCfop = _perfilCfop.Descricao;
                CodigoPerfilCfop = _perfilCfop.Codigo;
            }
        }

        public string Taf
        {
            get { return _taf; }
            set
            {
                if (value == _taf) return;
                _taf = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroRegistroEstadual
        {
            get { return _numeroRegistroEstadual; }
            set
            {
                if (value == _numeroRegistroEstadual) return;
                _numeroRegistroEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoTabelaIbpt
        {
            get { return _descricaoTabelaIbpt; }
            set
            {
                if (value == _descricaoTabelaIbpt) return;
                _descricaoTabelaIbpt = value;
                PropriedadeAlterada();
            }
        }

        public string DescricaoServico
        {
            get { return _descricaoServico; }
            set
            {
                if (value == _descricaoServico) return;
                _descricaoServico = value;
                PropriedadeAlterada();
            }
        }

        public decimal QuantidadePassageirosVolumes
        {
            get { return _quantidadePassageirosVolumes; }
            set
            {
                if (value == _quantidadePassageirosVolumes) return;
                _quantidadePassageirosVolumes = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaPerfilCfop => GetSimpleCommand(BuscaPerfilCfopAction);
        public ICommand CommandLimparVeiculo => GetSimpleCommand(LimparVeiculoAction);
        public ICommand CommandBuscaVeiculoTransportadora => GetSimpleCommand(BuscaVeiculoTransportadoraAction);
        public ICommand CommandBuscaTomador => GetSimpleCommand(BuscaTomadorAction);
        public ICommand CommandLimpaTomador => GetSimpleCommand(LimpaTomadorAction);
        public ICommand CommandBuscarIbpt => GetSimpleCommand(BuscarIbptAction);
        public ICommand CommandLimpaIbpt => GetSimpleCommand(LimpaIbptAction);

        private Veiculo Veiculo
        {
            get { return _veiculo; }
            set
            {
                _veiculo = value;

                if (_veiculo == null)
                {
                    VeiculoPlaca = string.Empty;
                    VeiculoDescricao = string.Empty;
                    return;
                }

                VeiculoPlaca = _veiculo.Placa;
                VeiculoDescricao = _veiculo.Descricao;
            }
        }

        private PessoaEntidade Tomador
        {
            get { return _tomador; }
            set
            {
                _tomador = value;
                PreencheInformacoesTomador();
            }
        }

        private Ibpt Ibpt { get; set; }

        public bool IsGerenciarEmissorFiscal
        {
            get => _isGerenciarEmissorFiscal;
            set
            {
                if (value == _isGerenciarEmissorFiscal) return;
                _isGerenciarEmissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        private void BuscaPerfilCfopAction(object obj)
        {
            var model = new PerfilCfopPickerModel();
            model.GetPickerView().ShowDialog();

            if (model.ItemSelecionado == null)
                return;

            var perfilCfopSelecionado = model.ItemSelecionado.ItemReal as PerfilCfopDTO;

            PerfilCfop = perfilCfopSelecionado;
        }

        private void LimparVeiculoAction(object obj)
        {
            Veiculo = null;
        }

        private void BuscaVeiculoTransportadoraAction(object obj)
        {
            var model = new VeiculoPickerModel();
            model.PickItemEvent += VeiculoItemPick;

            model.GetPickerView().ShowDialog();
        }

        private void VeiculoItemPick(object sender, GridPickerEventArgs e)
        {
            Veiculo = e.GetItem<Veiculo>();
        }

        private void BuscaTomadorAction(object obj)
        {
            var model = new PessoaPickerModel(new PessoaEngine());
            model.PickItemEvent += TomadorItemPick;

            model.GetPickerView().ShowDialog();
        }

        private void TomadorItemPick(object sender, GridPickerEventArgs e)
        {
            var pessoa = e.GetItem<PessoaEntidade>();

            ValidacaoTomadorCTeOs.Executar(pessoa);

            Tomador = pessoa;
        }

        private void LimpaTomadorAction(object obj)
        {
            Tomador = null;
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

        private void LimpaIbptAction(object obj)
        {
            DescricaoTabelaIbpt = null;
            Ibpt = null;
        }

        public void Loadded()
        {
            TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico.TransportePessoas;
            InicializaComboBoxEmissorFiscal();
            InicializaComboBoxUfInicio();
            InicializaComboBoxUfFinal();

            if (PerfilCteOs.Id != 0)
            {
                AtualizarModel();
            }
        }

        private void InicializaComboBoxEmissorFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosQueSejamCteOsParaComboBox();

                ListaEmissorFiscal = new ObservableCollection<EmissorFiscalComboBox>(emissores);
            }
        }

        private void InicializaComboBoxUfInicio()
        {
            InicioEstados = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia().GetEstados());
        }

        private void InicializaComboBoxUfFinal()
        {
            FinalEstados = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());
        }

        private void PreencheInformacoesTomador()
        {
            if (_tomador == null)
            {
                TomadorNome = string.Empty;
                RuaTomador = string.Empty;
                DocumentoUnicoTomador = string.Empty;
                return;
            }

            TomadorNome = _tomador.Nome;
            DocumentoUnicoTomador = _tomador.GetDocumentoUnico();

            var endereco = (PessoaEndereco)_tomador.Enderecos.First();

            RuaTomador = new StringBuilder(endereco.Logradouro).Append(", ").Append(endereco.Numero).ToString();
        }

        private ObservableCollection<CidadeDTO> BuscarCidadePor(EstadoDTO value)
        {
            if (value != null)
            {
                return new ObservableCollection<CidadeDTO>(LocalidadesServico.GetInstancia(false)
                    .GetCidades(x => x.SiglaUf == value.Sigla));
            }

            return new ObservableCollection<CidadeDTO>();
        }

        public void Salvar()
        {
            try
            {
                DeixaTipoStringVazia();
                Validacoes();

                PerfilCteOs.Descricao = Descricao;
                PerfilCteOs.EmissorFiscal = BuscarEmissor();
                PerfilCteOs.PerfilCfop = PerfilCfop;
                PerfilCteOs.NaturezaOperacao = NaturezaOperacao;
                PerfilCteOs.TipoCte = TipoCte;
                PerfilCteOs.TipoServico = TipoServico;
                PerfilCteOs.Observacao = Observacao;
                PerfilCteOs.Veiculo = Veiculo;
                PerfilCteOs.Seguro = ObterSeguro(PerfilCteOs);
                PerfilCteOs.Tomador = Tomador;
                PerfilCteOs.LocalInicialPrestacao = MontarLocalInicio();
                PerfilCteOs.LocalFinalPrestacao = MontarLocalFinal();
                PerfilCteOs.Taf = Taf;
                PerfilCteOs.NumeroRegistroEstadual = NumeroRegistroEstadual;
                PerfilCteOs.Ibpt = Ibpt;
                PerfilCteOs.DescricaoServico = DescricaoServico ?? string.Empty;
                PerfilCteOs.QuantidadePassageiroVolume = QuantidadePassageirosVolumes.Arredondar(4);

                Salvar(PerfilCteOs);

                DialogBox.MostraInformacao("Salvei para você!");
                OnFechar();
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

        private void Validacoes()
        {
            if (Descricao.IsNullOrEmpty())
                CriaExcecao.ArgumentoInvalido("Descrição e obrgatório");

            if (EmissorSelecionado == null) 
                CriaExcecao.ArgumentoInvalido("Emissor Fiscal obrigatório");

            if (PerfilCfop == null)
                CriaExcecao.ArgumentoInvalido("Cfop obrigatório");

            if (NaturezaOperacao.IsNullOrEmpty())
                CriaExcecao.ArgumentoInvalido("Nautreza da Operação obrigatório");
        }

        private EmissorFiscal BuscarEmissor()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);

                return repositorio.GetPeloId(EmissorSelecionado.Id);
            }
        }

        private PerfilCteOsSeguro ObterSeguro(PerfilCteOs perfilCteOs)
        {
            if (ResponsavelSeguro == ResponsavelSeguro.Nenhum)
                return null;

            var seguro = new PerfilCteOsSeguro
            {
                Perfil = perfilCteOs,
                NomeSeguradora = NomeSeguradora,
                NumeroApolice = NumeroApolice,
                ResponsavelSeguro = ResponsavelSeguro
            };


            var perfilId = perfilCteOs.Id;
            if (perfilId != 0)
                seguro.PerfilCteOsId = perfilId;

            return seguro;
        }

        private LocalInicialPrestacao MontarLocalInicio()
        {
            var localInicioPrestacao = new LocalInicialPrestacao
            {
                Cidade = InicioCidade,
                EstadoUF = InicioEstado
            };

            return localInicioPrestacao;
        }

        private LocalFinalPrestacao MontarLocalFinal()
        {
            var localFinalPrestacao = new LocalFinalPrestacao
            {
                Cidade = FinalCidade,
                EstadoUF = FinalEstado
            };

            return localFinalPrestacao;
        }

        private static void Salvar(PerfilCteOs perfilCteOs)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPerfilCteOs(sessao);
                repositorio.Salvar(perfilCteOs);

                transacao.Commit();
            }
        }

        private void AtualizarModel()
        {
            Descricao = PerfilCteOs.Descricao;
            EmissorSelecionado = PegaEmissorCompativel(PerfilCteOs.EmissorFiscal);
            PerfilCfop = PerfilCteOs.PerfilCfop;
            NaturezaOperacao = PerfilCteOs.NaturezaOperacao;
            TipoCte = PerfilCteOs.TipoCte;
            TipoServico = PerfilCteOs.TipoServico;
            Observacao = PerfilCteOs.Observacao;
            Veiculo = PerfilCteOs.Veiculo;
            TrataSeguro(PerfilCteOs.Seguro);
            Tomador = PerfilCteOs.Tomador;
            InicioEstado = PerfilCteOs.LocalInicialPrestacao?.EstadoUF;
            FinalEstado = PerfilCteOs.LocalFinalPrestacao?.EstadoUF;
            InicioCidade = PerfilCteOs.LocalInicialPrestacao?.Cidade;
            FinalCidade = PerfilCteOs.LocalFinalPrestacao?.Cidade;
            Taf = PerfilCteOs.Taf;
            NumeroRegistroEstadual = PerfilCteOs.NumeroRegistroEstadual;
            Ibpt = PerfilCteOs.Ibpt;
            DescricaoTabelaIbpt = Ibpt?.Descricao;
            DescricaoServico = PerfilCteOs.DescricaoServico;
            QuantidadePassageirosVolumes = PerfilCteOs.QuantidadePassageiroVolume;
        }

        private EmissorFiscalComboBox PegaEmissorCompativel(EmissorFiscal emissorFiscal)
        {
            return ListaEmissorFiscal.FirstOrDefault(i => i.Id == emissorFiscal.Id);
        }

        private void TrataSeguro(PerfilCteOsSeguro seguro)
        {
            if (seguro == null) return;

            ResponsavelSeguro = seguro.ResponsavelSeguro;
            NomeSeguradora = seguro.NomeSeguradora;
            NumeroApolice = seguro.NumeroApolice;
        }
    }
}