using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Federal;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Compras
{
    public class IpiCompra : Entidade
    {
        private IpiCompra()
        {
            //nhibernate
        }

        public IpiCompra(ItemCompra item, TributacaoIpi ipi = null) : this()
        {
            Item = item;
            Id = item.Id;
            Ipi = ipi;
        }

        private int Id { get; set; }
        public ItemCompra Item { get; private set; }
        public TributacaoIpi Ipi { get; set; }
        public decimal Aliquota { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorIpi { get; set; }
        protected override int ReferenciaUnica => Id;
    }
}