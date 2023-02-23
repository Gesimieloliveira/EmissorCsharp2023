using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Calculadoras;
using FusionCore.Tributacoes.Federal;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FusionCore.FusionAdm.Fiscal.Tributacoes
{
    public class ImpostoCofins : Entidade
    {
        private readonly CalculadoraCofins _calculadoraCofins = new CalculadoraCofins();
        private int _itemId;
        private ItemNfe _item;

        private ImpostoCofins()
        {
            //for nhibernate
        }

        public ImpostoCofins(ItemNfe item) : this()
        {
            _item = item;
        }

        public ItemNfe Item
        {
            get => _item;
            set
            {
                _item = value;
                _itemId = value?.Id ?? 0;
            }
        }

        public TributacaoCofins Cst { get; set; }
        public decimal ValorBcCofins { get; set; }
        public decimal AliquotaCofins { get; set; }
        public decimal ValorCofins { get; set; }

        protected override int ReferenciaUnica => _itemId;

        public void AjustarCofins()
        {
            _calculadoraCofins.Aliquota = AliquotaCofins;
            _calculadoraCofins.ValorTributavel = Item.TotalTributavel;

            var res = _calculadoraCofins.Calcula();

            ValorBcCofins = res.Bc;
            ValorCofins = res.Valor;
        }
    }
}