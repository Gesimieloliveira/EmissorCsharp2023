using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Federal;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Compras
{
    public class PisCompra : EntidadeBase<int>
    {
        private int Id { get; set; }
        public ItemCompra Item { get; private set; }
        public TributacaoPis Pis { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorPis { get; set; }
        protected override int ChaveUnica => Id;

        private PisCompra()
        {
            //nhibernate
        }

        public PisCompra(ItemCompra item, TributacaoPis pis =  null) : this()
        {
            Item = item;
            Id = item.Id;
            Pis = pis;
        }
    }
}
