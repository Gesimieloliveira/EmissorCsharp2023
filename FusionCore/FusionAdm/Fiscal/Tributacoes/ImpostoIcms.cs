using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Estadual;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.Tributacoes
{
    public class ImpostoIcms : Entidade
    {
        private readonly CalculadoraIcms _calculadoraIcms = new CalculadoraIcms();
        private readonly CalculadoraIcmsSt _calculadoraSt = new CalculadoraIcmsSt();
        private int _itemId;
        private ItemNfe _item;

        private ImpostoIcms()
        {
            ModalidadeBcIcms = DeterminacaoBcIcms.ValorOperacao;
            ModalidadeBcSt = DeterminacaoBcIcmsSt.MargemValorAgregado;
            OrigemMercadoria = OrigemMercadoria.Nacional;
        }

        public ImpostoIcms(ItemNfe item, TributacaoCst cst) : this()
        {
            Item = item;
            Cst = cst;
            OrigemMercadoria = item.Produto.OrigemMercadoria;
        }

        private ItemNfe Item
        {
            get => _item;
            set
            {
                _item = value;
                _itemId = value?.Id ?? 0;
            }
        }

        public ItemNfe ObterItemNfe()
        {
            return Item;
        }

        public TributacaoCst Cst { get; set; }
        public OrigemMercadoria OrigemMercadoria { get; set; }
        public decimal AliquotaCredito { get; set; }
        public decimal ValorCredito { get; set; }
        public DeterminacaoBcIcms ModalidadeBcIcms { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal ReducaoBcIcms { get; set; }
        public decimal ValorBcIcms { get; set; }
        public decimal ValorIcms { get; set; }
        public DeterminacaoBcIcmsSt ModalidadeBcSt { get; set; }
        public decimal AliquotaSt { get; set; }
        public decimal AliquotaIcmsInternoSt { get; set; }
        public decimal MvaSt { get; set; }
        public decimal ReducaoBcSt { get; set; }
        public decimal ValorBcSt { get; set; }
        public decimal ValorIcmsSt { get; set; }
        public bool IpiCompoeBcIcmsSt { get; set; }
        protected override int ReferenciaUnica => _itemId;
        public decimal ValorBcFcpSt { get; set; }
        public decimal AliquotaFcpSt { get; set; }
        public decimal ValorFcpSt { get; set; }
        public decimal AliquotaFcp { get; set; }
        public decimal ValorBcFcp { get; set; }
        public decimal ValorFcp { get; set; }

        public bool EhConsumidorFinal()
        {
            return _item.Nfe.Destinatario.IndicadorOperacaoFinal == IndicadorOperacaoFinal.ConsumidorFinal;
        }

        public void AjustarImposto()
        {
            AjustarIcms();
            AjustarIcmsSubstituicao();
        }

        private void AjustarIcms()
        {
            if (!Cst.PermiteIcms())
            {
                ZerarValoresIcms();
                return;
            }

            _calculadoraIcms.ValorTributavel = Item.TotalTributavel;
            _calculadoraIcms.Reducao = ReducaoBcIcms;
            _calculadoraIcms.Aliquota = AliquotaIcms;
            _calculadoraIcms.ValorFrete = Item.ValorFreteFixoRateio;
            _calculadoraIcms.ValorOutros = Item.ValorDespesasFixaRateio;
            _calculadoraIcms.ValorSeguro = Item.ValorSeguroFixoRateio;
            _calculadoraIcms.ValorIpi = 0.00M;

            var res = _calculadoraIcms.Calcula();

            ValorBcIcms = res.Bc;
            ValorIcms = res.Valor;
        }

        private void ZerarValoresIcms()
        {
            ReducaoBcIcms = 0.00M;
            AliquotaIcms = 0.00M;
            ValorBcIcms = 0.00M;
            ValorIcms = 0.00M;
        }

        private void AjustarIcmsSubstituicao()
        {
            if (!Cst.PermiteSubstituicao())
            {
                ZerarValoresSt();
                return;
            }

            _calculadoraSt.ValorTributavel = Item.TotalTributavel;
            _calculadoraSt.AliquotaIcmsInterno = AliquotaIcmsInternoSt;
            _calculadoraSt.Reducao = ReducaoBcSt;
            _calculadoraSt.Mva = MvaSt;
            _calculadoraSt.Aliquota = AliquotaSt;
            _calculadoraSt.ValorFrete = Item.ValorFreteFixoRateio;
            _calculadoraSt.ValorOutros = Item.ValorDespesasFixaRateio;
            _calculadoraSt.ValorSeguro = Item.ValorSeguroFixoRateio;
            _calculadoraSt.ValorIpi = Item.Ipi.ValorIpi;

            var res = _calculadoraSt.Calcula();

            ValorBcSt = res.Bc;
            ValorIcmsSt = res.Valor;
        }

        private void ZerarValoresSt()
        {
            AliquotaIcmsInternoSt = 0.00M;
            ReducaoBcSt = 0.00M;
            MvaSt = 0.00M;
            AliquotaSt = 0.00M;
            ValorBcSt = 0.00M;
            ValorIcmsSt = 0.00M;
        }
    }
}