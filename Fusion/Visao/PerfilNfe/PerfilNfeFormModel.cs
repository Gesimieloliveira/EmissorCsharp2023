using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Veiculos;
using FusionCore.Core.Flags;
using FusionCore.Core.Tributario;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionLibrary.Command;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using JetBrains.Annotations;
using PerfilNfeModel = FusionCore.FusionAdm.Fiscal.NF.Perfil.PerfilNfe;

// ReSharper disable MemberCanBePrivate.Global

namespace Fusion.Visao.PerfilNfe
{
    public class PerfilNfeFormModel : ViewModel
    {
        private ICommand _commandBuscaDestinatario;
        private ICommand _commandLimpaDestinatario;
        private ICommand _commandBuscaTransportadora;
        private ICommand _commandLimpaTransportadora;
        private ICommand _commandBuscaVeiculoTransportadora;
        private ICommand _commandLimpaVeiculoTransportadora;
        private TipoOperacao _tipoOperacao;
        private FinalidadeEmissao _finalidadeEmissao;
        private string _observacao;
        private Cliente _destinatario;
        private Transportadora _transportadora;
        private Veiculo _veiculo;
        private readonly RecipienteCsosn _recipienteCsosn = RecipienteFactory.Get<RecipienteCsosn>();
        private ObservableCollection<EmissorFiscalComboBox> _listaEmissorFiscal;
        private readonly PerfilNfeModel _objetoEdicao;
        private bool _movimentarEstoqueProduto = true;
        private bool _usarIpiTagPropria;

        public PerfilNfeFormModel()
        {
            AutoAtivarPartilhaIcms = true;
            FinalidadeEmissao = FinalidadeEmissao.Normal;
            TipoOperacao = TipoOperacao.Saida;
        }

        public PerfilNfeFormModel(PerfilNfeModel perfilNfe) : this()
        {
            _objetoEdicao = perfilNfe;
        }

        public PerfilNfeModel Model
        {
            get => GetValue<PerfilNfeModel>();
            set => SetValue(value);
        }

        public ICommand CommandBuscaDestinatario
        {
            get
            {
                return _commandBuscaDestinatario ??
                       (
                           _commandBuscaDestinatario = new SimpleCommand
                           {
                               CanExecuteDelegate = x => true,
                               ExecuteDelegate = x =>
                               {
                                   var model = new PessoaPickerModel(new ClienteEngine());
                                   model.PickItemEvent += DestinatarioPickHandler;
                                   model.GetPickerView().ShowDialog();
                               }
                           });
            }
        }

