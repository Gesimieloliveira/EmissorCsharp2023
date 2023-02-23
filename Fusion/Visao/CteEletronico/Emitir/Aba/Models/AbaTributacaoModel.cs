using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Fiscal;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Tributacoes.Estadual;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using MotorTributarioNet.Facade;
using MotorTributarioNet.Impostos.Csts;
using MotorTributarioNet.Impostos.Implementacoes;
using NFe.Classes;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public class AbaTributacaoModel : ViewModel
    {
        public event EventHandler AnteriorHandler;
        public event EventHandler<AbaTributacaoModel> ProximoHandler;

        private bool _habilitado;
        private bool _selecionado;
        private ObservableCollection<TributacaoIcms> _tributacaoLista;
        private TributacaoIcms _tributacaoIcmsSelecionado;
        private bool _isPermiteCredito;
        private bool _isIcms;
        private bool _isIcmsSt;
        private bool _isIcmsReducao;
        private decimal _baseCalculoIcms;
        private decimal _percentualIcms;
        private decimal _percentualReducaoIcms;
        private decimal _valorIcms;
        private decimal _percentualCredito;
        private decimal _valorCredito;
        private decimal _baseCalculoIcmsSt;
        private decimal _percentualIcmsSt;
        private decimal _valorIcmsSt;
        private bool _isCreditoAutomatico;
        private bool _isCreditoValorAtivado;
        private bool _isPercentualCreditoAtivado;
        private bool _isCalcularTributosAutomatico;
        private bool _isToogleCreditoVisivel;
        private bool _isValorIcmsAtivado;
        private bool _isPartilhaIcms;
        private decimal _partilhaBaseCalculo;
        private decimal _percentualFcp;
        private decimal _valorIcmsFcp;
        private decimal _percentualInterestadual;
        private decimal _percentualInternoUfTermino;
        private decimal _valorIcmsPartilhaUfInicio;
        private decimal _valorIcmsPartilhaUfTermino;
        private bool _isValorPartilhaAtivado;
        private decimal _percentualPartilhaUfTermino;
        private bool _isPercentualPartilhaAtivado;
        private decimal _baseCalculoPadrao;
        private bool _visivel;

        public bool Visivel
        {
            get { return _visivel; }
            set
            {
                _visivel = value;
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

        public ObservableCollection<TributacaoIcms> TributacaoLista
        {
            get => _tributacaoLista;
            set
            {
                if (Equals(value, _tributacaoLista)) return;
                _tributacaoLista = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCreditoAutomatico
        {
            get => _isCreditoAutomatico;
            set
            {
                _isCreditoAutomatico = value;
                PropriedadeAlterada();

                IsCreditoValorAtivado = !value;

                ValorCredito = 0.0m;
                PercentualCredito = 0.0m;

                if (IsCreditoAutomatico)
                {
                    CalculaImpostosIcms();
                }
            }
        }

        public bool IsCreditoValorAtivado
        {
            get => _isCreditoValorAtivado;
            set
            {
                if (value == _isCreditoValorAtivado) return;
                _isCreditoValorAtivado = value;
                PropriedadeAlterada();
                IsPercentualCreditoAtivado = !value;
            }
        }

        public bool IsPercentualCreditoAtivado
        {
            get => _isPercentualCreditoAtivado;
            set
            {
                _isPercentualCreditoAtivado = value;
                PropriedadeAlterada();
            }
        }

        public TributacaoIcms TributacaoIcmsSelecionado
        {
            get => _tributacaoIcmsSelecionado;
            set
            {
                _tributacaoIcmsSelecionado = value;
                PropriedadeAlterada();

                ResetaCampos();

                if (value == null) return;

                switch (value.Codigo)
                {
                    case "00":
                        IsPermiteCredito = false;
                        IsIcmsSt = false;
                        IsIcmsReducao = false;
                        IsIcms = true;
                        BaseCalculoIcms = _baseCalculoPadrao;
                    break;

                    case "20":
                        IsPermiteCredito = false;
                        IsIcmsSt = false;
                        IsIcmsReducao = true;
                        IsIcms = true;
                        BaseCalculoIcms = _baseCalculoPadrao;
                    break;

                    case "40":
                    case "41":
                    case "51":
                        IsPermiteCredito = false;
                        IsIcmsSt = false;
                        IsIcmsReducao = false;
                        IsIcms = false;
                    break;

                    case "60":
                        IsPermiteCredito = true;
                        IsIcmsSt = true;
                        IsIcmsReducao = false;
                        IsIcms = false;
                        BaseCalculoIcmsSt = _baseCalculoPadrao;
                    break;

                    case "90":
                        IsPermiteCredito = true;
                        IsIcmsSt = false;
                        IsIcmsReducao = true;
                        IsIcms = true;
                        BaseCalculoIcms = _baseCalculoPadrao;
                    break;
                }
            }
        }

        public ICommand CommandAnterior => GetSimpleCommand(AnteriorAction);
        public ICommand CommandProximo => GetSimpleCommand(ProximoAction);

        private void AnteriorAction(object obj)
        {
            OnAnteriorHandler();
        }

        private void ProximoAction(object obj)
        {
            try
            {

                OnProximoHandler();
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void ResetaCampos()
        {
            PercentualCredito = 0.0m;
            ValorCredito = 0.0m;
            BaseCalculoIcms = 0.0m;
            PercentualIcms = 0.0m;
            PercentualReducaoIcms = 0.0m;
            ValorIcms = 0.0m;
            BaseCalculoIcmsSt = 0.0m;
            PercentualIcmsSt = 0.0m;
            ValorIcmsSt = 0.0m;

            if (TributacaoIcmsSelecionado != null)
            {
                switch (TributacaoIcmsSelecionado.Codigo)
                {
                    case "60":
                        BaseCalculoIcmsSt = _baseCalculoPadrao;
                        break;
                    default:
                        BaseCalculoIcms = _baseCalculoPadrao;
                        break;
                }
            }

            ResetarBaseCalculoPartilhaDefault();

            ResetarPartilha();
        }

        private void ResetarBaseCalculoPartilhaDefault()
        {
            PartilhaBaseCalculo = 0.0m;

            if (IsPartilhaIcms)
                PartilhaBaseCalculo = _baseCalculoPadrao;
        }

        private void ResetarPartilha()
        {
            PercentualFcp = 0.0m;
            ValorIcmsFcp = 0.0m;
            PercentualInterestadual = 0.0m;
            PercentualInternoUfTermino = 0.0m;
            ValorIcmsPartilhaUfInicio = 0.0m;
            ValorIcmsPartilhaUfTermino = 0.0m;
            PercentualPartilhaUfTermino = 0.0m;
        }

        public bool IsIcmsReducao
        {
            get => _isIcmsReducao;
            set
            {
                if (value == _isIcmsReducao) return;
                _isIcmsReducao = value;
                PropriedadeAlterada();
            }
        }

        public AbaTributacaoModel()
        {
            IsCalcularTributosAutomatico = true;
            IsPartilhaIcms = false;

            var recipiente = new RecipienteIcmsCTe();
            recipiente.RecarregaCache();
            TributacaoLista = new ObservableCollection<TributacaoIcms>(recipiente.GetTodos());
        }

        public bool IsPartilhaIcms
        {
            get => _isPartilhaIcms;
            set
            {
                _isPartilhaIcms = value;
                PropriedadeAlterada();

                if (value)
                {
                    ResetarBaseCalculoPartilhaDefault();
                    ResetarPartilha();
                    return;
                }
                PartilhaBaseCalculo = 0.0m;
                ResetarPartilha();
            }
        }

        public bool IsPermiteCredito
        {
            get => _isPermiteCredito;
            set
            {
                _isPermiteCredito = value;
                PropriedadeAlterada();
            }
        }

        public bool IsIcms
        {
            get => _isIcms;
            set
            {
                if (value == _isIcms) return;
                _isIcms = value;
                PropriedadeAlterada();
            }
        }

        public bool IsIcmsSt
        {
            get => _isIcmsSt;
            set
            {
                _isIcmsSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal BaseCalculoIcms
        {
            get => _baseCalculoIcms;
            set
            {
                _baseCalculoIcms = value;
                PropriedadeAlterada();
                CalculaImpostosIcms();
            }
        }

        public decimal PercentualIcms
        {
            get => _percentualIcms;
            set
            {
                _percentualIcms = value;
                PropriedadeAlterada();
                CalculaImpostosIcms();
            }
        }

        public decimal PercentualReducaoIcms
        {
            get => _percentualReducaoIcms;
            set
            {
                _percentualReducaoIcms = value;
                PropriedadeAlterada();
                CalculaImpostosIcms();
            }
        }

        public decimal ValorIcms
        {
            get => _valorIcms;
            set
            {
                _valorIcms = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualCredito
        {
            get => _percentualCredito;
            set
            {
                _percentualCredito = value;
                PropriedadeAlterada();

                if (IsCreditoAutomatico)
                    CalculaImpostosIcms();
            }
        }

        public decimal ValorCredito
        {
            get => _valorCredito;
            set
            {
                _valorCredito = value;
                PropriedadeAlterada();
            }
        }

        public decimal BaseCalculoIcmsSt
        {
            get => _baseCalculoIcmsSt;
            set
            {
                _baseCalculoIcmsSt = value;
                PropriedadeAlterada();
                CalculaImpostosIcms();
            }
        }

        public decimal PercentualIcmsSt
        {
            get => _percentualIcmsSt;
            set
            {
                _percentualIcmsSt = value;
                PropriedadeAlterada();
                CalculaImpostosIcms();
            }
        }

        public decimal ValorIcmsSt
        {
            get => _valorIcmsSt;
            set
            {
                _valorIcmsSt = value;
                PropriedadeAlterada();
            }
        }

        public decimal PartilhaBaseCalculo
        {
            get => _partilhaBaseCalculo;
            set
            {
                _partilhaBaseCalculo = value;
                PropriedadeAlterada();
                CalculaImpostosPartilha();
            }
        }

        public decimal PercentualFcp
        {
            get => _percentualFcp;
            set
            {
                if (value == _percentualFcp) return;
                _percentualFcp = value;
                PropriedadeAlterada();
                CalculaImpostosPartilha();
            }
        }

        public decimal ValorIcmsFcp
        {
            get => _valorIcmsFcp;
            set
            {
                _valorIcmsFcp = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualInterestadual
        {
            get => _percentualInterestadual;
            set
            {
                _percentualInterestadual = value;
                PropriedadeAlterada();
                CalculaImpostosPartilha();
            }
        }

        public decimal PercentualInternoUfTermino
        {
            get => _percentualInternoUfTermino;
            set
            {
                _percentualInternoUfTermino = value;
                PropriedadeAlterada();
                CalculaImpostosPartilha();
            }
        }

        public decimal ValorIcmsPartilhaUfInicio
        {
            get => _valorIcmsPartilhaUfInicio;
            set
            {
                _valorIcmsPartilhaUfInicio = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorIcmsPartilhaUfTermino
        {
            get => _valorIcmsPartilhaUfTermino;
            set
            {
                _valorIcmsPartilhaUfTermino = value;
                PropriedadeAlterada();
            }
        }

        public decimal PercentualPartilhaUfTermino
        {
            get => _percentualPartilhaUfTermino;
            set
            {
                _percentualPartilhaUfTermino = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPercentualPartilhaAtivado
        {
            get => _isPercentualPartilhaAtivado;
            set
            {
                if (value == _isPercentualPartilhaAtivado) return;
                _isPercentualPartilhaAtivado = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCalcularTributosAutomatico
        {
            get => _isCalcularTributosAutomatico;
            set
            {
                _isCalcularTributosAutomatico = value;
                PropriedadeAlterada();

                IsToogleCreditoVisivel = value;
                IsPercentualCreditoAtivado = value;
                IsCreditoAutomatico = value;
                IsValorIcmsAtivado = !value;
                IsValorPartilhaAtivado = !value;
                IsPercentualPartilhaAtivado = !value;

                ResetaCampos();
            }
        }

        public bool IsValorIcmsAtivado
        {
            get => _isValorIcmsAtivado;
            set
            { 
                _isValorIcmsAtivado = value;
                PropriedadeAlterada();
            }
        }

        public bool IsToogleCreditoVisivel
        {
            get => _isToogleCreditoVisivel;
            set
            {
                if (value == _isToogleCreditoVisivel) return;
                _isToogleCreditoVisivel = value;
                PropriedadeAlterada();
            }
        }

        public bool IsValorPartilhaAtivado
        {
            get => _isValorPartilhaAtivado;
            set
            {
                _isValorPartilhaAtivado = value;
                PropriedadeAlterada();
            }
        }

        public string Observacao { get; set; }

        private void CalculaImpostosPartilha()
        {
            if (IsCalcularTributosAutomatico == false)
            {
                Observacao = ResultadoCalculoDifal.GetObservacaoDifal(new DadosMensagemDifal(PercentualFcp.Arredondar(2),
                    ValorIcmsPartilhaUfTermino.Arredondar(2),
                    ValorIcmsPartilhaUfInicio.Arredondar(2)));
                return;
            }

            NotificaPartilhaSilenciosamente();

            if (VerificaTributacaoIcmsSelecionado()) return;

            var freteImpostos = CriaFreteComDifal();

            var facade = new FacadeCalculadoraTributacao(freteImpostos);

            var resultadoCalculoDifal = facade.CalculaDifalFcp();

            ValorIcmsFcp = resultadoCalculoDifal.Fcp;
            ValorIcmsPartilhaUfInicio = resultadoCalculoDifal.ValorIcmsOrigem < 0 ? 0 : resultadoCalculoDifal.ValorIcmsOrigem.Arredondar(2);
            ValorIcmsPartilhaUfTermino = resultadoCalculoDifal.ValorIcmsDestino < 0 ? 0 : resultadoCalculoDifal.ValorIcmsDestino.Arredondar(2);
            Observacao = resultadoCalculoDifal.GetObservacao(new DadosMensagemDifal(PercentualFcp.Arredondar(2),
                ValorIcmsPartilhaUfTermino.Arredondar(2),
                ValorIcmsPartilhaUfInicio.Arredondar(2)));

            PercentualProvisorio();
        }

        private void PercentualProvisorio()
        {
            decimal percentualProvisorio = 60;

            if (DateTime.Now.Year == 2018)
            {
                percentualProvisorio = 80;
            }

            if (DateTime.Now.Year >= 2019)
            {
                percentualProvisorio = 100;
            }

            PercentualPartilhaUfTermino = percentualProvisorio.Arredondar(2);
        }

        private FreteTributavel CriaFreteComDifal()
        {
            var freteImpostos = new FreteTributavel
            {
                ValorProduto = PartilhaBaseCalculo,
                QuantidadeProduto = 1,
                PercentualFcp = PercentualFcp,
                PercentualDifalInterna = PercentualInternoUfTermino,
                PercentualDifalInterestadual = PercentualInterestadual
            };
            return freteImpostos;
        }

        private void NotificaPartilhaSilenciosamente()
        {
            _partilhaBaseCalculo = PartilhaBaseCalculo.Arredondar(2);
            _percentualFcp = PercentualFcp.Arredondar(2);
            _valorIcmsFcp = ValorIcmsFcp.Arredondar(2);
            _percentualInterestadual = PercentualInterestadual.Arredondar(2);
            _percentualInternoUfTermino = PercentualInternoUfTermino.Arredondar(2);
            _valorIcmsPartilhaUfInicio = ValorIcmsPartilhaUfInicio.Arredondar(2);
            _valorIcmsPartilhaUfTermino = ValorIcmsPartilhaUfTermino.Arredondar(2);

            PropriedadeAlterada(nameof(PartilhaBaseCalculo));
            PropriedadeAlterada(nameof(PercentualFcp));
            PropriedadeAlterada(nameof(ValorIcmsFcp));
            PropriedadeAlterada(nameof(PercentualInterestadual));
            PropriedadeAlterada(nameof(PercentualInternoUfTermino));
            PropriedadeAlterada(nameof(ValorIcmsPartilhaUfInicio));
            PropriedadeAlterada(nameof(ValorIcmsPartilhaUfTermino));
        }

        private void CalculaImpostosIcms()
        {
            if (IsCalcularTributosAutomatico == false) return;

            NotificacaoSilenciosaTodosCamposIcms();


            if (VerificaTributacaoIcmsSelecionado()) return;

            var freteImpostos = CriaFrete();

            switch (TributacaoIcmsSelecionado.Codigo)
            {
                case "00":
                    var cst00 = new Cst00();
                    cst00.Calcula(freteImpostos);
                    ValorIcms = cst00.ValorIcms.Arredondar(2);
                    _baseCalculoIcms = cst00.ValorBcIcms.Arredondar(2);
                    PropriedadeAlterada(nameof(BaseCalculoIcms));
                    break;

                case "20":
                    var cst20 = new Cst20();
                    cst20.Calcula(freteImpostos);
                    ValorIcms = cst20.ValorIcms.Arredondar(2);
                    _baseCalculoIcms = cst20.ValorBcIcms.Arredondar(2);
                    PropriedadeAlterada(nameof(BaseCalculoIcms));
                    break;

                case "60":
                    var cst60 = new Cst60();
                    cst60.Calcula(freteImpostos);
                    ValorIcmsSt = cst60.ValorIcmsStRetido.Arredondar(2);
                    _baseCalculoIcmsSt = cst60.ValorBcStRetido.Arredondar(2);
                    PropriedadeAlterada(nameof(BaseCalculoIcmsSt));
                    AtualizaValorCredito(cst60.ValorCreditoOutorgadoOuPresumido.Arredondar(2));
                    break;

                case "90":
                    var cst90 = new Cst90();
                    cst90.Calcula(freteImpostos);
                    ValorIcms = cst90.ValorIcms.Arredondar(2);
                    _baseCalculoIcms = cst90.ValorBcIcms.Arredondar(2);
                    PropriedadeAlterada(nameof(BaseCalculoIcms));
                    AtualizaValorCredito(cst90.ValorCredito.Arredondar(2));
                    break;
            }
        }

        private FreteTributavel CriaFrete()
        {
            var freteImpostos = new FreteTributavel
            {
                ValorProduto = BaseCalculoIcms != 0 ? BaseCalculoIcms : BaseCalculoIcmsSt,
                QuantidadeProduto = 1,
                PercentualIcms = PercentualIcms,
                PercentualReducao = PercentualReducaoIcms,
                PercentualCredito = PercentualCredito,
                PercentualIcmsSt = PercentualIcmsSt
            };
            return freteImpostos;
        }

        private void NotificacaoSilenciosaTodosCamposIcms()
        {
            _baseCalculoIcms = BaseCalculoIcms.Arredondar(2);
            _percentualIcms = PercentualIcms.Arredondar(2);
            _percentualReducaoIcms = PercentualReducaoIcms.Arredondar(2);
            _valorIcms = ValorIcms.Arredondar(2);
            _percentualCredito = PercentualCredito.Arredondar(2);
            _valorCredito = ValorCredito.Arredondar(2);
            _baseCalculoIcmsSt = BaseCalculoIcmsSt.Arredondar(2);
            _valorIcmsSt = ValorIcmsSt.Arredondar(2);

            NotificaIcmsSilenciosamente();
            NotificaIcmsStSilenciosamente();
            NotificaCreditoSilenciosamente();
        }

        private void AtualizaValorCredito(decimal credito)
        {
            if (IsCreditoAutomatico)
                ValorCredito = credito;
        }

        private void NotificaIcmsSilenciosamente()
        {
            PropriedadeAlterada(nameof(BaseCalculoIcms));
            PropriedadeAlterada(nameof(PercentualIcms));
            PropriedadeAlterada(nameof(PercentualReducaoIcms));
            PropriedadeAlterada(nameof(ValorIcms));
        }

        private void NotificaIcmsStSilenciosamente()
        {
            PropriedadeAlterada(nameof(BaseCalculoIcmsSt));
            PropriedadeAlterada(nameof(ValorIcmsSt));
        }

        private void NotificaCreditoSilenciosamente()
        {
            PropriedadeAlterada(nameof(PercentualCredito));
            PropriedadeAlterada(nameof(ValorCredito));
        }

        private bool VerificaTributacaoIcmsSelecionado()
        {
            return TributacaoIcmsSelecionado == null;
        }

        protected virtual void OnAnteriorHandler()
        {
            AnteriorHandler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnProximoHandler()
        {
            ProximoHandler?.Invoke(this, this);
        }

        public void PreencherCom(Cte cte)
        {
            _baseCalculoPadrao = cte.ValorServico;
            var cteConfig = cte.CteConfigImposto;
            var impostoCst = cte.CteImpostoCst;
            var impostoDifal = cte.CteImpostoDifal;

            if (impostoCst != null)
                TributacaoIcmsSelecionado = impostoCst.TributacaoIcms;

            AtualizarConfig(cteConfig);

            AtualizarImpostoCst(impostoCst);

            AtualizaPartilha(impostoDifal);
        }

        private void AtualizaPartilha(CteImpostoDifal impostoDifal)
        {
            if (impostoDifal != null)
            {
                _percentualFcp = impostoDifal.PercentualFcp;
                Observacao = impostoDifal.Observacao;
                _valorIcmsFcp = impostoDifal.ValorIcmsFcp;
                _partilhaBaseCalculo = impostoDifal.BaseCalculo;
                _percentualInterestadual = impostoDifal.PercentualAliquotaInterestadual;
                _percentualInternoUfTermino = impostoDifal.PercentualAliquotaInterna;
                _percentualPartilhaUfTermino = impostoDifal.PercentualProvisorio;
                _valorIcmsPartilhaUfInicio = impostoDifal.ValorIcmsUfInicio;
                _valorIcmsPartilhaUfTermino = impostoDifal.ValorIcmsUfTermino;
                NotificaPartilhaSilenciosamente();
            }
        }

        private void AtualizarImpostoCst(CteImpostoCst impostoCst)
        {
            if (impostoCst != null)
            {
                _baseCalculoIcms = impostoCst.BaseCalculoIcms;
                _valorCredito = impostoCst.ValorCredito;
                _percentualIcms = impostoCst.PercentualIcms;
                _percentualCredito = impostoCst.PercentualCredito;
                _valorIcms = impostoCst.ValorIcms;
                _baseCalculoIcmsSt = impostoCst.BaseCalculoIcmsSt;
                _percentualIcmsSt = impostoCst.PercentualIcmsSt;
                _valorIcmsSt = impostoCst.ValorIcmsSt;

                switch (TributacaoIcmsSelecionado.Codigo)
                {
                    case "00":
                    case "20":
                    case "90":
                        _percentualReducaoIcms = impostoCst.PercentualReducaoBc;
                        break;
                }

                NotificaCreditoSilenciosamente();
                NotificaIcmsSilenciosamente();
                NotificaIcmsStSilenciosamente();
            }
        }

        private void AtualizarConfig(CteConfigImposto cteConfig)
        {
            if (cteConfig != null)
            {
                IsCalcularTributosAutomatico = cteConfig.IsCalculosAutomaticos;
                IsCreditoAutomatico = cteConfig.IsCreditoIcmsAutomatico;
                IsPartilhaIcms = cteConfig.IsPartilha;
            }
        }

        public void SetBaseCalculoPadrao(decimal baseCalculoPadrao)
        {
            _baseCalculoPadrao = baseCalculoPadrao;
        }
    }
}