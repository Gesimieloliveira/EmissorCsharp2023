using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Flags;
using FusionCore.Tributacoes.Repositorio;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public class IcmsContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private readonly CalculadoraIcms _calculadoraIcms = new CalculadoraIcms();
        private readonly CalculadoraIcmsSt _calculadoraIcmsSt = new CalculadoraIcmsSt();
        private readonly CalculadoraFcp _calculadoraFcp = new CalculadoraFcp();
        private readonly CalculadoraFcpSt _calculadoraFcpSt = new CalculadoraFcpSt();
        private readonly CalculadoraCredito _calculadoraCredito = new CalculadoraCredito();
        private RegimeTributario _regimeTributario;
        private PerfilNfeSimplesNacional _simplesNacionalPerfil;

        public IEnumerable<TributacaoCst> IcmsDisponiveis
        {
            get => GetValue<IEnumerable<TributacaoCst>>();
            set => SetValue(value);
        }

        public TributacaoCst CstSelecionado
        {
            get => GetValue<TributacaoCst>();
            set
            {
                SetValue(value);
                AjustarTodosOsImpostos();
            }
        }

        public decimal ValorTributavel
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarTodosOsImpostos();
            }
        }

        public decimal ValorIpi
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarTodosOsImpostos();
            }
        }

        public decimal Aliquota
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcms();
            }
        }

        public decimal Reducao
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcms();
            }
        }

        public decimal BaseCalculo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorIcms
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal Mva
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcmsSt();
            }
        }

        public decimal AliquotaIcmsInternoSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcmsSt();
            }
        }

        public decimal AliquotaSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcmsSt();
            }
        }

        public decimal ReducaoSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoIcmsSt();
            }
        }

        public decimal BaseCalculoSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorIcmsSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliquotaFcp
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoFcp();
            }
        }

        public decimal BcFcp
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorFcp
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliquotaFcpSt
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarImpostoFcpSt();
            }
        }

        public decimal BcFcpSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorFcpSt
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal AliqoutaCredito
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                AjustarCreditoIcms();
            }
        }

        public decimal ValorCredito
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool AutoAjusteImposto
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                AjustarTodosOsImpostos();
            }
        }

        public event EventHandler<RegimeTributario> RegimeTributarioChanged;

        public void ConfigurarOpcoesCst(RegimeTributario regime)
        {
            _regimeTributario = regime;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var rcst = new RepositorioTributacaoCst(sessao);
                IcmsDisponiveis = rcst.ParaNfe(regime);
            }

            RegimeTributarioChanged?.Invoke(this, _regimeTributario);
        }

        private void AjustarTodosOsImpostos()
        {
            AjustarCreditoIcms();
            AjustarImpostoIcms();
            AjustarImpostoIcmsSt();
            AjustarImpostoFcp();
            AjustarImpostoFcpSt();
        }

        private void ZerarCamposIcms()
        {
            SetValue(0.00M, nameof(Aliquota));
            SetValue(0.00M, nameof(Reducao));
            SetValue(0.00M, nameof(BaseCalculo));
            SetValue(0.00M, nameof(ValorIcms));
        }

        private void ZerarCamposSt()
        {
            SetValue(0.00M, nameof(AliquotaIcmsInternoSt));
            SetValue(0.00M, nameof(AliquotaSt));
            SetValue(0.00M, nameof(ReducaoSt));
            SetValue(0.00M, nameof(Mva));
            SetValue(0.00M, nameof(BaseCalculoSt));
            SetValue(0.00M, nameof(ValorIcmsSt));
        }

        public void PreencherCom(ProdutoDTO produto, ItemContexto contexto)
        {
            ZerarCamposIcms();
            ZerarCamposSt();

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var codCst = produto.RegraTributacaoSaida.CodigoCstParaNfe(_regimeTributario);

                if (_regimeTributario == RegimeTributario.SimplesNacional
                    && _simplesNacionalPerfil?.Csosn != null)
                {
                    codCst = _simplesNacionalPerfil.Csosn.Codigo;
                } 

                var rcst = new RepositorioTributacaoCst(sessao);
                var cst = rcst.GetPeloId(codCst);

                SetValue(cst, nameof(CstSelecionado));
                SetValue(contexto.Total, nameof(ValorTributavel));

                if (cst.PermiteIcms())
                {
                    var pReducao = cst.PermiteReducaoIcms() ? produto.ReducaoIcms : 0.00M;

                    SetValue(produto.AliquotaIcms, nameof(Aliquota));
                    SetValue(pReducao, nameof(Reducao));

                    AjustarImpostoIcms();
                }

                if (cst.PermiteSubstituicao())
                {
                    SetValue(produto.AliquotaIcms, nameof(AliquotaSt));
                    SetValue(produto.ReducaoIcms, nameof(ReducaoSt));
                    SetValue(produto.PercentualMva, nameof(Mva));

                    AjustarImpostoIcmsSt();
                }
            }
        }

        private void AjustarImpostoIcms()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            if (CstSelecionado?.PermiteIcms() != true || Aliquota <= 0)
            {
                ZerarCamposIcms();
                return;
            }

            _calculadoraIcms.Reducao = Reducao;
            _calculadoraIcms.ValorTributavel = ValorTributavel;
            _calculadoraIcms.Aliquota = Aliquota;

            var res = _calculadoraIcms.Calcula();

            SetValue(res.Bc, nameof(BaseCalculo));
            SetValue(res.Valor, nameof(ValorIcms));
        }

        private void AjustarImpostoIcmsSt()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            if (CstSelecionado?.PermiteSubstituicao() != true || AliquotaSt <= 0)
            {
                ZerarCamposSt();
                return;
            }

            _calculadoraIcmsSt.ValorTributavel = ValorTributavel;
            _calculadoraIcmsSt.ValorIpi = ValorIpi;
            _calculadoraIcmsSt.Aliquota = AliquotaSt;
            _calculadoraIcmsSt.Reducao = ReducaoSt;
            _calculadoraIcmsSt.Mva = Mva;
            _calculadoraIcmsSt.AliquotaIcmsInterno = AliquotaIcmsInternoSt;

            var res = _calculadoraIcmsSt.Calcula();

            SetValue(res.Bc, nameof(BaseCalculoSt));
            SetValue(res.Valor, nameof(ValorIcmsSt));
        }

        private void AjustarImpostoFcp()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            if (CstSelecionado?.PermiteFcp() != true || AliquotaFcp <= 0)
            {
                ZerarCamposFcp();
                return;
            }

            _calculadoraFcp.ValorTributavel = ValorTributavel;
            _calculadoraFcp.Percentual = AliquotaFcp;

            var res = _calculadoraFcp.Calcula();

            SetValue(res.Bc, nameof(BcFcp));
            SetValue(res.Valor, nameof(ValorFcp));
        }

        private void ZerarCamposFcp()
        {
            SetValue(0.00M, nameof(AliquotaFcp));
            SetValue(0.00M, nameof(BcFcp));
            SetValue(0.00M, nameof(ValorFcp));
        }

        private void AjustarImpostoFcpSt()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            if (CstSelecionado?.PermiteFcpSt() != true || AliquotaFcpSt <= 0)
            {
                ZerarCamposFcpSt();
                return;
            }

            _calculadoraFcpSt.ValorTributavel = ValorTributavel;
            _calculadoraFcpSt.Percentual = AliquotaFcpSt;

            var res = _calculadoraFcpSt.Calcula();

            SetValue(res.Bc, nameof(BcFcpSt));
            SetValue(res.Valor, nameof(ValorFcpSt));
        }

        private void ZerarCamposFcpSt()
        {
            SetValue(0.00M, nameof(AliquotaFcpSt));
            SetValue(0.00M, nameof(BcFcpSt));
            SetValue(0.00M, nameof(ValorFcpSt));
        }

        private void AjustarCreditoIcms()
        {
            if (!AutoAjusteImposto)
            {
                return;
            }

            if (AliqoutaCredito <= 0)
            {
                ZerarCamposCredito();
                return;
            }

            _calculadoraCredito.Aliqutoa = AliqoutaCredito;
            _calculadoraCredito.ValorTributavel = ValorTributavel;

            var res = _calculadoraCredito.Calcula();

            SetValue(res.Valor, nameof(ValorCredito));
        }

        private void ZerarCamposCredito()
        {
            SetValue(0.00M, nameof(AliqoutaCredito));
            SetValue(0.00M, nameof(ValorCredito));
        }

        public void AplicarAlteracoesEm(ItemNfe item)
        {
            if (item.ImpostoIcms == null)
            {
                item.ImpostoIcms = new ImpostoIcms(item, CstSelecionado);
            }

            item.ImpostoIcms.Cst = CstSelecionado;
            item.ImpostoIcms.AliquotaCredito = AliqoutaCredito;
            item.ImpostoIcms.ValorCredito = ValorCredito;
            item.ImpostoIcms.AliquotaIcms = Aliquota;
            item.ImpostoIcms.ValorBcIcms = BaseCalculo;
            item.ImpostoIcms.ReducaoBcIcms = Reducao;
            item.ImpostoIcms.ValorIcms = ValorIcms;
            item.ImpostoIcms.MvaSt = Mva;
            item.ImpostoIcms.AliquotaSt = AliquotaSt;
            item.ImpostoIcms.ValorBcSt = BaseCalculoSt;
            item.ImpostoIcms.ReducaoBcSt = ReducaoSt;
            item.ImpostoIcms.ValorIcmsSt = ValorIcmsSt;
            item.ImpostoIcms.ValorFcp = ValorFcp;
            item.ImpostoIcms.ValorBcFcp = BcFcp;
            item.ImpostoIcms.AliquotaFcp = AliquotaFcp;
            item.ImpostoIcms.ValorFcpSt = ValorFcpSt;
            item.ImpostoIcms.ValorBcFcpSt = BcFcpSt;
            item.ImpostoIcms.AliquotaFcpSt = AliquotaFcpSt;
            item.ImpostoIcms.AliquotaIcmsInternoSt = AliquotaIcmsInternoSt;
        }

        public void Com(ItemNfe item)
        {
            SetValue(item.AutoAjustarImposto, nameof(AutoAjusteImposto));
            SetValue(item.ImpostoIcms.Cst, nameof(CstSelecionado));
            SetValue(item.ImpostoIcms.AliquotaIcms, nameof(Aliquota));
            SetValue(item.ImpostoIcms.ReducaoBcIcms, nameof(Reducao));
            SetValue(item.ImpostoIcms.ValorBcIcms, nameof(BaseCalculo));
            SetValue(item.ImpostoIcms.ValorIcms, nameof(ValorIcms));
            SetValue(item.ImpostoIcms.AliquotaSt, nameof(AliquotaSt));
            SetValue(item.ImpostoIcms.ReducaoBcSt, nameof(ReducaoSt));
            SetValue(item.ImpostoIcms.MvaSt, nameof(Mva));
            SetValue(item.ImpostoIcms.ValorBcSt, nameof(BaseCalculoSt));
            SetValue(item.ImpostoIcms.ValorIcmsSt, nameof(ValorIcmsSt));
            SetValue(item.ImpostoIcms.ValorFcp, nameof(ValorFcp));
            SetValue(item.ImpostoIcms.AliquotaFcp, nameof(AliquotaFcp));
            SetValue(item.ImpostoIcms.ValorBcFcp, nameof(BcFcp));
            SetValue(item.ImpostoIcms.ValorFcpSt, nameof(ValorFcpSt));
            SetValue(item.ImpostoIcms.AliquotaFcpSt, nameof(AliquotaFcpSt));
            SetValue(item.ImpostoIcms.ValorBcFcpSt, nameof(BcFcpSt));
            SetValue(item.ImpostoIcms.AliquotaIcmsInternoSt, nameof(AliquotaIcmsInternoSt));
            SetValue(item.ImpostoIcms.AliquotaCredito, nameof(AliqoutaCredito));
            SetValue(item.ImpostoIcms.ValorCredito, nameof(ValorCredito));
        }

        public void ThrowExceptionSeInvalido()
        {
            if (CstSelecionado == null) throw new InvalidOperationException("Preciso que escolha um ICMS");
            if (AliqoutaCredito < 0) throw new InvalidOperationException("Aliquota Crédito não pode ser negativo");
            if (Aliquota < 0) throw new InvalidOperationException("Aliquota ICMS não pode ser negativo");
            if (AliquotaSt < 0) throw new InvalidOperationException("Aliquota ICMS ST não pode ser negativo");
            if (AliquotaFcp < 0) throw new InvalidOperationException("Aliquota FCP não pode ser negativo");
            if (AliquotaFcpSt < 0) throw new InvalidOperationException("Aliquota FCP ST não pode ser negativo");
        }

        public void PriorizarCsosn(PerfilNfeSimplesNacional simplesNacional)
        {
            _simplesNacionalPerfil = simplesNacional;

            if (_regimeTributario != RegimeTributario.SimplesNacional)
            {
                return;
            }

            if (_simplesNacionalPerfil?.Csosn == null)
            {
                return;
            }

            CstSelecionado = IcmsDisponiveis.FirstOrDefault(x => x.Id == _simplesNacionalPerfil.Csosn.Codigo);
            AliqoutaCredito = simplesNacional.AliquotaCredito;
        }
    }
}