        public ICommand CommandLimpaDestinatario
        {
            get
            {
                return _commandLimpaDestinatario ??
                       (_commandLimpaDestinatario = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               Destinatario = null;
                               Model.Destinatario = null;
                           }
                       });
            }
        }

        public ICommand CommandBuscaTransportadora
        {
            get
            {
                return _commandBuscaTransportadora ??
                       (
                           _commandBuscaTransportadora = new SimpleCommand
                           {
                               CanExecuteDelegate = x => true,
                               ExecuteDelegate = x =>
                               {
                                   var model = new PessoaPickerModel(new TransportadoraEngine());
                                   model.PickItemEvent += TransportadoraPickerHandler;
                                   model.GetPickerView().ShowDialog();
                               }
                           });
            }
        }

        public ICommand CommandLimpaTransportadora
        {
            get
            {
                return _commandLimpaTransportadora ??
                       (_commandLimpaTransportadora = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               Transportadora = null;
                               Model.Transportadora = null;
                           }
                       });
            }
        }

        public ICommand CommandBuscaVeiculoTransportadora
        {
            get
            {
                return _commandBuscaVeiculoTransportadora ??
                       (
                           _commandBuscaVeiculoTransportadora = new SimpleCommand
                           {
                               CanExecuteDelegate = x => true,
                               ExecuteDelegate = x =>
                               {
                                   var model = new VeiculoPickerModel();

                                   var pickerCfop = new GridPicker(model);
                                   pickerCfop.ShowDialog();

                                   if (model.ItemSelecionado == null)
                                       return;

                                   Veiculo = model.ItemSelecionado.ItemReal as Veiculo;
                               }
                           });
            }
        }

        public ICommand CommandLimpaVeiculoTransportadora
        {
            get
            {
                return _commandLimpaVeiculoTransportadora ??
                       (_commandLimpaVeiculoTransportadora = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x => { Veiculo = null; }
                       });
            }
        }

        [Required(ErrorMessage = @"Emissor Fiscal é obrigatório")]
        public EmissorFiscalComboBox EmissorSelecionado
        {
            get => GetValue<EmissorFiscalComboBox>();
            set => SetValue(value);
        }
        public int Id
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Descrição é obrigatória")]
        public string Descricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Natureza da Operação é obrigatória")]
        public string NaturezaOperacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TipoOperacao TipoOperacao
        {
            get => _tipoOperacao;
            set
            {
                if (value == _tipoOperacao) return;
                _tipoOperacao = value;
                PropriedadeAlterada();
            }
        }

        public FinalidadeEmissao FinalidadeEmissao
        {
            get => _finalidadeEmissao;
            set
            {
                if (value == _finalidadeEmissao) return;
                _finalidadeEmissao = value;
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

        public Cliente Destinatario
        {
            get => _destinatario;
            set
            {
                if (Equals(value, _destinatario)) return;
                _destinatario = value;
                DestinatarioNome = value?.Nome;
            }
        }

        public string DestinatarioNome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Transportadora Transportadora
        {
            get => _transportadora;
            set
            {
                if (Equals(value, _transportadora)) return;
                _transportadora = value;
                TransportadoraNome = value?.Nome;
            }
        }

        public string TransportadoraNome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Veiculo Veiculo
        {
            get => _veiculo;
            set
            {
                if (Equals(value, _veiculo)) return;
                _veiculo = value;
                VeiculoPlaca = value?.Placa;
                VeiculoDescricao = value?.Descricao;
            }
        }

        public string VeiculoPlaca
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string VeiculoDescricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscalComboBox> ListaEmissorFiscal
        {
            get => _listaEmissorFiscal;
            set
            {
                if (Equals(value, _listaEmissorFiscal)) return;
                _listaEmissorFiscal = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<PerfilCfopDTO> ListaCfops
        {
            get => GetValue<ObservableCollection<PerfilCfopDTO>>();
            set => SetValue(value);
        }

        public decimal AliquotaCredito
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public ObservableCollection<TributacaoCsosn> TributacoesCsosn
        {
            get => GetValue<ObservableCollection<TributacaoCsosn>>();
            set => SetValue(value);
        }

        public TributacaoCsosn TributacaoCsosn
        {
            get => GetValue<TributacaoCsosn>();
            set => SetValue(value);
        }

        public bool UtilizaCsosn
        {
            get => GetValue<bool>();
            set
            {
                if (value == false)
                    TributacaoCsosn = null;

                SetValue(value);
            }
        }

        public bool DesativarInfoCreditoItem
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [CanBeNull]
        public PerfilCfopDTO PerfilCfop
        {
            get => GetValue<PerfilCfopDTO>();
            set => SetValue(value);
        }

        public bool AutoAtivarPartilhaIcms
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool MovimentarEstoqueProduto
        {
            get => _movimentarEstoqueProduto;
            set
            {
                if (value == _movimentarEstoqueProduto) return;
                _movimentarEstoqueProduto = value;
                PropriedadeAlterada();
            }
        }

        public bool UsarIpiTagPropria
        {
            get => _usarIpiTagPropria;
            set
            {
                _usarIpiTagPropria = value;
                PropriedadeAlterada();
            }
        }

        public void Inicializar()
        {
            CarregaListasParaComboBox();

            if (_objetoEdicao != null)
            {
                Model = _objetoEdicao;
                CarregaDados();
                return;
            }

            Model = new PerfilNfeModel();
            CarregaDados();
        }

        private void CarregaDados()
        {
            EmissorSelecionado = ConvertParaEmissorComboBox(Model.EmissorFiscal);
            Id = Model.Id;
            Descricao = Model.Descricao;
            TipoOperacao = Model.TipoOperacao;
            FinalidadeEmissao = Model.FinalidadeEmissao;
            NaturezaOperacao = Model.NaturezaOperacao;
            Observacao = Model.Observacao;
            Destinatario = Model.Destinatario?.Destinatario;
            Transportadora = Model.Transportadora?.Transportadora;
            Veiculo = Model.Transportadora?.Veiculo;
            PerfilCfop = Model.Cfop;
            AutoAtivarPartilhaIcms = Model.AutoAtivarPartilhaIcms;
            MovimentarEstoqueProduto = Model.MovimentarEstoqueProduto;
            UsarIpiTagPropria = Model.UsarIpiTagPropria;
            DesativarInfoCreditoItem = Model.DesativarInfoCreditoItem;

            if (Model.SimplesNacional != null)
            {
                UtilizaCsosn = Model.SimplesNacional.Csosn != null;
                TributacaoCsosn = Model.SimplesNacional.Csosn;
                AliquotaCredito = Model.SimplesNacional.AliquotaCredito;
               
            }
        }

        private void TransportadoraPickerHandler(object sender, GridPickerEventArgs e)
        {
            Transportadora = e.GetItem<Transportadora>();
        }

        private void DestinatarioPickHandler(object sender, GridPickerEventArgs e)
        {
            Destinatario = e.GetItem<Cliente>();
        }

        public void CarregaListasParaComboBox()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissores = new RepositorioEmissorFiscal(sessao).BuscaTodosQueSejamNfeParaComboBox();
                var cfops = new RepositorioPerfilCfop(sessao).BuscaTodos();

                ListaEmissorFiscal = new ObservableCollection<EmissorFiscalComboBox>(emissores);
                ListaCfops = new ObservableCollection<PerfilCfopDTO>(cfops);
            }

            TributacoesCsosn = new ObservableCollection<TributacaoCsosn>(_recipienteCsosn.GetTodos());
        }

        private static EmissorFiscalComboBox ConvertParaEmissorComboBox(EmissorFiscal emissorFiscal)
        {
            if (emissorFiscal == null)
                return null;

            return new EmissorFiscalComboBox
            {
                Id = emissorFiscal.Id,
                Descricao = emissorFiscal.Descricao
            };
        }

        public void SalvaAlteracoes()
        {
            ThrowExceptionSeExistirErros();
            CheckConfiguracaoCfop();

            Model.Ativo = true;
            Model.EmissorFiscal = GetEmissorFiscal();
            Model.Descricao = Descricao;
            Model.TipoOperacao = TipoOperacao;
            Model.FinalidadeEmissao = FinalidadeEmissao;
            Model.NaturezaOperacao = NaturezaOperacao;
            Model.Observacao = Observacao ?? string.Empty;
            Model.SimplesNacional = GetConfiguracoesSimplesNacional();
            Model.Cfop = PerfilCfop;
            Model.AutoAtivarPartilhaIcms = AutoAtivarPartilhaIcms;
            Model.MovimentarEstoqueProduto = MovimentarEstoqueProduto;
            Model.UsarIpiTagPropria = UsarIpiTagPropria;
            Model.DesativarInfoCreditoItem = DesativarInfoCreditoItem;
            

            if (Destinatario != null && Destinatario.Id != 0)
            {
                Model.Destinatario = new PerfilNfeDestinatario(Model, Destinatario);
            }

            if (Transportadora != null && Transportadora.Id != 0)
            {
                Model.Transportadora = new PerfilNfeTransportadora(Model, Transportadora, Veiculo);
            }

            if (Veiculo != null && Veiculo.Id != 0)
            {
                Model.Transportadora = new PerfilNfeTransportadora(Model, Transportadora, Veiculo);
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                new RepositorioPerfilNfe(sessao).SalvarAlteracoes(Model);
                transacao.Commit();
            }
        }

        private void CheckConfiguracaoCfop()
        {
            if (PerfilCfop == null)
            {
                return;
            }

            if (PerfilCfop.Operacao == OpercaoCfop.Entrada && TipoOperacao != TipoOperacao.Entrada)
            {
                throw new InvalidOperationException("Para operações de ENTRADA utilize um CFOP de ENTRADA");
            }

            if (PerfilCfop.Operacao == OpercaoCfop.Saida && TipoOperacao != TipoOperacao.Saida)
            {
                throw new InvalidOperationException("Para operações de SAIDA utilize um CFOP de SAIDA");
            }
        }

        private PerfilNfeSimplesNacional GetConfiguracoesSimplesNacional()
        {
            if (TributacaoCsosn == null && UtilizaCsosn)
            {
                throw new InvalidOperationException("Você marcou para utilizar CSOSN, preciso que informe um CSOSN.");
            }

            var perfilsn = new PerfilNfeSimplesNacional
            {
                Parent = Model,
                Csosn = TributacaoCsosn,
                AliquotaCredito = AliquotaCredito              
            };

            return perfilsn;
        }

        private EmissorFiscal GetEmissorFiscal()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                return repositorio.GetPeloId(EmissorSelecionado.Id);
            }
        }

        public void DeletaRegistro()
        {
            if (Model.Id == 0)
            {
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                new RepositorioPerfilNfe(sessao).Deleta(Model);
            }
        }
    }
}