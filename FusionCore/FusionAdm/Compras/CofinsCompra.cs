using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Federal;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Compras
{
    public class CofinsCompra : EntidadeBase<int>
    {
        private int Id { get; set; }
        public ItemCompra Item { get; private set; }
        public TributacaoCofins Cofins { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorCofins { get; set; }
        protected override int ChaveUnica => Id;

        private CofinsCompra()
        {
            //nhibernate
        }

        public CofinsCompra(ItemCompra item, TributacaoCofins cofins = null) : this()
        {
            Item = item;
            Id = item.Id;
            Cofins = cofins;
        }
    }
}