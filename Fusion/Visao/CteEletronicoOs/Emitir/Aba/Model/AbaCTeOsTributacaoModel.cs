using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Visao.CteEletronicoOs.Emitir.OpcoesTributacao;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.Tributacoes.Flags;
using FusionWPF.Base.Utils.Dialogs;
using MotorTributarioNet.Impostos.Implementacoes;
using NFe.Classes;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public class AbaCTeOsTributacaoModel : AbaCTeOSViewModel
    {
        private CteOs _cteOs;

        public AbaCTeOsTributacaoModel()
        {
            OpcoesTributacao = new ObservableCollection<OpcaoTributacao>();
        }

        public event EventHandler<AbaCTeOsTributacaoModel> Anterior;
        public event EventHandler<AbaCTeOsTributacaoModel> Proximo;
        public ICommand CommandAnterior => GetSimpleCommand(AnteriorCommand);
        public ICommand CommandProximo => GetSimpleCommand(ProximoCommand);

        public RegimeTributario RegimeTributario
        {
            get => GetValue<RegimeTributario>();
            set => SetValue(value);
        }

        public ObservableCollection<OpcaoTributacao> OpcoesTributacao
        {
            get => GetValue<ObservableCollection<OpcaoTributacao>>();
            set => SetValue(value);
        }

        public OpcaoTributacao TributacaoSelecionada
        {
            get => GetValue<OpcaoTributacao>();
            set
            {
                SetValue(value);
                AjustarImpostosDeAcordoComTributacao();
            }
        }

        private void AjustarImpostosDeAcordoComTributacao()
        {
            if (TributacaoSelecionada == null) return;
            if (!TributacaoSelecionada.PermiteIcms)
            {
                BaseCalculoIcms = 0;
                PercentualReducaoIcms = 0;
                AliquotaIcms = 0;
                ValorIcms = 0;
            }

            if (!TributacaoSelecionada.PermiteReducao) PercentualReducaoIcms = 0;
            if (!TributacaoSelecionada.PermiteCredito) ValorCredito = 0;
        }

        public decimal PercentualCredito
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal BaseCalculoIcms
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosIcms();
            }
        }

        public decimal AliquotaIcms
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosIcms();
            }
        }

        public decimal PercentualReducaoIcms
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosIcms();
            }
        }

        public decimal ValorIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PartilhaBaseCalculo
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosPartilha();
            }
        }

        public decimal PercentualProvisorioUFFim
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosPartilha();
            }
        }

        public decimal PercnetualPartilhaFcp
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosPartilha();
            }
        }

        public decimal ValorIcmsFcp
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliquotaInterestadual
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosPartilha();
            }
        }

        public decimal AliquotaInternaUFFim
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaImpostosPartilha();
            }
        }

        public decimal ValorIcmsUFInicio
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorIcmsUFFIm
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorCredito
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool UsarIcmsPartilha
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (value == false)
                {
                    ZerarValoresPartilha();
                    return;
                }

                PartilhaBaseCalculo = _cteOs?.PrecoServico.Valor ?? 0M;
                PercentualProvisorioUFFim = 100M;
            }
        }

        public bool UsarTributacaoFederal
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (value == false) ZerarValoresTributacaoFederal();
            }
        }

        public decimal ValorPis
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorCofins
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorImpostoRenda
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorInss
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorClss
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string Observacao { get; set; }

        protected virtual void OnAnterior()
        {
            Anterior?.Invoke(this, this);
        }

        protected virtual void OnProximo()
        {
            Proximo?.Invoke(this, this);
        }

        private void ProximoCommand(object obj)
        {
            try
            {
                OnProximo();
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

        private void AnteriorCommand(object obj)
        {
            OnAnterior();
        }

        public void ConfigurarRegimeTributario(RegimeTributario regime)
        {
            RegimeTributario = regime;
            OpcoesTributacao = new ObservableCollection<OpcaoTributacao>(
                OpcoesTributacaoFactory.CriarOpcoes(regime)
            );
        }

        private void CalculaImpostosIcms()
        {
            var bcReduzida = (BaseCalculoIcms - BaseCalculoIcms * PercentualReducaoIcms / 100).Arredondar(2);
            var icms = (bcReduzida * AliquotaIcms / 100).Arredondar(2);
            SetValue(icms, nameof(ValorIcms));
        }

        private void CalculaImpostosPartilha()
        {
            if (UsarIcmsPartilha == false)
            {
                ZerarValoresPartilha();
                return;
            }

            var baseCalculo = PartilhaBaseCalculo;

            var diferencaAliquota = AliquotaInternaUFFim - AliquotaInterestadual;
            var aliquotaPartilha = diferencaAliquota <= 0 ? 0 : diferencaAliquota;

            var icmsPartilha = baseCalculo * (aliquotaPartilha / 100);
            var icmsUFFim = (icmsPartilha * PercentualProvisorioUFFim / 100).Arredondar(2);
            var icmsUFInicio = icmsUFFim - icmsUFFim;

            SetValue(icmsUFFim, nameof(ValorIcmsUFFIm));
            SetValue(icmsUFInicio, nameof(ValorIcmsUFInicio));

            // Calculo FCP

            var baseCaculo = PartilhaBaseCalculo;
            var valorFcp = (baseCaculo * (PercnetualPartilhaFcp / 100)).Arredondar(2);

            SetValue(valorFcp, nameof(ValorIcmsFcp));

            Observacao = ResultadoCalculoDifal.GetObservacaoDifal(
                new DadosMensagemDifal(ValorIcmsFcp, ValorIcmsUFFIm, ValorIcmsUFInicio)
            );
        }

        private void ZerarValoresPartilha()
        {
            SetValue(0.00M, nameof(PercnetualPartilhaFcp));
            SetValue(0.00M, nameof(ValorIcmsFcp));
            SetValue(0.00M, nameof(PartilhaBaseCalculo));
            SetValue(0.00M, nameof(AliquotaInterestadual));
            SetValue(0.00M, nameof(AliquotaInternaUFFim));
            SetValue(0.00M, nameof(PercentualProvisorioUFFim));
            SetValue(0.00M, nameof(ValorIcmsUFInicio));
            SetValue(0.00M, nameof(ValorIcmsUFFIm));
        }

        private void ZerarValoresTributacaoFederal()
        {
            SetValue(default(decimal), nameof(ValorPis));
            SetValue(default(decimal), nameof(ValorCofins));
            SetValue(default(decimal), nameof(ValorImpostoRenda));
            SetValue(default(decimal), nameof(ValorInss));
            SetValue(default(decimal), nameof(ValorClss));
        }

        public void ComCteOs(CteOs cteOs)
        {
            _cteOs = cteOs;
            ConfigurarRegimeTributario(_cteOs.Emitente.RegimeTributario);
            AtualizarConfig();
            AtualizarImpostoCst();
            AtualizarImpostoFederal();
            AtualizaPartilha();
        }

        private void AtualizarImpostoCst()
        {
            if (_cteOs?.TributacaoIcms == null)
            {
                var precoServico = _cteOs?.PrecoServico.Valor ?? 0M;
                SetValue(precoServico, nameof(BaseCalculoIcms));
                return;
            }

            var tibutacao = _cteOs.TributacaoIcms;

            TributacaoSelecionada =
                OpcoesTributacao.SingleOrDefault(i => i.Cst == tibutacao.TributacaoIcms.Codigo);

            SetValue(tibutacao.BaseCalculo, nameof(BaseCalculoIcms));
            SetValue(tibutacao.Aliquota, nameof(AliquotaIcms));
            SetValue(tibutacao.PercentualReducao, nameof(PercentualReducaoIcms));
            SetValue(tibutacao.Valor, nameof(ValorIcms));
            SetValue(tibutacao.ValorCredito, nameof(ValorCredito));
        }

        private void AtualizarImpostoFederal()
        {
            if (_cteOs?.TributacaoFederal == null) return;

            var i = _cteOs.TributacaoFederal;

            SetValue(i.ValorPis, nameof(ValorPis));
            SetValue(i.ValorCofins, nameof(ValorCofins));
            SetValue(i.ValorImpostoRenda, nameof(ValorImpostoRenda));
            SetValue(i.ValorInss, nameof(ValorInss));
            SetValue(i.ValorClss, nameof(ValorClss));
        }

        private void AtualizaPartilha()
        {
            if (_cteOs?.TributacaoDifal == null) return;
            var difal = _cteOs.TributacaoDifal;

            SetValue(difal.PercentualFcp, nameof(PercnetualPartilhaFcp));
            SetValue(difal.ValorIcmsFcp, nameof(ValorIcmsFcp));
            SetValue(difal.BaseCalculo, nameof(PartilhaBaseCalculo));
            SetValue(difal.PercentualAliquotaInterestadual, nameof(AliquotaInterestadual));
            SetValue(difal.PercentualAliquotaInterna, nameof(AliquotaInternaUFFim));
            SetValue(difal.PercentualProvisorio, nameof(PercentualProvisorioUFFim));
            SetValue(difal.ValorIcmsUfInicio, nameof(ValorIcmsUFInicio));
            SetValue(difal.ValorIcmsUfTermino, nameof(ValorIcmsUFFIm));
        }

        private void AtualizarConfig()
        {
            if (_cteOs?.ConfigImposto == null) return;

            SetValue(_cteOs.ConfigImposto.IsPartilha, nameof(UsarIcmsPartilha));
            SetValue(_cteOs.ConfigImposto.UsarTributacaoFederal, nameof(UsarTributacaoFederal));
        }
    }
}