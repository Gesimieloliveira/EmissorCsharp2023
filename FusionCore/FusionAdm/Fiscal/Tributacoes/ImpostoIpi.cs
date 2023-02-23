using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Federal;
using static System.String;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FusionCore.FusionAdm.Fiscal.Tributacoes
{
    public class ImpostoIpi : Entidade
    {
        private CalculadoraIpi _calculadoraIpi = new CalculadoraIpi();
        private int _itemId;
        private ItemNfe _item;

        private ImpostoIpi()
        {
            //nhibernate
        }

        public ImpostoIpi(ItemNfe item, TributacaoIpi tributacaoIpi) : this()
        {
            Item = item;
            TributacaoIpi = tributacaoIpi;
            CodigoEnquadramentoLegal = short.Parse(Item.Produto.EnquadramentoIpi.Id);
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

        public string ClasseEnquadramento { get; set; } = Empty;
        public string CnpjProdutor { get; set; } = Empty;
        public string Selo { get; set; } = Empty;
        public int QuantidadeSelo { get; set; } = 0;
        public short CodigoEnquadramentoLegal { get; set; }
        public TributacaoIpi TributacaoIpi { get; set; }
        public decimal ValorBcIpi { get; set; }
        public decimal AliquotaIpi { get; set; }
        public decimal ValorIpi { get; set; }
        protected override int ReferenciaUnica => _itemId;

        public void AjustarIpi()
        {
            _calculadoraIpi.Aliquota = AliquotaIpi;
            _calculadoraIpi.ValorTributavel = Item.TotalTributavel;

            var res = _calculadoraIpi.Calcula();

            ValorBcIpi = res.Bc;
            ValorIpi = res.Valor;
        }
    }
}