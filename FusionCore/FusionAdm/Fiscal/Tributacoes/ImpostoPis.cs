using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Federal;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace FusionCore.FusionAdm.Fiscal.Tributacoes
{
    public class ImpostoPis : Entidade
    {
        private CalculadoraPis _calculadoraPis = new CalculadoraPis();
        private int _itemId;
        private ItemNfe _item;

        public ItemNfe Item
        {
            get { return _item; }
            set
            {
                _item = value;
                _itemId = value?.Id ?? 0;
            }
        }

        public TributacaoPis Cst { get; set; }
        public decimal ValorBcPis { get; set; }
        public decimal AliquotaPis { get; set; }
        public decimal ValorPis { get; set; }

        protected override int ReferenciaUnica => _itemId;

        private ImpostoPis()
        {
            //nhibernate
        }

        public ImpostoPis(ItemNfe item) : this()
        {
            _item = item;
        }

        public void AjustarPis()
        {
            _calculadoraPis.Aliquota = AliquotaPis;
            _calculadoraPis.ValorTributavel = Item.TotalTributavel;

            var res = _calculadoraPis.Calcula();

            ValorBcPis = res.Bc;
            ValorPis = res.Valor;
        }
    }
}