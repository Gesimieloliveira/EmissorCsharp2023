using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Command;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.ValidacaoAnotacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;
using NHibernate.Util;
using TipoCertificadoDigital = FusionCore.FusionAdm.Fiscal.Flags.TipoCertificadoDigital;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public class EmissorFiscalFormModel : ViewModel
    {
        private ObservableCollection<EmpresaDTO> _empresas;
        private ICommand _commandBuscaCertificado;
        private ICommand _commandBuscaCertificadoRepositorio;
        private ICommand _commandLimpaArquivoCertificado;
        private ICommand _commandLimpaSerialNumberCertificado;
        private bool _isContainerValidadeCertificado;
        private TipoCertificadoDigital _tipoCertificadoDigital;
        private bool _isA3;
        private bool _isSenha;
        private bool _isA1;

        private VersaoSat _versaoSat = VersaoSat.V7;
        private bool _isMFe;
        private string _chaveAcessoValidador;
        private bool _isAutorizaBaixarXml;
        private string _descricaoAutorizaBaixarXml;
        private string _documentoUnicoAutorizaBaixarXml;
        private bool _isIntegradorCeara;
        private bool _naoEditar;
        private bool _isFaturamento;
        private bool _editarAmbiente;
        private bool _editar;
        private bool _mensagemEdicaoAmbiente;

        public bool NaoEditar
        {
            get => _naoEditar;
            set
            {
                if (value == _naoEditar) return;
                _naoEditar = value;
                PropriedadeAlterada();
            }
        }

        public EmissorFiscalFormModel(EmissorFiscal emissorFiscal)
        {
            CarregarEmpresas();
            Model = emissorFiscal;
        }

        // ReSharper disable once UnusedMember.Global
        public ICommand CommandBuscaCertificado
        {
            get
            {
                return _commandBuscaCertificado ??
                       (_commandBuscaCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               var janelaArquivo = new OpenFileDialog
                               {
                                   Filter = "Certificado digital(*.pfx)|*.pfx"
                               };
                               if (janelaArquivo.ShowDialog() == true)
                               {
                                   ArquivoCertificado = janelaArquivo.FileName;
                                   IsContainerValidadeCertificado = false;
                               }
                           }
                       });
            }
        }

        // ReSharper disable once UnusedMember.Global
        public ICommand CommandBuscaCertificadoRepositorio
        {
            get
            {
                return _commandBuscaCertificadoRepositorio ??
                       (_commandBuscaCertificadoRepositorio = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               try
                               {
                                   var cert = CertificadoDigitalUtils.ListareObterDoRepositorio();
                                   SerialNumberCertificado = cert.SerialNumber?.Trim();
                                   IsContainerValidadeCertificado = false;
                               }
                               catch (Exception ex)
                               {
                                   DialogBox.MostraInformacao(ex.Message);
                               }
                           }
                       });
            }
        }

        // ReSharper disable once UnusedMember.Global
        public ICommand CommandLimpaArquivoCertificado
        {
            get
            {
                return _commandLimpaArquivoCertificado ??
                       (_commandLimpaArquivoCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               ArquivoCertificado = null;
                               IsContainerValidadeCertificado = false;
                           }
                       });
            }
        }

        public bool IsContainerValidadeCertificado
        {
            get => _isContainerValidadeCertificado;
            set
            {
                if (value == _isContainerValidadeCertificado) return;
                _isContainerValidadeCertificado = value;
                PropriedadeAlterada();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public ICommand CommandLimpaSerialNumberCertificado
        {
            get
            {
                return _commandLimpaSerialNumberCertificado ??
                       (_commandLimpaSerialNumberCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               SerialNumberCertificado = null;
                               IsContainerValidadeCertificado = false;
                           }
                       });
            }
        }


        public string DescricaoAutorizaBaixarXml
        {
            get => _descricaoAutorizaBaixarXml;
            set
            {
                if (value == _descricaoAutorizaBaixarXml) return;
                _descricaoAutorizaBaixarXml = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoUnicoAutorizaBaixarXml
        {
            get => _documentoUnicoAutorizaBaixarXml;
            set
            {
                if (value == _documentoUnicoAutorizaBaixarXml) return;
                _documentoUnicoAutorizaBaixarXml = value;
                PropriedadeAlterada();
            }
        }

        public bool IsIntegradorCeara
        {
            get => _isIntegradorCeara;
            set
            {
                if (value == _isIntegradorCeara) return;
                _isIntegradorCeara = value;
                PropriedadeAlterada();
            }
        }

        public bool IsFaturamento
        {
            get => _isFaturamento;
            set
            {
                if (value == _isFaturamento) return;
                _isFaturamento = value;
                PropriedadeAlterada();
                AtualizarFormCertificadoDigital();
            }
        }

        public ICommand ComandoDeletarAutorizarXml => GetSimpleCommand(AcaoDeletarAutorizarXml);

        private void AcaoDeletarAutorizarXml(object obj)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente deletar?", MessageBoxImage.Question)) return;

            DescricaoAutorizaBaixarXml = string.Empty;
            DocumentoUnicoAutorizaBaixarXml = string.Empty;

            if (Model.Id == 0) return;

            SalvarModel();
            Model.AutorizadoBaixarXml = new AutorizadoBaixarXml();
        }

        public bool IsAutorizaBaixarXml
        {
            get => _isAutorizaBaixarXml;
            set
            {
                if (value == _isAutorizaBaixarXml) return;
                _isAutorizaBaixarXml = value;
                PropriedadeAlterada();
            }
        }

        public bool FlagNfe
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AutorizaBaixarXml();
                SerieNfe = 0;
                AtualizarFormCertificadoDigital();
            }
        }

        public bool FlagNfce
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AutorizaBaixarXml();
                AtualizarFormCertificadoDigital();
            }
        }

        public bool IsMFe
        {
            get => _isMFe;
            set
            {
                if (value == _isMFe) return;
                _isMFe = value;

                if (value)
                {
                    FabricanteModelo = Modelo.Nenhum;

                    if (string.IsNullOrWhiteSpace(ChaveAcessoValidador))
                    {
                        ChaveAcessoValidador = "25CFE38D-3B92-46C0-91CA-CFF751A82D3D";
                    }
                }

                PropriedadeAlterada();
            }
        }

        public TipoCertificadoDigital TipoCertificadoDigital
        {
            get => _tipoCertificadoDigital;
            set
            {
                _tipoCertificadoDigital = value;
                PropriedadeAlterada();
                LimparCertificado();
                HabilitaConformeUsoTipoCertificado(value);
            }
        }

        public bool IsA3
        {
            get => _isA3;
            set
            {
                if (value == _isA3) return;
                _isA3 = value;
                PropriedadeAlterada();
            }
        }

        public bool IsSenha
        {
            get => _isSenha;
            set
            {
                if (value == _isSenha) return;
                _isSenha = value;
                PropriedadeAlterada();
            }
        }

        public bool IsA1
        {
            get => _isA1;
            set
            {
                if (value == _isA1) return;
                _isA1 = value;
                PropriedadeAlterada();
            }
        }

        public bool FlagCte
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AtualizarFormCertificadoDigital();
            }
        }

        public bool FlagCteOs
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AtualizarFormCertificadoDigital();
            }
        }

        public bool FlagMdfe
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AtualizarFormCertificadoDigital();
            }
        }

        public bool FlagSat
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AtualizarFormCertificadoDigital();
            }
        }
        public byte Id
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Descrição é obrigatória")]
        public string Descricao
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Empresa é obrigatório")]
        public EmpresaDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaDTO>();
            set => SetValue(value);
        }

        public string ArquivoCertificado
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string SenhaCertificado
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string SerialNumberCertificado
        {
            get => GetValue();
            set => SetValue(value);
        }

        public ObservableCollection<EmpresaDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public BitmapImage ArquivoLogoNfe
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        public BitmapImage ArquivoLogoNfce
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        public BitmapImage ArquivoLogoCte
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        public BitmapImage ArquivoLogoCteOs
        {
            get { return GetValue<BitmapImage>(); }
            set => SetValue(value);
        }

        public BitmapImage ArquivoLogoMdfe
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        public BitmapImage ArquivoLogoSat
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Número caixa é obrigatório")]
        [Alcance(nameof(FlagSat), 1, 999, ErrorMessage = @"Número caixa mínimo 1, número máximo 999")]
        public string NumeroCaixa
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Fabricante é obrigatório")]
        public Modelo FabricanteModelo
        {
            get => GetValue<Modelo>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfe), ErrorMessage = @"Informe o ambiente da NFE")]
        public TipoAmbiente AmbienteNfe
        {
            get => GetValue<TipoAmbiente>();
            set
            {
                SetValue(value);
                SerieNfe = 0;
                NumeroAtualNfe = string.Empty;
            }
        }

        [Requirido(nameof(FlagNfce), ErrorMessage = @"Informe o ambiente da NFCE")]
        public TipoAmbiente AmbienteNfce
        {
            get => GetValue<TipoAmbiente>();
            set
            {
                SetValue(value);
                SerieNfce = 0;
                NumeroAtualNfce = string.Empty;
                SerieContingenciaNfce = 0;
                NumeroAtualContingenciaNfce = string.Empty;
                IdToken = null;
                Csc = string.Empty;
            }
        }

        [Requirido(nameof(FlagCte), ErrorMessage = @"Informe o ambiente do CTE")]
        public TipoAmbiente AmbienteCte
        {
            get => GetValue<TipoAmbiente>();
            set
            {
                SetValue(value);
                SerieCte = 0;
                NumeroAtualCte = string.Empty;
            }
        }

        [Requirido(nameof(FlagCteOs), ErrorMessage = @"Porfavor preencher este campo")]
        public TipoAmbiente AmbienteCteOs
        {
            get => GetValue<TipoAmbiente>();
            set
            {
                SetValue(value);
                SerieCteOs = 0;
                NumeroAtualCteOs = string.Empty;
            }
        }

        [Requirido(nameof(FlagMdfe), ErrorMessage = @"Informe o ambiente do MDFE")]
        public TipoAmbiente AmbienteMdfe
        {
            get => GetValue<TipoAmbiente>();
            set
            {
                SetValue(value);
                SerieMdfe = 0;
                NumeroAtualMdfe = string.Empty;
            }
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Informe o ambiente do SAT")]
        public TipoAmbiente AmbienteSat
        {
            get => GetValue<TipoAmbiente>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Informe o código de ativação para SAT")]
        public string CodigoAtivacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Informe o código de associação para o SAT")]
        public string CodigoAssociacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ProtocoloSeguranca ProtocoloSeguranca
        {
            get => GetValue<ProtocoloSeguranca>();
            set => SetValue(value);
        }

        public bool AdicionarCertificadoDigital
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagSat), ErrorMessage = @"Informe a codificação arquivo xml para SAT")]
        public CodificacaoArquivoXml CodificacaoArqivoXml
        {
            get => GetValue<CodificacaoArquivoXml>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfe), ErrorMessage = @"Informe uma série para NFE")]
        [Alcance(nameof(FlagNfe), 1, 999, ErrorMessage = "Série da NFE precisa estar entre 1 e 999")]
        public short SerieNfe
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfce), ErrorMessage = @"Informe uma série para NFCE")]
        [Alcance(nameof(FlagNfce), 1, 999, ErrorMessage = "Série da NFCE precisa estar entre 1 e 999")]
        public short SerieNfce
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagCte), ErrorMessage = @"Informe uma série para CTE")]
        [Alcance(nameof(FlagCte), 1, 999, ErrorMessage = "Série do CTE precisa estar entre 1 e 999")]
        public short SerieCte
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagCteOs), ErrorMessage = @"Porfavor preencher este campo")]
        public short SerieCteOs
        {
            get { return GetValue<short>(); }
            set => SetValue(value);
        }

        [Requirido(nameof(FlagMdfe), ErrorMessage = @"Informe uma série para MDFE")]
        [Alcance(nameof(FlagMdfe), 1, 999, ErrorMessage = "Série do MDFE precisa estar entre 1 e 999")]
        public short SerieMdfe
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        [Requirido(nameof(UsaNumeracaoDiferenteContigencia),
            ErrorMessage = "Informe uma série de contigência para NFCE")]
        [Alcance(nameof(UsaNumeracaoDiferenteContigencia),
            1,
            999,
            ErrorMessage = "Série da contigência da NFCE precisa estar entre 1 e 999")]
        public short SerieContingenciaNfce
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfe), ErrorMessage = @"Informe um número atual para NFE")]
        public string NumeroAtualNfe
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfce), ErrorMessage = @"Informe um número atual para NFCE")]
        public string NumeroAtualNfce
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagCte), ErrorMessage = @"Informe um número atual para o CTE")]
        public string NumeroAtualCte
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagCteOs), ErrorMessage = @"Porfavor preencher este campo")]
        public string NumeroAtualCteOs
        {
            get { return GetValue<string>(); }
            set => SetValue(value);
        }

        [Requirido(nameof(FlagMdfe), ErrorMessage = @"Informe um número atual para MDFE")]
        public string NumeroAtualMdfe
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Requirido(nameof(UsaNumeracaoDiferenteContigencia),
            ErrorMessage = @"Informe uma númeração atual para contigência da NFCE")]
        public string NumeroAtualContingenciaNfce
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfce), ErrorMessage = @"Informe um Id Token para NFCE")]
        [Alcance(nameof(FlagNfce), 1, 999999, ErrorMessage = @"Id token deve estar entre 1 e 999999")]
        public int? IdToken
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Requirido(nameof(FlagNfce), ErrorMessage = @"Informe o código CSC para NFCE")]
        public string Csc
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public EmissorFiscal Model
        {
            get => GetValue<EmissorFiscal>();
            set => SetValue(value);
        }

        public VersaoSat VersaoSat
        {
            get => _versaoSat;
            set
            {
                if (value == _versaoSat) return;
                _versaoSat = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveAcessoValidador
        {
            get => _chaveAcessoValidador;
            set
            {
                if (value == _chaveAcessoValidador) return;
                _chaveAcessoValidador = value;
                PropriedadeAlterada();
            }
        }

        public bool UsaNumeracaoDiferenteContigencia
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                SerieContingenciaNfce = 0;
                NumeroAtualContingenciaNfce = "0";
            }
        }

        public bool PermiteAlterarTipo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EmProgresso
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EditarAmbiente
        {
            get => _editarAmbiente;
            set
            {
                _editarAmbiente = value;
                PropriedadeAlterada();
            }
        }

        private void HabilitaConformeUsoTipoCertificado(TipoCertificadoDigital value)
        {
            switch (value)
            {
                case TipoCertificadoDigital.A1Arquivo:
                    IsA1 = true;
                    IsSenha = true;
                    IsA3 = false;
                    break;
                case TipoCertificadoDigital.A1Repositorio:
                    IsA1 = false;
                    IsSenha = false;
                    IsA3 = true;
                    break;
                case TipoCertificadoDigital.A3:
                    IsA1 = false;
                    IsSenha = true;
                    IsA3 = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        private void LimparCertificado()
        {
            SerialNumberCertificado = string.Empty;
            ArquivoCertificado = string.Empty;
            SenhaCertificado = string.Empty;
        }

        public void AtualizarFormCertificadoDigital()
        {
            if (FlagNfe || FlagCte || FlagMdfe || FlagCteOs || IsFaturamento)
            {
                AdicionarCertificadoDigital = true;
                return;
            }

            ArquivoCertificado = string.Empty;
            SerialNumberCertificado = string.Empty;
            SenhaCertificado = string.Empty;
            AdicionarCertificadoDigital = false;
        }

        private void CarregarEmpresas()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<EmpresaDTO>(sessao);
                var empresas = repositorio.Busca(new TodasEmpresas());
                Empresas = new ObservableCollection<EmpresaDTO>(empresas);
            }
        }

        public EmissorFiscalFormModel InicializaContexto()
        {
            FlagNfe = Model.FlagNfe;
            FlagNfce = Model.FlagNfce;
            FlagCte = Model.FlagCte;
            FlagCteOs = Model.FlagCteOs;
            FlagMdfe = Model.FlagMdfe;
            FlagSat = Model.FlagSat;

            PermiteAlterarTipo = Model.PermiteAlterarTipo();

            Id = Model.Id;
            TipoCertificadoDigital = Model.TipoCertificadoDigital;
            IsContainerValidadeCertificado = false;
            Descricao = Model.Descricao;
            EmpresaSelecionada = Model.Empresa;
            ArquivoCertificado = Model.ArquivoCertificado;
            SenhaCertificado = SimetricaCrip.Descomputar(Model.SenhaCertificado);
            NumeroCaixa = Model.EmissorFiscalSat.NumeroCaixa.ToString();
            ProtocoloSeguranca = Model.ProtocoloSeguranca;

            AmbienteNfe = Model.EmissorFiscalNfe.Ambiente;
            AmbienteNfce = Model.EmissorFiscalNfce.Ambiente;
            AmbienteCte = Model.EmissorFiscalCte.Ambiente;
            AmbienteCteOs = Model.EmissorFiscalCteOs.Ambiente;
            AmbienteMdfe = Model.EmissorFiscalMdfe.Ambiente;
            AmbienteSat = Model.EmissorFiscalSat.Ambiente;

            SerieNfe = Model.EmissorFiscalNfe.Serie;
            SerieNfce = Model.EmissorFiscalNfce.Serie;
            SerieCte = Model.EmissorFiscalCte.Serie;
            SerieCteOs = Model.EmissorFiscalCteOs.Serie;
            SerieMdfe = Model.EmissorFiscalMdfe.Serie;

            NumeroAtualNfe = Model.EmissorFiscalNfe.NumeroAtual.ToString();
            NumeroAtualNfce = Model.EmissorFiscalNfce.NumeroAtual.ToString();
            NumeroAtualCte = Model.EmissorFiscalCte.NumeroAtual.ToString();
            NumeroAtualCteOs = Model.EmissorFiscalCteOs.NumeroAtual.ToString();
            NumeroAtualMdfe = Model.EmissorFiscalMdfe.NumeroAtual.ToString();

            UsaNumeracaoDiferenteContigencia = Model.EmissorFiscalNfce?.UsaNumeracaoDiferenteContigencia ?? false;
            SerieContingenciaNfce = Model.EmissorFiscalNfce.SerieContingencia;
            NumeroAtualContingenciaNfce = Model.EmissorFiscalNfce.NumeroAtualContingencia.ToString();

            IdToken = Model.EmissorFiscalNfce.IdToken;
            Csc = Model.EmissorFiscalNfce.Csc;
            IsIntegradorCeara = Model.EmissorFiscalNfce.IsIntegradorCeara;

            CodificacaoArqivoXml = Model.EmissorFiscalSat.CodificacaoArquivoXml;
            VersaoSat = Model.EmissorFiscalSat.VersaoLayoutSat;
            CodigoAtivacao = Model.EmissorFiscalSat.CodigoAtivacao.TrimOrEmpty();
            CodigoAssociacao = Model.EmissorFiscalSat.CodigoAcossiacao.TrimOrEmpty();
            FabricanteModelo = Model.EmissorFiscalSat.Fabricante;
            IsMFe = Model.EmissorFiscalSat.IsMFe;
            ChaveAcessoValidador = Model.EmissorFiscalSat.ChaveAcessoValidador;
            IsFaturamento = Model.IsFaturamento;

            DocumentoUnicoAutorizaBaixarXml = Model.AutorizadoBaixarXml.DocumentoUnico;
            DescricaoAutorizaBaixarXml = Model.AutorizadoBaixarXml.Descricao;

            if (Model.SerialNumberCertificado != null)
            {
                SerialNumberCertificado = SimetricaCrip.Descomputar(Model.SerialNumberCertificado.Trim());
            }

            if (Model.EmissorFiscalNfe.ArquivoLogo != null)
            {
                VerificaSeArquivoLogoEoDaVersao2SistemaFusion();

                ArquivoLogoNfe = ConverteImage.ByteEmImagem(Model.EmissorFiscalNfe.ArquivoLogo);
            }

            if (Model.EmissorFiscalCte.ArquivoLogo != null)
            {
                ArquivoLogoCte = ConverteImage.ByteEmImagem(Model.EmissorFiscalCte.ArquivoLogo);
            }

            if (Model.EmissorFiscalCteOs.ArquivoLogo != null)
            {
                ArquivoLogoCteOs = ConverteImage.ByteEmImagem(Model.EmissorFiscalCte.ArquivoLogo);
            }

            if (Model.EmissorFiscalMdfe.ArquivoLogo != null)
            {
                ArquivoLogoMdfe = ConverteImage.ByteEmImagem(Model.EmissorFiscalMdfe.ArquivoLogo);
            }

            if (Model.EmissorFiscalNfce.ArquivoLogo != null)
            {
                ArquivoLogoNfce = ConverteImage.ByteEmImagem(Model.EmissorFiscalNfce.ArquivoLogo);
            }

            if (Model.EmissorFiscalSat.ArquivoLogo != null)
            {
                ArquivoLogoSat = ConverteImage.ByteEmImagem(Model.EmissorFiscalSat.ArquivoLogo);
            }

            EditarCadastro();
            EditarAmbiente = Model.Id == 0;

            return this;
        }

        private void EditarCadastro()
        {
            NaoEditar = Model.Id == 0;
            Editar = Model.Id != 0;
        }

        public bool Editar
        {
            get => _editar;
            set
            {
                _editar = value;
                PropriedadeAlterada();
            }
        }

        public bool MensagemEdicaoAmbiente
        {
            get => _mensagemEdicaoAmbiente;
            set
            {
                _mensagemEdicaoAmbiente = value;
                PropriedadeAlterada();
            }
        }

        private void VerificaSeArquivoLogoEoDaVersao2SistemaFusion()
        {
            var tamanhoLogo = Model.EmissorFiscalNfe.ArquivoLogo.Length;

            if (Model.EmissorFiscalNfe == null) return;

            if (tamanhoLogo != 7) return;

            Model.EmissorFiscalNfe.ArquivoLogo.ForEach(p =>
            {
                if (p == 32)
                {
                    tamanhoLogo--;
                }
            });

            if (tamanhoLogo == 0)
            {
                Model.EmissorFiscalNfe.ArquivoLogo = null;
            }
        }

        public void SalvarModel()
        {
            Validar();

            if ((FlagNfce && IsFaturamento == false) || FlagSat)
            {
                ArquivoCertificado = string.Empty;
                SenhaCertificado = string.Empty;
                SerialNumberCertificado = string.Empty;
            }

            DescricaoAutorizaBaixarXml = DescricaoAutorizaBaixarXml.TrimOrEmpty();
            DocumentoUnicoAutorizaBaixarXml = DocumentoUnicoAutorizaBaixarXml.TrimOrEmpty();

            Model.Descricao = Descricao;
            Model.Empresa = EmpresaSelecionada;
            Model.ProtocoloSeguranca = ProtocoloSeguranca;

            Model.ArquivoCertificado = string.IsNullOrEmpty(ArquivoCertificado)
                ? string.Empty
                : ArquivoCertificado;

            var senha = SimetricaCrip.Computar(SenhaCertificado);
            Model.SenhaCertificado = string.IsNullOrEmpty(senha) ? string.Empty : senha;

            var serialNumberCertificado = SimetricaCrip.Computar(SerialNumberCertificado);

            Model.SerialNumberCertificado = string.IsNullOrEmpty(serialNumberCertificado)
                ? string.Empty
                : serialNumberCertificado.Trim();

            Model.TipoCertificadoDigital = TipoCertificadoDigital;
            Model.AlteradoEm = DateTime.Now;
            Model.IsFaturamento = IsFaturamento;

            Model.FlagNfe = FlagNfe;
            Model.FlagNfce = FlagNfce;
            Model.FlagCte = FlagCte;
            Model.FlagMdfe = FlagMdfe;
            Model.FlagSat = FlagSat;
            Model.FlagCteOs = FlagCteOs;

            if (FlagNfe)
            {
                Model.EmissorFiscalNfe.Ambiente = AmbienteNfe;
                Model.EmissorFiscalNfe.Modelo = ModeloDocumento.NFe;
                Model.EmissorFiscalNfe.Serie = SerieNfe;
                Model.EmissorFiscalNfe.NumeroAtual = int.Parse(NumeroAtualNfe);
                Model.EmissorFiscalNfe.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoNfe, new PngBitmapEncoder());
                Model.EmissorFiscalNfe.EmissorFiscal = Model;
            }
            else
            {
                Model.EmissorFiscalNfe = null;
            }

            if (FlagNfce)
            {
                SetaDadosParaNfce();
            }
            else
            {
                Model.EmissorFiscalNfce = null;
            }

            if (FlagCte)
            {
                Model.EmissorFiscalCte.Ambiente = AmbienteCte;
                Model.EmissorFiscalCte.Modelo = ModeloDocumento.CTe;
                Model.EmissorFiscalCte.Serie = SerieCte;
                Model.EmissorFiscalCte.NumeroAtual = int.Parse(NumeroAtualCte);
                Model.EmissorFiscalCte.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoCte, new PngBitmapEncoder());
                Model.EmissorFiscalCte.EmissorFiscal = Model;
            }
            else
            {
                Model.EmissorFiscalCte = null;
            }

            if (FlagCteOs)
            {
                Model.EmissorFiscalCteOs.Ambiente = AmbienteCteOs;
                Model.EmissorFiscalCteOs.Modelo = ModeloDocumento.CTeOS;
                Model.EmissorFiscalCteOs.Serie = SerieCteOs;
                Model.EmissorFiscalCteOs.NumeroAtual = int.Parse(NumeroAtualCteOs);
                Model.EmissorFiscalCteOs.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoCteOs, new PngBitmapEncoder());
                Model.EmissorFiscalCteOs.EmissorFiscal = Model;
            }
            else
            {
                Model.EmissorFiscalCteOs = null;
            }

            if (FlagMdfe)
            {
                Model.EmissorFiscalMdfe.Ambiente = AmbienteMdfe;
                Model.EmissorFiscalMdfe.Modelo = ModeloDocumento.MDFe;
                Model.EmissorFiscalMdfe.Serie = SerieMdfe;
                Model.EmissorFiscalMdfe.NumeroAtual = int.Parse(NumeroAtualMdfe);
                Model.EmissorFiscalMdfe.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoMdfe, new PngBitmapEncoder());
                Model.EmissorFiscalMdfe.EmissorFiscal = Model;
            }
            else
            {
                Model.EmissorFiscalMdfe = null;
            }

            if (FlagSat)
            {
                Model.EmissorFiscalSat.Ambiente = AmbienteSat;
                Model.EmissorFiscalSat.ModeloDocumento = ModeloDocumento.SAT;
                Model.EmissorFiscalSat.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoSat, new PngBitmapEncoder());
                Model.EmissorFiscalSat.NumeroCaixa = short.Parse(NumeroCaixa);
                Model.EmissorFiscalSat.CodificacaoArquivoXml = CodificacaoArqivoXml;
                Model.EmissorFiscalSat.VersaoLayoutSat = VersaoSat;
                Model.EmissorFiscalSat.CodigoAtivacao = CodigoAtivacao;
                Model.EmissorFiscalSat.CodigoAcossiacao = CodigoAssociacao;
                Model.EmissorFiscalSat.Fabricante = FabricanteModelo;
                Model.EmissorFiscalSat.EmissorFiscal = Model;
                Model.EmissorFiscalSat.IsMFe = IsMFe;
                Model.EmissorFiscalSat.ChaveAcessoValidador = IsMFe ? ChaveAcessoValidador.TrimOrEmpty() : string.Empty;
            }
            else
            {
                Model.EmissorFiscalSat = null;
            }

            if (DescricaoAutorizaBaixarXml.IsNotNullOrEmpty() 
                && DocumentoUnicoAutorizaBaixarXml.IsNotNullOrEmpty() 
                && IsAutorizaBaixarXml)
            {
                Model.AutorizadoBaixarXml.DocumentoUnico = DocumentoUnicoAutorizaBaixarXml;
                Model.AutorizadoBaixarXml.Descricao = DescricaoAutorizaBaixarXml;
                Model.AutorizadoBaixarXml.EmissorFiscal = Model;
            }
            else
            {
                Model.AutorizadoBaixarXml = null;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                repositorio.Salvar(Model);

                transacao.Commit();
            }

            EditarCadastro();
            EditarAmbiente = false;
        }

        private void SetaDadosParaNfce()
        {
            Model.EmissorFiscalNfce.Ambiente = AmbienteNfce;
            Model.EmissorFiscalNfce.Modelo = ModeloDocumento.NFCe;
            Model.EmissorFiscalNfce.Serie = SerieNfce;
            Model.EmissorFiscalNfce.NumeroAtual = int.Parse(NumeroAtualNfce);
            Model.EmissorFiscalNfce.ArquivoLogo = ConverteImage.ImagemEmByte(ArquivoLogoNfce, new PngBitmapEncoder());
            Model.EmissorFiscalNfce.EmissorFiscal = Model;
            Model.EmissorFiscalNfce.IdToken = IdToken ?? 0;
            Model.EmissorFiscalNfce.Csc = Csc;
            Model.EmissorFiscalNfce.AlteradoEm = DateTime.Now;
            Model.EmissorFiscalNfce.SerieContingencia = 0;
            Model.EmissorFiscalNfce.NumeroAtualContingencia = 0;
            Model.EmissorFiscalNfce.UsaNumeracaoDiferenteContigencia = UsaNumeracaoDiferenteContigencia;
            Model.EmissorFiscalNfce.IsIntegradorCeara = IsIntegradorCeara;

            if (!UsaNumeracaoDiferenteContigencia)
            {
                return;
            }

            Model.EmissorFiscalNfce.SerieContingencia = SerieContingenciaNfce;
            Model.EmissorFiscalNfce.NumeroAtualContingencia = int.Parse(NumeroAtualContingenciaNfce);
        }

        private void Validar()
        {
            ThrowExceptionSeExistirErros();

            var hasFlagAtivo = FlagNfe || FlagCte || FlagMdfe || FlagNfce || FlagSat || FlagCteOs;

            if (hasFlagAtivo == false)
            {
                throw new InvalidOperationException("Preciso que selecione um tipo para o emissor");
            }

            if (string.IsNullOrEmpty(ArquivoCertificado) && string.IsNullOrEmpty(SerialNumberCertificado) &&
                (FlagNfe || FlagCte || FlagMdfe || IsFaturamento))
            {
                throw new InvalidOperationException("Preciso que selecione um certificado digital");
            }

            if (TipoCertificadoDigital == TipoCertificadoDigital.A1Arquivo &&
                !string.IsNullOrEmpty(ArquivoCertificado) && string.IsNullOrEmpty(SenhaCertificado) &&
                (FlagNfe || FlagCte || FlagMdfe || IsFaturamento))
            {
                throw new InvalidOperationException("Para certificado A1 como arquivo é necessário a SENHA");
            }

            if (IsAutorizaBaixarXml)
            {
                if (DescricaoAutorizaBaixarXml.IsNullOrEmpty() && DocumentoUnicoAutorizaBaixarXml.IsNotNullOrEmpty())
                {
                    throw new InvalidOperationException("Adicionar descrição do autorizado a baixar o xml");
                }

                if (DescricaoAutorizaBaixarXml.IsNotNullOrEmpty() && DocumentoUnicoAutorizaBaixarXml.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("Adicionar cpf/cnpj do autorizado a baixar o xml");
                }
            }

            if (FlagCte && (SerieCte < 1 || SerieCte > 999))
            {
                throw new InvalidOperationException("Série do CT-e deve estar entre 1 e 999");
            }

            if (FlagNfe && (SerieNfe < 1 || SerieNfe > 999))
            {
                throw new InvalidOperationException("Série da NF-e deve entre 1 e 999");
            }

            if (FlagNfce && (SerieNfce < 1 || SerieNfce > 999))
            {
                throw new InvalidOperationException("Série da NFC-e deve estar entre 1 e 999");
            }

            if (FlagNfce && UsaNumeracaoDiferenteContigencia &&
                (SerieContingenciaNfce < 1 || SerieContingenciaNfce > 999))
            {
                throw new InvalidOperationException("Série da contigência da NFC-e deve estar entre 1 e 999");
            }

            if (FlagMdfe && (SerieMdfe < 1 || SerieMdfe > 999))
            {
                throw new InvalidOperationException("Série do MDF-e deve estar entre 1 e 999");
            }

            if (FlagSat && IsMFe)
            {
                if (ChaveAcessoValidador.IsNullOrEmpty())
                {
                    throw new InvalidOperationException("MF-e deverá ter a Chave Acesso Validador");
                }
            }

            if (!IsMFe)
            {
                if (FabricanteModelo == Modelo.Nenhum)
                    throw new InvalidOperationException("SAT-Fiscal deverá ter um fabricante");
            }

            if (UsaNumeracaoDiferenteContigencia && SerieNfce == SerieContingenciaNfce)
            {
                throw new ArgumentException("A série deve ser diferente da série de contingência");
            }

            if (IsFaturamento)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioEmissorFiscal = new RepositorioEmissorFiscal(sessao);

                    if (repositorioEmissorFiscal.EmpresaJaEstaVinculada(EmpresaSelecionada.Id) && Model.Id == 0)
                        throw new InvalidOperationException("Empresa já tem um emissor fiscal para NFC-e - Faturamento\n" +
                                                            "So pode ter um emissor por empresa para NFC-e - Faturamento");
                }
            }
        }

        public void DeletarModel()
        {
            var naoPodeDeletar = Model.Id == 0;

            if (naoPodeDeletar)
                throw new ArgumentException(
                    "Somente é possível deletar um registro existente, o registro atual não existe ainda...");

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var repositorio = new RepositorioComun<EmissorFiscal>(sessao);
            var transacao = sessao.BeginTransaction();

            try
            {
                repositorio.Deleta(Model);
                transacao.Commit();
            }
            catch (InvalidOperationException ex)
            {
                transacao.Rollback();
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception e)
            {
                transacao.Rollback();
                throw new InvalidDataException(e.Message);
            }
            finally
            {
                sessao.Close();
            }
        }

        private void AutorizaBaixarXml()
        {
            DescricaoAutorizaBaixarXml = string.Empty;
            DocumentoUnicoAutorizaBaixarXml = string.Empty;

            IsAutorizaBaixarXml = FlagNfe == true || FlagNfce == true;
        }
    }
}